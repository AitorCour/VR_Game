using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo   //Options for each axle.
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; 			// is this wheel attached to motor?
    public bool steering; 		// does this wheel apply steer angle?
    public bool brake;          // does this wheel has brakes?
}

public class NewCarController : MonoBehaviour
{
    public HingeJoint wheel;            // Put here the physical wheel.
    private float maxTurnAngle = 180;   // Max wheel turn angle
    private Rigidbody rigidbody;

    public List<AxleInfo> axleInfos; 	// the information about each individual axle
    public float maxMotorTorque; 		// maximum torque the motor can apply to wheel
    public float maxBrakesTorque;       // maximum torque to brake the wheels. Must be positive.
    public float maxSteeringAngle; 		// maximum steer angle the wheel can have

    public float maxSpeed;
    public float speed;

    private float motor = 0f;
    private float brake = 0f;
    private float steeringPos = 0f;
    // finds the corresponding visual wheel
    // correctly applies the transform
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        speed = rigidbody.velocity.sqrMagnitude;
        if (speed < maxSpeed)
        {
            motor = maxMotorTorque * Input.GetAxis("XRI_Right_Trigger");	//Car accelerates with torque amount. We can consider torque as acceleration.
            //If we see that accelerate and manage the rest is complicated, we maybe can do the velocity fixed, so player can select which speed wants to be, like with a manual change but it set the speed
        }//Right now this will make that the input will be ignored if speed is superior, and then take in account, and thus. May correct in the future
        if (speed > 0)
        {
            brake = maxBrakesTorque * Input.GetAxis("XRI_Left_Trigger");  //If speed less than 0, won't brake.
        }
        /*if(wheel.angle > 10)    //dead
        {
            steeringPos = Mathf.Clamp(wheel.angle / maxTurnAngle, 0, 1);   // Clamp between 0 and 1;
        }
        else if(wheel.angle < -10)   //dead
        {
            steeringPos = Mathf.Clamp(wheel.angle / maxTurnAngle, -1, 0);   // Clamp between -1 and 0;
        }
        else steeringPos = 0;*/
        steeringPos = Mathf.Clamp(wheel.angle / maxTurnAngle, -1, 1);
        float steering = maxSteeringAngle * steeringPos;                      // Set the steer with the max steer angle of the wheels

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            if (axleInfo.brake)
            {
                axleInfo.leftWheel.brakeTorque = brake;
                axleInfo.rightWheel.brakeTorque = brake;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
}
