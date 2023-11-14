using UnityEngine;

public class Locomotion : MonoBehaviour
{
    private GameObject capsule;

    private Transform CameraTransform;
    private GameObject XRRig;
    private Camera XRCamera;
    private CharacterController characterController;

    private Vector2 moveInput = Vector2.zero;
    private Vector3 lerpedMoveVector = Vector3.zero;

    private float moveMultiplier = 2.5f;
    private const int moveAcceleration = 14;
    private const float playspaceSpeedWindow = 1.2f;

    private const float minSprintThreshold = 0.7f;
    private const float sprintDelay = 0.5f;
    private float sprintCooldown = 0.5f;

    private const float gravity = 0.15f;
    private float gravityForce = 0f;


    private XRControls XRControls;

    private void Start()
    {
        capsule = gameObject;
        XRRig = XRRig_Behaviour.Instance.gameObject;
        XRCamera = XRRig_Behaviour.Instance.XRCamera;
        CameraTransform = XRCamera.GetComponent<Transform>();
        characterController = capsule.GetComponent<CharacterController>();

        XRControls = XRRig_Behaviour.Instance.XRControls;
    }

    private void Update()
    {
        Vector3 moveVector = Vector3.zero;

        moveInput = XRControls.Character.Move.ReadValue<Vector2>(); // set moveInput to the joystick vector2

        if (moveInput.y > minSprintThreshold && sprintCooldown > 0) // is the move stick in the sprinting position?
        {
            sprintCooldown -= Time.deltaTime; // count down the timer
            if (sprintCooldown <= 0) // is the cooldown done?
            {
                sprintCooldown = 0; // probably dont want to keep stacking a negative value but idunno
                moveMultiplier = Mathf.Lerp(moveMultiplier, 4.0f, 1f); // set speed to sprint
            }
        }
        else if (moveInput.y < minSprintThreshold) // is the move stick not in the sprinting position?
        {
            sprintCooldown = sprintDelay; // reset sprint cooldown
            moveMultiplier = 2.5f; // set speed to walk
        }

        float height = XRCamera.transform.position.y - XRRig.transform.position.y;
        if (height < 1)
        {
            moveMultiplier = 1.5f;
        }

        // Calculate the VR camera transform on a plane for the moveInput to add to
        Transform flatVectorTransform = CameraTransform;
        flatVectorTransform.rotation *= Quaternion.Euler(Vector3.right * -flatVectorTransform.eulerAngles.x) * Quaternion.Euler(Vector3.forward * -flatVectorTransform.eulerAngles.z);

        // integrate moveInput into the transform and vector3 and move character
        Vector3 rawMoveVector = Time.deltaTime * moveMultiplier * CameraTransform.TransformDirection(new Vector3(moveInput.x, 0, moveInput.y));
        rawMoveVector = Vector3.ProjectOnPlane(rawMoveVector, Vector3.up);

        // get the move vector with acceleration curve
        lerpedMoveVector = Vector3.Lerp(lerpedMoveVector, rawMoveVector, moveAcceleration * Time.deltaTime);

        // add lerpMoveVector to the total moveVector
        moveVector += lerpedMoveVector;

        // Gravity
        if (characterController.isGrounded && gravityForce > 0)
            gravityForce = 0;
        else
        {
            gravityForce += gravity * Time.deltaTime;
            characterController.Move(new Vector3(0, -gravityForce, 0));
        }

        // Convert roomscale to player offset
        Vector3 HMDOffset = new Vector3(XRCamera.transform.position.x - capsule.transform.position.x, 0, XRCamera.transform.position.z - capsule.transform.position.z);
        XRRig.transform.position -= HMDOffset;

        // add roomscale to the total moveVector
        moveVector += HMDOffset;

        MoveCharacter(moveVector); // ok cool
    }

    // the sum of all vectors will be capped by max player speed to prevent playspace abusing
    private void MoveCharacter(Vector3 newMoveVector)
    {
        float movementCap = moveMultiplier * Time.deltaTime * playspaceSpeedWindow;
        newMoveVector = new Vector3(Mathf.Clamp(newMoveVector.x, -movementCap, movementCap), 0, Mathf.Clamp(newMoveVector.z, -movementCap, movementCap));

        characterController.Move(newMoveVector);
    }


}
