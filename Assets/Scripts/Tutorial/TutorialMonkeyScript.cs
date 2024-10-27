using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMonkeyScript : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }


    // Alternatively, if you're using triggers instead of collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cannonball") )  // Ensure we have a valid player and controller
        {
            Destroy(gameObject);  

        }
    }
}
