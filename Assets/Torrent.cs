using UnityEngine;

public class OrbittingTurret : MonoBehaviour
{
    public Transform earth;
    public float orbitRadius = 5f;
    public float orbitSpeed = 30f;
    public float detectionRange = 10f;
    public GameObject bulletPrefab;
    public float fireRate = 1f;

    private float nextFireTime = 0f;
    private float angle = 0f;

    void Start()
    {
        if (earth == null)
        {
            earth = GameObject.FindGameObjectWithTag("Earth").transform;
        }

        // Initialize the turret's position based on the orbit radius
        UpdatePosition();
    }

    void Update()
    {
        if (earth == null) return;

        // Update the angle based on the orbit speed
        angle += orbitSpeed * Time.deltaTime;
        if (angle >= 360f) angle -= 360f;

        // Update the turret's position based on the new angle
        UpdatePosition();

        // Detect nearby asteroids
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Asteroid") && Time.time > nextFireTime)
            {
                FireBullet(hitCollider.transform);
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    void UpdatePosition()
    {
        float radians = angle * Mathf.Deg2Rad;
        float x = earth.position.x + orbitRadius * Mathf.Cos(radians);
        float y = earth.position.y + orbitRadius * Mathf.Sin(radians);
        transform.position = new Vector3(x, y, transform.position.z);
    }

    void FireBullet(Transform target)
    {
        //GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        //Bullet bulletScript = bullet.GetComponent<Bullet>();
        //if (bulletScript != null)
        //{
        //    bulletScript.SetTarget(target);
        //}
    }
}
