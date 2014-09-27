import importlib
import glob
import re
import os
import game.tiles

class World:

    def __init__(self, engine):
        self._engine = engine
        self._tile_manager = game.tile.TileManager(self)

        self.name = "Maze"

        self._maps = {}
        self._base_maps = {}
        self._players = []
        self._game_objects = {}

    @property
    def tile_manager(self):
        return self._tile_manager

    @property
    def maps(self):
        return _maps

    def setup(self):
        self.load_maps()
        game.tiles.create_default_tiles(self._tile_manager)

    def load_maps(self):
        files = glob.glob("maps/*.py")

        for file in files:
            if file == "maps/__init__.py":
                continue

            print("Attepmting to load map: ", file)
            mod_file = re.sub(".py$", "", file.replace("/", "."))
            print("Attempt to load mod: ", mod_file)
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

    def add_player(self, user):
        player = game.PlayerObject(self._engine, user)
        self._players.append(player)
        self.add_game_object(player)
        return player

    def add_game_object(self, obj):
        self._game_objects[obj.id] = obj
        self._engine.server.broadcast(obj.create_for_network())
        return obj

    def serialise(self):
        world = {}
        maps = {}
        players = []
        world["maps"] = maps
        world["players"] = players

        for map_name in self._maps.keys():
            maps[map_name] = self._maps[map_name].serialise()

        for player in self._players:
            players.append(player.serialise())

        return world

