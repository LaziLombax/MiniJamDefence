using UnityEngine;

public class Atmosphere : MonoBehaviour
{
    public float destroyDelay = 0.1f; // Time before a collectable asteroid is destroyed

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectable"))
        {
            // Destroy the collectable asteroid after a short delay
            GameManager.Instance.AddResources(3);
            Destroy(other.gameObject, destroyDelay);
        }
        else
        {
            // Record damage in the GameManager
            GameManager.Instance.RecordDamage(other.gameObject);

            // Handle the explosion of the bigger asteroid
            ExplodeAsteroid(other.gameObject);
        }
    }

    void ExplodeAsteroid(GameObject asteroid)
    {
        if (asteroid.GetComponent<Asteroid>() != null)
            // Assuming the asteroid has an Explode method
            asteroid.GetComponent<Asteroid>().RunExplode();

    }
}
