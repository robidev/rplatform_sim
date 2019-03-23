using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServoController : EventReceiver {

	private string servo = "servo:";
	private float targetAngle = 0;
	private float deadzone = 2;
	private float speed = 0.9f;
	/*private HingeJoint hinge;
	private JointMotor motor;*/
	public Transform ServoArm; 

	public void Start()
	{
		/*hinge = GetComponent<HingeJoint>();
		motor = hinge.motor;
		hinge.useMotor = true;
		motor.targetVelocity = 0;*/
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
		Debug.Log("ServoController received Event:'" + message + "'");
		if(message == "identify"){
			response = "id:servo:" + gameObject.name;// + ":servo";
			retVal = 0;
		}
		else if(message.StartsWith(servo) == true){
			Debug.Log("servo");
			message = message.Substring(servo.Length);
			
			int x = 0;
			if (int.TryParse(message, out x))
			{
				targetAngle = (float)x;
			}

			response = gameObject.name + ":OK";
			retVal = 0;
		}
		return retVal;
	}

	public void Update()
	{
		Debug.Log(ServoArm.localEulerAngles.x);
		if(targetAngle > ServoArm.localEulerAngles.x + deadzone || targetAngle < ServoArm.localEulerAngles.x - deadzone)
		{
			if(targetAngle > ServoArm.localEulerAngles.x)
				ServoArm.Rotate (speed,0,0);
			else
				ServoArm.Rotate (-speed,0,0);
		}
		/*if (targetAngle < hinge.angle - deadzone || targetAngle > hinge.angle + deadzone)
		{
			if (targetAngle > hinge.angle)
				motor.targetVelocity = 10;
			else
				motor.targetVelocity = -10;

			hinge.motor = motor;
			hinge.useMotor = true;
		}
		else
		{
			motor.targetVelocity = 0;
			hinge.motor = motor;
			hinge.useMotor = true;
		}*/
	}
}
