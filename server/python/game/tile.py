class Tile:
    
    def __init__(self, name, walkable=True):
        self._name = name
        self.walkable = walkable

    @property
    def name(self):
        return self._name


class TileInstance:

    def __init__(self, tile, height = 0.0):
        self.tile = tile
        self.height = height

    def __str__(self):
        return "tile '" + self.tile.name + "'"

    def serialise(self):
        return {
            "name": self.tile.name,
            "height": self.height
        }


class TileManager:

    def __init__(self, world):
        self._world = world
        self._tiles = {}

    @property
    def tiles(self):
        return self._tiles

    @property
    def world(self):
        return self._world

    def add_tile(self, tile):
        print("Adding tile: ", tile.name, tile)
        self._tiles[tile.name] = tile

    def create_inst(self, name, height = 0.0):
        if name in self._tiles:
            return TileInstance(self._tiles[name], height)
        return None

