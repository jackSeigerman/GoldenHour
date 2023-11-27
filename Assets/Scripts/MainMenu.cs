using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.InputSystem;
public class MainMenu : MonoBehaviour
{
    private GameObject XRRig;
    private XRControls XRControls;

    [SerializeField] private GameObject start;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject exit;

    void Start()
    {
        XRRig = XRRig_Behaviour.Instance.gameObject;
        XRControls = XRRig_Behaviour.Instance.XRControls;


        XRControls.Character.TriggerLeft.started += useLeft;
        XRControls.Character.TriggerRight.started += useRight;
        Ray ray = new Ray(transform.position, transform.forward);
    }

    private void useLeft(InputAction.CallbackContext context)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData))

        {
            Debug.Log(hitData.transform.name);

            if(hitData.transform.name.Equals("Exit"))
            {
                if (EditorApplication.isPlaying)
                {
                    EditorApplication.isPlaying = false;
                }
                else
                {
                    Application.Quit();
                }
            }
            if (hitData.transform.name.Equals("Credits"))
            {
                SceneManager.LoadScene("credits");
            }
            if (hitData.transform.name.Equals("Settings"))
            {

            }
            if (hitData.transform.name.Equals("Play"))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
        
    }
    private void useRight(InputAction.CallbackContext context)
    {

    }
    

    void Update()
    {
        start.transform.GetComponent<MeshRenderer>().material.color = Color.white;
        settings.transform.GetComponent<MeshRenderer>().material.color = Color.white;
        credits.transform.GetComponent<MeshRenderer>().material.color = Color.white;
        exit.transform.GetComponent<MeshRenderer>().material.color = Color.white;



        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData))
        {
            if ((hitData.transform.name.Equals("Exit")) || (hitData.transform.name.Equals("Credits")) || (hitData.transform.name.Equals("Settings")) || (hitData.transform.name.Equals("Play")))
            {
                hitData.transform.GetComponent<MeshRenderer>().material.color = Color.red;
            }
           

        }
        
    }
}
