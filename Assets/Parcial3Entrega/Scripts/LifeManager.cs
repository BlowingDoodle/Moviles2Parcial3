using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public int maxLives = 3;
    public Text livesText;
    public GameObject adsPanel;

    private int currentLives;

    void Start()
    {
        currentLives = maxLives;
        UpdateLivesUI();
        adsPanel.SetActive(false);
    }

    public void LoseLife()
    {
        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    private void UpdateLivesUI()
    {
        livesText.text = "Lives: " + currentLives + "/" + maxLives;
    }

    private void GameOver()
    {
        Time.timeScale = 0; // Pausar el juego
        adsPanel.SetActive(true); // Mostrar el panel de anuncios
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Reanudar el juego
        currentLives = maxLives;
        UpdateLivesUI();
        adsPanel.SetActive(false);
    }
}
