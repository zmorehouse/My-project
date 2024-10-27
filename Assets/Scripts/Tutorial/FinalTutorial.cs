using UnityEngine;
using TMPro;
using System.Collections;

public class AdditionalTutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI instructionText;

    private ShipController shipController;
    private int currentStep = 0;
    private bool canProceed = true;
    public bool tutorialCompleted = false;

    private string[] tutorialSteps = new string[]
    {
        "Look at ye go, you're a natural!",
        "Now it looks like you found some fruits on the island!",
        "You can see how many fruits you've got - alongside your quota - on the left.",
        "If you come back to us with less than the quota... you'll walk the plank!",
        "If you bring us the right amount, we'll pay you in gold.",
        "And if you bring us more than the quota... we'll pay you a bonus!",
        "Remember, there’s only so many hours in a day... so use your island visits wisely.",
        "See how many days you can last as an employee for the Boogie Merchants!",
        "Righto, that's all for now. Let's drop our fruits off at HQ and get going!"
    };

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
            shipController.SetMovement(false); // Lock movement at start
        }

        DisplayCurrentStep();
    }

    void Update()
    {
        // Only allow progression with E key if the tutorial is not completed
        if (Input.GetKeyDown(KeyCode.E) && canProceed && !tutorialCompleted)
        {
            int randomIndex = Random.Range(0, animationOptions.Length);
            playerAnimator.Play(animationOptions[randomIndex], -1, 0f);
            NextStep();
        }

        // Check if player has returned to HQ on the final step to complete the tutorial
        if (currentStep == 8 && PlayerAtHQ() && Input.GetKeyDown(KeyCode.E))
        {
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

            // Allow movement only on the final step
            if (currentStep == 8 && shipController != null)
            {
                shipController.SetMovement(true);
                instructionText.text = "Sail back to HQ and press E to deliver your fruit.";
                canProceed = false; // Prevent further E key presses until player reaches HQ
            }
        }
        else
        {
            EndTutorial();
        }
    }

    private void UpdateInstructionText()
    {
        if (currentStep == 8)
        {
            instructionText.text = "Sail back to HQ and press E to deliver your fruit.";
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

    private bool PlayerAtHQ()
    {
        // Replace with logic to check if the player is within range of HQ
        return false;
    }

    private void EndTutorial()
    {
        tutorialText.text = "Great job! You've completed the tutorial.";
        instructionText.text = "You're now ready to start your adventure!";
        tutorialCompleted = true; // Mark the tutorial as completed
        if (shipController != null)
        {
            shipController.SetMovement(true); // Ensure movement is unlocked after tutorial
        }
    }
}
