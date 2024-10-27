using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoodSpawnManager : MonoBehaviour
{
    private int xrange = 25;
    private int zrange = 25;
    public float minTreeDistance = 5f; // Minimum distance between trees

    public GameObject treePrefab;
    public GameObject coconutPrefab;
    public GameObject bananaPrefab;
    public GameObject mangoPrefab;

    public float spawnInterval = 30.0f;

    private List<GameObject> spawnedTrees = new List<GameObject>();
    private List<GameObject> spawnedFruits = new List<GameObject>();

    private Scene minigameScene;

    public int quotaCoconut = 5;   // Example quotas
    public int quotaBanana = 5;
    public int quotaMango = 5;
    public int collectedCoconut = 0;
    public int collectedBanana = 0;
    public int collectedMango = 0;

    void Start()
    {
        minigameScene = SceneManager.GetSceneByName("Minigame");

        // Spawn initial trees
        SpawnInitialTrees(4);

        // Start the spawning coroutine
        StartCoroutine(SpawnTreesOverTime());
    }

    void SpawnInitialTrees(int treeCount)
    {
        for (int i = 0; i < treeCount; i++)
        {
            Vector3 treePosition = GenerateSpawnPosition();
            GameObject tree = Instantiate(treePrefab, treePosition, treePrefab.transform.rotation);
            SceneManager.MoveGameObjectToScene(tree, minigameScene);
            spawnedTrees.Add(tree);

            // Choose fruit type based on quotas
            string fruitType = ChooseRandomFruit();
            SpawnSpecificFood(fruitType, treePosition);
        }
    }

    // Coroutine to spawn trees over time
    IEnumerator SpawnTreesOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 treePosition = GenerateSpawnPosition();
            GameObject tree = Instantiate(treePrefab, treePosition, treePrefab.transform.rotation);
            SceneManager.MoveGameObjectToScene(tree, minigameScene);
            spawnedTrees.Add(tree);

            string fruitType = ChooseRandomFruit();
            SpawnSpecificFood(fruitType, treePosition);
        }
    }

    // Generate a position that maintains a minimum distance from other trees
    Vector3 GenerateSpawnPosition()
    {
        Vector3 position;
        bool validPosition;

        do
        {
            position = new Vector3(Random.Range(-xrange, xrange), 0.5f, Random.Range(-zrange, zrange));
            validPosition = true;

            // Check distance from all spawned trees
            foreach (GameObject tree in spawnedTrees)
            {
                if (Vector3.Distance(position, tree.transform.position) < minTreeDistance)
                {
                    validPosition = false;
                    break;
                }
            }
        } while (!validPosition);

        return position;
    }

    // Adjusted fruit selection logic based on quota
    string ChooseRandomFruit()
    {
        List<string> fruitChoices = new List<string>();

        if (collectedCoconut < quotaCoconut)
        {
            fruitChoices.Add("coconut");
        }
        if (collectedBanana < quotaBanana)
        {
            fruitChoices.Add("banana");
        }
        if (collectedMango < quotaMango)
        {
            fruitChoices.Add("mango");
        }

        // Add some fallback options if all quotas are met
        if (fruitChoices.Count == 0)
        {
            fruitChoices.Add("coconut");
            fruitChoices.Add("banana");
            fruitChoices.Add("mango");
        }

        return fruitChoices[Random.Range(0, fruitChoices.Count)];
    }

    void SpawnSpecificFood(string fruitType, Vector3 treePosition)
    {
        GameObject fruitPrefab = null;

        switch (fruitType)
        {
            case "coconut":
                fruitPrefab = coconutPrefab;
                break;
            case "banana":
                fruitPrefab = bananaPrefab;
                break;
            case "mango":
                fruitPrefab = mangoPrefab;
                break;
        }

        if (fruitPrefab != null)
        {
            int fruitCount = Random.Range(1, 5); // 1 to 4 fruits

            List<Vector3> fruitPositions = new List<Vector3>
            {
                new Vector3(treePosition.x + 1, 0.5f, treePosition.z),
                new Vector3(treePosition.x - 1, 0.5f, treePosition.z),
                new Vector3(treePosition.x, 0.5f, treePosition.z + 1),
                new Vector3(treePosition.x, 0.5f, treePosition.z - 1)
            };

            // Shuffle the positions
            for (int i = 0; i < fruitPositions.Count; i++)
            {
                Vector3 temp = fruitPositions[i];
                int randomIndex = Random.Range(i, fruitPositions.Count);
                fruitPositions[i] = fruitPositions[randomIndex];
                fruitPositions[randomIndex] = temp;
            }

            for (int i = 0; i < fruitCount; i++)
            {
                GameObject fruit = Instantiate(fruitPrefab, fruitPositions[i], fruitPrefab.transform.rotation);
                SceneManager.MoveGameObjectToScene(fruit, minigameScene);
                spawnedFruits.Add(fruit);
            }
        }
    }

    // Destroy all spawned objects when leaving the minigame scene
    public void DestroyAllSpawnedObjects()
    {
        foreach (GameObject tree in spawnedTrees)
        {
            if (tree != null)
            {
                Destroy(tree);
            }
        }

        foreach (GameObject fruit in spawnedFruits)
        {
            if (fruit != null)
            {
                Destroy(fruit);
            }
        }

        spawnedTrees.Clear();
        spawnedFruits.Clear();
    }
}
