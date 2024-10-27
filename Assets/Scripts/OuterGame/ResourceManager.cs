using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;  // Singleton instance

    // Track resources from previous islands
    public int totalCoconuts = 0;
    public int totalMangoes = 0;
    public int totalBananas = 0;

    // Track resources from the current island
    public int currentCoconuts = 0;
    public int currentMangoes = 0;
    public int currentBananas = 0;

    public TextMeshProUGUI BananaText; // UI element to display the quotas
    public TextMeshProUGUI LemonText; // UI element to display the quotas
    public TextMeshProUGUI WatermelonText; // UI element to display the quotas

    public TextMeshProUGUI resourceText;     // UI element to display resources

        private QuotaManager quotaManager;

    private void Awake()
    {
        // Singleton pattern to persist resources across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent the object from being destroyed on scene load
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Start()
    {                quotaManager = FindObjectOfType<QuotaManager>();

        // Update UI with the current resource count when the game starts
        UpdateResourceUI();
    }

    // Method to add coconuts on the current island
    public void AddCurrentCoconuts(int amount)
    {
        currentCoconuts += amount;
        Debug.Log($"Coconuts collected on this island: {currentCoconuts}");
        UpdateResourceUI();
    }

    // Method to add mangoes on the current island
    public void AddCurrentMangoes(int amount)
    {
        currentMangoes += amount;
        Debug.Log($"Mangoes collected on this island: {currentMangoes}");
        UpdateResourceUI();
    }

    // Method to add bananas on the current island
    public void AddCurrentBananas(int amount)
    {
        currentBananas += amount;
        Debug.Log($"Bananas collected on this island: {currentBananas}");
        UpdateResourceUI();
    }

    // Method to transfer current island resources to total after successful turn-in
    public void TransferCurrentResourcesToTotal()
    {
        totalCoconuts += currentCoconuts;
        totalMangoes += currentMangoes;
        totalBananas += currentBananas;

        // Reset current island resources
        ResetCurrentResources();

        Debug.Log("Transferred current island resources to total.");
        UpdateResourceUI();
    }

    // Method to reset current island resources (when hit by enemy or leaving island)
    public void ResetCurrentResources()
    {
        currentCoconuts = 0;
        currentMangoes = 0;
        currentBananas = 0;
        UpdateResourceUI();
    }

public void UpdateResourceUI()
{
    if (resourceText != null)
    {
        // Update Banana Text and color based on quota
        BananaText.text = $"{totalBananas + currentBananas} / {quotaManager.bananaQuota}";
        if (totalBananas + currentBananas >= quotaManager.bananaQuota)
        {
            BananaText.color = Color.green;
        }
        else
        {
            BananaText.color = Color.white;  // Default color
        }

        // Update Lemon Text and color based on quota
        LemonText.text = $"{totalCoconuts + currentCoconuts} / {quotaManager.coconutQuota}";
        if (totalCoconuts + currentCoconuts >= quotaManager.coconutQuota)
        {
            LemonText.color = Color.green;
        }
        else
        {
            LemonText.color = Color.white;  // Default color
        }

        // Update Watermelon Text and color based on quota
        WatermelonText.text = $"{totalMangoes + currentMangoes} / {quotaManager.mangoQuota}";
        if (totalMangoes + currentMangoes >= quotaManager.mangoQuota)
        {
            WatermelonText.color = Color.green;
        }
        else
        {
            WatermelonText.color = Color.white;  // Default color
        }
    }
}
    // Method to reset all resources (if needed)
    public void ResetAllResources()
    {
        totalCoconuts = 0;
        totalMangoes = 0;
        totalBananas = 0;
        ResetCurrentResources();  // Also reset current resources
        UpdateResourceUI();
    }
}
