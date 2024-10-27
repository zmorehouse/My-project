using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuotaManager : MonoBehaviour
{
    public int requiredIslandLimit = 3;  // The number of island visits
    public int currentIslandVisits = 0;  // Track current visits

    // Initial ranges for random quotas
    private int minQuota = 1;
    private int maxQuota = 5;

    // Quotas for resources
    public int coconutQuota;
    public int mangoQuota;
    public int bananaQuota;

    public TextMeshProUGUI timeText; // UI element to display the time
    public TextMeshProUGUI quotaText; // UI element to display the quotas
    public TextMeshProUGUI instructionsText; // UI element to display instructions

    // Variables for controlling the lighting
    public Light mainLight;
    public Light quotaAreaLight; // New light to highlight quota area
    public GameObject quotaSpotlight;  // Assign QuotaSpotlight in the Inspector
    public float middayIntensity = 1.0f;
    public float sunsetIntensity = 0.6f;
    public float nightIntensity = 0.2f;
    public Color middayColor = Color.white;
    public Color sunsetColor = new Color(1.0f, 0.5f, 0.3f); // Soft orange for sunset
    public Color nightColor = new Color(0.1f, 0.1f, 0.2f); // Dark blue for night

    public TextMeshProUGUI moneyText;

    private bool hasTurnedInQuota = false;

    private ResourceManager resourceManager;

    private void Start()
    {
        UpdateTimeUI();
        UpdateInstructions();
        UpdateLighting(); // Initialize lighting at the start
        UpdateMoneyText();

        b1.gameObject.SetActive(true);
        b2.gameObject.SetActive(true);
        b3.gameObject.SetActive(true);

        resourceManager = ResourceManager.instance;


        // Initialize the quotas for the first stage
        SetRandomQuotas();
        UpdateQuotaUI();


        quotaAreaLight.enabled = false;


    quotaSpotlight = GameObject.FindGameObjectWithTag("QuotaSpotlight");    }

    public void IncrementIslandVisit()
    {
        currentIslandVisits++;
        UpdateTimeUI();   // Update the "time" UI based on visits
        UpdateInstructions();
        UpdateLighting(); // Update lighting when an island is visited
    }

    public bool HasMetIslandLimit()
    {
        return currentIslandVisits >= requiredIslandLimit;
    }

    public void ResetIslandLimit()
    {
        currentIslandVisits = 0;
        hasTurnedInQuota = false;
        Debug.Log("Island visits have been reset for the new cycle.");

        // Increment the quota ranges for a new stage
        minQuota += 2;
        maxQuota += 2;
        SetRandomQuotas();

        UpdateTimeUI();
        UpdateQuotaUI();
        UpdateInstructions();
        UpdateLighting();
    }

    private void SetRandomQuotas()
    {
        coconutQuota = Random.Range(minQuota, maxQuota + 1);
        mangoQuota = Random.Range(minQuota, maxQuota + 1);
        bananaQuota = Random.Range(minQuota, maxQuota + 1);
        Debug.Log($"New Quotas - Coconut: {coconutQuota}, Mango: {mangoQuota}, Banana: {bananaQuota}");
    }

    private void UpdateLighting()
    {
        if (mainLight != null)
        {
            if (currentIslandVisits == 0 || currentIslandVisits == 1)
            {
                mainLight.intensity = middayIntensity;
                mainLight.color = middayColor;

                if (quotaAreaLight != null) {
                    quotaAreaLight.enabled = false; // Disable quota light during day

                }
            }

            else if (currentIslandVisits == 2)
            {
                mainLight.intensity = sunsetIntensity;
                mainLight.color = sunsetColor;
                if (quotaAreaLight != null)
                {
                    quotaAreaLight.enabled = true; // Disable quota light during day
                }

            }
            else if (currentIslandVisits >= requiredIslandLimit && !hasTurnedInQuota)
            {
                mainLight.intensity = nightIntensity;
                mainLight.color = nightColor;

                if (quotaAreaLight != null)
                {
                    quotaAreaLight.enabled = true; // Enable quota light at end of day
                }
            }
        }
    }
    public GameObject b1,b2,b3;
    public GameObject skyDome;
 
    private void UpdateTimeUI()
    {
        switch (currentIslandVisits)
        {
            case 0:
                skyDome.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 45);  //Set Rotation value of y  to 0 and rest 0;

                b1.gameObject.SetActive(true);
                b2.gameObject.SetActive(true);
                b3.gameObject.SetActive(true);
                break;
            case 1:
                skyDome.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 135);  //Set Rotation value of y  to 0 and rest 0;

                b1.gameObject.SetActive(true);
                b2.gameObject.SetActive(true);
                b3.gameObject.SetActive(false);
                break;
            case 2:
                skyDome.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 225);  //Set Rotation value of y  to 0 and rest 0;

                b1.gameObject.SetActive(true);
                b2.gameObject.SetActive(false);
                b3.gameObject.SetActive(false);
                break;
            case 3:
                skyDome.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 315);  //Set Rotation value of y  to 0 and rest 0;

                b1.gameObject.SetActive(false);
                b2.gameObject.SetActive(false);
                b3.gameObject.SetActive(false);
                break;
            default:
                skyDome.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 45);  //Set Rotation value of y  to 0 and rest 0;

                b1.gameObject.SetActive(true);
                b2.gameObject.SetActive(true);
                b3.gameObject.SetActive(true);
                break;
        }
    }

    private void UpdateQuotaUI()
    {
        resourceManager.UpdateResourceUI();
    }

    public void UpdateInstructions()
    {
        if (instructionsText != null)
        {
            switch (currentIslandVisits)
            {
                case 0:
                    instructionsText.text = "The day is ahead of you!";
                    break;
                case 1:
                    instructionsText.text = "Midday! Keep pushing forward.";
                    break;
                case 2:
                    instructionsText.text = "The sun is setting! Almost there...";
                    break;
                case 3:
                    if (!hasTurnedInQuota)
                        instructionsText.text = "The day is over! Turn in your quota... or walk the plank!";
                    break;
                default:
                    if (hasTurnedInQuota)
                        instructionsText.text = "Good job! Here's your reward... now get back to work!";
                    break;
            }
        }
    }



    public void ShowMessage(string message)
    {
        if (instructionsText != null)
        {
            instructionsText.text = message;
        }
    }

    public void TurnInQuota()
    {
        hasTurnedInQuota = true;
        UpdateInstructions();
        UpdateLighting();
    }

    public void LoseGame()
    {
        Debug.Log("Player has failed to meet the quota. Game Over.");
        DestroyDontDestroyOnLoadObjects();
        PlayerPrefs.SetInt("Money", 0);
        PlayerPrefs.SetInt("SpeedUpgradeLevel", 0);
        PlayerPrefs.SetInt("HealthUpgradeLevel", 0);
        PlayerPrefs.SetInt("StamUpgradeLevel", 0);
        PlayerPrefs.SetInt("TripleShot", 0);
        PlayerPrefs.SetInt("FullAuto", 0);


        SceneManager.LoadScene("GameOver");
    }

    private void DestroyDontDestroyOnLoadObjects()
    {
        GameObject ScoreManager = GameObject.Find("ScoreManager");
        if (ScoreManager != null)
        {
            Destroy(ScoreManager);
            Debug.Log("ScoreManager destroyed.");
        }

        GameObject resourceManager = GameObject.Find("ResourceTracker");
        if (resourceManager != null)
        {
            Destroy(resourceManager);
            Debug.Log("ResourceManager destroyed.");
        }

        GameObject UIManager = GameObject.Find("UI");
        if (UIManager != null)
        {
            Destroy(UIManager);
            Debug.Log("UI destroyed.");
        }
        GameObject SpotLight = GameObject.Find("Spot Light");
        if (SpotLight != null)
        {
            Destroy(SpotLight);
            Debug.Log("SpotLight destroyed.");
        }
    }

    public void ShowAlreadyVisitedMessage()
    {
        ShowMessage("You've already visited that island today!");
    }

    
    public void UpdateMoneyText()
    {

            moneyText.text = $"Gold : ${PlayerPrefs.GetInt("Money")}";

       
    }

    public void turnOnLight() {
        quotaSpotlight.SetActive(true);
    }


}
