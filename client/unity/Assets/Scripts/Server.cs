using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Maze.Service;

public class Server : MonoBehaviour {

    public string Host = "localhost";
    public int Port = 9091;

    private Service Client;
    private int Counter = 0;

    void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 50, 50), "Server!")) {
            Client.Send("derp", "herp-" + Counter++);
        }
    }

	// Use this for initialization
	void Start ()
    {
        Client = new Service(Host, Port);
        Client.Connect(() => {
            Client.Send("join", null, (object result) => {
                Debug.Log("Result from join: " + result);
            });
        });
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnApplicationQuit () 
    {
        Client.Shutdown();
    }

}
