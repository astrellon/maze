#!/usr/bin/env python3

import threading
import signal
import command_service
import command_processor
import world

if __name__ == "__main__":
    # Port 0 means to select an arbitrary unused port
    HOST, PORT = "0.0.0.0", 9090

    server = command_service.TCPServer((HOST, PORT), command_service.Handler)
    game_world = world.World()
    game_world.load_maps()
    game_world.create_map("maps.map1", "testmap")
    game_world.serialise()

    processor = command_processor.Processor(game_world)
    server.set_world_processor(game_world, processor)
    ip, port = server.server_address

    # Start a thread with the server -- that thread will then start one
    # more thread for each request
    server_thread = threading.Thread(target=server.serve_forever)
    # Exit the server thread when the main thread terminates
    server_thread.daemon = True
    server_thread.start()
    print("Starting server on: " + ip + ":" + str(port) + ", welcome to " + game_world.name)

    def signal_handler(signal, frame):
        print("Shutting down server.")
        server.shutdown()

    signal.signal(signal.SIGINT, signal_handler)

    server_thread.join()
