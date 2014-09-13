import cmds.command_service
import cmds.command_processor
import game.world

class Engine:
    world = None

    name = "Server Name"
    description = "Server description"
    server = None
    processor = None

    def setup(self):
        # Port 0 means to select an arbitrary unused port
        HOST, PORT = "0.0.0.0", 9090

        self.server = cmds.command_service.TCPServer((HOST, PORT), cmds.command_service.Handler)
        self.processor = cmds.command_processor.Processor(self)
        self.server.set_engine_processor(self, processor)

    def create_world(self):
        self.world = game.world.World(engine)
        self.world.setup()

        self.world.load_maps()
        self.world.create_map("maps.map1", "testmap")
        self.world.serialise()

