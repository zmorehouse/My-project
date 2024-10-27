using System.Collections;
using UnityEngine;

public class PackageSpawner : MonoBehaviour
{
    public GameObject packagePrefab;          // Assign the package prefab in the Inspector
    public float checkRadius = 2.0f;          // Radius to check for colliders before spawning
    private float spawnAreaMinX = -50.0f;     // Minimum X bounds for the play area
    private float spawnAreaMaxX = 50.0f;      // Maximum X bounds for the play area
    private float spawnAreaMinZ = -35.0f;     // Minimum Z bounds for package spawning
    private float spawnAreaMaxZ = 50.0f;      // Maximum Z bounds for the play area
    private float spawnHeight = -0.51f;       // Y position for packages
    public Vector3 packageScale = new Vector3(1.17f, 1.17f, 1.17f);  // Slightly larger scale for packages
    private Quaternion packageRotation = Quaternion.Euler(-73.77f, 0f, 0f);  // Rotation on X axis

    // Method to spawn two packages at the start of a new day
    public void SpawnPackagesForNewDay()
    {
        StartCoroutine(SpawnPackages(2));  // Spawns 2 packages
    }

    // Coroutine to spawn a specified number of packages
    // Coroutine to spawn a specified number of packages
IEnumerator SpawnPackages(int count)
{
    int spawned = 0;
    int attempts = 0;
    const int maxAttempts = 20; // Limit attempts to avoid infinite loops

    while (spawned < count && attempts < maxAttempts * count)
    {
        // Generate a random position within bounds
        float randomX = Random.Range(spawnAreaMinX, spawnAreaMaxX);
        float randomZ = Random.Range(spawnAreaMinZ, spawnAreaMaxZ);
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, randomZ);

        // Check for colliders in the area to avoid overlapping
        if (!Physics.CheckSphere(spawnPosition, checkRadius))
        {
            GameObject package = Instantiate(packagePrefab, spawnPosition, packageRotation, transform);  // Set the spawner as the parent
            package.transform.localScale = packageScale;  // Increase the size of the package
            spawned++;
        }

        attempts++;
        yield return null;  // Wait a frame between attempts to avoid lag spikes
    }

    if (spawned < count)
    {
        Debug.LogWarning("Not enough clear positions found for all packages.");
    }
}

}
