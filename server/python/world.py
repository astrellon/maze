import importlib
import glob
import re
import os

class World:

    name = "Maze"

    _maps = {}
    _base_maps = {}

    @property
    def maps(self):
        return _maps

    def load_maps(self):
        files = glob.glob("maps/*.py")

        for file in files:
            if file == "maps/__init__.py":
                continue

            mod_file = re.sub(".py$", "", file.replace("/", "."))
            mod = importlib.import_module(mod_file)

            if mod is None:
                print("Unable to load: ", file, " as python module.")
                continue

            self._base_maps[mod_file] = mod.__dict__
            print("Added base map: ", mod_file)
            if "setup" in mod.__dict__.keys():
                mod.__dict__["setup"](self, mod_file)

    def add_map_base(self, name, map):
        self._base_maps[name] = map

    def create_map(self, base_name, name):
        print("Creating map: ", base_name, name)
        if base_name in self._base_maps.keys():
            try:
                self._base_maps[base_name][ "create" ](self, name)
            except e as Exception:
                print("Error creating map: ", str(e))
        else:
            print("Unable to find base map: ", base_name)

    def add_map(self, name, map):
        print("Adding map: ", name, map)
        self._maps[name] = map

    def save_world(self):
        os.mkdir("worlds/" + name)

    def serialise(self):
        world = {}
        maps = {}
        world["maps"] = maps

        for map_name in self._maps.keys():
            maps[map_name] = self._maps[map_name].serialise()

        print("Serialised world: ", maps)


