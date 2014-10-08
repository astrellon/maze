import game
import json

module_name = ""

class Map1(game.Map):
    def create_map(self):
        self.draw_rect(0, 0, self.width, self.height, "dirt")
        self.draw_rect(2, 2, self.width - 3, self.height - 3, "dirt")

    @property
    def base_map(self):
        print("Return basemap: ", module_name)
        return module_name

def setup(world, mod_file):
    module_name = mod_file
    print("Setting module name to: ", module_name)

def create(world, name):
    gmap = Map1(world)
    gmap.init_map(32, 32)
    world.add_map(name, gmap)

