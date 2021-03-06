﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarController : EventReceiver {
	public List<AxleInfo> axleInfos; // the information about each individual axle
	public float maxMotorTorque; // maximum torque the motor can apply to wheel
	public float maxSteeringAngle; // maximum steer angle the wheel can have
	public float myMotor = 0;
        private float mySteering = 0;
        private string steerangle = "steerangle:";

    	void OnTriggerEnter() {
        	var gpio = GameObject.Find("gpio").GetComponent<GPIOController>();
		gpio.setGPIO(3,1);
    	}

    	void OnTriggerExit() {
        	var gpio = GameObject.Find("gpio").GetComponent<GPIOController>();
		gpio.setGPIO(3,0);
    	}

	public void FixedUpdate()
	{
	    float motor = (maxMotorTorque * Input.GetAxis("Vertical")) + myMotor;
	    float steering = maxSteeringAngle * Input.GetAxis("Horizontal") + mySteering;
	    //Debug.Log("steeringval:" + steering.ToString());

	    foreach (AxleInfo axleInfo in axleInfos) {
		/*if (axleInfo.steering) {
		    axleInfo.leftWheel.steerAngle = steering;
		    axleInfo.rightWheel.steerAngle = steering;
		}*/
		if (axleInfo.motor) {
		    axleInfo.leftWheel.motorTorque = motor + steering;
		    axleInfo.rightWheel.motorTorque = motor - steering;
		}
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
		Debug.Log("CarController received Event:'" + message + "'");
		if(message == "identify"){
			response = "id:car:" + gameObject.name; // + ":forward,back,stop";
			retVal = 0;
		}
		else if(message == "forward"){
			Debug.Log("forward");
			myMotor = 4;
			retVal = 0;
			response = gameObject.name + ":OK";
		}
		else if(message == "back"){
			Debug.Log("back");
			myMotor = -4;
			retVal = 0;
			response = gameObject.name + ":OK";
		}
		else if(message == "stop"){
			Debug.Log("stop");
			myMotor = 0;
			retVal = 0;
			response = gameObject.name + ":OK";
		}
                else if(message.StartsWith(steerangle) == true){
			Debug.Log("steerangle");
			message = message.Substring(steerangle.Length);
			
			int x = 0;
			if (int.TryParse(message, out x))
			{
				mySteering = (float)x;
			}

			response = gameObject.name + ":OK";
			retVal = 0;
		}
		return retVal;
	}
}

[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor; // is this wheel attached to motor?
	public bool steering; // does this wheel apply steer angle?
}