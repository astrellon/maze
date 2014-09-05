import socket
import socketserver
import json
import command_processor

class Handler(socketserver.BaseRequestHandler):
    running = True

    def handle(self): 
        while (self.running):
            data = str(self.request.recv(1024), 'ascii')
            if data:
                data_json = json.loads(data)
                response = self.server.processor.handle(data_json)
                print("Server Received: {}".format(data));
                response_bytes = bytes(json.dumps(response), 'ascii')
                self.request.sendall(response_bytes)

class TCPServer(socketserver.ThreadingMixIn, socketserver.TCPServer):
    daemon_threads = True
    allow_reuse_adress = True

    world = None
    processor = None

    def set_world_processor(self, world, processor):
        self.world = world
        self.processor = processor

