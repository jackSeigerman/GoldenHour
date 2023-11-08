using UnityEngine;
using UnityEngine.InputSystem;

public class XRRig_Behaviour : MonoBehaviour
{
    public static XRRig_Behaviour Instance { get; private set; }
    

    public Camera XRCamera;
    public GameObject leftController;
    public GameObject rightController;

    public XRControls XRControls;

    private void Awake()
    {
        // Singleton
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        // Create controls?
        XRControls = new XRControls();

        // Enable input
        SetInputState(true);

    }

    private void OnDestroy()
    {
        // Disable input
        SetInputState(false);
    }

    public void SetInputState(bool state)
    {
        if (state == true)
            XRControls.Enable();
        //else if(state == false)
            //XRControls.Disable();

    }

    public void SetInputActionMap(string branch) // Character or Menu
    {
        this.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap(branch);
        Debug.Log(this.gameObject.GetComponent<PlayerInput>().currentActionMap);
    }
}
