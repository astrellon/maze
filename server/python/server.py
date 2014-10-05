#!/usr/bin/env python3

import game.engine
import argparse

if __name__ == "__main__":

    parser = argparse.ArgumentParser(description='Test Maze Game Server.')
    parser.add_argument('-p', '--port', nargs='?', type=int, default=9090, help='Port to start the server on. Defaults to %(default)s.')

    args = parser.parse_args()
    
    engine = game.engine.Engine()
    engine.port = args.port
    engine.setup()
    engine.start_server_thread()
    engine.start_main_loop()

