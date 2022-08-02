using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        public HingeJoint steeringWheel;
        private float maxTurnAngle = 180;
        float f = 0;
        float v = 0;
        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            /*float h = CrossPlatformInputManager.GetAxis("Horizontal");*/
            //float v = CrossPlatformInputManager.GetAxis("Vertical");
            float h = Mathf.Clamp(steeringWheel.angle/maxTurnAngle, -1, 1);

            /*if (Input.GetAxisRaw("XRI_Right_TriggerButton") > 0)
            {
                Debug.Log("Hola");
            }*/
            //Accelerar
            if (Input.GetAxis("Trigger_Right") > 0 || Input.GetAxis("XRI_Right_Trigger") > 0)
            {
                //amount = Input.GetAxis("Trigger_Right");
                
                v = Input.GetAxis("XRI_Right_Trigger");
                Debug.Log(v);
            }
            else v = 0;
            //Frenar
            if (Input.GetAxis("Trigger_Left") > 0 || Input.GetAxis("XRI_Left_Trigger") > 0)
            {
                //f = Input.GetAxis("Trigger_Left");
                f = Input.GetAxis("XRI_Left_Trigger");
            }
            else f = 0;
#if !MOBILE_INPUT
            float handbrake = 0;
            m_Car.Move(h, v, f, handbrake);//Cambiada la segunda v por f. Así indicaremos que es el footbrake. Handbrake es un Input fisico dentro del coche.
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
