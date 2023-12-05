using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public enum HandType
    {
        Left,
        Right,
        Both
    }

    [SerializeField] private HandType side;
    [SerializeField] private GameObject heldItem = null;
    [SerializeField] private GameObject heldGripPoint = null;
    [SerializeField] private bool gripping;
    [SerializeField] private GameObject controller = null;

    private Item ItemComponent = null;
    private Grip_Point heldGripPointComponent = null;

    private Grip_Point.GripPointType heldGripPointType;

    private Quaternion handRotOffset = Quaternion.Euler(90, 0, 0);

    public float triggerSqueeze, gripSqueeze;
    public float primaryButton, secondaryButton;


    //private GameObject XRRig;
    private XRControls XRControls;
    private Interact interactComponent;
    private void Start()
    {
        //XRRig = XRRig_Behaviour.Instance.gameObject;
        XRControls = XRRig_Behaviour.Instance.XRControls;
        interactComponent = gameObject.transform.parent.parent.parent.GetComponent<Interact>();
    }


    public GameObject GetHeldItem()
    {
        return heldItem;
    }
    public void SetHeldItem(GameObject newHeldItem)
    {
        heldItem = newHeldItem;
        if (heldItem != null && newHeldItem.TryGetComponent(out Item newItemComponent))
            ItemComponent = newItemComponent;
    }

    public GameObject GetHeldGripPoint()
    {
        return heldGripPoint;
    }
    public void SetHeldGripPoint(GameObject newHeldGripPoint)
    {
        heldGripPoint = newHeldGripPoint;

        if (heldGripPoint != null && heldGripPoint.TryGetComponent(out Grip_Point newGripPointComponent))
            heldGripPointComponent = newGripPointComponent;
    }

    public HandType GetSide()
    {
        return side;
    }

    public bool GetGrippingState()
    {
        return gripping;
    }
    public void SetGrippingState(bool state)
    {
        gripping = state;
    }

    public void ForceUngrab()
    {
        interactComponent.Ungrab(gameObject);
    }

    private void Update()
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, controller.transform.position, 25 * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, controller.transform.rotation * handRotOffset, 25 * Time.deltaTime);
        //gameObject.transform.SetPositionAndRotation(controller.transform.position, controller.transform.rotation * handRotOffset);

        switch (side)
        {
            case HandType.Left:
                triggerSqueeze = XRControls.Character.TriggerLeft.ReadValue<float>();
                gripSqueeze = XRControls.Character.GrabLeft.ReadValue<float>();
                primaryButton = XRControls.Character.PrimaryModeLeft.ReadValue<float>();
                secondaryButton = XRControls.Character.SecondaryModeLeft.ReadValue<float>();
                break;

            case HandType.Right:
                triggerSqueeze = XRControls.Character.TriggerRight.ReadValue<float>();
                gripSqueeze = XRControls.Character.GrabRight.ReadValue<float>();
                primaryButton = XRControls.Character.PrimaryModeRight.ReadValue<float>();
                secondaryButton = XRControls.Character.SecondaryModeRight.ReadValue<float>();
                break;

        }

        bool primaryOrSecondary = (heldGripPointType == Grip_Point.GripPointType.Primary || heldGripPointType == Grip_Point.GripPointType.Secondary);

        if (ItemComponent && heldGripPointComponent && primaryOrSecondary)
        {
            heldGripPointComponent.SetInputs(triggerSqueeze, gripSqueeze, primaryButton, secondaryButton);
        }
    }

}
