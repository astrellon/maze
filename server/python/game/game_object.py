class GameObject:
    _next_id = 0

    def __init__(self, engine):
        GameObject._next_id = GameObject._next_id + 1
        self._id = GameObject._next_id
        self._display_name = "Noname"
        self._engine = engine
        self._type = "obj"
        self._created = {}

    @property
    def id(self):
        return self._id

    @property
    def engine(self):
        return self._engine

    @property
    def type(self):
        return self._type

    @property
    def display_name(self, name):
        if name != self._display_name:
            self._display_name = name
            self.engine.server.broadcast({
                "cmd": "name_change",
                "data": {
                    "id": self.id,
                    "name": name
                }
            })

    @property
    def display_name(self):
        return self._display_name

    @property
    def create_for_network(self):
        return {
            "cmd": "new_obj",
            "data": {
                "id": self.id,
                "name": self.display_name,
                "type": self.type
            }
        }

