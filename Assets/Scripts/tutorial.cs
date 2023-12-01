using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.InputSystem;
public class tutorial : MonoBehaviour
{
    private GameObject XRRig;
    private XRControls XRControls;


    [SerializeField] private GameObject exit;

    [SerializeField] private GameObject LC;
    [SerializeField] private GameObject RC;

    [SerializeField] private GameObject LR;
    [SerializeField] private GameObject RR;


    [SerializeField] private Material clear;
    [SerializeField] private Material selected;

    [SerializeField] private GameObject grip;
    [SerializeField] private GameObject grip2;
    [SerializeField] private GameObject grip3;
    [SerializeField] private GameObject grip4;
    [SerializeField] private GameObject trigger;
    [SerializeField] private GameObject trigger2;
    [SerializeField] private GameObject move;
    [SerializeField] private GameObject move2;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject button2;

    [SerializeField] private GameObject text;
    [SerializeField] private GameObject text2;
    [SerializeField] private GameObject text3;
    [SerializeField] private GameObject text4;
    [SerializeField] private GameObject text5;


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

        StartCoroutine(wait());

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


    private IEnumerator wait()
    {
        text.SetActive(true);
        yield return new WaitForSeconds(3f);
        text.SetActive(false);
        StartCoroutine(gripe());
    }

    private IEnumerator gripe()
    {
        text2.SetActive(true);

        grip.GetComponent<MeshRenderer>().material = selected;
        grip2.GetComponent<MeshRenderer>().material = selected;
        grip3.GetComponent<MeshRenderer>().material = selected;
        grip4.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        grip.GetComponent<MeshRenderer>().material = clear;
        grip2.GetComponent<MeshRenderer>().material = clear;
        grip3.GetComponent<MeshRenderer>().material = clear;
        grip4.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        grip.GetComponent<MeshRenderer>().material = selected;
        grip2.GetComponent<MeshRenderer>().material = selected;
        grip3.GetComponent<MeshRenderer>().material = selected;
        grip4.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        grip.GetComponent<MeshRenderer>().material = clear;
        grip2.GetComponent<MeshRenderer>().material = clear;
        grip3.GetComponent<MeshRenderer>().material = clear;
        grip4.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        grip.GetComponent<MeshRenderer>().material = selected;
        grip2.GetComponent<MeshRenderer>().material = selected;
        grip3.GetComponent<MeshRenderer>().material = selected;
        grip4.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        grip.GetComponent<MeshRenderer>().material = clear;
        grip2.GetComponent<MeshRenderer>().material = clear;
        grip3.GetComponent<MeshRenderer>().material = clear;
        grip4.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        grip.GetComponent<MeshRenderer>().material = selected;
        grip2.GetComponent<MeshRenderer>().material = selected;
        grip3.GetComponent<MeshRenderer>().material = selected;
        grip4.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        grip.GetComponent<MeshRenderer>().material = clear;
        grip2.GetComponent<MeshRenderer>().material = clear;
        grip3.GetComponent<MeshRenderer>().material = clear;
        grip4.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        grip.GetComponent<MeshRenderer>().material = selected;
        grip2.GetComponent<MeshRenderer>().material = selected;
        grip3.GetComponent<MeshRenderer>().material = selected;
        grip4.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        grip.GetComponent<MeshRenderer>().material = clear;
        grip2.GetComponent<MeshRenderer>().material = clear;
        grip3.GetComponent<MeshRenderer>().material = clear;
        grip4.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);

        text2.SetActive(false);
        yield return new WaitForSeconds(2f);
        StartCoroutine(triggere());
    }

    private IEnumerator triggere()
    {
        text3.SetActive(true);

        trigger.GetComponent<MeshRenderer>().material = selected;
        trigger2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        trigger.GetComponent<MeshRenderer>().material = clear;
        trigger2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        trigger.GetComponent<MeshRenderer>().material = selected;
        trigger2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        trigger.GetComponent<MeshRenderer>().material = clear;
        trigger2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        trigger.GetComponent<MeshRenderer>().material = selected;
        trigger2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        trigger.GetComponent<MeshRenderer>().material = clear;
        trigger2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        trigger.GetComponent<MeshRenderer>().material = selected;
        trigger2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        trigger.GetComponent<MeshRenderer>().material = clear;
        trigger2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        trigger.GetComponent<MeshRenderer>().material = selected;
        trigger2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        trigger.GetComponent<MeshRenderer>().material = clear;
        trigger2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);

        text3.SetActive(false);
        yield return new WaitForSeconds(2f);
        StartCoroutine(movee());
    }

    private IEnumerator movee()
    {
        text4.SetActive(true);

        move.GetComponent<MeshRenderer>().material = selected;
        move2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        move.GetComponent<MeshRenderer>().material = clear;
        move2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        move.GetComponent<MeshRenderer>().material = selected;
        move2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        move.GetComponent<MeshRenderer>().material = clear;
        move2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        move.GetComponent<MeshRenderer>().material = selected;
        move2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        move.GetComponent<MeshRenderer>().material = clear;
        move2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        move.GetComponent<MeshRenderer>().material = selected;
        move2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        move.GetComponent<MeshRenderer>().material = clear;
        move2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        move.GetComponent<MeshRenderer>().material = selected;
        move2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        move.GetComponent<MeshRenderer>().material = clear;
        move2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);


        text4.SetActive(false);
        yield return new WaitForSeconds(2f);
        StartCoroutine(opene());
    }

    private IEnumerator opene()
    {
        text5.SetActive(true);

        button.GetComponent<MeshRenderer>().material = selected;
        button2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        button.GetComponent<MeshRenderer>().material = clear;
        button2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        button.GetComponent<MeshRenderer>().material = selected;
        button2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        button.GetComponent<MeshRenderer>().material = clear;
        button2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        button.GetComponent<MeshRenderer>().material = selected;
        button2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        button.GetComponent<MeshRenderer>().material = clear;
        button2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        button.GetComponent<MeshRenderer>().material = selected;
        button2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        button.GetComponent<MeshRenderer>().material = clear;
        button2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);
        button.GetComponent<MeshRenderer>().material = selected;
        button2.GetComponent<MeshRenderer>().material = selected;
        yield return new WaitForSeconds(1f);
        button.GetComponent<MeshRenderer>().material = clear;
        button2.GetComponent<MeshRenderer>().material = clear;
        yield return new WaitForSeconds(1f);

        text5.SetActive(false);
        yield return new WaitForSeconds(2f);
        StartCoroutine(wait());
    }
}

