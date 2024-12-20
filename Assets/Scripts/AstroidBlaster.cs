using UnityEngine;

public class AstroidBlaster : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile prefab to be fired
    public float fireRate = 1.0f;       // Rate at which projectiles are fired (in seconds)
    public float projectileSpeed = 10f; // Speed of the projectile
    public float inaccuracy = 5f;       // Amount of random spread (in degrees)

    private float nextFireTime;
    private InputHandler playerInput;

    private void Start()
    {
        playerInput = GameManager.Instance.InputHandler;
    }

    void Update()
    {
        // Check if the player is trying to fire a projectile
        if (playerInput.ShootInput() && Time.time > nextFireTime)
        {
            Debug.Log("Shot");
            FireProjectile();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireProjectile()
    {
        // Instantiate the projectile at the shooter's position
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

        //// Get the direction the shooter is currently facing
        //Vector3 baseDirection = transform.right;

        //// Apply a random spread to the projectile's direction
        //float randomSpread = Random.Range(-inaccuracy, inaccuracy);
        //Vector3 shootDirection = Quaternion.Euler(0, 0, randomSpread) * baseDirection;

        //// Add force to the projectile's Rigidbody2D component
        //Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        //rb.linearVelocity = shootDirection * projectileSpeed;

        //// Align the projectile's local right direction with its velocity
        //projectile.transform.right = rb.linearVelocity.normalized;

        Destroy(projectile, 3f);
    }
}
