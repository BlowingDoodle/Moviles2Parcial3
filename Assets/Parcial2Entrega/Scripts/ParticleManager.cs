using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    public ObjectPool explosionParticlePool;

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

    public void SpawnParticle(Vector3 position)
    {
        GameObject particle = explosionParticlePool.GetObject();
        particle.transform.position = position;

        StartCoroutine(ReturnAfterDelay(particle, 2f)); // Duración de la animación
    }

    private System.Collections.IEnumerator ReturnAfterDelay(GameObject particle, float delay)
    {
        yield return new WaitForSeconds(delay);
        explosionParticlePool.ReturnObject(particle);
    }
}

