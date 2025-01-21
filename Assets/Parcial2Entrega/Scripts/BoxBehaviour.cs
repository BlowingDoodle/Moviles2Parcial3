using UnityEngine;

public class BoxBehaviour : MonoBehaviour
{
    public enum BoxType { Bomb, Score, Empty }
    public BoxType boxType;

    public void OnBoxClicked()
    {
        switch (boxType)
        {
            case BoxType.Bomb:
                GameManager.Instance.ReduceLife();
                SpawnExplosion(); // Invocar la partícula
                break;
            case BoxType.Score:
                GameManager.Instance.AddPoint();
                break;
            case BoxType.Empty:
                Debug.Log("Caja vacía. Solo se destruye.");
                break;
        }

        Destroy(gameObject);
    }

    private void SpawnExplosion()
    {
        ParticleManager.Instance.SpawnParticle(transform.position);
    }
}




