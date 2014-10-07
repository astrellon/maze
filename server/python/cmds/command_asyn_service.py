import asyncore
import socket
import json

class AsynService(asyncore.dispatcher):
    def __init__(self, host, port, engine):
        asyncore.dispatcher.__init__(self)
        self.create_socket(socket.AF_INET, socket.SOCK_STREAM)
        self.bind((host, port))
        self.listen(1)
        self.running = False
        self.connections = []
        self.processor = engine.processor

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
        self.player = None
        self.address = address
        self.server = server
        self.obuffer = []
        self.user = None

    def write(self, data):
        try:
            if isinstance(data, str):
                self.obuffer.append(bytes(data, 'utf8'))
            else:
                self.obuffer.append(bytes(json.dumps(data), 'utf8'))

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
            decoded = data.decode("utf-8")
            data_json = None
            try:
                data_json = json.loads(decoded)
            except BaseException as e:
                self.write({
                    "error": str(e)
                })
                return
                
            try:
                response = self.server.processor.handle(self, data_json)
                if response is not None:
                    self.write(response)
            except BaseException as e:
                self.write({
                    "error": str(e)
                })

    def handle_close(self):
        self.server.connections.remove(self)

    def writable(self):
        return len(self.obuffer) > 0

    def handle_write(self):
        if self.obuffer[0] is None:
            self.close()
            return

        sent = self.send(self.obuffer[0])
        if sent >= len(self.obuffer[0]):
            self.obuffer.pop(0)
        else:
            self.obuffer[0] = self.obuffer[0][sent:]

