using UnityEngine;

public class Mine : MonoBehaviour
{
    public float initialSpeed = 5f;
    public float damageRadius = 3f;
    public float damageAmount = 50f;
    public float minSettleTime = 1f;
    public float maxSettleTime = 3f;
    public float slowDownRate = 0.5f;

    private Vector2 direction;
    private float currentSpeed;
    private bool isSettling = false;

    void Start()
    {
        // Set a random direction
        float angle = Random.Range(0f, 360f);
        direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        currentSpeed = initialSpeed;

        // Set a random settle time within the specified range
        float settleTime = Random.Range(minSettleTime, maxSettleTime);

        // Start the settling process
        Invoke("StartSettling", settleTime);
    }

    void Update()
    {
        if (isSettling)
        {
            // Gradually reduce the speed to zero
            currentSpeed = Mathf.Max(0, currentSpeed - slowDownRate * Time.deltaTime);
        }

        // Move the mine in the set direction
        transform.Translate(direction * currentSpeed * Time.deltaTime);
    }

    void StartSettling()
    {
        isSettling = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            // Apply damage to nearby asteroids
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Asteroid"))
                {
                    // Assuming the asteroid has a method to take damage
                    hitCollider.GetComponent<Asteroid>().TakeDamage(damageAmount, WeaponType.Missile, 0.2f);
                }
            }

            // Destroy the mine after collision
            Destroy(gameObject);
        }
    }
}
