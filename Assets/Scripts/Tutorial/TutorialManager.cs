using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI instructionText;

    private ShipController shipController;
    private int currentStep = 0;
    private bool isWaitingForWASD = false;
    private bool canProceed = true;
    public bool tutorialCompleted = false; // New flag to track if the tutorial is completed

    private string[] tutorialSteps = new string[]
    {
        "Ahoy there, swabbie! Welcome aboard!",
        "So, you've joined the Boogie Merchants, eh? First day, exciting stuff!",
        "Right, as a merchant, you'll need to hit your KPIs. Yeah, that's right—KPIs!",
        "What's a KPI, you ask? Key Pirate Indicators... or something like that. We just make it up as we go.",
        "Wait—what? You've never worked a real job before? Well, let’s start with the basics, shall we?",
        "Step One: Driving the ship. Use those trusty WASD keys to sail the seven seas.",
        "Look at you, a natural! Now, your job is to sail to islands and grab all the resources you can carry.",
        "Not gonna lie, it's a tough gig. There's a quota to hit each day if you want to keep this job.",
        "But don’t worry, you’ll get paid in gold for your troubles. Ah, sweet gold!",
        "You can spend that gold on shiny new upgrades in the store. Or don’t—whatever floats your boat. I'm not your boss.",
        "But remember, there’s only so many hours in a day! Once the day’s over, you’ll have to sail back—whether you’ve got goods or not.",
    };

    public string animationName;
    public GameObject playerModel;

    private Animator playerAnimator;
    private string[] animationOptions = { "Talking", "Talking (1)", "Talking (2)" };

    void Start()
    {
        playerAnimator = playerModel.GetComponent<Animator>();
        if (playerAnimator == null)
        {
            Debug.LogError("Animator component not found on player model!");
        }

        GameObject ship = GameObject.FindGameObjectWithTag("Player");
        if (ship != null)
        {
            shipController = ship.GetComponent<ShipController>();
        }

        if (shipController != null)
        {
            shipController.SetMovement(false); // Disable ship movement at the start
        }

        DisplayCurrentStep();
    }

    void Update()
    {
        // Only play animations if the tutorial is not completed
        if (Input.GetKeyDown(KeyCode.E) && canProceed && !tutorialCompleted)
        {
            int randomIndex = Random.Range(0, animationOptions.Length);
            playerAnimator.Play(animationOptions[randomIndex], -1, 0f);

            NextStep();
        }

        // Check for WASD movement on step 5
        if (currentStep == 5 && PlayerStartedDriving() && !isWaitingForWASD)
        {
            canProceed = true;  // Allow proceeding only after WASD movement
            StartCoroutine(WASDDelay());
        }

        if (currentStep == 11 && ShipReachedIsland())
        {
            canProceed = true;
            NextStep();
        }
    }

    private void DisplayCurrentStep()
    {
        if (currentStep < tutorialSteps.Length)
        {
            tutorialText.text = tutorialSteps[currentStep];
            UpdateInstructionText();
            canProceed = true;
        }

        // Unlock movement for WASD instruction (Step 5)
        if (currentStep == 5 && shipController != null)
        {
            canProceed = false; // Prevent pressing E to skip WASD step
            shipController.SetMovement(true); // Enable ship movement
            instructionText.text = "Use WASD to move the ship.";
        }

        // Lock movement again after WASD step
        if (currentStep == 6 && shipController != null)
        {
            shipController.SetMovement(false); // Disable ship movement again
        }
    }

    private void UpdateInstructionText()
    {
        if (currentStep == 5)
        {
            instructionText.text = "Use WASD to move the ship.";
        }
        else
        {
            instructionText.text = "Press E to continue.";
        }
    }

    private void NextStep()
    {
        if (currentStep < tutorialSteps.Length - 1)
        {
            currentStep++;
            DisplayCurrentStep();
        }
        else
        {
            EndTutorial();
        }
    }

    private bool PlayerStartedDriving()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }

    private bool ShipReachedIsland()
    {
        return false; // Replace with actual logic
    }

    private void EndTutorial()
    {
        tutorialText.text = "Alrighty, let's get you started. Sail over to Tutorial Island and get those resources!";
        instructionText.text = "Press E on tutorial island";

        shipController.SetMovement(true); // Enable ship movement after tutorial

        tutorialCompleted = true; // Mark the tutorial as completed
    }

    IEnumerator WASDDelay()
    {
        isWaitingForWASD = true;
        yield return new WaitForSeconds(1.5f); // Short delay to simulate movement
        NextStep();
        isWaitingForWASD = false;
    }
}
