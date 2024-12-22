using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 shootDirection;
    public BulletType bulletType = BulletType.Basic;
    private float bulletSpeed;
    private void Start()
    {
        // Calculate the random spread
        float randomSpread = Random.Range(-GameManager.Instance.bulletInaccuracy, GameManager.Instance.bulletInaccuracy);
        // Apply the random spread to the forward direction of the projectile
        shootDirection = Quaternion.Euler(0, 0, randomSpread) * transform.right;

        if (bulletType == BulletType.Basic)
            bulletSpeed = GameManager.Instance.blasterBulletSpeed;
        else if (bulletType == BulletType.Turret)
            bulletSpeed = GameManager.Instance.turretBulletSpeed;
        else
            Debug.LogError("Bullet type not recognized.");
    }

    private void Update()
    {
        // Move the projectile in the calculated direction
        transform.position += shootDirection * bulletSpeed * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            collision.gameObject.GetComponent<Asteroid>().TakeDamage(CalculateDamage(), WeaponType.Basic, 0f);
            Destroy(gameObject);
        }
    }

    private float CalculateDamage()
    {
        switch (bulletType)
        {
            case BulletType.Basic:
                return GameManager.Instance.blasterDamage;
            case BulletType.Turret:
                return GameManager.Instance.turretDamage;
        }
        return 0;
    }
}


public enum BulletType
{
    Basic,
    Turret,
}