using UnityEngine;
using System;
using System.Collections.Generic;

namespace Maze.Service
{
    public class CommandProcessor
    {
        public Service OwnerService { get; protected set; }

        protected Dictionary<string, CommandHandler> Handlers = new Dictionary<string, CommandHandler>();
        public CommandProcessor(Service service)
        {
            OwnerService = service;
        }

        public void AddHandler(string command, CommandHandler handler)
        {
            Handlers[command] = handler;
        }

        public void ProcessData(Dictionary<string, object> data)
        {
            if (!data.ContainsKey("cmd"))
            {
                Debug.Log("Cannot process command without cmd");
                return;
            }
            string cmd = data["cmd"]; 

            if (!Handlers.ContainsKey(cmd))
            {
                Debug.Log("Unknown local command '" + cmd + "'");
                return;
            }

            object result = Handlers[cmd](data);
        }
    }
}
