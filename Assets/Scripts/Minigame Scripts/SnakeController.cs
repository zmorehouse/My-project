using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    public List<GameObject> inventory = new List<GameObject>();
    private List<Vector3> obstacles = new List<Vector3>();
    public List<GameObject> foodObjects = new List<GameObject>();
    public static SnakeController Instance;

    public float movementSpeed = 7f;
    public float sprintSpeed = 9f;
    public float rotationSpeed = 100F;
    public float followDistance = 0.5f;
    private ResourceManager resourceManager;
    private bool inMinigame = true;
    private bool isMovementEnabled = true;
    private QuotaManager quotaManager;
    private GameObject quotaSpotlight;

    // Stamina variables
    public float maxStamina = 60f;
    public float currentStamina;
    public float staminaConsumptionRate = 20f;
    public float staminaRecoveryRate = 10f;
    public float staminaRecoveryDelay = 1.5f;
    private float lastSprintTime;

    public int maxHealth = 3;
    public int currentHealth;

    // UI Components for Health and Stamina
    public Slider healthSlider;
    public Slider staminaSlider;

    // Red flash panel
    public Image redFlashPanel;

    public bool tutorialActive = false;

    public GameObject playerModel;
    private Animator playerAnimator;

    private float xRange = 25;
    private float zRange = 25;

    void Start()
    {   
        quotaManager = FindObjectOfType<QuotaManager>();
        quotaSpotlight = GameObject.FindGameObjectWithTag("QuotaSpotlight");

        playerAnimator = playerModel.GetComponent<Animator>();
        if (playerAnimator == null)
        {
            Debug.LogError("Animator component not found on player model!");
        }

        resourceManager = ResourceManager.instance;

        // Initialize player health and stamina
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        
        UpdateHealthUI();
        UpdateStaminaUI();

        // Automatically set tutorialActive based on the scene name
        string currentSceneName = SceneManager.GetActiveScene().name;
        tutorialActive = currentSceneName == "MinigameTutorial";
        isMovementEnabled = !tutorialActive;
                CheckHealthUpgrade();
        CheckStaminaUpgrade();
        CheckSpeedUpgrade();

    }


    void Update()
    {

        if (isMovementEnabled)
        {
            MovePlayer();
            MoveInventory();
            UpdateStamina();
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
        }

        ClampPlayerPosition();
    }

    private void CheckHealthUpgrade()
    {
        int changeHealthSignal = PlayerPrefs.GetInt("HealthUpgradeLevel");
        if (changeHealthSignal == 1)
        {
            
            maxHealth = 4;
        }
        else if (changeHealthSignal == 2)
        {
            maxHealth = 5;
        } else if (changeHealthSignal == 3)
        {
            maxHealth = 7;

        }
    }

    private void CheckStaminaUpgrade()
    {
        int changeStaminaSignal = PlayerPrefs.GetInt("StamUpgradeLevel");
        if (changeStaminaSignal == 1)
        {
            maxStamina = 45;
            
        }
        else if (changeStaminaSignal == 2)
        {
            maxStamina = 55;
        } else if (changeStaminaSignal == 3)
        {
            maxStamina = 75;

        }

    }

    private void CheckSpeedUpgrade()
    {
        int changeSpeedSignal = PlayerPrefs.GetInt("SpeedUpgradeLevel");
        if (changeSpeedSignal == 1)
        {
            movementSpeed = 7.5f;
            sprintSpeed = 9.5f;
        } else if (changeSpeedSignal == 2)
        {
            movementSpeed = 8f;
            sprintSpeed = 10.5f;
        } else if (changeSpeedSignal == 3)
        {
            movementSpeed = 9f;
            sprintSpeed = 12f;
            Debug.Log("Deez ");
        }
        
    }

    public void SetMovement(bool canMove)
    {
        isMovementEnabled = canMove;
    }

    void MovePlayer()
    {
        float speed = movementSpeed;
        float animationSpeedMultiplier = 1f;

        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            speed += sprintSpeed;
            currentStamina -= staminaConsumptionRate * Time.deltaTime;
            lastSprintTime = Time.time;
            animationSpeedMultiplier = 1.25f;
        }
        else
        {
            if (Time.time - lastSprintTime > staminaRecoveryDelay)
            {
                currentStamina += staminaRecoveryRate * Time.deltaTime;
            }
            animationSpeedMultiplier = 0.7f;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateStaminaUI();

        playerAnimator.speed = animationSpeedMultiplier;

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            playerAnimator.SetBool("isMoving", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            playerAnimator.SetBool("isMoving", true);
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
        }

        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);
    }

    void MoveInventory()
    {
        Vector3 previousPosition = transform.position;
        int visibleLimit = 15;

        for (int i = 0; i < inventory.Count; i++)
        {
            GameObject currentFood = inventory[i];
            if (i < visibleLimit)
            {
                Vector3 targetPosition = previousPosition - transform.forward * followDistance;
                currentFood.transform.position = Vector3.Lerp(currentFood.transform.position, targetPosition, Time.deltaTime * movementSpeed);
                previousPosition = currentFood.transform.position;
                currentFood.SetActive(true);
            }
            else
            {
                currentFood.SetActive(false);
            }
        }
    }

    void UpdateStamina()
    {
        staminaSlider.value = currentStamina / maxStamina;
    }

    void ClampPlayerPosition()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -xRange, xRange),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -zRange, zRange)
        );
    }

    public void PickUpFood(GameObject food)
    {
        inventory.Add(food);
        food.transform.position = transform.position - transform.forward * followDistance;
        food.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food" && !inventory.Contains(other.gameObject))
        {
            PickUpFood(other.gameObject);
            Debug.Log("Picked up food");

            string foodName = other.gameObject.name.Replace("(Clone)", "").Trim();
            if (foodName.Contains("Lemon")) ResourceManager.instance.AddCurrentCoconuts(1);
            else if (foodName.Contains("Watermelon")) ResourceManager.instance.AddCurrentMangoes(1);
            else if (foodName.Contains("Banana")) ResourceManager.instance.AddCurrentBananas(1);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log("Player health is zero. Exiting minigame.");
            DestroyMinigameScene();
            quotaManager.UpdateInstructions();
            quotaManager.turnOnLight();
        }

        StartCoroutine(FlashRedPanel());
    }

    IEnumerator FlashRedPanel()
    {
        Color panelColor = redFlashPanel.color;
        for (float alpha = 0f; alpha <= 0.5f; alpha += Time.deltaTime)
        {
            panelColor.a = alpha;
            redFlashPanel.color = panelColor;
            yield return null;
        }
        for (float alpha = 0.5f; alpha >= 0f; alpha -= Time.deltaTime)
        {
            panelColor.a = alpha;
            redFlashPanel.color = panelColor;
            yield return null;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    private void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina / maxStamina;
        }
    }

    public void DropResources()
    {
        resourceManager.ResetCurrentResources();
        foreach (GameObject item in inventory)
        {
            if (item != null)
            {
                item.transform.parent = null;
                Destroy(item);
            }
        }
        inventory.Clear();
        Debug.Log("All following items have been dropped and destroyed.");
    }

    private void DestroyMinigameScene()
    {
        Debug.Log("Destroying minigame scene and exiting.");
        PopupTrigger popupTrigger = FindObjectOfType<PopupTrigger>();
        if (popupTrigger != null)
        {
            popupTrigger.DestroyMinigameScene();
        }
        else
        {
            Debug.LogError("PopupTrigger script not found! Unable to exit minigame.");
        }
   
    }
}