import cmds
import traceback

class Processor:

    @property
    def world(self):
        if self.engine is not None:
            return self.engine.world
        return None

    def __init__(self, engine):
        self.handlers = {}
        self.engine = engine
        self.add_handlers()

    def add_handler(self, name, handler):
        self.handlers[name] = handler

    def make_response(self, resp, error, input):
        returned = {
            "result": resp,
            "error": error
        }

        if "id" in input:
            returned["rid"] = input["id"]

        return returned

    def handle(self, network_handler, input):

        cmd = None
        if "cmd" in input:
            cmd = input["cmd"]
        else:
            return self.make_response(None, "no cmd in command", input)

        data = None
        if "data" in input:
            data = input["data"]

        resp = None
        error = None
        try:
            print("Looking for cmd ", cmd)
            if cmd in self.handlers:
                try:
                    resp = self.handlers[cmd](network_handler, data)
                except BaseException as e:
                    error = str(e)
            else:
                error = "Cannot find cmd '" + cmd + "'"
                traceback.print_exc()

        except BaseException as e: 
            error = "exception executing command: " + str(e)
            traceback.print_exc()

        print("Responding with: ", error, " | ", resp)
        return self.make_response(resp, error, input)

    # -- HANDLERS --
    def add_handlers(self):
        self.add_handler("server_info", self.server_info_handler)
        self.add_handler("join_server", self.join_server_handler)
        self.add_handler("join_world", self.join_world_handler)
        self.add_handler("create_world", self.create_world_handler)

    def server_info_handler(self, handler, input):
        world_name = None
        if self.world is not None:
            world_name = self.world.name

        return {
            "name": self.engine.name,
            "description": self.engine.description,
            "version": self.engine.version,
            "current_world": world_name
        } 

    def join_server_handler(self, handler, input):
        handler.user = self.engine.users.add_user(handler)
        if "name" in input:
            handler.user._name = input["name"]
        
        return {
            "result": "success"
        }

    def create_world_handler(self, handler, input):
        if handler.user is None:
            raise RuntimeError("Cannot create world without joining server.")
        
        if self.engine.world is not None:
            raise RuntimeError("World already exists on server.")
        
        self.engine.create_world()
        self.world.create_map("maps.map1", "testmap")

    def join_world_handler(self, handler, input):
        if handler.user is None:
            raise RuntimeError("Cannot join world without joining server.")
        if self.world is None:
            raise RuntimeError("Cannot join world as one is not loaded.")

        player = self.world.add_player(handler.user)
        
        return {
            "world": self.world.serialise(),
            "tiles": self.world.tile_manager.serialise(),
            "player": player.id
        }, None

