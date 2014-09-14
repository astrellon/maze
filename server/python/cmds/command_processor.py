class Processor:

    def join_handle(self, input):
        if self.world is None:
            return {
                    "world": None
            }
        return {
            "world": self.world.name
        }

    engine = None
    handlers = {}

    @property
    def world(self):
        if self.engine is not None:
            return self.engine.world
        return None

    def __init__(self, engine):
        self.engine = engine

        self.handlers["join"] = self.join_handle

    def make_response(self, resp, error, input):
        returned = {
            "result": resp,
            "error": error
        }

        if "id" in input:
            returned["rid"] = input["id"]

        return returned

    def handle(self, input):

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
            if cmd in self.handlers:
                resp = self.handlers[cmd](data);

        except BaseException as e: 
            error = "exception executing command: " + str(e)

        return self.make_response(resp, error, input)
