using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Item : MonoBehaviour
{

    [SerializeField] private GameObject[] gripPoints;
    [SerializeField] private bool overrideTransformBehaviour;

    public bool Held { get; private set; }
    private GameObject influenceGripPoint;
    private GameObject influenceHand = null;

    public GameObject currentOperator { get; private set; }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (overrideTransformBehaviour || !Held || influenceGripPoint == null || influenceHand == null) return;

        gameObject.transform.SetPositionAndRotation(influenceHand.transform.TransformPoint(-influenceGripPoint.transform.localPosition*10), influenceHand.transform.rotation * influenceGripPoint.transform.localRotation);
    }

    public GameObject[] GetGripPoints() 
    {
        return gripPoints;
    }

    public GameObject GetInfluenceHand()
    {
        return influenceHand;
    }
    public GameObject GetInfluenceGripPoint()
    {
        return influenceGripPoint;
    }

    public void GrabPoint(GameObject newOperator, GameObject gripPoint, GameObject hand)
    {

        if (currentOperator != newOperator)
            currentOperator = newOperator;

        // Tell the grip point that it is grabbed
        Grip_Point gripPointComponent = gripPoint.GetComponent<Grip_Point>();
        gripPointComponent.SetHand(hand);
        gripPointComponent.SetHeld(true);

        Held = true;

        // If it was a primary grip, then give control to the primary grip.
        // If it was a secondary grip, and the primary grip isn't held, then give control to the secondary grip.
        
        switch (gripPointComponent.GetType())
        {
            case Grip_Point.GripPointType.Primary:

                influenceGripPoint = gripPoint;
                influenceHand = hand;
                break;

            case Grip_Point.GripPointType.Secondary:

                if (GetGripPointComponent(Grip_Point.GripPointType.Primary).GetHeld() == false)
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
                for (Grip_Point.GripPointType i = Grip_Point.GripPointType.Primary; i <= Grip_Point.GripPointType.Modular; i++) {

                    Grip_Point vGripComponent = GetGripPointComponent(i);
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
                Held = false;
                break;

            default:

                // If the secondary grip isn't in control, then it doesn't get to tell other grips whos in control.
                if (gripPoint != influenceGripPoint) break;

                heldGripFound = false;
                for (Grip_Point.GripPointType i = Grip_Point.GripPointType.Primary; i <= Grip_Point.GripPointType.Modular; i++)
                {

                    Grip_Point vGripComponent = GetGripPointComponent(i);
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
                Held = false;
                break;
        }
    }

    public GameObject GetGripPoint(Grip_Point.GripPointType type)
    {
        foreach (GameObject v in GetGripPoints())
        {
            if (v.GetComponent<Grip_Point>().GetType() == type)
                return v;

        }
        return null;
    }

    public Grip_Point GetGripPointComponent(Grip_Point.GripPointType type)
    {
        GameObject gripPoint = GetGripPoint(type);
        if (gripPoint)
            return gripPoint.GetComponent<Grip_Point>();
        return null;
    }
}
