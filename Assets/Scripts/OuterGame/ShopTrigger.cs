using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShopTrigger : MonoBehaviour
{
    public GameObject shopMenuUI;
    private ShopLogic shopLogic;
    private bool isPlayerInRange = false;
    public TextMeshProUGUI messageText;
    private QuotaManager quotaManager;

    private void Start()
    {   
        quotaManager = FindObjectOfType<QuotaManager>();
        // Ensure the shop menu is initially hidden
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(false);
        }
        shopLogic = FindObjectOfType<ShopLogic>();
    }

    private void Update()
    {         

        // Check if the player is in range and presses the 'E' key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Debug.Log($"Shop money =  ${shopLogic.money}");
            OpenShopMenu();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            quotaManager.ShowMessage("Press E to Enter Shop");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            quotaManager.UpdateInstructions();
        }
    }

    private void OpenShopMenu()
    {
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(true);  // Show the shop menu UI
            Time.timeScale = 0f;  // Pause the game
        }
    }

    // Make this method public so the button can access it
    public void CloseShopMenu()
    {
        if (shopMenuUI != null)
        {
            shopMenuUI.SetActive(false);
            Time.timeScale = 1f;  // Resume the game
            messageText.text = "Yarrrr! Welcome to me humble shoppe!";

        }
    }
}
