#!/usr/bin/env python3

import game.engine

if __name__ == "__main__":

    engine = game.engine.Engine()
    engine.setup()
    engine.start_server_thread()
    engine.start_main_loop()

