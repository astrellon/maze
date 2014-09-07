class Map:

    _data = []
    _width = 0
    _height = 0

    def init_map(self, width, height):
        self._width = width
        self._height = height
        self._data = [0 for x in range(height * width)]
        self.create_map()

    def add_border(self):
        x = 0
        top = self.height - 1
        while x < self.width:
            self.set_map(x, 0, 1)
            self.set_map(x, top, 1)
            x = x + 1

        y = 0
        right = self.width - 1
        while y < self.height:
            self.set_map(0, y, 1)
            self.set_map(right, y, 1)
            y = y + 1

    def set_map(self, x, y, tile):
        self.data[y * self.width + x] = tile

    def create_map(self):
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
        return {
            "width": self.width,
            "height": self.height,
            "data": self._data,
            "base_map": self.base_map
        }

    def deserialise(self, input):
        print("Input map data: ", input)

