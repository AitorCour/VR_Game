using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Snapper : MonoBehaviour
{
    //public Transform newParent;
    //private XRBaseInteractable interactable;
    //Left Hand
    public GameObject leftHand;
    private Transform leftHandParent;
    private Vector3 leftHandOriginalPos;
    public bool leftHandOnWheel;
    //Right Hand
    public GameObject rightHand;
    private Transform rightHandParent;
    private Vector3 rightHandOriginalPos;
    public bool rightHandOnWheel;
    // Start is called before the first frame update
    void Start()
    {
        leftHand = GameObject.FindGameObjectWithTag("PlayerLeftHand");
        rightHand = GameObject.FindGameObjectWithTag("PlayerRightHand");
        //interactable = GetComponent<XRBaseInteractable>();
        leftHandParent = leftHand.transform.parent;
        leftHandOriginalPos = leftHand.transform.localPosition;
        rightHandOriginalPos = rightHand.transform.localPosition;
        leftHandOnWheel = false;
        rightHandParent = rightHand.transform.parent;
        rightHandOnWheel = false;
        /*interactable.onSelectEntered.AddListener(StartHandFollow);
        interactable.onSelectExited.AddListener();*/
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerLeftHand"))
        {
            //Debug.Log("Trigger Left");
            if (!leftHandOnWheel && Input.GetAxisRaw("XRI_Left_GripButton") > 0) // Later trigger
            {
                //Debug.Log("Agarrar");
                PlaceHandOnWheel(leftHand);
            }
            if (leftHandOnWheel && Input.GetAxisRaw("XRI_Left_GripButton") <= 0)
            {
                //Debug.Log("Soltar");
                ReleaseHandFromWheel(leftHand);
            }
        }
        if (other.CompareTag("PlayerRightHand"))
        { 
            if (!rightHandOnWheel && Input.GetAxisRaw("XRI_Right_GripButton") > 0) // Later trigger
            {
                //Debug.Log("Agarrar");
                PlaceHandOnWheel(rightHand);
            }
            if (rightHandOnWheel && Input.GetAxisRaw("XRI_Right_GripButton") <= 0)
            {
                //Debug.Log("Soltar");
                ReleaseHandFromWheel(rightHand);
            }
        }
    }
    private void PlaceHandOnWheel(GameObject hand)
    {
        hand.transform.parent = this.transform;
        hand.transform.position = this.transform.position;
        if (hand == leftHand)
        {
            leftHandOnWheel = true;
        }
        else rightHandOnWheel = true;
    }
    private void ReleaseHandFromWheel(GameObject hand)
    {
        if (hand == leftHand)
        {
            hand.transform.parent = leftHandParent;
            //hand.transform.position = leftHandParent.position;
            hand.transform.localPosition = leftHandOriginalPos;
            leftHandOnWheel = false;
        }
        if (hand == rightHand)
        {
            hand.transform.parent = rightHandParent;
            //hand.transform.position = rightHandParent.position;
            hand.transform.localPosition = rightHandOriginalPos;
            rightHandOnWheel = false;
        }
    }
}
