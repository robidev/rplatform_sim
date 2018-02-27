using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReceiver : MonoBehaviour {
	
	private string all = "all.";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual int parseEvent(string message, ref string response)
	{
		int retVal = -1;
		if(message.StartsWith(gameObject.name + ".")){
			retVal = gameObject.name.Length + 1;
		}
		else if(message.StartsWith(all)){
			retVal = all.Length;
		}
		return retVal;
	}
}
