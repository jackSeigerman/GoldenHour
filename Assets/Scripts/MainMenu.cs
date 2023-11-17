using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Scene mainMenu;
    [SerializeField] private GameObject ray;
    [SerializeField] private GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("mainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        //if ray.GetComponent<Rigidbody>().cS
    }
}
