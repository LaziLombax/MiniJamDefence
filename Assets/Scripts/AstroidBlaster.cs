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
        SoundManager.Instance.PlaySound(SoundManager.Instance.blasterSounds);
        Destroy(projectile, 3f);
    }
}
