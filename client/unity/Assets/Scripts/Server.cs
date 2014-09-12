using UnityEngine;
using System;
using System.Collections;

public class Server : MonoBehaviour {

    private maze.service.Client Client;
    private int Counter = 0;

    void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 50, 50), "Server!")) {
            Client.SendString("{\"derp\": \"herp-" + Counter++ + "\"}");
        }
    }

	// Use this for initialization
	void Start ()
    {
        Client = new maze.service.Client("localhost", 9090);
        Client.Connect();
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
