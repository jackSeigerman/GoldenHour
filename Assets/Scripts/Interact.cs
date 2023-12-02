using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private readonly float reach_distance = 0.5f;
    private readonly float grip_threshold = 0.5f;
    private readonly float ungrip_threshold = 0.4f;

    //private GameObject XRRig;
    private XRControls XRControls;

    [SerializeField] private GameObject LeftHand, RightHand, Head;


    // Data to be stored locally so that we don't have to get component every fucking frame
    private Hand leftHandBehaviour, rightHandBehaviour;

    void Start()
    {
        //XRRig = XRRig_Behaviour.Instance.gameObject;
        XRControls = XRRig_Behaviour.Instance.XRControls;

        leftHandBehaviour = LeftHand.GetComponent<Hand>();
        rightHandBehaviour = RightHand.GetComponent<Hand>();
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
                Grab(LeftHand);
            }
            if ((rightGripValue > grip_threshold) && !rightGripping) // is it right one?
            {
                Grab(RightHand);
            }
        }
        
        // Check if controller(s) are not gripping
        if (leftGripValue < ungrip_threshold || rightGripValue < ungrip_threshold) // check if either hand is under the ungrip threshold
        {
            if (leftGripValue < ungrip_threshold && leftGripping) // is it the left one?
            {
                Ungrab(LeftHand);
            }
            if (rightGripValue < ungrip_threshold && rightGripping) // is it right one? yeayeay
            {
                Ungrab(RightHand);
            }
        }
    }

    public void Grab(GameObject hand)
    {
        Hand HandComponent = hand.GetComponent<Hand>();
        HandComponent.SetGrippingState(true);

        GameObject closestGripPoint = GetClosestCompatibleGripPointInItems(hand);

        if (closestGripPoint != null)
        {
            HandComponent.SetHeldGripPoint(closestGripPoint);
            GameObject closestItem = closestGripPoint.GetComponent<Grip_Point>().GetItem();
            Item closestItemComponent = closestItem.GetComponent<Item>();
            closestItemComponent.GrabPoint(gameObject, closestGripPoint, hand);

            HandComponent.SetHeldItem(closestItem);

            
        }
    }

    public void Ungrab(GameObject hand)
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

            if (current_distance > reach_distance * 2)
                continue;
                
            foreach (GameObject gripPoint in v.GetComponent<Item>().GetGripPoints())
            {
                gripPoints.Add(gripPoint);
            }

        }

        GameObject target = null;
        float target_distance = Mathf.Infinity;

        foreach (GameObject v in gripPoints)
        {
            Grip_Point vComponent = v.GetComponent<Grip_Point>();
            float current_distance = (v.transform.position - hand.transform.position).magnitude;
            
            // Check if grip is compatible with hand
            bool compatible = (v != GetOppositeHand(hand).GetComponent<Hand>().GetHeldGripPoint()) && (vComponent.GetCompatibleHandType() == hand.GetComponent<Hand>().GetSide() || vComponent.GetCompatibleHandType() == Hand.HandType.Both) && (vComponent.GetCanManuallyGrip());

            if (!compatible || current_distance > target_distance)
                continue;

            target = v;
            target_distance = current_distance;
        }

        return target;
    }

    private GameObject GetOppositeHand(GameObject hand)
    {
        if (hand == LeftHand)
            return RightHand;
        else
            return LeftHand;
    }

    public GameObject GetHead()
    { return Head; }

}
