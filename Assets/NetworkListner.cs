using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkListner : MonoBehaviour {
	public GameObject player;
	public UDPRT myUDPRT;
	// Use this for initialization
	void Start () {
		myUDPRT = UDPRT.CreateInstance(5000);
		Debug.Log("listening on UDP 5000");
		
	}
	
	// Update is called once per frame
	void Update () {
		if(UDPRT.ReceivedMsg != null){
			Debug.Log("Received udp:" + UDPRT.ReceivedMsg);
			if(UDPRT.ReceivedMsg == "forward"){
				Debug.Log("forward");
				var car = player.GetComponent<CarController>();
				if (car != null)
					car.myMotor = 4;
				else
					Debug.Log("cannot find car");
			}
			if(UDPRT.ReceivedMsg == "back"){
				Debug.Log("forward");
				var car = player.GetComponent<CarController>();
				if (car != null)
					car.myMotor = -4;
				else
					Debug.Log("cannot find car");
			}
			if(UDPRT.ReceivedMsg == "stop"){
				Debug.Log("forward");
				var car = player.GetComponent<CarController>();
				if (car != null)
					car.myMotor = 0;
				else
					Debug.Log("cannot find car");
			}
			UDPRT.ReceivedMsg = null;
		}
	}
	void OnDestroy() {
		Destroy(myUDPRT);
		Debug.Log("NetworkListner was destroyed");
	}
}
