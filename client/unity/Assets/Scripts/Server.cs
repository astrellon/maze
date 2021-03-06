﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Maze.Service;
using Maze.Game;
using MiniJSON;

public class Server : MonoBehaviour {

    //TODO
    //Add base class for MonoBehaviours that accept command responses and queues them
    //up until the next Update tick comes around

    public string Host = "localhost";
    public int Port = 9091;
    public MapRenderer MainMap;

    private Service Client;
    private int Counter = 0;
    private string log = "";

    public string ServerName { get; protected set; }
    public string ServerDescription { get; protected set; }
    public string ServerVersion{ get; protected set; }
    public string ServerWorldName { get; protected set; }

    public World ServerWorld { get; protected set; }
    private bool updateMapRenderer = false;

    public GUIText Logger = null;

    void OnGUI() {
        /*
        if (GUI.Button(new Rect(10, 10, 50, 50), "Server!")) {
            Client.Send("derp", "herp-" + Counter++);
        }
        */
    }

	// Use this for initialization
	void Start ()
    {
        Client = new Service(Host, Port);
        Client.OnData += new ResponseCallback((Response resp, object data) => {
                string dataStr = data.ToString();
                log += "\n" + dataStr.Substring(0, Math.Min(dataStr.Length, 128));
        });
        Client.Connect(() => {
            Client.Send("server_info", null, (Response resp, object result) => {
                if (resp.IsError)
                {
                    Debug.Log("Error getting server info");
                }
                else
                {
                    Debug.Log("SERVER INFO!");
                    foreach (KeyValuePair<string, object> pair in resp.HashResult)
                    {
                        string v = "Null";
                        if (pair.Value != null)
                        {
                            v = pair.Value.ToString();
                        }
                        Debug.Log("Info resp " + pair.Key.ToString() + ": " + v);
                    }
                    ServerName = resp.GrabValue<string>("name", null);
                    ServerDescription = resp.GrabValue<string>("description", null);
                    ServerVersion = resp.GrabValue<string>("version", null);
                    ServerWorldName = resp.GrabValue<string>("current_world", null);
                    Debug.Log("Server info: " + ServerName + " | " + ServerWorldName);
                    
                    Client.Send("join_server", new Hashtable() {
                        { "name", "Crazy name" }
                    }, (Response respJoin, object resultJoin) => {
                        if (respJoin.IsError)
                        {
                            Debug.Log("Error joining server: " + respJoin.ErrorMessage);
                        }
                        else
                        {
                            if (ServerWorldName == null)
                            {
                                CreateWorld();
                            }
                            else
                            {
                                JoinWorld();
                            }
                        }
                    });
                }
            });
        });
	}
    
    protected void CreateWorld()
    {
        Client.Send("create_world", null, (Response resp, object result) => {
            Debug.Log("Created world");

            if (resp.IsError)
            {
                Debug.Log("Error creating world.");
            }
            else
            {
                JoinWorld();
            }
        });
    }

    protected void JoinWorld()
    {
        Client.Send("join_world", new Hashtable() {
            { "name", "Whut" }
        }, (Response resp, object result) => {
            if (resp.IsError)
            {
                Debug.Log("Error joining world");
                return;
            }
            Debug.Log("Joined world");
            ServerWorld = new World();
            ServerWorld.Deserialise(resp.HashResult);
            updateMapRenderer = true;
        });
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Logger != null && log.Length > 0)
        {
            Logger.text += log;
            log = "";
        }
        if (MainMap != null && updateMapRenderer)
        {
            updateMapRenderer = false;
            MainMap.CurrentMap = ServerWorld.FirstMap;
            MainMap.RenderMap();
        }
	}

    void OnApplicationQuit () 
    {
        Client.Shutdown();
    }

}
