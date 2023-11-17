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
    // Start is called before the first frame update
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
        //if (itemComponent.GetInfluenceHand() == null || itemComponent.GetInfluenceHand().GetComponent<Hand>().GetSide() != Hand.HandType.Right) return;
        //Debug.Log("takingingPhoto");
        //takePhoto();
    }
    
   /* void FireRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData);
        string tag = hitData.collider.tag;
    }*/
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData))
        
        {
            Debug.Log(hitData.transform.name);
        }
    }
}
