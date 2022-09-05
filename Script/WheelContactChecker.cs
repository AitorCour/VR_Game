using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelContactChecker : MonoBehaviour
{
    private bool[] grounded;
    public bool noneGrounded;
    public float rayDistanceVertical;
    public LayerMask defaultMask;
    private GameObject[] wheelMesh;
    // Start is called before the first frame update
    void Start()
    {
        wheelMesh = GameObject.FindGameObjectsWithTag("Wheel");
        grounded = new bool[4];
    }
    private void OnDrawGizmosSelected()
    {
        if(wheelMesh == null) return;
        Gizmos.color = Color.red;
        Vector3 direction_Down = -transform.up * rayDistanceVertical;//Change right by up if Z rotation is different
        foreach(GameObject wheel in wheelMesh)
        {
            Gizmos.DrawRay(wheel.transform.position, direction_Down);
        }
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        for (int i = 0; i < wheelMesh.Length; i++)
        {
            if (Physics.Raycast(wheelMesh[i].transform.position, -transform.up, out hit, rayDistanceVertical, defaultMask))  // Rayo hacia abajo
            {
                if (hit.transform.tag == "Ground")
                {
                    //Debug.Log("Ground");
                    grounded[i] = true;
                }
                else grounded[i] = false;
            }
            else grounded[i] = false;
        }
        int neg = Count(grounded, false);   // return how many false
        if(neg >= 4)
        {
            noneGrounded = true;
        }
        else noneGrounded = false;
    }

    private int Count(bool[] array, bool flag)
    {
        int value = 0;
 
        for(int i = 0; i < array.Length; i++) 
        {
            if(array[i] == flag) value++;
        }
 
        return value;
    }
}
