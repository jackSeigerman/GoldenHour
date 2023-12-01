using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.InputSystem;
public class credits : MonoBehaviour
{
    private GameObject XRRig;
    private XRControls XRControls;


    [SerializeField] private GameObject exit;

    [SerializeField] private GameObject LC;
    [SerializeField] private GameObject RC;

    [SerializeField] private GameObject LR;
    [SerializeField] private GameObject RR;

    private GameObject x;
    void Start()
    {
        LR.SetActive(true);
        RR.SetActive(false);

        XRRig = XRRig_Behaviour.Instance.gameObject;
        XRControls = XRRig_Behaviour.Instance.XRControls;

        XRControls.Character.TriggerLeft.started += useLeft;
        XRControls.Character.TriggerRight.started += useRight;
        x = LC;
        Ray ray = new Ray(x.transform.position, x.transform.forward);
    }

    private void useLeft(InputAction.CallbackContext context)
    {
        switchLeft();
        Ray ray = new Ray(LC.transform.position, LC.transform.forward);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData))

        {

            if(hitData.transform.name.Equals("Exit"))
            {
                SceneManager.LoadScene("mainMenu");
            }
        
        }
    }
    private void useRight(InputAction.CallbackContext context)
    {
        switchRight();
        Ray ray = new Ray(RC.transform.position, RC.transform.forward);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData))

        {
            if(hitData.transform.name.Equals("Exit"))
            {
                SceneManager.LoadScene("mainMenu");
            }
        }
    }
    

    void Update()
    {
        exit.transform.GetComponent<MeshRenderer>().material.color = Color.white;



        Ray ray = new Ray(x.transform.position, x.transform.forward);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData))
        {
            Debug.Log(x);
            Debug.Log(hitData.transform.name);
            if (hitData.transform.name.Equals("Exit"))
            {
                hitData.transform.GetComponent<MeshRenderer>().material.color = Color.red;
            }
           

        }
        
    }
    private void switchLeft()
    {
        LR.SetActive(true);
        RR.SetActive(false);
        x = LC;
    }
    private void switchRight()
    {
        RR.SetActive(true);
        LR.SetActive(false);
        x = RC;
    }
}

