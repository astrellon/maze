import cmds
import game
import threading
import time

class Engine:

    def __init__(self):
        self.world = None

        self.name = "Server Name"
        self.description = "Server description"
        self.version = "0.0.1"
        self.server = None
        self.processor = None
        self.users = None
        self.host = "0.0.0.0"
        self.port = 9091
        self.running = False

    def setup(self):
        self.processor = cmds.Processor(self)
        self.users = cmds.Users(self)

    def start_server_thread(self):
        
        print("Starting server on: " + self.host + ":" + str(self.port))
        self.server = cmds.AsynService(self.host, self.port, self)

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
        self.world = game.World(self)
        self.world.setup()

        self.world.create_map("maps.map1", "testmap")
        #self.world.serialise()

