using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // For UI text

public class SimpleEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;   // Assign your enemy prefab in the Inspector
    public float initialSpawnInterval = 3.0f; // Starting interval for spawning enemies
    public float spawnRange = 25f;   // Range within which enemies can be spawned
    public TextMeshProUGUI ferocityText; // UI element to display "monkey ferocity" level
    public Material[] monkeyMaterials; // Array of materials to randomize monkey colors

    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Track spawned enemies
    private Scene minigameScene; // Reference to the minigame scene
    private float currentSpawnInterval;
    private int ferocityLevel = 1; // Initial ferocity level
    private float rampUpTime = 22.5f; // Time interval to ramp up ferocity

    // Array of descriptive ferocity levels
    private string[] ferocityDescriptions = { "Chill", "Cool", "Heated", "Wild", "Intense", "FRENZY!" };

    void Start()
    {
        // Ensure the minigame scene is loaded
        minigameScene = SceneManager.GetSceneByName("minigame");
        if (!minigameScene.IsValid())
        {
            Debug.LogError("Minigame scene not found!");
            return;
        }

        currentSpawnInterval = initialSpawnInterval;

        // Start the enemy spawning and ferocity ramping coroutines
        StartCoroutine(SpawnEnemiesOverTime());
        StartCoroutine(RampUpFerocity());
    }

    // Coroutine to spawn enemies over time with the current spawn interval
    IEnumerator SpawnEnemiesOverTime()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(currentSpawnInterval); // Wait for the interval before spawning the next enemy
        }
    }

    // Coroutine to increase ferocity level and reduce spawn interval over time
    IEnumerator RampUpFerocity()
    {
        while (true)
        {
            yield return new WaitForSeconds(rampUpTime); // Wait for the next ramp-up

            ferocityLevel++;
            currentSpawnInterval = Mathf.Max(1.0f, currentSpawnInterval - 0.5f); // Reduce interval, min value of 1 sec
            UpdateFerocityText();
        }
    }

    // Method to spawn a new enemy with random scale and color
void SpawnEnemy()
{
    GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find the player for distance calculation
    Vector3 spawnPosition;

    // Loop to find a spawn position outside the 12.5f radius from the player
    do
    {
        spawnPosition = new Vector3(
            Random.Range(-spawnRange, spawnRange), 
            -0.25f, 
            Random.Range(-spawnRange, spawnRange)
        );
    } while (player != null && Vector3.Distance(spawnPosition, player.transform.position) < 12.5f);

    // Instantiate the enemy at the valid spawn position
    GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

    // Apply random scale
    float randomScale = Random.Range(0.9f, 2.25f);
    enemy.transform.localScale = new Vector3(randomScale, randomScale, randomScale + 1.25f);

    // Apply random color from materials array
    if (monkeyMaterials.Length > 0)
    {
        Material randomMaterial = monkeyMaterials[Random.Range(0, monkeyMaterials.Length)];
        enemy.GetComponent<Renderer>().material = randomMaterial;
    }

    // Move the enemy to the minigame scene and set it as a child of the minigame scene
    SceneManager.MoveGameObjectToScene(enemy, minigameScene);
    spawnedEnemies.Add(enemy); // Track the spawned enemy
}


    // Update the UI text to show the current "monkey ferocity" level description
    void UpdateFerocityText()
    {
        if (ferocityText != null)
        {
            string ferocityDescription = ferocityLevel <= 5 ? ferocityDescriptions[ferocityLevel - 1] : ferocityDescriptions[5];
            ferocityText.text = $"Monkey Ferocity: {ferocityDescription}";
        }
    }

    // Destroy all spawned enemies when leaving the minigame scene
    public void DestroyAllSpawnedEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        spawnedEnemies.Clear();
    }
}
