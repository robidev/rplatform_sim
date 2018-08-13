using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryController : EventReceiver {

	private string charge = "charge";
	// Use this for initialization
	void Start () {
		
	}
	
	public override int parseEvent(string message, ref string response, ref UDPRT MyUdp)
	{
		int retVal = -1;
		int temp  = base.parseEvent(message, ref response, ref MyUdp);//check if message is for us

		if(temp == -1){
			return -1;
		}
		else{//remove header until first .
			message = message.Substring(temp);
		}
		Debug.Log("Battery received Event:'" + message + "'");
		if(message == "identify"){
			response = "id:battery:" + gameObject.name;
			retVal = 0;
		}
		else if(message.StartsWith(charge) == true){
			Debug.Log("read charge ");
			response = gameObject.name + ":60";
			retVal = 0;
		}
		return retVal;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
