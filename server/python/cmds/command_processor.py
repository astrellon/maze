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
                resp = self.handlers[cmd](network_handler, data);
            else:
                error = "Cannot find cmd '" + cmd + "'"

        except BaseException as e: 
            error = "exception executing command: " + str(e)

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

        pass

    def create_world_handler(self, handler, input):
        if self.engine.world is not None:
            return {
                "error": "World already exists on server."
            }
        
        self.engine.create_world()
        self.world.create_map("maps.map1", "testmap")

    def join_world_handler(self, handler, input):
        if "name" not in input:
            return {
                "error": "Need a player name to join a world"
            }

            """
        handler.server.broadcast({
            "cmd": "new_player",
            "data": {
                "name": input["name"]
            }
        })
        """

        world_name = None
        if self.world is not None:
            world_name = self.world.name

        player = self.world.add_player(handler)
        
        return {
            "world": world_name,
            "player": player.id
        }

