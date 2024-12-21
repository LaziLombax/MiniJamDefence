using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;

public class Asteroid : MonoBehaviour
{
    public float initialSpeed = 5f;
    public float acceleration = 0.5f;
    public int splitUpperBound = 5;
    public float health = 3;
    public float initHealth;
    public float homingFactor = 0.1f; // Factor to adjust how strongly the asteroid homes towards Earth
    public float splitInitialSpeeduUpperBound = 1.5f; // Upper bound for the speed of the split asteroids
    public float randomSpawnOffsetRange = 0.5f; // Range for random spawn offset
    public float splitSizingFactor = 1.2f;
    public float maxVelocity = 10f; // Maximum velocity that can be changed in the editor
    public float splitVelocity = 2f; // Velocity of the split asteroids that can be changed in the editor
    public float collectableSize = 0.5f; // Size threshold for collectable asteroids
    public int resourceValue = 1; // Resource value of the asteroid
    public float pushForce = 2f; // Force applied to push asteroids away from each other

    private Transform earth;
    private Vector3 velocity = Vector3.zero;
    private Dictionary<WeaponType, float> lastDamageTimes = new Dictionary<WeaponType, float>(); // Tracks the last time the asteroid took damage for each weapon type

    public Color pickUpColour;
    public SpriteRenderer SpriteRenderer;

    public VisualEffect breakVFX;
    void Start()
    {
        initHealth = health;
        earth = GameObject.FindGameObjectWithTag("Earth").transform;
        if (earth == null)
        {
            Debug.LogError("Earth not found. Please tag the Earth object with 'Earth'.");
            return;
        }

        Vector3 direction = (earth.position - transform.position).normalized;
        velocity = velocity == Vector3.zero ? direction * initialSpeed : velocity;

        // Set the tag if the asteroid is small enough to be collectable
        if (transform.localScale.x <= collectableSize)
        {
            gameObject.tag = "Collectable";
            SpriteRenderer.color = pickUpColour;
            transform.localScale = new Vector3(collectableSize / 0.8f, collectableSize / 0.8f, collectableSize / 0.8f);
        }
    }

    void Update()
    {
        if (earth == null) return;

        Vector3 directionToEarth = (earth.position - transform.position).normalized;
        velocity = Vector3.Lerp(velocity, directionToEarth * initialSpeed, homingFactor * Time.deltaTime);
        velocity += directionToEarth * acceleration * Time.deltaTime;

        // Clamp the velocity to the maximum velocity
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);

        transform.position += velocity * Time.deltaTime;
    }

    public void DestroyAsteroid()
    {
        if (transform.localScale.x > collectableSize) // Check if the asteroid is larger than the collectable size
        {
            SplitAsteroid();
            Destroy(gameObject);
        }
        else
        {
            // Make the asteroid collectable
            gameObject.tag = "Collectable";
        }
    }

    private void SplitAsteroid()
    {
        int splitCount = Random.Range(2, splitUpperBound);
        float totalSize = transform.localScale.x;
        float averageSize = (totalSize / splitCount) * splitSizingFactor;
        float sizeVariation = (averageSize * 0.2f); // 20% variation

        for (int i = 0; i < splitCount; i++)
        {
            // Randomize the spawn position slightly
            Vector3 randomSpawnOffset = new Vector3(Random.Range(-randomSpawnOffsetRange, randomSpawnOffsetRange), Random.Range(-randomSpawnOffsetRange, randomSpawnOffsetRange), 0);
            GameObject smallerAsteroid = Instantiate(gameObject, transform.position + (randomSpawnOffset * transform.localScale.magnitude), Quaternion.identity);
            Asteroid asteroidScript = smallerAsteroid.GetComponent<Asteroid>();

            if (asteroidScript != null)
            {
                asteroidScript.health = initHealth / splitCount;

                // Calculate the size of the new asteroid with a small variation
                float newSize = averageSize + Random.Range(-sizeVariation, sizeVariation);

                // Set the size of the new asteroid
                smallerAsteroid.transform.localScale = new Vector3(newSize, newSize, newSize);

                // Calculate direction away from the center of the initial asteroid
                Vector3 directionAwayFromCenter = (smallerAsteroid.transform.position - transform.position).normalized;

                // Set the velocity of the split asteroids
                asteroidScript.velocity = directionAwayFromCenter * splitVelocity;
            }
        }
    }

    public void SetInitialVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
    }

    public void TakeDamage(float damage, WeaponType weaponType, float cooldownTime = 0)
    {
        if (!lastDamageTimes.ContainsKey(weaponType))
        {
            lastDamageTimes[weaponType] = -Mathf.Infinity;
        }

        if (Time.time - lastDamageTimes[weaponType] >= cooldownTime)
        {
            health -= damage;
            lastDamageTimes[weaponType] = Time.time;

            if (health <= 0)
            {
                DestroyAsteroid();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Collector") && gameObject.CompareTag("Collectable"))
        {
            // Add resources to the GameManager
            GameManager.Instance.AddResources(resourceValue);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Asteroid"))
        {
            // Push the asteroids away from each other
            Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
            Asteroid otherAsteroid = collision.gameObject.GetComponent<Asteroid>();
            if (otherAsteroid != null)
            {
                // Adjust velocities to push asteroids away from each other
                velocity += -pushDirection * pushForce;
                otherAsteroid.velocity += pushDirection * pushForce;
            }
        }
    }

    public void RunExplode()
    {
        // Implement the logic for the asteroid explosion
        Debug.Log("Asteroid exploded: " + gameObject.name);
        // Add explosion effects, sound, etc.
        Invoke("Explode", 0.5f);
    }

    public void Explode()
    {
        GameManager.Instance.RecordDamage(gameObject);

        // Destroy the asteroid after the explosion
        Destroy(gameObject);
    }
    public void BreakVFX(bool isDestoryed)
    {
        if(isDestoryed)
        {
            breakVFX.SetFloat("Amount", 15f);
            Destroy(breakVFX.gameObject, 2f);
        }
        else
        {
            breakVFX.SetFloat("Amount", 15f);
        }
        breakVFX.SendEvent("Break");
    }

}

public enum WeaponType
{
    Basic,
    Laser,
    Missile,
    // Add other weapon types as needed
}
