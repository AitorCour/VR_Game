using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelContactChecker : MonoBehaviour
{
    public bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision other) 
    {
        if(other.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true;
        }
        else grounded = false;
    }
}
