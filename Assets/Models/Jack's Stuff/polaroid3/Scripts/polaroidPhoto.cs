
using System.Collections;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class polaroidPhoto : MonoBehaviour
{
    private GameObject XRRig;
    private XRControls XRControls;
    private Item itemComponent;

    [SerializeField]
    private GameObject cameraPrefab;
    [SerializeField]
    private GameObject cameraLocation;
    [SerializeField]
    private GameObject polaroidPrefab;
    [SerializeField]
    private GameObject polaroidPrefabLocation;
    [SerializeField]
    private GameObject flash;
    [SerializeField]
    private GameObject sound;


    private GameObject currentPhoto;

    void Start()
    {
        XRRig = XRRig_Behaviour.Instance.gameObject;
        XRControls = XRRig_Behaviour.Instance.XRControls;

        itemComponent = gameObject.GetComponent<Item>();

        XRControls.Character.TriggerLeft.started += PhotoLeft;
        XRControls.Character.TriggerRight.started += PhotoRight;

    }

    private void PhotoLeft(InputAction.CallbackContext context)
    {
        if (itemComponent.GetInfluenceHand() == null || itemComponent.GetInfluenceHand().GetComponent<Hand>().GetSide() != Hand.HandType.Left) return;
        Debug.Log("takingingPhoto");
        takePhoto();
    }

    private void PhotoRight(InputAction.CallbackContext context)
    {
        if (itemComponent.GetInfluenceHand() == null || itemComponent.GetInfluenceHand().GetComponent<Hand>().GetSide() != Hand.HandType.Right) return;
        Debug.Log("takingingPhoto");
        takePhoto();
    }
    private void takePhoto()
    {
        // currentPhoto is the photo that is still ready to be plucked from the camera. if there is no photo in the camera, currentPhoto will be null.
        // We don't want another photo to be taken if there still is a photo ready to be plucked.
       if (currentPhoto != null) return;

        GameObject newCamera = Instantiate(cameraPrefab);
        newCamera.transform.SetPositionAndRotation(cameraLocation.transform.position, cameraLocation.transform.rotation);
        GameObject newPolaroid = Instantiate(polaroidPrefab, gameObject.transform);
        newPolaroid.transform.localScale = new(466.015f, 560.836f, 2.247187f);
        newPolaroid.transform.SetPositionAndRotation(polaroidPrefabLocation.transform.position, polaroidPrefabLocation.transform.rotation);
        RenderTexture rend = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32);
        newCamera.GetComponent<Camera>().targetTexture = rend;
        newPolaroid.GetNamedChild("renderTex").GetComponent<Renderer>().material.mainTexture = rend;
        StartCoroutine(Coroutine1(newCamera, newPolaroid));

        currentPhoto = newPolaroid;


    }
    //main
    IEnumerator Coroutine1(GameObject t, GameObject l)
    {
        //play photo sound
        sound.GetComponent<AudioSource>().Play();
        //wait a frame
        yield return null;
        //set camera inactive
        t.SetActive(false);
        //flash
        StartCoroutine(Coroutine3());
        //wait for .4 seconds
        StartCoroutine(Coroutine4());
        //start spitting out photo
        StartCoroutine(Coroutine2(l));
        //wait for photo
        StartCoroutine(Coroutine5(l));


    }
    //photo spit out
    IEnumerator Coroutine2(GameObject l)
    {
        Vector3 currentPosition = l.transform.localPosition;
        for (float i = 0; i < 1; i += 0.5f * Time.deltaTime)
        {
            l.transform.localPosition = Vector3.Lerp(currentPosition, currentPosition + new Vector3(0, 0, 14), Mathf.SmoothStep(0, 1, i));
            yield return null;
        }
    }
    //flash
    IEnumerator Coroutine3()
    {
        flash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        flash.SetActive(false);
    }
    //timer for photo spit out
    IEnumerator Coroutine4()
    {
        yield return new WaitForSeconds(.4f);
    }
    IEnumerator Coroutine5(GameObject l)
    {
        Item i = l.GetComponent<Item>();
        while (!(i.GetHeld()))
        {
            yield return null;
        }
        currentPhoto = null;
        GameObject cube = new GameObject("invisCube");
        cube.transform.localScale = new Vector3(0.00818518456f, 0.00818518456f, 0.00818518456f);
        l.transform.parent = cube.transform;

    } 
}


    

