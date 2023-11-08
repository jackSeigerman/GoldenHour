using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public float forwardSpeed = 10.0f;
    public float descentSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }



    void Update()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        Vector3 descentVector = Vector3.down * descentSpeed * Time.deltaTime;
        transform.Translate(descentVector);
    }
}
