using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private double topBound = 25;
    private double bottomBound = -25;
    private double leftBound = -25;
    private double rightBound = 25;
    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > topBound) {
            Destroy(gameObject); 
        }
        else if (transform.position.z < bottomBound){
            Destroy(gameObject);
        }
        else if (transform.position.x > rightBound){
            Destroy(gameObject);
        }
        else if (transform.position.x < leftBound){
            Destroy(gameObject);
        }
    }
}
