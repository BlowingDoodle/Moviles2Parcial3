using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int lives = 3;
    public int score = 0;
    

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
    }

    private void Start()
    {
        Debug.Log("Juego iniciado. Vidas: " + lives + ", Puntos: " + score);
        GridManager.Instance.GenerateBoxes();

        // Inicializar la UI
        UIManager.Instance.UpdateScore(score);
        UIManager.Instance.UpdateLives(lives);
    }

    public void ReduceLife()
    {
        lives--;
        Debug.Log("Bomba activada. Vidas restantes: " + lives);

        UIManager.Instance.UpdateLives(lives); // Actualizar la UI

        if (lives <= 0)
        {
            GameOverUIManager.Instance.ShowGameOverCanvas();
        }
    }

    public void AddPoint()
    {
        score++;
        Debug.Log("¡Punto obtenido! Puntuación actual: " + score);

        UIManager.Instance.UpdateScore(score); // Actualizar la UI

        GridManager.Instance.GenerateBoxes(); // Regenerar la cuadrícula
    }

    public void RevivePlayer()
    {
        lives = 1; // Dar 1 vida al jugador
        UIManager.Instance.UpdateLives(lives);
        Debug.Log("Jugador revivido!");
    }

    public void ReviveWithoutScoreReset()
    {
        lives = 1; // Dar una vida
        UIManager.Instance.UpdateLives(lives);
        Debug.Log("Jugador revivido sin reiniciar el puntaje.");
        GridManager.Instance.GenerateBoxes(); // Regenerar las cajas
    }

    public void ResetGame()
    {
        // Reiniciar valores
        lives = 3;
        score = 0;
        

        Debug.Log("Reiniciando juego. Vidas: " + lives + ", Puntos: " + score);

        // Actualizar la UI
        UIManager.Instance.UpdateScore(score);
        UIManager.Instance.UpdateLives(lives);

        // Regenerar el Grid
        GridManager.Instance.GenerateBoxes();
    }
}