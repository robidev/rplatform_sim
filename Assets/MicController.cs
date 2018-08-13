using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicController : EventReceiver  {

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
		Debug.Log("Mic received Event:'" + message + "'");
		if(message == "identify"){
			response = "id:mic:" + gameObject.name;
			retVal = 0;
		}
		//stub
		return retVal;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
