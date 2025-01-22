using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public int maxLives = 3;
    public Text livesText;
    public GameObject adsPanel;
    public Button reviveButton;
    public Button dieButton; // Nuevo botón "Die"

    private int currentLives;

    void Start()
    {
        currentLives = maxLives;
        UpdateLivesUI();
        adsPanel.SetActive(false);

        // Conectar botones a sus respectivos métodos
        reviveButton.onClick.AddListener(ShowAdForRevive);
        dieButton.onClick.AddListener(ResetGameWithDie); // Conectar el botón "Die"
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

    private void ShowAdForRevive()
    {
        // Mostrar anuncio utilizando AdManager
        AdManager.Instance.ShowRewardedVideo(OnAdWatched);
    }

    private void OnAdWatched(bool success)
    {
        if (success)
        {
            Debug.Log("Ad watched successfully. Granting one extra life.");
            currentLives = Mathf.Min(currentLives + 1, maxLives); // Agregar una vida sin exceder el máximo
            UpdateLivesUI();

            Time.timeScale = 1; // Reanudar el juego
            adsPanel.SetActive(false); // Ocultar el panel de anuncios
        }
        else
        {
            Debug.Log("Ad not watched or failed to show.");
            // Puedes manejar el caso en que no se vea el anuncio, si es necesario
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Reanudar el juego
        currentLives = 1;
        UpdateLivesUI();
        adsPanel.SetActive(false);
    }

    private void ResetGameWithDie()
    {
        Debug.Log("Game reset using 'Die' button.");
        Time.timeScale = 1; // Reanudar el juego
        currentLives = maxLives; // Restablecer las vidas al máximo
        UpdateLivesUI();
        adsPanel.SetActive(false); // Ocultar el panel de anuncios
    }
}
