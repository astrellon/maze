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
    public class Service : ISendService
    {
        public event ConnectedCallback OnConnect;

        public string Host { get; protected set; }
        public int Port { get; protected set; }
        public bool Running = true;

        public bool Connected { get; protected set; }

        protected Socket Client;
        protected byte[] ReceiveBuffer = new byte[1024];

        protected Thread ReceiveThread;

        protected Dictionary<int, ResponseCallback> ResponseCallbacks = new Dictionary<int, ResponseCallback>();
        protected int callbackCounter = 0;
        protected int GetNextCallback()
        {
            return ++callbackCounter;
        }

        public Service(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public void Connect(ConnectedCallback callback = null)
        {
            if (callback != null)
            {
                OnConnect += new ConnectedCallback(callback);
            }

            try
            {
                IPHostEntry ipHostInfo = Dns.Resolve(Host);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, Port);

                Client = new Socket(AddressFamily.InterNetwork, 
                        SocketType.Stream, ProtocolType.Tcp);

                Client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), Client);
            }
            catch (Exception e)
            {
                Debug.Log("Error connecting to server: " + e.ToString());
            }
        }

        protected void ConnectCallback(IAsyncResult result) 
        {
            try
            {
                Client.EndConnect(result);

                Debug.Log("Connected to server: " + Client.RemoteEndPoint.ToString());

                Connected = true;

                if (OnConnect != null)
                {
                    OnConnect();
                }

                ReceiveThread = new Thread(Receive);
                ReceiveThread.Start();

            }
            catch (Exception e)
            {
                Debug.Log("Error in connection callback: " + e.ToString());
            }
        }

        public void Shutdown()
        {
            Running = false;
            Client.Shutdown(SocketShutdown.Both);
            Client.Close();        
        }

        public void SendData(object data, ResponseCallback callback = null)
        {
            Debug.Log("Sending data");
            SendString(Json.Serialize(data), callback);
        }
        public void SendString(string data, ResponseCallback callback = null)
        {
            Debug.Log("About to send string: " + data);
            StringBuilder builder = new StringBuilder();
            if (callback == null)
            {
                builder.Append("{\"data\":");
                builder.Append(data);
                builder.Append('}');
            }
            else
            {
                int id = GetNextCallback();
                ResponseCallbacks[id] = callback;
                builder.Append("{\"id\":");
                builder.Append(id);
                builder.Append(", \"data\":");
                builder.Append(data);
                builder.Append('}');
            }
            string toSend = builder.ToString();
            Debug.Log("Sending data: " + toSend);
            byte[] bytes = Encoding.ASCII.GetBytes(toSend);

            Client.BeginSend(bytes, 0, bytes.Length, 0, new AsyncCallback(SendCallback), Client);
        }

        protected void SendCallback(IAsyncResult result)
        {
            try
            {
                int bytesSent = Client.EndSend(result);
                Debug.Log("Bytes sent: " + bytesSent);
            }
            catch (Exception e)
            {
                Debug.Log("Error in send callback: " + e.ToString());
            }
        }

        protected void Receive()
        {
            while (Running)
            {
                try
                {
                    Debug.Log("Waiting for data");
                    int bytesRead = Client.Receive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None);
                    if (bytesRead == 0)
                    {
                        Debug.Log("Connection lost");
                        Running = false;
                        break;
                    }
                    Debug.Log("Bytes received: " + bytesRead);

                    string receivedString = Encoding.UTF8.GetString(ReceiveBuffer, 0, bytesRead);
                    Debug.Log("String received: " + receivedString);
                    object receivedObject = Json.Deserialize(receivedString);
                    Dictionary<string, object> receivedData = receivedObject as Dictionary<string, object>;
                    if (receivedData.ContainsKey("rid"))
                    {
                        int rid = Convert.ToInt32(receivedData["rid"]);
                        if (ResponseCallbacks.ContainsKey(rid))
                        {
                            ResponseCallbacks[rid](receivedData);
                        }
                    }

                }
                catch (Exception e)
                {
                    Debug.Log("Error receiving data: " + e.ToString());
                    Running = false;
                }
            }
        }
    }
}
