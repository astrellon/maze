import game.tile

class Map:

    _data = []
    _width = 0
    _height = 0
    _world = None

    def __init__(self, world):
        self._world = world

    def init_map(self, width, height):
        self._width = width
        self._height = height

        tm = self._world.tile_manager
        """
        for x in range(width):
            col = []
            self._data.insert(-1, col)
            for y in range(height):
                col.insert(-1, tm.create_inst("grass"))
                """
        self._data = [None] * (width * height)
        for x in range(width * height):
            inst = tm.create_inst("grass")
            self._data[x] = inst

        self.create_map()

    def add_border(self, type="map_border"):
        x = 0
        top = self.height - 1
        while x < self.width:
            self.set_map(x, 0, type)
            self.set_map(x, top, type)
            x = x + 1

        y = 0
        right = self.width - 1
        while y < self.height:
            self.set_map(0, y, type)
            self.set_map(right, y, type)
            y = y + 1

    def set_map(self, x, y, inst, height = 0.0):
        pos = y * self.width + x
        if isinstance(inst, str):
            self.data[pos] = self._world.tile_manager.create_inst(inst, height)
        else:
            self.data[pos] = inst
            if height is not None:
                inst.height = height


    def create_map(self, name):
        pass

    @property
    def base_map(self):
        print("BM None")
        return "none"

    @property
    def width(self):
        return self._width

    @property
    def height(self):
        return self._height

    @property
    def data(self):
        return self._data

    def serialise(self):
        map_data = [None] * (self.width * self.height)
        for x in range(self.width * self.height):
            map_data[x] = self.data[x].serialise()

        return {
            "width": self.width,
            "height": self.height,
            "data": map_data,
            "base_map": self.base_map
        }

    def deserialise(self, input):
        print("Input map data: ", input)

