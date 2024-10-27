// This script is responsible for managing the shop UI and player upgrades. It handles the purchase of upgrades and displays messages to the player. 
// It also keeps track of the player's score, money, and lives as these directly correlate with the upgrades available in the shop.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopLogic : MonoBehaviour
{
    private QuotaTurnInTrigger q;
    public float spawnOffset = 7.5f;

    // Money Logic
    public int money = 0;
    public TextMeshProUGUI moneyText;

    public TextMeshProUGUI bigBulletDescription;
    public TextMeshProUGUI movementSpeedDescription;
    public TextMeshProUGUI maxHealthDescription;
    public TextMeshProUGUI maxStamDescription;
    public TextMeshProUGUI tripleShotDescription;
    public TextMeshProUGUI fullAutoDescription;

    public Button biggerBulletButton;
    public Button movementSpeedButton;
    public Button maxHpButton;
    public Button maxStamButton;
    public Button tripleShotButton;
    public Button fullAutoButton;

    public TextMeshProUGUI messageText;
    public TextMeshProUGUI inshopbalanceText;

    public QuotaManager quotaManager;

    // Upgrade Levels
    public int bulletUpgradeLevel = 0;
    public int speedUpgradeLevel = 0;
    public int healthUpgradeLevel = 0;
    public int stamUpgradeLevel = 0;
    public bool hasTripleShot = false;
    public bool hasFullAuto = false;

    void Start()
    {
        q = FindObjectOfType<QuotaTurnInTrigger>();
        quotaManager = FindObjectOfType<QuotaManager>();
        LoadCurrentUpgradeLevels(); // Load upgrades on start
        UpdateMoneyText();
    }

    // Method to load the current upgrade levels from PlayerPrefs
    private void LoadCurrentUpgradeLevels()
    {
        // Load each upgrade level from PlayerPrefs (default to 0 if not set)
        bulletUpgradeLevel = PlayerPrefs.GetInt("BulletSize", 0);
        speedUpgradeLevel = PlayerPrefs.GetInt("SpeedUpgradeLevel", 0);
        healthUpgradeLevel = PlayerPrefs.GetInt("HealthUpgradeLevel", 0);
        stamUpgradeLevel = PlayerPrefs.GetInt("StamUpgradeLevel", 0);
        hasTripleShot = PlayerPrefs.GetInt("TripleShot", 0) == 1;
        hasFullAuto = PlayerPrefs.GetInt("FullAuto", 0) == 1;

        // Update UI text and button states based on loaded levels
        bigBulletDescription.text = $"Bigger Bullets ({bulletUpgradeLevel}/1) : $200";
        movementSpeedDescription.text = $"Increase Movement Speed ({speedUpgradeLevel}/3): $30";
        maxHealthDescription.text = $"Increase Maximum Health ({healthUpgradeLevel}/3): $50";
        maxStamDescription.text = $"Increase Maximum Stamina ({stamUpgradeLevel}/3): $25";
        tripleShotDescription.text = $"Triple Shot: $150";
        fullAutoDescription.text = $"Full Auto: $300";

        // Disable buttons if the upgrade is fully purchased
        if (bulletUpgradeLevel == 1)
        {
            biggerBulletButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
            biggerBulletButton.interactable = false;
        }
        if (speedUpgradeLevel == 3)
        {
            movementSpeedButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
            movementSpeedButton.interactable = false;
        }
        if (healthUpgradeLevel == 3)
        {
            maxHpButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
            maxHpButton.interactable = false;
        }
        if (stamUpgradeLevel == 3)
        {
            maxStamButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
            maxStamButton.interactable = false;
        }
        if (hasTripleShot)
        {
            tripleShotButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
            tripleShotButton.interactable = false;
        }
        if (hasFullAuto)
        {
            fullAutoButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
            fullAutoButton.interactable = false;
        }
    }

public void biggerBulletButtonPress()
{
    int cost = 200;
    int m = PlayerPrefs.GetInt("Money");

    if (m >= cost && bulletUpgradeLevel < 1)
    {
        bulletUpgradeLevel += 1;
        bigBulletDescription.text = $"Bigger Bullets ({bulletUpgradeLevel}/1)";
        PlayerPrefs.SetInt("BulletSize", bulletUpgradeLevel);
        PlayerPrefs.SetInt("Money", m - cost);
        UpdateMoneyText();

        // Mark as sold and display success message
        biggerBulletButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
        biggerBulletButton.interactable = false;
        messageText.text = "Enjoy your bigger bullets!";
    }
    else
    {
        messageText.text = "You don't have enough money for that!";
    }
}

public void movementButtonPress()
{
    int cost = 30;
    int m = PlayerPrefs.GetInt("Money");

    if (m >= cost && speedUpgradeLevel < 3)
    {
        speedUpgradeLevel += 1;
        movementSpeedDescription.text = $"Increase Movement Speed ({speedUpgradeLevel}/3): $30";
        PlayerPrefs.SetInt("SpeedUpgradeLevel", speedUpgradeLevel);
        PlayerPrefs.SetInt("Money", m - cost);
        UpdateMoneyText();
        messageText.text = "Enjoy your speed upgrade!";

        // Mark as sold if at max level
        if (speedUpgradeLevel == 3)
        {
            movementSpeedButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
            movementSpeedButton.interactable = false;
        }
    }
    else
    {
        messageText.text = "You don't have enough money for that!";
    }
}

public void healthButtonPress()
{
    int cost = 50;
    int m = PlayerPrefs.GetInt("Money");

    if (m >= cost && healthUpgradeLevel < 3)
    {
        healthUpgradeLevel += 1;
        maxHealthDescription.text = $"Increase Maximum Health ({healthUpgradeLevel}/3): $50";
        PlayerPrefs.SetInt("HealthUpgradeLevel", healthUpgradeLevel);
        PlayerPrefs.SetInt("Money", m - cost);
        UpdateMoneyText();
        messageText.text = "Enjoy your health upgrade!";

        // Mark as sold if at max level
        if (healthUpgradeLevel == 3)
        {
            maxHpButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
            maxHpButton.interactable = false;
        }
    }
    else
    {
        messageText.text = "You don't have enough money for that!";
    }
}

public void stamButtonPress()
{
    int cost = 25;
    int m = PlayerPrefs.GetInt("Money");

    if (m >= cost && stamUpgradeLevel < 3)
    {
        stamUpgradeLevel += 1;
        maxStamDescription.text = $"Increase Maximum Stamina ({stamUpgradeLevel}/3): $25";
        PlayerPrefs.SetInt("StamUpgradeLevel", stamUpgradeLevel);
        PlayerPrefs.SetInt("Money", m - cost);
        UpdateMoneyText();
        messageText.text = "Enjoy your stamina upgrade!";

        // Mark as sold if at max level
        if (stamUpgradeLevel == 3)
        {
            maxStamButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
            maxStamButton.interactable = false;
        }
    }
    else
    {
        messageText.text = "You don't have enough money for that!";
    }
}


    // New Method for Triple Shot purchase
    public void TripleShotButtonPress()
    {
        int cost = 300;
        int m = PlayerPrefs.GetInt("Money");

        if (m >= cost && !hasTripleShot)
        {
            hasTripleShot = true;
            PlayerPrefs.SetInt("TripleShot", 1);
            PlayerPrefs.SetInt("Money", m - cost);
            UpdateMoneyText();

            // Mark as sold and display success message
            tripleShotButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
            tripleShotButton.interactable = false;
            messageText.text = "You now have Triple Shot!";
        }
        else
        {
            messageText.text = "You don't have enough money for that!";
        }
    }

    // New Method for Full Auto purchase
    public void FullAutoButtonPress()
    {
        int cost = 500;
        int m = PlayerPrefs.GetInt("Money");

        if (m >= cost && !hasFullAuto)
        {
            hasFullAuto = true;
            PlayerPrefs.SetInt("FullAuto", 1);
            PlayerPrefs.SetInt("Money", m - cost);
            UpdateMoneyText();

            // Mark as sold and display success message
            fullAutoButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
            fullAutoButton.interactable = false;
            messageText.text = "You now have Full Auto!";
        }
        else
        {
            messageText.text = "You don't have enough money for that!";
        }
    }

    public void UpdateMoneyText()
    {
        quotaManager.UpdateMoneyText();
        inshopbalanceText.text = $"Gold: ${PlayerPrefs.GetInt("Money")}";
    }
}


    //     public void PurchaseSpeedUpgrade1()
    // {
    //     int cost = 75; // Cost of the first speed upgrade

    //     if (money >= cost && speedUpgradeLevel == 0)
    //     {
    //         // Deduct the money and increase speed
    //         money -= cost;
    //         baseSpeed += speedIncrement;
    //         speedUpgradeLevel = 1;
    //         UpdateMoneyText();
    //         UpdateBalanceText();
    //         UpdateSpeedUpgradeButtons();

    //         DisplayMessage("Purchased Speed Upgrade 1!");
    //     }
    //     else
    //     {
    //         DisplayMessage("You don't have enough gold for that.");
    //     }
    // }








