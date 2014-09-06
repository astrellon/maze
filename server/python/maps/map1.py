import game_map
import json

def setup(world):
    gmap = game_map.Map()
    gmap.create_map(12, 8)
    world.add_map("map1", gmap)

    jsond = json.dumps(gmap.serialise()) 

    print("Loaded map1: ", jsond)
