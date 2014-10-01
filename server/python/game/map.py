class Map:

    def __init__(self, world):
        self._world = world
        self._data = []
        self._width = 0
        self._height = 0

    def init_map(self, width, height, type="grass", tile_height=0.0):
        self._width = width
        self._height = height

        tm = self._world.tile_manager
        
        self._data = [None] * (width * height)
        for x in range(width * height):
            inst = tm.create_inst(type, tile_height)
            self._data[x] = inst

        self.create_map()

    def draw_rect(self, left, bottom, right, top, type, height=None):
        left = max(left, 0)
        bottom = max(bottom, 0)
        right = min(right, self.width - 1)
        top = min(top, self.height - 1)

        x = left
        while x <= right:
            self.set_map(x, 0, type, height)
            self.set_map(x, top, type, height)
            x = x + 1

        y = bottom 
        while y <= top:
            self.set_map(0, y, type, height)
            self.set_map(right, y, type, height)
            y = y + 1

    def set_map(self, x, y, inst, height=None):
        pos = y * self.width + x
        if isinstance(inst, str):
            self.data[pos] = self._world.tile_manager.create_inst(inst, height)
        else:
            self.data[pos] = inst
            if height is not None:
                inst.height = height

    def set_height(self, x, y, heightBL, heightBR=None, heightTL=None, heightTR=None):
        pos = y * self.width + x
        inst = self.data[pos]
        if heightBR is None:
            inst.set_heights(heightBL)
        else:
            inst.heightBL = heightBL
            inst.heightBR = heightBR
            inst.heightTL = heightTL
            inst.heightTR = heightTR

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

