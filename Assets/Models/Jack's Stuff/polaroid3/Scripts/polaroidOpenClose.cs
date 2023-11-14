using Palmmedia.ReportGenerator.Core.Parser.Filtering;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class polaroidOpenClose : MonoBehaviour
{
    [SerializeField] 
    private GameObject lid;
    [SerializeField]
    private GameObject openNoise;
    [SerializeField]
    private GameObject closeNoise;
    private bool state = false;
    private Vector3 open = new Vector3(1.4140029f, 180, 0);
    private Vector3 closed = new Vector3(344.910278f, 180, 0f);

    private GameObject XRRig;
    private XRControls XRControls;

    private Item itemComponent;

    void Start()
    {
        XRRig = XRRig_Behaviour.Instance.gameObject;
        XRControls = XRRig_Behaviour.Instance.XRControls;

        itemComponent = gameObject.GetComponent<Item>();

        XRControls.Character.PrimaryModeLeft.started += OpenLeft;
        XRControls.Character.PrimaryModeRight.started += OpenRight;
    }
    void checker()
    {
        if (state)
        {
            StartCoroutine(Coroutine2());
        }
        else
        {
            StartCoroutine(Coroutine1());
        }
    }
    IEnumerator Coroutine1()
    {
        for(float i = 0; i<1; i+=0.05f)
        {
            lid.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(closed), Quaternion.Euler(open),Mathf.SmoothStep(0,1,i));
            state = true;
            yield return null;
        }
        lid.transform.localRotation = Quaternion.Euler(open);
        openNoise.GetComponent<AudioSource>().Play();

        

    }
    IEnumerator Coroutine2()
    {
        for (float i = 0; i < 1; i += 0.05f)
        {
            lid.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(open), Quaternion.Euler(closed), Mathf.SmoothStep(0, 1, i));
            state = false;
            yield return null;
        }
        lid.transform.localRotation = Quaternion.Euler(closed);
        closeNoise.GetComponent<AudioSource>().Play();


    }


    private void OpenLeft(InputAction.CallbackContext context)
    {
        if (itemComponent.GetInfluenceHand() == null | itemComponent.GetInfluenceHand().GetComponent<Hand>().GetSide() != Hand.HandType.Left) return;
        checker();
    }

    private void OpenRight(InputAction.CallbackContext context)
    {
        if (itemComponent.GetInfluenceHand() == null | itemComponent.GetInfluenceHand().GetComponent<Hand>().GetSide() != Hand.HandType.Right) return;
        checker();
    }


}
