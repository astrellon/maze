class Processor:

    def join_handle(self, input):
        return {
            "welcome_to": self.world.name
        }

    world = None
    handlers = {}

    def __init__(self, world):
        self.world = world

        self.handlers["join"] = self.join_handle

    def make_response(self, resp, error, input):
        return {
            "rid": input["id"],
            "result": resp,
            "error": error
        }

    def handle(self, input):
        resp = None
        error = None

        try:
            if input["cmd"] in self.handlers:
                resp = self.handlers[input["cmd"]](input);

        except e as BaseException: 
            error = "exception executing command: " + str(e)

        if resp is None:
            error = "unknown input"

        return self.make_response(resp, error, input);
