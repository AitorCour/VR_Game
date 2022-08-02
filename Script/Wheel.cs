using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Wheel : MonoBehaviour
{
    //Left Hand
    public GameObject leftHand;//Pasar objecto de la left hand
    private Transform leftHandParent;
    public bool leftHandOnWheel;

    public Transform directionalObject;
    public Transform wheelObj;

    public Transform[] snappPositions;
    private void Start()
    {
        leftHandParent = leftHand.transform.parent;
        leftHandOnWheel = false;
        /*XRBaseInteractor interactor = selectingInteractor;
        IXRSelectInteractor newInteractor = firstInteractorSelecting;
        List<IXRSelectInteractor> moreInteractors = interactorsSelecting;*/
        snappPositions = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            snappPositions[i] = transform.GetChild(i);
        }
    }
    private void Update()
    {
        //currentSteeringWheelRotation = -transform.rotation.eulerAngles.z;
    }
    /*protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (HasMultipleInteractors())
            Debug.Log("Multiple");
    }
    private bool HasMultipleInteractors()
    {
        return interactorsSelecting.Count > 1;
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if(HasNoInteractors())
            Debug.Log("None");
    }
    private bool HasNoInteractors()
    {
        return interactorsSelecting.Count == 0;
    }*/
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("PlayerHand"))
        {
            Debug.Log("Trigger Left");
            if (!leftHandOnWheel && Input.GetAxisRaw("XRI_Left_GripButton") > 0) // Later trigger
            {
                Debug.Log("Agarrar");
                PlaceHandOnWheel(leftHand, leftHandParent);
            }
            if(leftHandOnWheel && Input.GetAxisRaw("XRI_Left_GripButton") > 0)
            {
                //ConvertHandRotationToSteeringWheelRotation();
            }
            if(leftHandOnWheel && Input.GetAxisRaw("XRI_Left_GripButton") <= 0)
            {
                Debug.Log("Soltar");
                ReleaseHandFromWheel(leftHand);
            }
        }
    }
    private void PlaceHandOnWheel(GameObject hand, Transform originalParent)
    {
        //Set variables to first point
        float shortestDistance = Vector3.Distance(snappPositions[0].position, hand.transform.position);
        Transform bestSnapp = snappPositions[0];
        //loop to best position
        foreach (Transform snappPosition in snappPositions)
        {
            if(snappPosition.childCount == 0)
            {
                float distance = Vector3.Distance(snappPosition.position, hand.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    bestSnapp = snappPosition;
                }
            }
        }

        originalParent = hand.transform.parent;
        hand.transform.parent = bestSnapp.transform;
        hand.transform.position = bestSnapp.transform.position;
        if (hand == leftHand)
        {
            leftHandOnWheel = true;
        }
    }
    private void ReleaseHandFromWheel(GameObject hand)
    {
        if (hand == leftHand)
        {
            hand.transform.parent = leftHandParent;
            hand.transform.position = leftHandParent.position;
            leftHandOnWheel = false;
        }
    }
    //Erase
    /*private void ConvertHandRotationToSteeringWheelRotation()
    {
        if(leftHandOnWheel)
        {
            Debug.Log("Girar");
            Quaternion newRot = Quaternion.Euler(0, 0, leftHandParent.transform.rotation.eulerAngles.z);
            directionalObject.localRotation = newRot;
            transform.parent = directionalObject;
            //wheelObj = directionalObject;
        }
    }*/
}
