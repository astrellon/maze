from game import Tile

def create_default_tiles(tile_manager):
    tile_manager.add_tile(Tile("map_border", False))
    tile_manager.add_tile(Tile("grass", True))
    tile_manager.add_tile(Tile("dirt", True))

