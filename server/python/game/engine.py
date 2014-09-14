import cmds.command_service
import cmds.command_processor
import game.world
import threading
import signal

class Engine:
    world = None

    name = "Server Name"
    description = "Server description"
    server = None
    processor = None
    host = "0.0.0.0"
    port = 9091

    def setup(self):
        self.server = cmds.command_service.TCPServer((self.host, self.port), cmds.command_service.Handler)
        self.processor = cmds.command_processor.Processor(self)
        self.server.set_engine_processor(self, self.processor)

    def start_server_thread(self):
        
        # Start a thread with the server -- that thread will then start one
        # more thread for each request
        self.server_thread = threading.Thread(target=self.server.serve_forever)
        # Exit the server thread when the main thread terminates
        self.server_thread.daemon = True
        self.server_thread.start()
        print("Starting server on: " + self.host + ":" + str(self.port))

        server = self.server

        def signal_handler(signal, frame):
            print("Shutting down server.")
            server.shutdown()

        signal.signal(signal.SIGINT, signal_handler)

        self.server_thread.join()

    def create_world(self):
        self.world = game.world.World(engine)
        self.world.setup()

        self.world.load_maps()
        self.world.create_map("maps.map1", "testmap")
        self.world.serialise()

