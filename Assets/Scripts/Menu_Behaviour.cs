using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menu_Behaviour : MonoBehaviour
{
    [SerializeField] private GameObject pointerPrefab;

    private XRControls XRControls;

    private float exitCooldown = 0.2f; // time before the player can close menu in seconds

    private void Awake()
    {
        XRControls = XRRig_Behaviour.Instance.XRControls; // get XR controls
        
        // Subscribe to...
        XRControls.Menu.Exit.started += RequestMenuExit;
    }

    private void FixedUpdate()
    {
        exitCooldown = Mathf.Clamp01(exitCooldown - Time.deltaTime); // decriment timer for menu toggle cooldown
    }

    private void OnDestroy()
    { 
        // Unsubscribe from my channel
        XRControls.Menu.Exit.started -= RequestMenuExit;
    }

    private void RequestMenuExit(InputAction.CallbackContext context)
    {
        if (exitCooldown !> 0) return;

        Debug.Log("Remove");
        Summon_Pause_Menu.Instance.RemovePauseMenu();
    }

}
