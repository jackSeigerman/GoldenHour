using System.Collections;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public float forwardSpeed = 10.0f;
    public float descentSpeed = 1.0f;

    private bool isGrounded = false;
    private bool isFlying = false;
    private bool isLanding = false;
    private bool buffet2 = false;
    // Start is called before the first frame update
    void Start()
    {

    }



    void Update()
    {
        if (isFlying == false)
        {
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
            if(buffet2==false) 
            { 
                buffet2 = true; 
                StartCoroutine(buffet()); 
            }


        }
        if (isLanding == false)
        {
            Vector3 descentVector = Vector3.down * descentSpeed * Time.deltaTime;
            transform.Translate(descentVector);
        }
        if (isGrounded == false)
        {
            if (transform.position.y < 7.2f)
            {
                isGrounded = true;
                isLanding = true;
                StartCoroutine(ground());
                isFlying = true;
                StartCoroutine(slow());

            }
        }


    }
    IEnumerator ground()
    {
        Quaternion targetRotation = Quaternion.Euler(3.7f, 0f, 0f) * transform.rotation;
        for (float i = 0; i < 1; i += 0.15f * Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, i);
            yield return null;
        }

    }
    IEnumerator slow()
    {
        for (float i = forwardSpeed; i >= 0; i -= 3.7f * Time.deltaTime)
        {

            transform.Translate(Vector3.forward * i * Time.deltaTime);

            Debug.Log(i);
            yield return null;
        }

    }
    IEnumerator buffet()
    {

        // rotate z axis 
        for (float i = 0; !(isLanding); i += Time.deltaTime)
        {

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Sin(i)*2);
            yield return null;

        }
    }
}
