class Map:

    _data = []
    _width = 0
    _height = 0

    def create_map(self, width, height):
        self._width = width
        self._height = height
        self._data = [x[:] for x in [[0]*width]*height]

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
            "data": self._data
        }

    def deserialise(self, input):
        print("Input map data: ", input)

