using UnityEngine;

public class Artificial_Turn : MonoBehaviour
{

    public int turnRate = 360; // smooth turn degrees per second

    private GameObject XRRig;
    private XRControls XRControls;

    private void Start()
    {
        XRRig = XRRig_Behaviour.Instance.gameObject;
        XRControls = XRRig_Behaviour.Instance.XRControls;
    }

    void Update()
    {
        // get turn input
        float turnInput = XRControls.Character.Turn.ReadValue<float>();

        // Rotate the VR rig around the player head.
        XRRig.transform.RotateAround(XRRig_Behaviour.Instance.XRCamera.transform.position, Vector3.up, turnInput * turnRate * Time.deltaTime);
    }
}