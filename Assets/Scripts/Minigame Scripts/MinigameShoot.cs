using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameShoot : MonoBehaviour
{
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;
    public GameObject projectilePrefab;
    public GameObject player;
    public int disableShoot;
    public float bulletScale;
    
    // Shooting mode flags
    private bool isTripleShot;
    private bool isFullAuto;

    void Start()
    {
        // Check if Triple Shot or Full Auto is unlocked
        isTripleShot = PlayerPrefs.GetInt("TripleShot", 0) == 1;
        isFullAuto = PlayerPrefs.GetInt("FullAuto", 0) == 1;

        // Adjust time between shots for full auto mode
        if (isFullAuto)
        {
            timeBetweenFiring /= 2; // Full auto fires twice as fast
        }
    }

    void Update()
    {
        if (!canFire && disableShoot != 1)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        // Shoot when Space is pressed
        if (Input.GetKey(KeyCode.Space) && canFire && disableShoot != 1)
        {
            canFire = false;

            // Fire single, triple, or full auto based on the player's upgrades
            if (isTripleShot)
            {
                FireTripleShot();
            }
            else
            {
                FireSingleShot();
            }
        }
    }

    void FireSingleShot()
    {
        GameObject instance = Instantiate(projectilePrefab, transform.position, player.transform.rotation * Quaternion.Euler(0, 180, 0));
        AdjustBulletSize(instance);
    }

    void FireTripleShot()
    {
        // Fire three bullets in a spread
        for (int i = -1; i <= 1; i++)
        {
            Quaternion spreadRotation = Quaternion.Euler(0, 180 + (15 * i), 0); // Adjust spread angle as needed
            GameObject instance = Instantiate(projectilePrefab, transform.position, player.transform.rotation * spreadRotation);
            AdjustBulletSize(instance);
        }
    }

    void AdjustBulletSize(GameObject bullet)
    {
        int newSize = PlayerPrefs.GetInt("BulletSize");
        if (newSize == 1)
        {
            bullet.transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);
        }
    }
}
