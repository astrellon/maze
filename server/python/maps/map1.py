import game_map
import json

module_name = ""

class Map1(game_map.Map):
    def create_map(self):
        self.add_border()

        self.set_map(2, 2, 1)
        self.set_map(2, 3, 1)
        self.set_map(2, 4, 1)
        self.set_map(2, 5, 1)

    @property
    def base_map(self):
        print("Return basemap: ", module_name)
        return module_name

def setup(world, mod_file):
    module_name = mod_file
    print("Setting module name to: ", module_name)

def create(world, name):
    gmap = Map1()
    gmap.init_map(12, 8)
    world.add_map(name, gmap)

