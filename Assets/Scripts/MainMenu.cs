using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        // Stop game music if it's playing
        if (GameMusic.instance != null)
        {
            GameMusic.instance.StopMusic();
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Gameplay");
    }
}