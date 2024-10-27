using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TutorialIslandTrigger : MonoBehaviour
{
    private bool isPlayerInRange = false;

    private TutorialManager tutorialManager; // Reference to TutorialManager
    private void Start()

    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
                tutorialManager = FindObjectOfType<TutorialManager>(); // Find the TutorialManager in the scene

    }

 private void Update()
    {
        // Check if the player is in range, presses 'E', and the tutorial is complete
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && tutorialManager != null && tutorialManager.tutorialCompleted)
        {
            SceneManager.LoadScene("MinigameTutorial", LoadSceneMode.Single); // Go to minigame tutorial
        }
        else if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && tutorialManager != null && !tutorialManager.tutorialCompleted)
        {
            Debug.Log("Complete the tutorial before accessing the island!"); // Message for incomplete tutorial
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

  
}