using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static float gameSpeed = 1f;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI redCoinText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI modeText;
    public TextMeshProUGUI worldText;
    public GameObject redCoinPrefab;

    private int score = 0;
    private int highScore = 0;
    private int scoreMultiplier = 1;
    private int redCoinsCollected = 0;
    private int redCoinsTotal = 0;
    private bool redCoinChallengeActive = false;
    private bool isGameOver = false;
    private int lastSpeedIncreaseScore = 0;
    private int currentWorld = 0;

    // Special modes
    public bool isGravityFlipped = false;
    public bool isReverseMode = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameSpeed = 1f;
        lastSpeedIncreaseScore = 0;
        isGravityFlipped = false;
        isReverseMode = false;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreText();
        UpdateHighScoreText();
        UpdateWorldText();
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
            CheckSpeedIncrease();
            UpdateWorldText();
        }
    }

    void CheckSpeedIncrease()
    {
        int scoreThreshold = lastSpeedIncreaseScore + 5;
        if (score >= scoreThreshold)
        {
            gameSpeed += 0.20f;
            lastSpeedIncreaseScore = scoreThreshold;
        }
    }

    public void StartGravityFlipMode()
    {
        isGravityFlipped = true;
        UpdateModeText();
    }

    public void StartReverseMode()
    {
        isReverseMode = true;
        UpdateModeText();
    }

    public void StopSpecialModes()
    {
        isGravityFlipped = false;
        isReverseMode = false;
        UpdateModeText();
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
        float[] yPositions = { 0f, 1.5f, 0f, -1.5f, 0f, 1.5f, 0f, -1.5f };

        for (int i = 0; i < 8; i++)
        {
            float xOffset;
            if (isReverseMode)
            {
                // Spawn coins to the LEFT of the ring (toward player on right)
                xOffset = -4f - (i * 3f);
            }
            else
            {
                // Spawn coins to the RIGHT of the ring (toward player on left)
                xOffset = 4f + (i * 3f);
            }

            Vector3 spawnPos = new Vector3(
                startPosition.x + xOffset,
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

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayGameOver();
        }

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

    void UpdateModeText()
    {
        if (modeText == null) return;

        if (isReverseMode)
        {
            modeText.text = "REVERSE MODE";
            modeText.color = Color.red;
        }
        else if (isGravityFlipped)
        {
            modeText.text = "GRAVITY FLIP";
            modeText.color = new Color(0.6f, 0f, 1f); // Purple
        }
        else
        {
            modeText.text = "";
        }
    }

    void UpdateWorldText()
    {
        if (worldText == null) return;

        int newWorld = score / 50;

        if (newWorld != currentWorld)
        {
            currentWorld = newWorld;
        }

        switch (currentWorld)
        {
            case 0:
                worldText.text = "World: Sky";
                worldText.color = Color.white;
                break;
            case 1:
                worldText.text = "World: Sunset";
                worldText.color = new Color(1f, 0.5f, 0f); // Orange
                break;
            case 2:
                worldText.text = "World: Night";
                worldText.color = new Color(0.5f, 0.5f, 1f); // Light blue
                break;
            case 3:
                worldText.text = "World: Space";
                worldText.color = new Color(1f, 0f, 1f); // Pink/Magenta
                break;
            default:
                worldText.text = "World: Chaos";
                worldText.color = new Color(
                    Random.Range(0.5f, 1f),
                    Random.Range(0.5f, 1f),
                    Random.Range(0.5f, 1f)
                );
                break;
        }
    }

}