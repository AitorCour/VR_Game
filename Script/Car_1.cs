using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_1 : MonoBehaviour
{
    private Rigidbody rBody;
    public Transform carModel;
    public enum carState { Stopped, MovingF, MovingB}
    public carState myState;

    [Header("MoveBools")]
    private bool canMoveF;
    private bool canMoveB;
    private bool canMoveR;
    private bool canMoveL;
    public bool canMove;

    private bool noneInContact = false;
    
    [Header("Movement")]
    public float speed;
    //public float rotSpeed;
    public float maxSpeed = 30f;//22        // La maxima velocidad del coche.
    private float tempMaxSpeed = 1f;
    public float maxRotationWheels = 5f;    // Lo maximo que el coche puede girar.
    public float brake = 10f;                // Brake force
    public float acceleration = 15f;         //4
    private float rotationReference;
    private int gear;

    private WheelContactChecker wheelChecker;     // WheelContactChecker

    public GameObject[] wheelHubs;           //Aqui van todas las ruedas que vayan a rotar.

    [Header("Raycast Settings")]
    public float rayDistanceVertical;
    public float rayDistanceTransversal;
    public float rayDistanceHorizontal;
    public float rayFrontRange;
    public float rayDownRange;
    public float frontFOV;
    public float downFOV;
    public LayerMask defaultMask;   // settear bien la mascara

    [Header("Time Reset")]
    private float timer = 0f;
    private float resetTimer = 3f;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        wheelChecker = GetComponent<WheelContactChecker>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
        Vector3 direction_Right = transform.right * rayDistanceHorizontal;
        Gizmos.DrawRay(transform.position, direction_Right);
        Vector3 direction_Left = -transform.right * rayDistanceHorizontal;
        Gizmos.DrawRay(transform.position, direction_Left);
        Vector3 direction_For = transform.forward * rayDistanceTransversal;
        Gizmos.DrawRay(transform.position, direction_For);
        Vector3 direction_Back = -transform.forward * rayDistanceTransversal;
        Gizmos.DrawRay(transform.position, direction_Back);

        //Front
        Gizmos.color = Color.blue;
        float r = frontFOV * Mathf.Deg2Rad;
        Vector3 right = (transform.forward * Mathf.Cos(r) + transform.right * Mathf.Sin(r)).normalized;
        Gizmos.DrawRay(transform.position, right * rayFrontRange);
        float l = -frontFOV * Mathf.Deg2Rad;
        Vector3 left = (transform.forward * Mathf.Cos(l) + transform.right * Mathf.Sin(l)).normalized;
        Gizmos.DrawRay(transform.position, left * rayFrontRange);
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        float l = -frontFOV * Mathf.Deg2Rad;
        Vector3 leftRayDir = (transform.forward * Mathf.Cos(l) + transform.right * Mathf.Sin(l)).normalized;
        float r = frontFOV * Mathf.Deg2Rad;
        Vector3 rightRayDir = (transform.forward * Mathf.Cos(r) + transform.right * Mathf.Sin(r)).normalized;

        //ForwardDetector
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistanceTransversal, defaultMask))   // Estos tres de forward dicen si hay algun obstaculo. Si no lo hay, el vehiculo avanza.
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Enemy")
            {
                //Debug.Log("Ground");
                canMoveF = false;
            }
            /*if (hit.transform.CompareTag("Enemy") && speed >= 3f)
            {
                float attackDistance = 2.2f;
                float force = 60 * speedForce;
                float upForce = 0;
                Vector3 origin = transform.position;
                Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
                Collider col = hit.transform.GetComponent<Collider>();
                rb.AddExplosionForce(force, origin, attackDistance, upForce, ForceMode.Impulse);
                Debug.Log("No null");
                CarIA target = hit.transform.gameObject.GetComponent<CarIA>();
                target.RecieveHit();
            }*/
        }
        else if (Physics.Raycast(transform.position, rightRayDir, out hit, rayFrontRange, defaultMask))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Enemy")
            {
                //Debug.Log("Ground");
                canMoveF = false;
            }
        }
        else if (Physics.Raycast(transform.position, leftRayDir, out hit, rayFrontRange, defaultMask))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Enemy")
            {
                //Debug.Log("Ground");
                canMoveF = false;
            }
        }
        else canMoveF = true;
        if (Physics.Raycast(transform.position, -transform.forward, out hit, rayDistanceTransversal, defaultMask))  // Aqui decide si puede moverse hacia detras.
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Enemy")
            {
                //Debug.Log("Ground");
                canMoveB = false;
            }
        }
        else canMoveB = true;
        if (Physics.Raycast(transform.position, transform.right, out hit, rayDistanceHorizontal, defaultMask))      // Decide si puede girar a la derecha.
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Enemy")
            {
                Debug.Log("Right");
                canMoveR = false;
            }
        }
        else canMoveR = true;
        if (Physics.Raycast(transform.position, -transform.right, out hit, rayDistanceHorizontal, defaultMask))     // Decide si puede girar a la izquierda.
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Enemy")
            {
                Debug.Log("Left");
                canMoveL = false;
            }
        }
        else canMoveL = true;

        switch (myState)
        {
            case carState.Stopped:
                {
                    if(speed > 0)
                    {
                        speed -= acceleration/2 * Time.fixedDeltaTime;
                    }
                    wheelHubs[0].SetActive(true);
                    wheelHubs[1].SetActive(false);
                    break;
                }
            case carState.MovingF:
                {
                    speed += acceleration * Time.fixedDeltaTime;
                    if (speed >= tempMaxSpeed) speed = tempMaxSpeed;
                    wheelHubs[0].SetActive(false);
                    wheelHubs[1].SetActive(true);
                    break;
                }
            case carState.MovingB:
                {
                    speed -= brake * Time.fixedDeltaTime;
                    if (speed <= tempMaxSpeed) speed = tempMaxSpeed;
                    wheelHubs[0].SetActive(false);
                    wheelHubs[1].SetActive(true);
                    break;
                }
            default:
                {
                    if (speed != 0)
                    {
                        if (speed > 0)
                        {
                            speed -= brake * Time.deltaTime;
                            if (speed < 0) speed = 0;
                        }
                        else if (speed < 0)
                        {
                            speed += brake * Time.deltaTime;
                            if (speed > 0) speed = 0;
                        }
                    }
                    wheelHubs[0].SetActive(true);
                    wheelHubs[1].SetActive(false);
                    break;
                }
            
        }
        rBody.AddForce(transform.forward * speed, ForceMode.Acceleration);
        rBody.drag = speed/tempMaxSpeed;

    }
    // Update is called once per frame
    void Update()
    {
        if(wheelChecker.noneGrounded && speed == 0)
        {
            timer += Time.deltaTime;
            if (timer >= resetTimer)
            {
                //Debug.Log("Reset");
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

                timer = 0;
            }
        }
    }
    public void Move(int i)
    {
        gear = i;
        if(gear > 0)
        {
            if (!canMoveF || wheelChecker.noneGrounded) 
            {
                myState = carState.Stopped;
                Debug.Log("Cannot move");
            }
            else
            {
                switch (gear)
                {
                    case 1:
                    {
                        tempMaxSpeed = maxSpeed/1.5f;
                        break;
                    }
                    case 2:
                    {
                        tempMaxSpeed = maxSpeed;
                        break;
                    }
                    default:
                    {
                        tempMaxSpeed = 1f;
                        break;
                    }
                }
                myState = carState.MovingF;
                Debug.Log("Can move");
            }
        }
        else if(gear < 0)
        {
            if (!canMoveB || wheelChecker.noneGrounded) myState = carState.Stopped;
            else 
            {
                tempMaxSpeed = maxSpeed/-2f;
                myState = carState.MovingB;
            }
        }
        else myState = carState.Stopped;

    }
    public void Stop()
    {
        myState = carState.Stopped;
    }
    public void RotateCar(float i)
    {   // Aqui se limita la rotacion segun la velocidad.
        switch (gear)
        {
            case 1:
            {
                rotationReference = maxRotationWheels / 2f;
                break;
            }
            case 2:
            {
                rotationReference = maxRotationWheels;
                break;
            }
            case -1:
            {
                rotationReference = maxRotationWheels / 2f;
                break;
            }
            default:
            {
                rotationReference = 0f;
                break;
            }
        }
        float steering = rotationReference * i;                          // Rotacion por transform

        Vector3 m_EulerAngleVelocity = new Vector3 (0, steering, 0);        // Rotacion cambiada por Rigidbody
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity);  // Tambien deberia estar en FixedUpdate
        rBody.MoveRotation(rBody.rotation * deltaRotation);

        // Girar ruedas fisicas
        //wheelDirection = steering;
        /*foreach (Transform mesh in wheelMesh)
        {
            //mesh.Rotate(wheelDirection, 0, 0, Space.Self);
            //transform.RotateAround(transform.position, transform.up, Time.deltaTime * steering);
        }*/
    }
    public void RecieveHit()
    {
        speed = 0;
        StartCoroutine(WaitForRestartForce());
        Debug.Log("Crash");
    }
    IEnumerator WaitForRestartForce()
    {
        yield return new WaitForSeconds(2f);
    }
}
