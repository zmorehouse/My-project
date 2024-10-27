using UnityEngine;
using UnityEngine.SceneManagement;

public class QuotaTurnInTrigger : MonoBehaviour
{
    private QuotaManager quotaManager;
    private ResourceManager resourceManager;
    private ShipController shipController;
    private bool isPlayerInRange = false;
    private GameObject[] sceneObjects;  // To hold the original scene objects

    // Reference to Warning UI GameObject
    public GameObject warningUI;

    private void Start()
    {
        // Find the QuotaManager in the scene
        quotaManager = FindObjectOfType<QuotaManager>();
    
        // Find the ResourceManager
        resourceManager = ResourceManager.instance;

        // Find the ShipController
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            shipController = player.GetComponent<ShipController>();
        }

        // Get all objects in the original scene
        sceneObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        // Ensure Warning UI is hidden initially
        if (warningUI != null)
        {
            warningUI.SetActive(false);
        }
    }

    private void Update()
    {
        // Check if the player is in range and presses the 'E' key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (quotaManager.currentIslandVisits < quotaManager.requiredIslandLimit)
            {
                // Show the Warning UI if there's still time left in the day
                ShowWarningUI();
            }
            else
            {
                // Proceed to dialogue scene if the day is over
                TriggerDialogueScene();
                quotaManager.ShowMessage("");
            }
        }
    }

    private void ShowWarningUI()
    {
        if (warningUI != null)
        {
            warningUI.SetActive(true);
            Time.timeScale = 0f;  // Pause

        }
    }

    private void TriggerDialogueScene()
    {
        // Hide the Warning UI just in case
        if (warningUI != null)
        {
            warningUI.SetActive(false);
        }

        // Disable the original scene objects to put it on "hold"
        HideOriginalSceneObjects();

        // Load the QuotaChecker scene in Additive mode
        SceneManager.LoadScene("QuotaChecker", LoadSceneMode.Additive);
    }

    // Method to hide all original scene objects
    private void HideOriginalSceneObjects()
    {
        foreach (GameObject obj in sceneObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);  // Disable each object in the original scene
            }
        }
    }

    // Method to show the original scene objects (called after dialogue)
    public void ShowOriginalSceneObjects()
    {
        foreach (GameObject obj in sceneObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);  // Re-enable each object in the original scene
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (quotaManager != null)
            {
                quotaManager.ShowMessage("Press E to turn in your quota!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (quotaManager != null)
            {
                quotaManager.UpdateInstructions();
            }

            // Hide the Warning UI when the player leaves the trigger
            if (warningUI != null)
            {
                warningUI.SetActive(false);

            }
        }
    }

    // Method to close the Warning UI when "Close" button is clicked
    public void CloseWarningUI()
    {
        if (warningUI != null)
        {
            warningUI.SetActive(false);
            Time.timeScale = 1f;  // Resume the game

        }
    }

    // Method to continue to the quota scene when "Continue" button is clicked
    public void ContinueToQuotaScene()
    {
        if (warningUI != null)
        {
            warningUI.SetActive(false);
            Time.timeScale = 1f;  // Resume the game

        }
        TriggerDialogueScene();
    }
}
