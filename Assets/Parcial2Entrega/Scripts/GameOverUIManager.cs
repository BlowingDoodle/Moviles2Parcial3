using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    public static GameOverUIManager Instance;

    [SerializeField] private TMP_Text adsRemainingText; // Cambiado a TMP_Text

    public Canvas gameOverCanvas; // Asignar el Canvas desde el Inspector
    public Button reviveButton;   // Botón "Sí"
    public Button restartButton;  // Botón "No"

    private int adsRemaining = 4;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Asegurarse de que el canvas esté oculto al iniciar
        gameOverCanvas.gameObject.SetActive(false);

        // Asignar funciones a los botones
        reviveButton.onClick.AddListener(OnReviveButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    public void ShowGameOverCanvas()
    {
        Time.timeScale = 0; // Detener el tiempo
        gameOverCanvas.gameObject.SetActive(true); // Mostrar el canvas
        UpdateUI();
    }

    private void UpdateUI()
    {
        adsRemainingText.text = $"Ads Remaining: {adsRemaining}";
    }

    public void HideGameOverCanvas()
    {
        Time.timeScale = 1; // Reanudar el tiempo
        gameOverCanvas.gameObject.SetActive(false); // Ocultar el canvas
    }

    private void OnReviveButtonClicked()
    {
        Debug.Log("Botón 'Sí' presionado. Verificando anuncios disponibles...");

        AdManager.Instance.ShowRewardedVideo(success =>
        {
            // Reducir los anuncios restantes aquí, no antes
            adsRemaining--;
            Debug.Log($"Anuncios restantes: {adsRemaining}");

            if (success)
            {
                Debug.Log("Anuncio visto correctamente. Reviviendo jugador.");
            }
            else
            {
                Debug.Log("No se pudo mostrar anuncio. Reviviendo sin anuncio.");
            }

            GameManager.Instance.ReviveWithoutScoreReset();
            HideGameOverCanvas(); // Asegurarse de ocultar el canvas.
            //UpdateUI(); // Actualizar la UI después de cambiar el contador
        });
    }


    public void OnRestartButtonClicked()
    {
        HideGameOverCanvas();
        GameManager.Instance.ResetGame(); // Reiniciar el juego
    }
}

