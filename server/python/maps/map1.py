import game
import json

module_name = ""

class Map1(game.Map):
    def create_map(self):
        self.draw_rect(0, 0, self.width, self.height, "dirt")
        """
        for y in range(self.height):
            for x in range(self.width):
                self.set_height(x, y, x +y)
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
    gmap.init_map(5, 3)
    world.add_map(name, gmap)

