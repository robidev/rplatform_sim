using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayController : EventReceiver {

	private string setimage = "setimage:";

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
		Debug.Log("DisplayController received Event:'" + message + "'");
		if(message == "identify"){
			response = "id:display:" + gameObject.name; // + ":setimage";
			retVal = 0;
		}
		else if(message.StartsWith(setimage) == true){
			Debug.Log("setimage");
			message = message.Substring(setimage.Length);
			//decode message (gzip compressed data)
			//set newTex to received image data
			//set gameObject.GetComponent<Renderer>().material.mainTexture = newTex;
			response = gameObject.name + "OK";
			retVal = 0;
		}
		return retVal;
	}
}
