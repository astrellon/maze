class Processor:

    def join_handle(self, input):
        return {
            "welcome_to": self.world.name
        }

    engine = None
    handlers = {}

    @property
    def world(self):
        if engine is not None:
            return engine.world
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
        resp = None
        error = None

        data = None
        try:
            data = input["data"]
        except e as BaseException:
            error = "no data in command"

        if data is not None:
            try:
                if data["cmd"] in self.handlers:
                    resp = self.handlers[data["cmd"]](input);

            except e as BaseException: 
                error = "exception executing command: " + str(e)

        if resp is None:
            error = "unknown input"

        return self.make_response(resp, error, input);
