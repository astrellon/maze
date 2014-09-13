using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using MiniJSON;

using UnityEngine;

namespace Maze.Service
{
    public class Client : ISendService
    {
        public maze.service.Service Service { get; protected set; }    

        public Client(string host, int port)
        {
            Service = new Service(host, port);
        }

        public void Connect()
        {
            Service.Connect(() => {
                Dictionary<string, object> join = new Dictionary<string, object>() {
                    {"cmd", "join"}
                };
                SendData(join, (object result) => {
                    Debug.Log("Result from join: " + result);
                });
            });
        }

        public void SendData(object data, ResponseCallback callback = null)
        {
            Service.SendData(data, callback);
        }
        public void SendString(string data, ResponseCallback callback = null)
        {
            Service.SendString(data, callback);
        }

        public void Shutdown()
        {
            Service.Shutdown();
        }
    }
}
