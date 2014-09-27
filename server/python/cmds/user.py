class User:

    @property
    def name(self):
        return self._name
    @property
    def name(self, name):
        self._name = name

    @property
    def network_handler(self):
        return self._network_handler

    def __init__(self, network_handler, name):
        self._name = name
        self._network_handler = network_handler


class Users:

    @property
    def engine(self):
        return self._engine

    def __init__(self, engine):
        self._engine = engine
        self._users = {}

    def add_user(self, network_handler):
        if network_handler.address not in self._users:
            self._users[network_handler.address] = User(network_handler, "No name")
        
        return self._users[network_handler.address]

    def get_user(self, address):
        if address in self._users:
            return self._users[address]

        return None

    def remove_user(self, address):
        pass

