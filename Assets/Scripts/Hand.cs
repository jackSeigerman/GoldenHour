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

    private Quaternion handRotOffset = Quaternion.Euler(90, 0, 0);


    public GameObject GetHeldItem()
    {
        return heldItem;
    }
    public void SetHeldItem(GameObject newHeldItem)
    {
        heldItem = newHeldItem;
    }

    public GameObject GetHeldGripPoint()
    {
        return heldGripPoint;
    }
    public void SetHeldGripPoint(GameObject newHeldGripPoint)
    {
        heldGripPoint = newHeldGripPoint;
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

    private void Update()
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, controller.transform.position, 12 * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, controller.transform.rotation * handRotOffset, 12 * Time.deltaTime);
    }

}
