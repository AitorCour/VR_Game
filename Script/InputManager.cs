using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //private DriftCarTest car_1;
    private Car_1 car_1;
    public HingeJoint wheel;
    private float maxTurnAngle = 180;   // Turn angle of the steering wheel
    
    public float turnThreshold = 0.2f;
    public float maxValue = 0.35f;
    public float minValue = -0.35f;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        //car_1 = GameObject.FindGameObjectWithTag("myCar").GetComponent<DriftCarTest>();
        car_1 = GameObject.FindGameObjectWithTag("Player").GetComponent<Car_1>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("XRI_Right_Trigger") > 0)
        {
            car_1.MoveForward();
            Debug.Log("Pressed");
        }
        else if (Input.GetAxis("XRI_Left_Trigger") > 0)
        {
            car_1.MoveBackward();
        }
        else car_1.Stop();

        //float steeringPos = Mathf.Clamp(wheel.angle / maxTurnAngle, -1, 1);
        //car_1.RotateCar(steeringPos);

        float steeringNormal = Mathf.InverseLerp(minValue, maxValue, wheel.transform.localRotation.x);
        float steeringRange = Mathf.Lerp(-1, 1, steeringNormal);
        if (Mathf.Abs(steeringRange) < turnThreshold)
        {
            steeringRange = 0;
        }
        else
        {
            car_1.RotateCar(steeringRange);
        }
    }
}
