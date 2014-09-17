import asyncore
import socket
import json
import cmds.command_processor

class AsynServer(asyncore.dispatcher):
    def __init__(self, host, port, engine):
        asyncore.dispatcher.__init__(self)
        self.create_socket(socket.AF_INET, socket.SOCK_STREAM)
        self.bind((host, port))
        self.listen(1)
        self.running = False
        self.connections = []

    def start(self):
        self.running = True
        while self.running:
            asyncore.loop(timeout=0.1)

    def stop(self):
        self.running = False

    def handle_accept(self):
        socket, address = self.accept()
        print("Connection from: ", address)
        conn = Handler(socket, address, self)
        self.connections.append(conn)
        self.broadcast("New player: " + str(address))

    def broadcast(self, message):
        print("Broadcasting new player: ", len(self.connections))
        for conn in self.connections:
            conn.write(message)
            

    def shutdown(self):
        self.close()
        for conn in self.connections:
            conn.write("Shutting down")
            conn.shutdown()

class Handler(asyncore.dispatcher):

    def __init__(self, socket, address, server):
        asyncore.dispatcher.__init__(self, socket)
        self.address = address
        self.server = server
        self.obuffer = []

    def write(self, data):
        try:
            print("Appending data for: ", self.address)
            self.obuffer.append(bytes(data, 'utf8'))
        except BaseException as e:
            print("Error appending output buffer: ", e)

    def shutdown(self):
        self.obuffer.append(None)

    def handle_read(self):
        # Should accumlate all data until a full command has been received.
        data = self.recv(4096)
        if not data:
            print("No data given, closing connection")
            self.close()
        else:
            print("Outbuffer: ", data)

    def handle_close(self):
        self.server.connections.remove(self)

    def writable(self):
        return len(self.obuffer) > 0

    def handle_write(self):
        if len(self.obuffer) == 0:
            return
        
        if self.obuffer[0] is None:
            self.close()
            return

        sent = self.send(self.obuffer[0])
        if sent >= len(self.obuffer[0]):
            self.obuffer.pop(0)
        else:
            self.obuffer[0] = self.obuffer[0][sent:]

