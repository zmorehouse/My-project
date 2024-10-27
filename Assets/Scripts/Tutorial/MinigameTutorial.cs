using UnityEngine;
using TMPro;
using System.Collections;

public class IslandTutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI instructionText;
    public GameObject monkeyPrefab;
    public GameObject tutMonkeyPrefab;

    private SnakeController snakeController;
    private int currentStep = 0;
    private bool canProceed = true;
    private bool isTutorialComplete = false; // Flag to prevent animations after tutorial

    private bool hasPickedUpFruit = false;
    private bool hasShot = false;
    private int initialInventoryCount = 0;

    private string[] tutorialSteps = new string[]
    {
        "Welcome to the island!",
        "Get those legs to work—used WASD to navigate the island.",
        "Look at you go! See those trees? They're packed with different fruits.",
        "Your quota will tell you how many you'll need. If the day finishes and you don't have enough fruit, you'll walk the plank!",
        "You can pick as many or as few as you like. Or leave with nothing—hey, it’s your call!",
        "Go grab a piece of fruit!",
        "Oh, one last thing. These islands are home to ravenous monkeys.",
        "Don't look at me like that! It was in the fine print...",
        "Oh look, there's one right now.",
        "Watch out, if a monkey hits you it'll hurt and you'll drop everything you've picked up.",
        "And don't even think about passing out on the island... you'll waste time and leave with nothing!",
        "Oh look, there's another monkey! Quick, hit spacebar to use your blunderbuss.",
        "Bullseye! Good shooting.",
        "Reminder, you can use the upgrade shop to make your life a little easier... I'd recommend it.",
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

        GameObject snake = GameObject.FindGameObjectWithTag("Player");
        if (snake != null)
        {
            snakeController = snake.GetComponent<SnakeController>();
        }

        if (snakeController != null)
        {
            snakeController.SetMovement(false);
        }

        DisplayCurrentStep();
    }

    void Update()
    {
        // Only play animations and proceed if the tutorial is not complete
        if (Input.GetKeyDown(KeyCode.E) && canProceed && !isTutorialComplete)
        {
            int randomIndex = Random.Range(0, animationOptions.Length);
            playerAnimator.Play(animationOptions[randomIndex], -1, 0f);
            NextStep();
        }

        // Check for fruit pickup during the "Go grab a piece of fruit!" step (step 5)
        if (currentStep == 5 && !hasPickedUpFruit)
        {
            if (snakeController.inventory.Count > initialInventoryCount)
            {
                hasPickedUpFruit = true;
                canProceed = true;
                NextStep();
            }
        }

        // Check for spacebar press during step 11 to move to step 12
        if (currentStep == 11 && !hasShot)
        {
              canProceed = false;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                hasShot = true;
                canProceed = true;
                NextStep();
            }
        }
    }

    private void DisplayCurrentStep()
    {
        if (currentStep < tutorialSteps.Length)
        {
            tutorialText.text = tutorialSteps[currentStep];
            UpdateInstructionText();
            canProceed = true;

            if (currentStep == 1)
            {
                snakeController.SetMovement(true);
                canProceed = false; // Prevent E key until movement
                StartCoroutine(PauseAfterDelay(5f));
                instructionText.text = "Use WASD to move.";
            }
            else if (currentStep == 2)
            {
                snakeController.SetMovement(false);
            }
            else if (currentStep == 5)
            {
                canProceed = false; // Prevent E key until fruit pickup
                instructionText.text = "Pick a piece of fruit!";
                hasPickedUpFruit = false;
                initialInventoryCount = snakeController.inventory.Count;
                snakeController.SetMovement(true);
            }
            else if (currentStep == 6)
            {
                snakeController.SetMovement(false);
            }
            else if (currentStep == 8)
            {
                Vector3 spawnPosition = snakeController.transform.position + snakeController.transform.forward * 5f;
                spawnPosition.y = -0.25f;
                Instantiate(monkeyPrefab, spawnPosition, Quaternion.identity);
            }
            else if (currentStep == 10)
            {
                snakeController.SetMovement(true);
                instructionText.text = "Explore the island";
                StartCoroutine(WaitAndProceedToNextStep(4f));
            }
            else if (currentStep == 11)
            {
                Vector3 spawnPosition = snakeController.transform.position + snakeController.transform.forward * 5f;
                spawnPosition.y = -0.25f;
                Instantiate(tutMonkeyPrefab, spawnPosition, Quaternion.identity);

                snakeController.SetMovement(false);
                PlayerPrefs.SetInt("disableShoot", 0);
                canProceed = false; // Prevent E key until shooting
                instructionText.text = "Press SPACE to shoot.";
            }
            else if (currentStep == 12)
            {
                snakeController.SetMovement(true);
            }
        }
        else
        {
            EndTutorial();
        }
    }

    private void UpdateInstructionText()
    {
        if (currentStep == 1)
        {
            instructionText.text = "Use WASD to move.";
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

    private void EndTutorial()
    {
        tutorialText.text = "Good stuff. I think you're ready. Race you back to the ship! (Shift to sprint)";
        instructionText.text = "Head back to your ship and press E";
        snakeController.SetMovement(true);
        isTutorialComplete = true; // Set tutorial as complete to prevent further animations
    }

    private IEnumerator PauseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        snakeController.SetMovement(false);
        canProceed = true;
        NextStep();
    }

    private IEnumerator WaitAndProceedToNextStep(float delay)
    {
        yield return new WaitForSeconds(delay);
        NextStep();
    }
}
