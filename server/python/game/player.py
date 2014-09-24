import game

class PlayerObject(game.GameObject):

    def __init__(self, engine, user):
        game.GameObject.__init__(self, engine)
        self._user = user

    @property
    def user(self):
        return self._user

    @property
    def network_handler(self):
        return self._user.network_handler

