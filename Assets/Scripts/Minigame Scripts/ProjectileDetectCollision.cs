using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDetectCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other) {
        Debug.Log("HIT");

        //Destroy objects if monkey is hit
        if (other.gameObject.CompareTag("Enemy")){ 
            Destroy(gameObject);
            Destroy(other.gameObject);   
        }
    }
}