/*
    public bool hasForwardShootingUpgrade = false; 

    public void PurchaseExtraLife()
    {
        int lifeCost = 100;

        if (money >= lifeCost && currentLives < maxLives)
        {
            money -= lifeCost;
            UpdateMoneyText();
            UpdateBalanceText();

            currentLives += 1;
            UpdateLifeText();

            DisplayMessage("Purchased an extra life!");
        }
        else if (currentLives >= maxLives)
        {
            DisplayMessage("You already have the maximum number of lives!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void PurchaseShootingUpgrade()
    {
        int upgradeCost = 250; 

        if (money >= upgradeCost && !hasForwardShootingUpgrade)
        {
            // Deduct the money
            money -= upgradeCost;
            UpdateMoneyText();
            UpdateBalanceText(); // Update the balance display in the shop

            // Enable the forward shooting upgrade
            hasForwardShootingUpgrade = true;
            forwardShootingUpgradeButton.interactable = false; // Enable the Pirate Patience upgrade 1 button
            DisplayMessage("Purchased forward shooting upgrade!");
        }
        else if (hasForwardShootingUpgrade)
        {
            DisplayMessage("You already have that upgrade!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void UpdateBalanceText()
    {
        balanceText.text = $"Current Balance: ${money}";
    }

    public void DisplayMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
    }

    public void PurchaseSpeedUpgrade1()
    {
        int cost = 75; // Cost of the first speed upgrade

        if (money >= cost && speedUpgradeLevel == 0)
        {
            // Deduct the money and increase speed
            money -= cost;
            baseSpeed += speedIncrement;
            speedUpgradeLevel = 1;
            UpdateMoneyText();
            UpdateBalanceText();
            UpdateSpeedUpgradeButtons();

            DisplayMessage("Purchased Speed Upgrade 1!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void PurchaseSpeedUpgrade2()
    {
        int cost = 150; // Cost of the second speed upgrade

        if (money >= cost && speedUpgradeLevel == 1)
        {
            // Deduct the money and increase speed
            money -= cost;
            baseSpeed += speedIncrement;
            speedUpgradeLevel = 2;
            UpdateMoneyText();
            UpdateBalanceText();
            UpdateSpeedUpgradeButtons();

            DisplayMessage("Purchased Speed Upgrade 2!");
        }
        else if (speedUpgradeLevel < 1)
        {
            DisplayMessage("You need to purchase Speed Upgrade 1 first!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void PurchaseSpeedUpgrade3()
    {
        int cost = 250; // Cost of the third speed upgrade

        if (money >= cost && speedUpgradeLevel == 2)
        {
            // Deduct the money and increase speed
            money -= cost;
            baseSpeed += speedIncrement;
            speedUpgradeLevel = 3;
            UpdateMoneyText();
            UpdateBalanceText();
            UpdateSpeedUpgradeButtons();

            DisplayMessage("Purchased Speed Upgrade 3!");
        }
        else if (speedUpgradeLevel < 2)
        {
            DisplayMessage("You need to purchase Speed Upgrade 2 first!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void UpdateSpeedUpgradeButtons()
    {
        // Enable or disable buttons based on current speed upgrade level
        if (speedUpgrade1Button != null)
        {
            speedUpgrade1Button.interactable = (speedUpgradeLevel == 0);
        }

        if (speedUpgrade2Button != null)
        {
            speedUpgrade2Button.interactable = (speedUpgradeLevel == 1);
        }

        if (speedUpgrade3Button != null)
        {
            speedUpgrade3Button.interactable = (speedUpgradeLevel == 2);
        }
    }

    public void PurchasePiratePatienceUpgrade1()
    {
        int cost = 75; // Cost of the first Pirate Patience upgrade

        if (money >= cost && piratePatienceLevel == 0)
        {
            // Deduct the money and increase delivery time
            money -= cost;
            piratePatienceLevel = 1;
            UpdateMoneyText();
            UpdateBalanceText();
            UpdatePiratePatienceUpgradeButtons();

            DisplayMessage("Purchased Pirate Patience Upgrade 1!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void PurchasePiratePatienceUpgrade2()
    {
        int cost = 150; // Cost of the second Pirate Patience upgrade

        if (money >= cost && piratePatienceLevel == 1)
        {
            // Deduct the money and increase delivery time
            money -= cost;
            piratePatienceLevel = 2;
            UpdateMoneyText();
            UpdateBalanceText();
            UpdatePiratePatienceUpgradeButtons();

            DisplayMessage("Purchased Pirate Patience Upgrade 2!");
        }
        else if (piratePatienceLevel < 1)
        {
            DisplayMessage("You need to purchase Pirate Patience Upgrade 1 first!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void PurchasePiratePatienceUpgrade3()
    {
        int cost = 250; // Cost of the third Pirate Patience upgrade

        if (money >= cost && piratePatienceLevel == 2)
        {
            // Deduct the money and increase delivery time
            money -= cost;
            piratePatienceLevel = 3;
            UpdateMoneyText();
            UpdateBalanceText();
            UpdatePiratePatienceUpgradeButtons();

            DisplayMessage("Purchased Pirate Patience Upgrade 3!");
        }
        else if (piratePatienceLevel < 2)
        {
            DisplayMessage("You need to purchase Pirate Patience Upgrade 2 first!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void UpdatePiratePatienceUpgradeButtons()
    {
        // Enable or disable buttons based on current Pirate Patience upgrade level
        if (piratePatienceUpgrade1Button != null)
        {
            piratePatienceUpgrade1Button.interactable = (piratePatienceLevel == 0);
        }

        if (piratePatienceUpgrade2Button != null)
        {
            piratePatienceUpgrade2Button.interactable = (piratePatienceLevel == 1);
        }

        if (piratePatienceUpgrade3Button != null)
        {
            piratePatienceUpgrade3Button.interactable = (piratePatienceLevel == 2);
        }
    }

    public void PurchaseCooldownUpgrade1()
    {
        int cost = 75; // Cost of the first cooldown upgrade

        if (money >= cost && cooldownUpgradeLevel == 0)
        {
            // Deduct the money and reduce cooldown
            money -= cost;
            baseCooldown -= cooldownReductionPerTier;
            cooldownUpgradeLevel = 1;
            UpdateMoneyText();
            UpdateBalanceText();
            UpdateCooldownUpgradeButtons();

            DisplayMessage("Purchased Cooldown Reduction Upgrade 1!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void PurchaseCooldownUpgrade2()
    {
        int cost = 150; // Cost of the second cooldown upgrade

        if (money >= cost && cooldownUpgradeLevel == 1)
        {
            // Deduct the money and reduce cooldown
            money -= cost;
            baseCooldown -= cooldownReductionPerTier;
            cooldownUpgradeLevel = 2;
            UpdateMoneyText();
            UpdateBalanceText();
            UpdateCooldownUpgradeButtons();

            DisplayMessage("Purchased Cooldown Reduction Upgrade 2!");
        }
        else if (cooldownUpgradeLevel < 1)
        {
            DisplayMessage("You need to purchase Cooldown Reduction Upgrade 1 first!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void PurchaseCooldownUpgrade3()
    {
        int cost = 250; // Cost of the third cooldown upgrade

        if (money >= cost && cooldownUpgradeLevel == 2)
        {
            // Deduct the money and reduce cooldown
            money -= cost;
            baseCooldown = 0;
            cooldownUpgradeLevel = 3;
            UpdateMoneyText();
            UpdateBalanceText();
            UpdateCooldownUpgradeButtons();

            DisplayMessage("Purchased Cooldown Reduction Upgrade 3!");
        }
        else if (cooldownUpgradeLevel < 2)
        {
            DisplayMessage("You need to purchase Cooldown Reduction Upgrade 2 first!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }

    public void UpdateCooldownUpgradeButtons()
    {
        if (cooldownUpgrade1Button != null)
        {
            cooldownUpgrade1Button.interactable = (cooldownUpgradeLevel == 0);
        }

        if (cooldownUpgrade2Button != null)
        {
            cooldownUpgrade2Button.interactable = (cooldownUpgradeLevel == 1);
        }

        if (cooldownUpgrade3Button != null)
        {
            cooldownUpgrade3Button.interactable = (cooldownUpgradeLevel == 2);
        }
    }

    public void PurchaseHullUpgrade()
    {
        int upgradeCost = 250; // Cost of the hull upgrade

        if (money >= upgradeCost && takesDamageFromTerrain)
        {
            // Deduct the money
            money -= upgradeCost;
            UpdateMoneyText();
            UpdateBalanceText(); // Update the balance display in the shop

            // Disable terrain damage
            takesDamageFromTerrain = false;

            // Grey out the button after purchase
            if (hullUpgradeButton != null)
            {
                hullUpgradeButton.interactable = false;
            }

            DisplayMessage("Purchased Hull Upgrade!");
        }
        else if (!takesDamageFromTerrain)
        {
            DisplayMessage("You already have the Hull Upgrade!");
        }
        else
        {
            DisplayMessage("You don't have enough gold for that.");
        }
    }


}
*/