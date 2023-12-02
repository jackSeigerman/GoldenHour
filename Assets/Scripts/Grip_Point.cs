using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Grip_Point : MonoBehaviour
{

    public enum GripPointType
    {
        Primary,
        Secondary,
        Fixed,
        Modular,
        
    }
    [SerializeField] private Hand.HandType compatibleHand;
    [SerializeField] private GripPointType gripPointType;
    [SerializeField] private bool canGripManually = true;

    private GameObject hand;
    [SerializeField] private bool held = false;
    private GameObject item;

    public float TriggerSqueeze { get; private set; }
    public float GripSqueeze { get; private set; }
    public bool PrimaryButton { get; private set; }
    public bool SecondaryButton { get; private set; }

    private void Start()
    {
        item = gameObject.transform.parent.gameObject;
    }

    public void SetHand(GameObject hand)
    {
        this.hand = hand;
    }
    public GameObject GetHand()
    {
        return this.hand;
    }

    public GameObject GetItem()
    {
        return this.item;
    }
    public Hand.HandType GetCompatibleHandType()
    {
        return this.compatibleHand;
    }
    
    public bool GetHeld()
    {
        return this.held;
    }
    public void SetHeld(bool value)
    {
        this.held = value;
    }
    public new GripPointType GetType()
    {
        return this.gripPointType;
    }
    public void SetInputs(float triggerSqueeze, float gripSqueeze, bool primaryButton, bool secondaryButton)
    {
        this.TriggerSqueeze = triggerSqueeze;
        this.GripSqueeze = gripSqueeze;
        this.PrimaryButton = primaryButton;
        this.SecondaryButton = secondaryButton;

    }

    public bool GetCanManuallyGrip()
    {
        return this.canGripManually;
    }

}
