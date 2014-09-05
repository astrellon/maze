using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Threading;

using UnityEngine;

public class MazeClient 
{
    public string Host { get; protected set; }
    public int Port { get; protected set; }
    public bool Running = true;

    protected Socket Client;
    protected byte[] ReceiveBuffer = new byte[1024];
    protected string ReceivedData = "";

    protected Thread ReceiveThread;
    
    public MazeClient(string host, int port)
    {
        Host = host;
        Port = port;
    }

    public void Connect()
    {
        try
        {
            IPHostEntry ipHostInfo = Dns.Resolve(Host);
            //IPHostEntry ipHostInfo = GetHostEntry(Host);
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

            Send("{\"cmd\": \"join\", \"id\": 1}");

            ReceiveThread = new Thread(Receive);
            ReceiveThread.Start();

        }
        catch (Exception e)
        {
            Debug.Log("Error in connection callback: " + e.ToString());
        }
    }

    public void Send(string data)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(data);

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

                ReceivedData = Encoding.ASCII.GetString(ReceiveBuffer, 0, bytesRead);
                Debug.Log("Received data: " + ReceivedData);
            }
            catch (Exception e)
            {
                Debug.Log("Error receiving data: " + e.ToString());
            }
        }
    }
    
}
