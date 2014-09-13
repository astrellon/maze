using UnityEngine;
using System;
using System.Collections;
using Maze.Service;

public class Server : MonoBehaviour {

    private Serivce Client;
    private int Counter = 0;

    void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 50, 50), "Server!")) {
            Client.SendString("{\"derp\": \"herp-" + Counter++ + "\"}");
        }
    }

	// Use this for initialization
	void Start ()
    {
        Client = new Service("localhost", 9090);
        Client.Connect(() => {
            Dictionary<string, object> join = new Dictionary<string, object>() {
                {"cmd", "join"}
            };
            Client.SendData(join, (object result) => {
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
