using UnityEngine;
using UnityEngine.InputSystem;

public class Summon_Pause_Menu : MonoBehaviour
{

    public static Summon_Pause_Menu Instance { get; private set; }

    [SerializeField] private bool isInMenu = false;
    private GameObject currentPauseMenu;

    [SerializeField] private GameObject pauseMenuPrefab;

    private XRControls XRControls;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 instance not allowed");
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    private void Start()
    {
        XRControls = XRRig_Behaviour.Instance.XRControls; // get XRControls

        // Subscribe to...
        XRControls.Character.SummonMenu.started += RequestPauseMenu;

    }

    private void RequestPauseMenu(InputAction.CallbackContext context)
    {
        SummonPauseMenu(XRRig_Behaviour.Instance.XRCamera.transform);
        
    }

    private void SummonPauseMenu(Transform transform)
    {
        // Unsubscribe from the pause button listener
        XRControls.Character.SummonMenu.started -= RequestPauseMenu;

        isInMenu = true;

        XRRig_Behaviour.Instance.SetInputActionMap("Menu"); // set input actions to menu branch

        Transform flatVectorTransform = transform;
        flatVectorTransform.rotation *= Quaternion.Euler(Vector3.right * -flatVectorTransform.eulerAngles.x) * Quaternion.Euler(Vector3.forward * -flatVectorTransform.eulerAngles.z);

        currentPauseMenu = Instantiate(pauseMenuPrefab, flatVectorTransform.TransformPoint(new Vector3(-0.2f,0,0.5f)), flatVectorTransform.rotation);
        currentPauseMenu.transform.SetParent(XRRig_Behaviour.Instance.transform);
    }

    public void RemovePauseMenu()
    {
        Destroy(currentPauseMenu);
        isInMenu = false;
        XRRig_Behaviour.Instance.SetInputActionMap("Character"); // set input actions to character branch

        // resubscribe to...
        XRControls.Character.SummonMenu.started += RequestPauseMenu;
    }

    public void ToggleMenu() // public because the menu behaviour needs to remove menu on "resume" prompt
    {
        if (!isInMenu)
            SummonPauseMenu(XRRig_Behaviour.Instance.XRCamera.transform);
        else
            RemovePauseMenu();
    }
}
