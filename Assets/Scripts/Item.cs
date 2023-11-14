using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Item : MonoBehaviour
{
    [SerializeField] private GameObject[] gripPoints;
    [SerializeField] private bool held;
    [SerializeField] private GameObject influenceGripPoint;
    [SerializeField] private GameObject influenceHand = null;

    private void Update()
    {
        if (!held | influenceGripPoint == null | influenceHand == null) return;


        Vector3 gripPointOffset = new(
            -influenceGripPoint.transform.localPosition.x * gameObject.transform.lossyScale.x,
            -influenceGripPoint.transform.localPosition.y * gameObject.transform.lossyScale.y,
            -influenceGripPoint.transform.localPosition.z * gameObject.transform.lossyScale.z
            );

        gameObject.transform.SetPositionAndRotation(influenceHand.transform.TransformPoint(gripPointOffset), influenceHand.transform.rotation * influenceGripPoint.transform.localRotation);
    }

    public GameObject[] GetGripPoints() 
    {
        return gripPoints;
    }

    public GameObject GetInfluenceHand()
    {
        return influenceHand;
    }

    public void GrabPoint(GameObject gripPoint, GameObject hand)
    {

        // Tell the grip point that it is grabbed
        Grip_Point gripPointComponent = gripPoint.GetComponent<Grip_Point>();
        gripPointComponent.SetHand(hand);
        gripPointComponent.SetHeld(true);

        held = true;

        // If it was a primary grip, then give control to the primary grip.
        // If it was a secondary grip, and the primary grip isn't held, then give control to the secondary grip.
        
        switch (gripPointComponent.GetType())
        {
            case Grip_Point.GripPointType.Primary:

                influenceGripPoint = gripPoint;
                influenceHand = hand;
                break;

            case Grip_Point.GripPointType.Secondary:

                if (GetGrip(Grip_Point.GripPointType.Primary).GetHeld() == false)
                {
                    influenceGripPoint = gripPoint;
                    influenceHand = hand;
                }
                break;
        }

        // If there still is no influenced grip, then this means the item has not been grabbed prior to this moment. Give control to the fixed grip.
        if (influenceGripPoint == null)
        {
            influenceGripPoint = gripPoint;
            influenceHand = hand;
        }

    }

    public void UngrabPoint(GameObject gripPoint)
    {
        Grip_Point gripPointComponent = gripPoint.GetComponent<Grip_Point>();
        gripPointComponent.SetHand(null);
        gripPointComponent.SetHeld(false);

        switch (gripPointComponent.GetType())
        {
            case Grip_Point.GripPointType.Primary:

                // FOR loop, but instead of i being an int, its a fucking GripPointType
                // For every specified value in the GripPointType enumerator

                bool heldGripFound = false;
                for (Grip_Point.GripPointType i = Grip_Point.GripPointType.Secondary; i <= Grip_Point.GripPointType.Fixed; i++) {

                    Grip_Point vGripComponent = GetGrip(i);
                    if (!vGripComponent) break;

                    if (vGripComponent.GetHeld() == true)
                    {
                        influenceGripPoint = vGripComponent.gameObject;
                        influenceHand = vGripComponent.GetHand();
                        heldGripFound = true;
                        break;
                    }
                }

                if (heldGripFound) break;

                // We only reach this part of the case if there are no held grip points anymore
                influenceGripPoint = null;
                influenceHand = null;
                held = false;
                break;

            case Grip_Point.GripPointType.Secondary:

                // If the secondary grip isn't in control, then it doesn't get to tell other grips whos in control.
                if (gripPoint != influenceGripPoint) break;

                // iterate through all possible grip points, if any of them are held, relinquish control to them.

                heldGripFound = false;
                foreach (GameObject v in gripPoints)
                {

                    Grip_Point vGripComponent = v.GetComponent<Grip_Point>();
                    if (!vGripComponent) break;

                    if (vGripComponent.GetHeld() == true)
                    {
                        influenceGripPoint = v;
                        influenceHand = vGripComponent.GetHand();
                        heldGripFound = true;
                        break;
                    }
                }

                if (heldGripFound) break;

                // We only reach this part of the case if there are no held grip points anymore
                influenceGripPoint = null;
                influenceHand = null;
                held = false;
                break;
        }
    }

    private Grip_Point GetGrip(Grip_Point.GripPointType type)
    {
        foreach (GameObject v in GetGripPoints())
        {
            if (v.GetComponent<Grip_Point>().GetType() == type) 
                return v.GetComponent<Grip_Point>();
            
        }
        return null;
    }
    public bool GetHeld()
    {
        return held;
    }
}
