using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class engineSpin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector3 rotationDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed*rotationDirection*Time.deltaTime);
    }
}
