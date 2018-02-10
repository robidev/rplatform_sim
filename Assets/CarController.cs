using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarController : MonoBehaviour {
	public List<AxleInfo> axleInfos; // the information about each individual axle
	public float maxMotorTorque; // maximum torque the motor can apply to wheel
	public float maxSteeringAngle; // maximum steer angle the wheel can have
	public float myMotor = 0;

	public void FixedUpdate()
	{
	    float motor = (maxMotorTorque * Input.GetAxis("Vertical")) + myMotor;
	    float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
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
}

[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor; // is this wheel attached to motor?
	public bool steering; // does this wheel apply steer angle?
}