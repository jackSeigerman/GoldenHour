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

            }
            if (hitData.transform.name.Equals("back"))
            {
                SceneManager.LoadScene("mainMenu");
            }
 
    }
    private void useRight(InputAction.CallbackContext context)
    {

    }
    

    void Update()
    {

    }
}
