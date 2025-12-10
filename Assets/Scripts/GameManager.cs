using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI redCoinText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public GameObject redCoinPrefab;

    private int score = 0;
    private int highScore = 0;
    private int scoreMultiplier = 1;
    private int redCoinsCollected = 0;
    private int redCoinsTotal = 0;
    private bool redCoinChallengeActive = false;
    private bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreText();
        UpdateHighScoreText();
        if (redCoinText != null)
        {
            redCoinText.gameObject.SetActive(false);
        }
        Time.timeScale = 1;
    }

    public void AddScore(int amount)
    {
        if (!isGameOver)
        {
            score += amount * scoreMultiplier;
            UpdateScoreText();
        }
    }

    public void StartRedCoinChallenge(Vector3 ringPosition)
    {
        if (!redCoinChallengeActive)
        {
            redCoinChallengeActive = true;
            redCoinsCollected = 0;
            redCoinsTotal = 8;
            UpdateRedCoinText();
            if (redCoinText != null)
            {
                redCoinText.gameObject.SetActive(true);
            }
            SpawnRedCoins(ringPosition);
            StartCoroutine(RedCoinTimer());
        }
    }

    void SpawnRedCoins(Vector3 startPosition)
    {
        // Spawn 8 red coins in a collectible wave pattern
        float[] yPositions = { -1f, 0.5f, 1.5f, 0.5f, -0.5f, 1f, 2f, 0f };

        for (int i = 0; i < 8; i++)
        {
            Vector3 spawnPos = new Vector3(
                startPosition.x + 3f + (i * 2f),
                startPosition.y + yPositions[i],
                0
            );
            Instantiate(redCoinPrefab, spawnPos, Quaternion.identity);
        }
    }

    System.Collections.IEnumerator RedCoinTimer()
    {
        yield return new WaitForSeconds(10f);

        if (redCoinChallengeActive)
        {
            EndRedCoinChallenge(false);
        }
    }

    public void CollectRedCoin()
    {
        if (redCoinChallengeActive)
        {
            redCoinsCollected++;
            UpdateRedCoinText();

            if (redCoinsCollected >= 8)
            {
                EndRedCoinChallenge(true);
            }
        }
    }

    public void MissedRedCoin()
    {
        if (redCoinChallengeActive)
        {
            redCoinsTotal--;

            // If not enough coins left to complete challenge
            if (redCoinsTotal < 8 && redCoinsCollected < redCoinsTotal)
            {
                // Challenge continues but can't win anymore
            }
        }
    }

    void EndRedCoinChallenge(bool success)
    {
        redCoinChallengeActive = false;

        if (success)
        {
            StartCoroutine(RedCoinMultiplier());
        }

        if (redCoinText != null)
        {
            redCoinText.gameObject.SetActive(false);
        }
    }

    System.Collections.IEnumerator RedCoinMultiplier()
    {
        scoreMultiplier = 2;
        yield return new WaitForSeconds(30f);
        scoreMultiplier = 1;
    }

    public void SetScoreMultiplier(int multiplier)
    {
        scoreMultiplier = multiplier;
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "Best: " + highScore;
        }
    }

    void UpdateRedCoinText()
    {
        if (redCoinText != null)
        {
            redCoinText.text = "Red Coins: " + redCoinsCollected + "/8";
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "Score: " + score + "\nBest: " + highScore;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}