using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPIOController : EventReceiver  {

	private string readinput = "get:";
	private string writeoutput = "set:";
	private static uint MAX_GPIO = 4;
	private int[] GPIO = new int[MAX_GPIO];
	public GameObject[] GPIOObjectList = new GameObject[MAX_GPIO];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < MAX_GPIO; i++)
		{
			GPIO[i] = 0;
		}
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
		Debug.Log("GPIO received Event:'" + message + "'");
		if(message == "identify"){
			response = "id:gpio:" + gameObject.name;// + ":servo";
			retVal = 0;
		}
		else if(message.StartsWith(readinput) == true){
			Debug.Log("gpio read input");
			message = message.Substring(readinput.Length);
			
			int x = 0;
			if (int.TryParse(message, out x))
			{
				if(x >=0 && x < MAX_GPIO)
					response = gameObject.name + ":GPIO" + x + "=" + GPIO[x];
				else
					response = gameObject.name + " - error reading input: cannot find gpio input";
			}
			else
				response = gameObject.name + " - error reading input: cannot parse int";
			retVal = 0;
		}
		else if(message.StartsWith(writeoutput) == true){
			Debug.Log("gpio write output");
			message = message.Substring(writeoutput.Length);
			
			string[] values = message.Split('=');

			int x = 0, y=0;
			if (int.TryParse(values[0], out x) && int.TryParse(values[1], out y))
			{
				if(x >=0 && x < MAX_GPIO)
				{
					GPIO[x] = y;
					UpdateGPIO(x,y);
					response = gameObject.name + ":OK " + x + "=" + y;
				}
				else
					response = gameObject.name + " - error reading input: cannot find gpio output";
			}
			else
				response = gameObject.name + " - error reading input: cannot parse int";
			retVal = 0;
		}
		return retVal;
	}

	// Update is called once per frame
	void Update () {
	}

	void UpdateGPIO(int x, int y)
	{
		GameObject _gpio = GPIOObjectList[x];
		if(_gpio != null)
		{
			SpriteRenderer m_SpriteRenderer = _gpio.GetComponent<SpriteRenderer>();
			if(m_SpriteRenderer != null)
			{
				if(y==0)
					m_SpriteRenderer.color = Color.black;
				else
					m_SpriteRenderer.color = Color.red;
			}
		}
	}
}
