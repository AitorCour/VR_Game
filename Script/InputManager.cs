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
    private bool move;
    private int gear;

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
        move = false;
        SetGear(-10f, false, 0);
        //StartCoroutine(WaitForSeconds(3f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //while(!canStart) return;

        if (Input.GetButton("Jump") || move)
        {
            car_1.Move(gear);
        }
        else car_1.Stop();
        if (Input.GetButton("Jump"))
        {
            car_1.Move(-1);
        }
        angle = wheel.eulerAngles.z;
        
        if(angle > 180 && angle < 360 || angle < 0 && angle > -360)
        {
            isNegative = true;
            canRotate = true;
            //Debug.Log("Negative");
        }
        else if (angle < 180f && angle > 0f)
        {
            isNegative = false;
            canRotate = true;
            //Debug.Log("Positive");
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

        //The limits should have a margin. If not, the gear will bounce and change automatically.
        if(gearShift.angle >= 10 && gearShift.angle < 50)
        {
            SetGear(25f, true, 1);          //Slow - 1
        }
        else if(gearShift.angle >= 50)      //Quick - 2
        {
            SetGear(60f, true, 2);
        }
        else if(gearShift.angle <= -25)     //Rear
        {
            SetGear(-45f, true, -1);
        }
        else                                //Dead
        {
            SetGear(-10f, false, 0);
        }
    }
    IEnumerator WaitForSeconds(float wait)
    {
        yield return new WaitForSeconds(wait);
        canStart = true;
    }
    private void SetGear(float position, bool willMove, int newGear)
    {
        JointSpring hingeSpring = gearShift.spring;
        hingeSpring.targetPosition = position;
        gearShift.spring = hingeSpring;
        move = willMove;
        gear = newGear;
    }
}
