using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_1 : MonoBehaviour
{
    float rotate, currentRotate;
    public float steering = 80f;

    private Rigidbody rBody;
    public Transform carModel;
    public enum carState { Stopped, MovingF, MovingB}
    public carState myState;

    [Header("MoveBools")]
    private bool canMoveF;
    private bool canMoveB;
    private bool canMoveU;
    private bool canMoveR;
    private bool canMoveL;
    private bool canMove;
    
    public bool rotating;
    public float speed;
    public float rotSpeed;
    private float speedForce = 0f;
    private float maxSpeed = 25f;//22
    private float maxRotationWheels = 10f;
    private float brake = 6f;
    private float acceleration = 4f;//4
    public float inclination;
    private int wheelDirection = 0;

    [Header("Raycast Settings")]
    public float rayDistanceVertical;
    public float rayDistanceTransversal;
    public float rayDistanceHorizontal;
    public float rayFrontRange;
    public float rayDownRange;
    public float frontFOV;
    public float downFOV;
    public LayerMask defaultMask;

    [Header("Time Reset")]
    private float timer = 0f;
    private float resetTimer = 3f;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction_Down = -transform.up * rayDistanceVertical;
        Gizmos.DrawRay(transform.position, direction_Down);
        Vector3 direction_Up = transform.up * rayDistanceVertical;
        Gizmos.DrawRay(transform.position, direction_Up);
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

        //Down
        float df = downFOV * Mathf.Deg2Rad;
        Vector3 downfront = (transform.forward * Mathf.Cos(df) - transform.up * Mathf.Sin(df)).normalized;
        Gizmos.DrawRay(transform.position, downfront * rayDownRange);
        float db = downFOV * Mathf.Deg2Rad;
        Vector3 downback = (-transform.forward * Mathf.Cos(db) - transform.up * Mathf.Sin(db)).normalized;
        Gizmos.DrawRay(transform.position, downback * rayDownRange);
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        float l = -frontFOV * Mathf.Deg2Rad;
        Vector3 leftRayDir = (transform.forward * Mathf.Cos(l) + transform.right * Mathf.Sin(l)).normalized;
        float r = frontFOV * Mathf.Deg2Rad;
        Vector3 rightRayDir = (transform.forward * Mathf.Cos(r) + transform.right * Mathf.Sin(r)).normalized;

        float df = downFOV * Mathf.Deg2Rad;
        Vector3 downfront = (transform.forward * Mathf.Cos(df) - transform.up * Mathf.Sin(df)).normalized;
        float db = downFOV * Mathf.Deg2Rad;
        Vector3 downback = (-transform.forward * Mathf.Cos(db) - transform.up * Mathf.Sin(db)).normalized;
        bool canMoveN;
        bool canMoveDF;
        bool canMoveDB;
        if (Physics.Raycast(transform.position, -transform.up, out hit, rayDistanceVertical, defaultMask))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Enemy")
            {
                //Debug.Log("Ground");
                canMoveN = true;
            }
            
            else canMoveN = false;
        }
        else canMoveN = false;
        if (Physics.Raycast(transform.position, downfront, out hit, rayDownRange, defaultMask))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Enemy")
            {
                //Debug.Log("Ground");
                canMoveDF = true;
            }
            else canMoveDF = false;
        }
        else canMoveDF = false;
        if (Physics.Raycast(transform.position, downback, out hit, rayDownRange, defaultMask))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Enemy")
            {
                //Debug.Log("Ground");
                canMoveDB = true;
            }
            else canMoveDB = false;
        }
        else canMoveDB = false;
        if (!canMoveDB && !canMoveDF && !canMoveN)
        {
            canMove = false;
        }
        else canMove = true;
        //ForwardDetector
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistanceTransversal, defaultMask))
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
        if (Physics.Raycast(transform.position, -transform.forward, out hit, rayDistanceTransversal, defaultMask))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Enemy")
            {
                //Debug.Log("Ground");
                canMoveB = false;
            }
        }
        else canMoveB = true;
        if (Physics.Raycast(transform.position, transform.up, out hit, rayDistanceVertical, defaultMask))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Enemy")
            {
                //Si detecta algo encima no se puede mover 
                canMoveU = false;
            }
        }
        else canMoveU = true;
        if (Physics.Raycast(transform.position, transform.right, out hit, rayDistanceHorizontal, defaultMask))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Enemy")
            {
                //Si detecta algo encima no se puede mover 
                Debug.Log("Right");
                canMoveR = false;
            }
        }
        else canMoveR = true;
        if (Physics.Raycast(transform.position, -transform.right, out hit, rayDistanceHorizontal, defaultMask))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Enemy")
            {
                //Si detecta algo encima no se puede mover 
                Debug.Log("Left");
                canMoveL = false;
            }
        }
        else canMoveL = true;
    }
    // Update is called once per frame
    void Update()
    {
        currentRotate = rotate;rotate = 0;
        if (speed > speedForce)
        {
            speedForce = speed;
        }
        switch (myState)
        {
            case carState.Stopped:
                {
                    if (speed != 0 && canMove)
                    {
                        if (inclination < 3 && inclination > -3)
                        {
                            //Debug.Log("NO Bajada");
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
                        else
                        {
                            if (inclination > 0 && inclination < 60)
                            {
                                speed += brake * Time.deltaTime;
                                //Debug.Log("Caer de Culo");
                            }
                            if (inclination < 360 && inclination > 270)
                            {
                                speed -= brake * Time.deltaTime;
                                //Debug.Log("Caer de Cara");
                            }
                        }
                        if (!canMoveF && speed > 0 || !canMoveB && speed < 0) speed = 0;
                    }
                    else if(speed != 0 && !canMove)
                    {
                        //Debug.Log("Aerial");
                        if (speed > 0)
                        {
                            speed -= brake * 2 * Time.deltaTime;
                            if (speed < 0) speed = 0;
                        }
                        else if (speed < 0)
                        {
                            speed += brake * Time.deltaTime;
                            if (speed > 0) speed = 0;
                        }
                    }
                    else speed = 0;
                    
                    float direction = 1 * Time.deltaTime * speed;
                    //myDirection = direction;
                    transform.Translate(0, 0, direction);
                    break;
                }
            case carState.MovingF:
                {
                    if (canMoveF)
                    {
                        if (inclination < 320 && inclination > 270)
                        {
                            //Debug.Log("Case NOT Move");
                            speed -= brake * Time.deltaTime;
                        }
                        else
                        {
                            //Debug.Log("Case Move");
                            speed += acceleration * Time.deltaTime;
                            if (speed > maxSpeed) speed = maxSpeed;
                            else if (speed < 0) speed += brake * 2 * Time.deltaTime;
                        }
                    }
                    else
                    {
                        //speed -= brake* 10 * Time.deltaTime;
                        speed = 0;
                        if (speed < 0) speed = 0;
                    }
                    float direction = 1 * Time.deltaTime * speed;
                    //myDirection = direction;
                    transform.Translate(0, 0, direction);
                    break;
                }
            case carState.MovingB:
                {
                    if(canMoveB)
                    {
                        if (inclination > 40 && inclination < 120)
                        {
                            speed += brake * Time.deltaTime;
                            //Debug.Log("Case NOT Move");
                        }
                        else
                        {
                            speed -= acceleration * Time.deltaTime;
                            if (speed < -maxSpeed / 2) speed = -maxSpeed / 2;
                            else if (speed > 0) speed -= brake * 2 * Time.deltaTime;
                        }
                    }
                    else
                    {
                        //speed += brake * 10 * Time.deltaTime;
                        speed = 0;
                        if (speed > 0) speed = 0;
                    }
                    float direction = 1 * Time.deltaTime * speed;
                    //myDirection = direction;
                    transform.Translate(0, 0, direction);
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
                    break;
                }
        }
        inclination = transform.rotation.eulerAngles.x;
        /*if (!rotating && rotSpeed != 0)
        {
            UnrotateCar();
        }*/
        if (!canMove && !canMoveF || !canMove && !canMoveB || !canMove && !canMoveU || !canMove && !canMoveR || !canMove && !canMoveL || !canMove && speed == 0)
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
    public void MoveForward()
    {
        if (!canMove) 
        {
            myState = carState.Stopped;
            Debug.Log("Cannot move");
        }
        else 
        {
            myState = carState.MovingF;
            Debug.Log("Can move");
        }
    }
    public void MoveBackward()
    {
        if (!canMove) myState = carState.Stopped;
        else myState = carState.MovingB;
    }
    public void Stop()
    {
        
        myState = carState.Stopped;
    }
    public void RotateCar(float i)
    {
        /*float steering = maxRotationWheels * i; 
        Debug.Log(steering);
        transform.Rotate(0, steering, 0);*/
        int dir = i > 0 ? 1 : -1;
        float amount = Mathf.Abs((i));
        Steer(dir, amount);
    }
    public void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount;
    }
    /*public void UnrotateCar()
    {
        if (rotSpeed < 0)
        {
            rotSpeed += rotAcceleration * Time.deltaTime;
            if (rotSpeed >= 0)
            {
                rotSpeed = 0;
            }
            float direction = 1 * Time.deltaTime * rotSpeed;
            transform.Rotate(0, direction, 0);
        }
        if (rotSpeed > 0)
        {
            rotSpeed -= rotAcceleration * Time.deltaTime;
            if (rotSpeed <= 0)
            {
                rotSpeed = 0;
            }
            float direction = 1 * Time.deltaTime * rotSpeed;
            transform.Rotate(0, direction, 0);
        }
    }*/
    public void RecieveHit()
    {
        speed = 0;
        StartCoroutine(WaitForRestartForce());
        Debug.Log("Crash");
    }
    IEnumerator WaitForRestartForce()
    {
        yield return new WaitForSeconds(2f);
        speedForce = 0;
    }
}
