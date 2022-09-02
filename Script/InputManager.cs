using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //private DriftCarTest car_1;
    private Car_1 car_1;
    private Transform wheel;
    private HingeJoint gearShift;
    private float maxTurnAngle = 180;   // Turn angle of the steering wheel

    public float steeringPos = 0;
    public float angle = 0;
    public bool isNegative = false;
    public bool canRotate;
    private bool canStart;
    private bool advance;
    private bool back;

    // Start is called before the first frame update
    void Start()
    {
        canStart = false;
        //Application.targetFrameRate = 60;
        //car_1 = GameObject.FindGameObjectWithTag("myCar").GetComponent<DriftCarTest>();
        car_1 = GameObject.FindGameObjectWithTag("Player").GetComponent<Car_1>();
        wheel = GameObject.FindGameObjectWithTag("Steering").GetComponent<Transform>();
        gearShift = GameObject.FindGameObjectWithTag("GearShift").GetComponent<HingeJoint>();
        //wheel.Rotate(30, 0, 0);
        canRotate = false;
        steeringPos = 0;
        angle = 0;
        advance = false;
        back = false;
        //StartCoroutine(WaitForSeconds(3f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //while(!canStart) return;

        if (/*Input.GetAxis("XRI_Right_Trigger") > 0*/ advance)
        {
            car_1.MoveForward();
            Debug.Log("Pressed Forward");
        }
        else if (/*Input.GetAxis("XRI_Left_Trigger") > 0*/back)
        {
            car_1.MoveBackward();
            Debug.Log("Pressed Backward");
        }
        else car_1.Stop();

        angle = wheel.eulerAngles.z;
        
        if(angle > 180 && angle < 360 || angle < 0 && angle > -360)
        {
            isNegative = true;
            canRotate = true;
            Debug.Log("Negative");
        }
        else if (angle < 180f && angle > 0f)
        {
            isNegative = false;
            canRotate = true;
            Debug.Log("Positive");
        } 
        else {
            canRotate = false;
        }
        if (canRotate)
        {
            if(isNegative)
            {
                angle -= 360;
                steeringPos = angle / maxTurnAngle;
            }
            else
            {
                steeringPos = angle / maxTurnAngle;
            }
        }
        else steeringPos = 0;
        //steeringPos = angle / maxTurnAngle;
        if(steeringPos < 0.1f && steeringPos > -0.1f || !canRotate || steeringPos > 1 || steeringPos < -1)
        {
            steeringPos = 0;
        }
        else car_1.RotateCar(-steeringPos);


        if(gearShift.angle >= 20)
        {
            JointSpring hingeSpring = gearShift.spring;
            hingeSpring.targetPosition = 40f;
            gearShift.spring = hingeSpring;
            advance = true;
            back = false;
        }
        else if(gearShift.angle <= -20)
        {
            JointSpring hingeSpring = gearShift.spring;
            hingeSpring.targetPosition = -40f;
            gearShift.spring = hingeSpring;
            advance = false;
            back = true;
        }
        else
        {
            JointSpring hingeSpring = gearShift.spring;
            hingeSpring.targetPosition = 0f;
            gearShift.spring = hingeSpring;
            advance = false;
            back = false;
        }
    }
    IEnumerator WaitForSeconds(float wait)
    {
        yield return new WaitForSeconds(wait);
        canStart = true;
    }
}
