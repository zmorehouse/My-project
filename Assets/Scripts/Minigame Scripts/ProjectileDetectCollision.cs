using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDetectCollision : MonoBehaviour
{

    public GameObject explosion; // drag your explosion prefab here
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
       

            GameObject expl = Instantiate(explosion, other.gameObject.transform.position, Quaternion.identity) as GameObject;
            Destroy(expl, 0.5f); // delete the explosion after 3 seconds
            Destroy(gameObject);
            Destroy(other.gameObject);   
           
        }
    }
}
