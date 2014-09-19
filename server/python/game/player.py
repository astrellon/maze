import game.game_object.GameObject

class Player(game.game_object.GameObject):

    def __init__(self, engine, handler):
        game.game_object.GameObject.__init__(self, engine)
        self._network_handler = handler

    @property
    def network_handler(self):
        return self._network_handler

