using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkListner : MonoBehaviour {
	//public GameObject player;
	public List<GameObject> gameObjectList;
	public UDPRT myUDPRT = null;
	// Use this for initialization
	void Start () {
		myUDPRT = new UDPRT(5000);
		Debug.Log("listening on UDP 5000");
		
	}
	
	// Update is called once per frame
	void Update () {
		if(myUDPRT.ReceivedMsg != null){
			Debug.Log("Received udp packet:'" + myUDPRT.ReceivedMsg.Length + "'");
			if(HandleMessage(myUDPRT.ReceivedMsg) == -1){
				Debug.Log("Could not parse message:" + myUDPRT.ReceivedMsg);
			}
			myUDPRT.ReceivedMsg = null;
		}
	}
	void OnDestroy() {
		myUDPRT.OnDestroy();
		if (myUDPRT != null)
			myUDPRT = null;
		Debug.Log("NetworkListner was destroyed");
	}

	int HandleMessage(string message)
	{
		int retVal = -1;
		foreach(GameObject gameObject in gameObjectList )
		{
			var eventReceiver = gameObject.GetComponent<EventReceiver>();
			if(eventReceiver != null){
				string response = "";
				int temp = eventReceiver.parseEvent(message, ref response, ref myUDPRT);
				if(temp != -1){
					Debug.Log(myUDPRT.IP.Address.ToString() + " msg:'" + response + "'");
					UDPRT SendUDP = new UDPRT(5001, myUDPRT.IP.Address.ToString(), response);
					if (!SendUDP.messageSent){;}
					retVal++; // increment each time we have a succesful parse
				}
			}
			else{
				Debug.Log("Could not find EventReceiver for '" + gameObject.name + "'");
			}

		}
                if(message == "all.identify"){
                	UDPRT SendUDP_resp = new UDPRT(5001, myUDPRT.IP.Address.ToString(), "identify:done");
		}
		return retVal;
	}
}
