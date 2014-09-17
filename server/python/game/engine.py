import cmds.command_service
import cmds.command_asyn_service
import cmds.command_processor
import game.world
import threading
import time

class Engine:
    world = None

    name = "Server Name"
    description = "Server description"
    version = "0.0.1"
    server = None
    processor = None
    host = "0.0.0.0"
    port = 9091
    running = False

    def setup(self):
        #self.server = cmds.command_service.TCPServer((self.host, self.port), cmds.command_service.Handler)
        self.processor = cmds.command_processor.Processor(self)
        #self.server.set_engine_processor(self, self.processor)

    def start_server_thread(self):
        
        print("Starting server on: " + self.host + ":" + str(self.port))
        self.server = cmds.command_asyn_service.AsynServer(self.host, self.port, self)

        # Start a thread with the server -- that thread will then start one
        # more thread for each request
        self.server_thread = threading.Thread(target=self.server.start)
        # Exit the server thread when the main thread terminates
        self.server_thread.daemon = True
        self.server_thread.start()

    def start_main_loop(self):
        print("Starting game loop")
        self.running = True

        try:
            while self.running:
                time.sleep(0.1)
        except BaseException as e:
            print("Stopping game loop")

    def stop_main_loop(self):
        self.running = False


    def create_world(self):
        self.world = game.world.World(engine)
        self.world.setup()

        self.world.load_maps()
        #self.world.create_map("maps.map1", "testmap")
        #self.world.serialise()

