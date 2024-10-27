using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;
    public GameObject projectilePrefab;
    public GameObject player;
    public int disableShoot;

    // Start is called before the first frame update
    void Start()
    {
        //   disableShoot = true;  
        PlayerPrefs.SetInt("disableShoot", 1);
    }

    // Update is called once per frame
    void Update()
    {
        disableShoot = PlayerPrefs.GetInt("disableShoot");
        // Restrict firing
        if(!canFire && disableShoot != 1){
            timer += Time.deltaTime;
            if(timer > timeBetweenFiring){
                canFire = true;
                timer = 0;
            }
        }

        // Shoot
        if (Input.GetKey(KeyCode.Space) && canFire && disableShoot != 1){
            canFire = false;
            // player.transform.rotation
            Instantiate(projectilePrefab, transform.position, player.transform.rotation * Quaternion.Euler(0, 180, 0));
        }
    }
}
