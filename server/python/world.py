import importlib
import glob
import re

class World:

    name = "Maze"

    _maps = {}

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
            if mod is not None:
                mod.__dict__["setup"](self)

    def add_map(self, name, map):
        print("Added map: ", name, map)


