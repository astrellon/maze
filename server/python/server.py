#!/usr/bin/env python3

import threading
import signal
import cmds.command_service
import cmds.command_processor
import game.world
import game.engine

if __name__ == "__main__":
    # Port 0 means to select an arbitrary unused port
    HOST, PORT = "0.0.0.0", 9090

    server = cmds.command_service.TCPServer((HOST, PORT), cmds.command_service.Handler)

    engine = game.engine.Engine()
    engine.setup()

    processor = cmds.command_processor.Processor(engine)
    server.set_engine_processor(engine, processor)
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
