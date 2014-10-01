class Tile:
    
    def __init__(self, name, walkable=True):
        self._name = name
        self.walkable = walkable

    @property
    def name(self):
        return self._name

    def serialise(self):
        return {
            "name": self.name,
            "walkable": self.walkable
        }


class TileInstance:

    def __init__(self, tile, height = 0.0):
        self.tile = tile
        self.heights = [0, 0, 0, 0]
        
    def set_heights(self, height):
        self.heights[0] = height
        self.heights[1] = height
        self.heights[2] = height
        self.heights[3] = height

    @property
    def heightBL(self):
        return self.heights[0]
    @heightBL.setter
    def heightBL(self, height):
        self.heights[0] = height

    @property
    def heightBR(self):
        return self.heights[1]
    @heightBR.setter
    def heightBR(self, height):
        self.heights[1] = height

    @property
    def heightTL(self):
        return self.heights[2]
    @heightTL.setter
    def heightTL(self, height):
        self.heights[2] = height

    @property
    def heightTR(self):
        return self.heights[3]
    @heightTR.setter
    def heightTR(self, height):
        self.heights[3] = height

    def __str__(self):
        return "tile '" + self.tile.name + "'"

    def serialise(self):
        return {
            "name": self.tile.name,
            "heights": self.heights
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
        # Create instance from instance.
        if height is None:
            height = 0.0

        if isinstance(name, TileInstance):
            return TileInstance(name.tile, height)
        
        if name in self._tiles:
            return TileInstance(self._tiles[name], height)
        return None

    def serialise(self):
        tiles = []

        for tile_name in self._tiles.keys():
            tiles.append(self._tiles[tile_name].serialise())
        
        return {
            "tiles": tiles
        }

