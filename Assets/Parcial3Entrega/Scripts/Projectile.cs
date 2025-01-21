using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        int points = 0;

        // Asignar puntos según el tag del objeto
        switch (collision.gameObject.tag)
        {
            case "Azul":
                points = 50;
                break;
            case "Verde":
                points = 100;
                break;
            case "Rot":
                points = 300;
                break;
            case "Dorado":
                points = 1000;
                break;
            case "dmg":
                FindObjectOfType<LifeManager>().LoseLife();
                break;
        }

        if (points > 0)
        {
            FindObjectOfType<ScoreManager>().AddScore(points);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "dmg")
        {
            Destroy(gameObject);
        }
    }
}
