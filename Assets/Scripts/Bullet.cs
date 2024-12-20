using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed of the projectile
    public float inaccuracy = 5f;             // Amount of random spread (in degrees)
    private Vector3 shootDirection;

    private void Start()
    {
        // Calculate the random spread
        float randomSpread = Random.Range(-inaccuracy, inaccuracy);
        // Apply the random spread to the forward direction of the projectile
        shootDirection = Quaternion.Euler(0, 0, randomSpread) * transform.right;
    }

    private void Update()
    {
        // Move the projectile in the calculated direction
        transform.position += shootDirection * speed * Time.deltaTime;
    }
}
