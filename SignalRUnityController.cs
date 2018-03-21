/*
This peciece of code implement cross-platform data conntection between Unity and iOS.

I have a Unity game and an iOS mobile application which are both used for indoor 
localization and navigation, so I start to think if I can share the position information 
bewteen two applications. I think share data will be useful to create a user-trainner system
that trainner can track the disabled users' activities in order to provide assistance.

I build the connection use SignalR, this is how I implement is in Unity side.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uSignalR.Hubs;
using uTasks;
using System;
using AssemblyCSharp;

public class SignalRUnityController : MonoBehaviour {
	
	public static SignalRUnityController _instance;

	public bool useSignalR = true;

	public string urlAzure = "http://signalrtoazure20170617053409.azurewebsites.net/";

	private HubConnection _hubConnection = null;

	private IHubProxy _hubProxy;

	private Subscription _subscription;

	public object[] receivedData;

	void Awake(){
		if (_instance == null)
		{
			_instance = this;
		}
		MainThread.Current = new UnityMainThread();
	}

	// Use this for initialization
	void Start () {

		if (useSignalR)
			StartSignalR ();
		
	}

	void StartSignalR()
	{
		if (_hubConnection == null) {
			_hubConnection = new HubConnection (urlAzure);

			_hubProxy = _hubConnection.CreateProxy ("MyHub");

			_subscription = _hubProxy.Subscribe ("broadcastMessage");

			_subscription.Data += data => {
				receivedData = data;
				Debug.Log(data[0].ToString() + ":" + data[1].ToString());
			};
				
			_hubConnection.Start ();

//			_hubProxy.Invoke ("Send", "UnityClient", message);
		} 
		else 
		{
			Debug.Log ("SignalR already connected...");
		}
	}

	public void Send (string method, string message)
	{
		if (!useSignalR)
			return;
		_hubProxy.Invoke ("Send", "UnityClient", message);
	}

	public object[] Received()
	{
		if (receivedData == null) 
		{
			_subscription.Data += data => {
				receivedData = data;
			};
		}
		return receivedData;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
