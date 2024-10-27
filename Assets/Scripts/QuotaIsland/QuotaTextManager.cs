using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class QuotaTextManager : MonoBehaviour
{
    public TextMeshProUGUI quotaText;        // Main text for dialogue
    public TextMeshProUGUI instructionText;  // Smaller instructional text (e.g., "Press E to continue")
    public GameObject playerModel;           // Assign this in the Inspector
    private Animator playerAnimator;

    private int currentStep = 0;
    private bool canProceed = true;
    private bool hasMetQuota;

    private string[] dialogueSteps;
    private string[] animationOptions = { "Talking", "Talking (1)", "Talking (2)" };

    private ResourceManager resourceManager;
    private QuotaManager quotaManager;
        private DayNightLogic dayNightLogic; // Reference to DayNightLogic



    void Start()
    {
        dayNightLogic = FindObjectOfType<DayNightLogic>(); // Assuming DayNightLogic exists in the scene

        // Initialize animator
        playerAnimator = playerModel.GetComponent<Animator>();
        if (playerAnimator == null)
        {
            Debug.LogError("Animator component not found on player model!");
        }

        // Get instances
        resourceManager = ResourceManager.instance;
        quotaManager = FindObjectOfType<QuotaManager>();
 


        // Set dialogue based on quota performance
        DetermineDialogueSteps();

        // Display the first dialogue step
        DisplayCurrentStep();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canProceed)
        {
            int randomIndex = Random.Range(0, animationOptions.Length);
            playerAnimator.Play(animationOptions[randomIndex], -1, 0f);

            NextStep();
        }
    }

    private void DetermineDialogueSteps()
    {
        int totalBananas = resourceManager.totalBananas + resourceManager.currentBananas;
        int totalCoconuts = resourceManager.totalCoconuts + resourceManager.currentCoconuts;
        int totalMangoes = resourceManager.totalMangoes + resourceManager.currentMangoes;

        hasMetQuota = (totalBananas >= quotaManager.bananaQuota) &&
                      (totalCoconuts >= quotaManager.coconutQuota) &&
                      (totalMangoes >= quotaManager.mangoQuota);

        if (hasMetQuota)
        {
            dialogueSteps = new string[]
            {
                "Yarr me hearty! Hope you had a good day.",
                "Now let's see how that haul's looking.",
                "Oh, lovely! Ye did well.",
                $"You were required to get {quotaManager.bananaQuota} bananas, {quotaManager.coconutQuota} lemons, {quotaManager.mangoQuota} watermelons.",
                $"You got: {totalBananas} bananas, {totalCoconuts} lemons, {totalMangoes} watermelons.",
                $"Great work! We'll pay ye $25 for your hard work, and a bonus of ${(GetBonusAmount())} for the extra ye brought in!",
                "Don’t spend it all at once! Go have a nap and be ready to work tomorrow. Ye should be fine with the new quota!"
            };
        }
        else
        {
            dialogueSteps = new string[]
            {
                "Yarr me hearty! Hope you had a good day.",
                "Now let's see how that haul's looking.",
                "Oh... this isn't looking too good.",
                $"You were required to get {quotaManager.bananaQuota} bananas, {quotaManager.coconutQuota} lemons, {quotaManager.mangoQuota} watermelons.",
                $"You got: {totalBananas} bananas, {totalCoconuts} lemons, {totalMangoes} watermelons.",
                "Well... ye know the drill. I hate to do this to ya, matey, but it’s time to walk the plank."
            };
        }
    }

    private int GetBonusAmount()
    {
        int extraBananas = Mathf.Max(0, (resourceManager.totalBananas + resourceManager.currentBananas) - quotaManager.bananaQuota);
        int extraCoconuts = Mathf.Max(0, (resourceManager.totalCoconuts + resourceManager.currentCoconuts) - quotaManager.coconutQuota);
        int extraMangoes = Mathf.Max(0, (resourceManager.totalMangoes + resourceManager.currentMangoes) - quotaManager.mangoQuota);

        int extraFruits = extraBananas + extraCoconuts + extraMangoes;
        return extraFruits * 2;
    }

    private void DisplayCurrentStep()
    {
        if (currentStep < dialogueSteps.Length)
        {
            quotaText.text = dialogueSteps[currentStep];
            UpdateInstructionText();
            canProceed = true;
        }
    }

    private void UpdateInstructionText()
    {
        instructionText.text = "Press E to continue.";
    }

    private void NextStep()
    {
        if (currentStep < dialogueSteps.Length - 1)
        {
            currentStep++;
            DisplayCurrentStep();
        }
        else
        {
            HandleQuotaTurnIn();
        }
    }

    private void HandleQuotaTurnIn()
    {
        if (hasMetQuota)
        {
            RewardPlayer();
            ResetDay();
        }
        else
        {
            quotaManager.LoseGame();
        }
        instructionText.text = ""; 
    }

private void RewardPlayer()
{
    // Update player money in PlayerPrefs
    int currentMoney = PlayerPrefs.GetInt("Money");
    int bonusAmount = GetBonusAmount();
    PlayerPrefs.SetInt("Money", currentMoney + 25 + bonusAmount);
    Debug.Log($"Player has been rewarded with ${25 + bonusAmount} for meeting the quota.");
    Debug.Log(PlayerPrefs.GetInt("Money"));
    // Update the UI to show the updated gold amount
    UpdateGoldText(currentMoney + 25 + bonusAmount);

            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.IncrementScore();
        }
}

private void UpdateGoldText(int updatedMoney)
{
    // Assuming you have a TextMeshProUGUI reference for displaying gold in this scene
TextMeshProUGUI goldText = GameObject.Find("Money")?.GetComponent<TextMeshProUGUI>();

    if (goldText != null)
    {
        goldText.text = $"Gold: ${updatedMoney}";
    }
}

    private void ResetDay()
    {
        resourceManager.ResetAllResources();
        quotaManager.TurnInQuota();
        ResetAllIslands();
        SceneManager.LoadScene("Game");


        dayNightLogic.ResetDayLimit();
        quotaManager.ResetIslandLimit();

    }

    private void ResetAllIslands()
    {
        IslandTrigger[] allIslands = FindObjectsOfType<IslandTrigger>();
        foreach (IslandTrigger island in allIslands)
        {
            island.ResetIsland();
        }
    }


}
