using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    public Transform gridParent;
    private const int rows = 4;

    private const int cols = 3;

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
    public void GenerateBoxes()
    {
        ResetGrid(); // Limpiar el grid antes de regenerar

        // Crear una lista de posiciones disponibles
        List<Vector3> positions = new List<Vector3>();
        float spacingX = 2.0f;
        float spacingZ = 2.0f;
        Vector3 startPosition = new Vector3(-3f, 0, -3f);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 position = startPosition + new Vector3(col * spacingX, 0, row * spacingZ);
                positions.Add(position);
            }
        }

        // Mezclar las posiciones para aleatorizar
        ShuffleList(positions);

        // Generar cajas según el criterio (1 Score, 3 Bombs, resto Vacías)
        PlaceBoxes(positions, 1, BoxBehaviour.BoxType.Score); // 1 Caja de puntuación
        PlaceBoxes(positions, 3, BoxBehaviour.BoxType.Bomb);  // 3 Bombas
        PlaceBoxes(positions, positions.Count, BoxBehaviour.BoxType.Empty); // Resto Vacías
    }

    private void PlaceBoxes(List<Vector3> positions, int count, BoxBehaviour.BoxType type)
    {
        for (int i = 0; i < count && positions.Count > 0; i++)
        {
            Vector3 position = positions[0];
            positions.RemoveAt(0);

            GameObject box = BoxPoolManager.Instance.GetBox(type);
            box.transform.position = position;
            box.transform.SetParent(gridParent);
        }
    }

    

    private void ResetGrid()
    {
        // Devolver todas las cajas al pool antes de regenerar
        foreach (Transform child in gridParent)
        {
            BoxPoolManager.Instance.ReturnBox(child.gameObject);
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}



