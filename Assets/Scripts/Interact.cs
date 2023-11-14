using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interact : MonoBehaviour
{

    private float
    reach_distance = 0.5f,
    grip_threshold = 0.5f,
    ungrip_threshold = 0.4f;

    private GameObject XRRig;
    private XRControls XRControls;

    [SerializeField]
    private GameObject leftHand, rightHand;

    // Data to be stored locally so that we don't have to get component every fucking frame
    private Hand leftHandBehaviour, rightHandBehaviour;

    void Start()
    {
        XRRig = XRRig_Behaviour.Instance.gameObject;
        XRControls = XRRig_Behaviour.Instance.XRControls;

        leftHandBehaviour = leftHand.GetComponent<Hand>();
        rightHandBehaviour = rightHand.GetComponent<Hand>();
    }

    void Update()
    {
        float leftGripValue = XRControls.Character.GrabLeft.ReadValue<float>();
        float rightGripValue = XRControls.Character.GrabRight.ReadValue<float>();

        bool leftGripping = leftHandBehaviour.GetGrippingState();
        bool rightGripping = rightHandBehaviour.GetGrippingState();

        // Check if controller(s) are gripping
        if (leftGripValue > grip_threshold || rightGripValue > grip_threshold) // Check if either hand has passed the grip threshold
        {
            if ((leftGripValue > grip_threshold) && !leftGripping) // is it the left one?
            {
                Grab(leftHand);
            }
            if ((rightGripValue > grip_threshold) && !rightGripping) // is it right one?
            {
                Grab(rightHand);
            }
        }
        
        // Check if controller(s) are not gripping
        if (leftGripValue < ungrip_threshold || rightGripValue < ungrip_threshold) // check if either hand is under the ungrip threshold
        {
            if (leftGripValue < ungrip_threshold && leftGripping) // is it the left one?
            {
                Ungrab(leftHand);
            }
            if (rightGripValue < ungrip_threshold && rightGripping) // is it right one? yeayeay
            {
                Ungrab(rightHand);
            }
        }

        //UpdateItemTransforms();
    }

    /*
    private void UpdateItemTransforms()
    {
        GameObject leftHeldItem = leftHandBehaviour.GetHeldItem();
        GameObject rightHeldItem = rightHandBehaviour.GetHeldItem();
        if (leftHeldItem != null)
        {
            leftHeldItem.transform.SetPositionAndRotation(leftHand.transform.position, leftHand.transform.rotation * Quaternion.Euler(itemRotOffset));
        }
        if (rightHeldItem != null)
        {
            rightHeldItem.transform.SetPositionAndRotation(rightHand.transform.position, rightHand.transform.rotation * Quaternion.Euler(itemRotOffset));
        }
    }
    */
    private void Grab(GameObject hand)
    {
        Hand HandComponent = hand.GetComponent<Hand>();
        HandComponent.SetGrippingState(true);

        GameObject closestGripPoint = GetClosestCompatibleGripPointInItems(hand);
        Debug.Log(closestGripPoint);
        if (closestGripPoint != null)
        {
            HandComponent.SetHeldGripPoint(closestGripPoint);
            GameObject closestItem = closestGripPoint.GetComponent<Grip_Point>().GetItem();
            Item closestItemComponent = closestItem.GetComponent<Item>();
            closestItemComponent.GrabPoint(closestGripPoint, hand);

            HandComponent.SetHeldItem(closestItem);

            
        }
    }

    private void Ungrab(GameObject hand)
    {
        Hand HandComponent = hand.GetComponent<Hand>();

        GameObject item = HandComponent.GetHeldItem();
        GameObject gripPoint = HandComponent.GetHeldGripPoint();

        HandComponent.SetHeldGripPoint(null);
        HandComponent.SetHeldItem(null);
        HandComponent.SetGrippingState(false);

        if (item != null)
        {
            Item itemComponent = item.GetComponent<Item>();
            itemComponent.UngrabPoint(gripPoint);
        }
        

    }

    private GameObject GetClosestCompatibleGripPointInItems(GameObject hand)
    {

        List<GameObject> gripPoints = new();

        foreach (GameObject v in GameObject.FindGameObjectsWithTag("Item"))
        {
            float current_distance = (v.transform.position - hand.transform.position).magnitude;
            if (current_distance <= reach_distance * 2)
            {
                
                foreach (GameObject gripPoint in v.GetComponent<Item>().GetGripPoints())
                {
                    gripPoints.Add(gripPoint);
                }

            }
        }

        GameObject target = null;
        float target_distance = Mathf.Infinity;

        foreach (GameObject v in gripPoints)
        {
            Grip_Point vComponent = v.GetComponent<Grip_Point>();
            float current_distance = (v.transform.position - hand.transform.position).magnitude;

            // Check if grip is compatible with hand
            bool compatible = (v != GetOppositeHand(hand).GetComponent<Hand>().GetHeldGripPoint()) && (vComponent.GetCompatibleHandType() == hand.GetComponent<Hand>().GetSide() || vComponent.GetCompatibleHandType() == Hand.HandType.Both);

            if (compatible && current_distance < target_distance)
            {
                target = v;
                target_distance = current_distance;
            }
        }

        return target;
    }

    private GameObject GetOppositeHand(GameObject hand)
    {
        if (hand == leftHand)
            return rightHand;
        else
            return leftHand;
    }

}
