using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        // Retrieve saved scores from ScoreManager
        int currentScore = PlayerPrefs.GetInt("CurrentScore", 0); // Grab the saved current score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);       // Grab the saved high score

        // Update the UI elements with the retrieved scores
        scoreText.text = $"Days Employed: {currentScore}";
        highScoreText.text = $"Longest Run: {highScore}";

        // Reset Player Prefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("HighScore", highScore);
    }
}
