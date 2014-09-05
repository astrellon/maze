using UnityEngine;
using System;
using System.Collections;

public class Server : MonoBehaviour {

    private MazeClient Client;
    private int Counter = 0;

    void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 50, 50), "Server!")) {
            Client.Send("{\"derp\": \"herp-" + Counter++ + "\"}");
        }
    }

	// Use this for initialization
	void Start () {
        Client = new MazeClient("localhost", 9090);
        Client.Connect();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
