using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform earth;
    public GameObject bulletPrefab;
    public int turretIndex = 0; // Index of this turret
    public int totalTurrets = 1; // Total number of turrets

    private float nextFireTime = 0f;
    private float angle = 0f;

    void Start()
    {
        if (earth == null)
        {
            earth = GameObject.FindGameObjectWithTag("Earth").transform;
        }

        // Initialize the turret's position based on the orbit radius and index
        UpdatePosition();
    }

    void Update()
    {
        if (earth == null) return;

        // Update the angle based on the orbit speed
        angle += GameManager.Instance.orbitSpeed * Time.deltaTime;
        if (angle >= 360f) angle -= 360f;

        // Update the turret's position based on the new angle
        UpdatePosition();

        // Detect nearby asteroids
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, GameManager.Instance.turretDetectionRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Asteroid") && Time.time > nextFireTime)
            {
                FireBullet(hitCollider.transform);
                nextFireTime = Time.time + 1f / GameManager.Instance.turretFireRate;
            }
        }
    }

    void UpdatePosition()
    {
        // Calculate the angle offset based on the turret index and total number of turrets
        float angleOffset = (360f / totalTurrets) * turretIndex;
        float radians = (angle + angleOffset) * Mathf.Deg2Rad;
        float x = earth.position.x + GameManager.Instance.turretRadius * Mathf.Cos(radians);
        float y = earth.position.y + GameManager.Instance.turretRadius * Mathf.Sin(radians);
        transform.position = new Vector3(x, y, transform.position.z);
    }

    void FireBullet(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.bulletType = BulletType.Turret;
        }
    }
}
