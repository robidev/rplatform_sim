using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepperController : EventReceiver {

	private string set_ = "set:";
	private string steps = "steps:";

	public override int parseEvent(string message, ref string response)
	{
		int retVal = -1;
		int temp  = base.parseEvent(message, ref response);//check if message is for us

		if(temp == -1){
			return -1;
		}
		else{//remove header until first .
			message = message.Substring(temp);
		}
		Debug.Log("StepperController received Event:'" + message + "'");
		if(message == "identify"){
			response = "stepper:" + gameObject.name; // + ":stepper";
			retVal = 0;
		}
		else if(message.StartsWith(set_) == true){
			Debug.Log("set");
			message = message.Substring(set_.Length);
			//
			
			response = "OK";
			retVal = 0;
		}
		else if(message.StartsWith(steps) == true){
			Debug.Log("steps");
			message = message.Substring(steps.Length);
			//
			
			response = "OK";
			retVal = 0;
		}
		return retVal;
	}
}
