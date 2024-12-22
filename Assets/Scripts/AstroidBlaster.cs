using UnityEngine;

public class AstroidBlaster : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile prefab to be fired

    private float nextFireTime;
    private InputHandler playerInput;

    private void Start()
    {
        playerInput = InputHandler.Instance;
    }

    void Update()
    {
        // Check if the player is trying to fire a projectile
        if (playerInput.ShootInput() && Time.time > nextFireTime)
        {
            Debug.Log("Shot");
            FireProjectile();
            nextFireTime = Time.time + GameManager.Instance.blasterFireRate + Random.Range(0, 0.2f);
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
