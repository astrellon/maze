import game.map
import json

module_name = ""

class Map1(game.map.Map):
    def create_map(self):
        self.add_border()

        """
        self.set_map(2, 2, "dirt")
        self.set_map(2, 3, "dirt")
        self.set_map(2, 4, "dirt")
        self.set_map(2, 5, "dirt")
        """

    @property
    def base_map(self):
        print("Return basemap: ", module_name)
        return module_name

def setup(world, mod_file):
    module_name = mod_file
    print("Setting module name to: ", module_name)

def create(world, name):
    gmap = Map1(world)
    gmap.init_map(3, 3)
    world.add_map(name, gmap)

