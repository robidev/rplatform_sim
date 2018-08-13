using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerController : EventReceiver {

	private string send_sound = "snd:";
	private int MAX_SOUND_SAMPLES = 1;
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
		Debug.Log("Speaker received Event:'" + message + "'");
		if(message == "identify"){
			response = "id:speaker:" + gameObject.name;
			retVal = 0;
		}
		else if(message.StartsWith(send_sound) == true){
			Debug.Log("speaker play sound");
			message = message.Substring(send_sound.Length);
			response = gameObject.name + ":OK=" + message;

			retVal = 0;
		}
		return retVal;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
