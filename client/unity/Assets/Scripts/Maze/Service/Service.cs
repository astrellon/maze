using System;
using System.IO;
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
    public class Service
    {
        public event ConnectedCallback OnConnect;
        public event ResponseCallback OnData;

        public string Host { get; protected set; }
        public int Port { get; protected set; }
        public bool Running = true;

        public bool Connected { get; protected set; }

        protected Socket Client;
        // Buffer is 10kb, should be enough for most things to work in one go.
        protected byte[] ReceiveBuffer = new byte[1048576];
        // The total buffer, as in this will keep acculating while
        // it fails to be parsed as JSON. Might need some kind of protection against
        // this continusing to acculate out of control if something goes wrong.
        // But for now this should mean that we can keep receiving incomplete data from the server
        // until it's all here and then process it.
        protected StringBuilder ReceivedBufferTotal = new StringBuilder();

        protected Thread ReceiveThread;

        protected Dictionary<int, ResponseCallback> ResponseCallbacks = new Dictionary<int, ResponseCallback>();
        protected int callbackCounter = 0;
        protected int GetNextCallback()
        {
            return ++callbackCounter;
        }

        public CommandProcessor LocalCommandProcessor { get; protected set; }

        protected StreamWriter LogFile;

        public Service(string host, int port)
        {
            Host = host;
            Port = port;

            LocalCommandProcessor = new CommandProcessor();

            LogFile = new StreamWriter(@"./log.txt", true);
            LogFile.WriteLine("\n- New Session\n-----------\n");
        }

        public void Connect(ConnectedCallback callback = null)
        {
            if (callback != null)
            {
                OnConnect += new ConnectedCallback(callback);
            }

            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Host);
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
            LogFile.Close();
        }

        public void Send(string cmd, object data, ResponseCallback callback = null)
        {
            Dictionary<string, object> output = new Dictionary<string, object>();
            output["data"] = data;
            output["cmd"] = cmd;
            if (callback != null)
            {
                int id = GetNextCallback();
                ResponseCallbacks[id] = callback;
                output["id"] = id;
            }

            string toSend = Json.Serialize(output);
            Debug.Log("Sending data: " + toSend);
            LogFile.WriteLine("Sending: " + toSend);
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
                    LogFile.WriteLine("Received: " + receivedString);
                    if (OnData != null)
                    {
                        OnData(null, receivedString);
                    }
                    ReceivedBufferTotal.Append(receivedString);
                    LogFile.WriteLine("- Received total: " + ReceivedBufferTotal.ToString());
                    try
                    {
                        object receivedObject = Json.Deserialize(ReceivedBufferTotal.ToString());
                        Dictionary<string, object> receivedHash = receivedObject as Dictionary<string, object>;
                        if (receivedHash.ContainsKey("cmd"))
                        {
                            LocalCommand cmd = new LocalCommand(receivedHash);
                            Debug.Log("Local command!: " + receivedHash["cmd"]);
                            ReceivedBufferTotal = new StringBuilder();
                        }
                        else
                        {
                            // Change to something that supports responses and local commands!
                            Response resp = new Response(receivedHash);
                            ReceivedBufferTotal = new StringBuilder();

                            Debug.Log("Response ID for data: " + resp.ResponseId);
                            if (resp.ResponseId > 0)
                            {
                                ResponseCallbacks[resp.ResponseId](resp, receivedObject);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Exception parsing repsonse: " + e.ToString());
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
