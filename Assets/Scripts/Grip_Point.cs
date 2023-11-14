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
        
    }
    [SerializeField] private Hand.HandType compatibleHand;
    [SerializeField] private GripPointType gripPointType;

    [SerializeField] private GameObject hand;
    [SerializeField] private bool held = false;
    private GameObject item;

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
        return hand;
    }

    public GameObject GetItem()
    {
        return item;
    }
    public Hand.HandType GetCompatibleHandType()
    {
        return compatibleHand;
    }
    
    public bool GetHeld()
    {
        return held;
    }
    public void SetHeld(bool value)
    {
        this.held = value;
    }
    public new GripPointType GetType()
    {
        return gripPointType;
    }
}
