using UnityEngine;
using System.Collections;

public class DefenseSystem : MonoBehaviour
{
    public int health = 10; // Health of the defense system
    public GameObject laserPrefab; // Prefab of the laser to spawn
    public GameObject turretPrefab; // Prefab of the turret to spawn
    public GameObject minePrefab; // Prefab of the mine to spawn
    public GameObject blasterPrefab; // Prefab of the blaster to spawn

    public float blasterSpawnRadius = 1f; // Radius for spawning blasters
    public float laserSpawnRadius = 1f; // Radius for spawning lasers

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        SpawnBlasters(1);
        StartCoroutine(SpawnMinesPeriodically());
    }

    void Update()
    {
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.z));

        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, GameManager.Instance.rigRotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            TakeDamage(1); // Adjust the damage value as needed
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DestroyDefenseSystem();
        }
    }

    void DestroyDefenseSystem()
    {
        // Add destruction effects or logic here
        Destroy(gameObject);
    }

    public void SpawnLasers(int numberOfLasers)
    {
        DestroyExistingLasers();
        for (int i = 0; i < numberOfLasers; i++)
        {
            // Calculate the angle for each laser
            float angle = i * Mathf.PI * 2 / numberOfLasers;
            Vector3 laserPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * laserSpawnRadius;

            // Instantiate the laser and set its position and parent
            GameObject laser = Instantiate(laserPrefab, transform.position + laserPosition, Quaternion.identity);
            laser.transform.SetParent(transform);

            // Point the laser away from the center
            float angleDegrees = angle * Mathf.Rad2Deg;
            laser.transform.rotation = Quaternion.AngleAxis(angleDegrees, Vector3.forward);
        }
    }

    void DestroyExistingLasers()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Laser"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void SpawnTurrets(int numberOfTurrets)
    {
        DestroyExistingTurrets();
        for (int i = 0; i < numberOfTurrets; i++)
        {
            GameObject turret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
            Turret turretScript = turret.GetComponent<Turret>();
            if (turretScript != null)
            {
                turretScript.turretIndex = i;
                turretScript.totalTurrets = numberOfTurrets;
            }
            turret.transform.SetParent(transform);
        }
    }

    void DestroyExistingTurrets()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Turret"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    IEnumerator SpawnMinesPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameManager.Instance.mineSpawnInterval);
            for (int i = 0; i < GameManager.Instance.numberOfMinesCount; i++)
            {
                yield return new WaitForSeconds(0.05f);
                SpawnMine();
            }
        }
    }

    void SpawnMine()
    {
        // Instantiate the mine at the defense system's position
        GameObject mine = Instantiate(minePrefab, transform.position, Quaternion.identity);
    }

    public void SpawnBlasters(int numberOfBlasters)
    {
        DestroyExistingBlasters();

        if (numberOfBlasters <= 0)
        {
            return; // No blasters to spawn
        }

        for (int i = 0; i < numberOfBlasters; i++)
        {
            // Calculate the angle for each blaster
            float angle = numberOfBlasters == 1 ? 0 : Mathf.Lerp(-45f, 45f, (float)i / (numberOfBlasters - 1));
            Vector3 blasterPosition = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * blasterSpawnRadius;

            // Instantiate the blaster and set its position and parent
            GameObject blaster = Instantiate(blasterPrefab, transform.position + blasterPosition, Quaternion.identity);
            blaster.transform.SetParent(transform);

            // Point the blaster away from the center
            blaster.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void DestroyExistingBlasters()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Blaster"))
            {
                Destroy(child.gameObject);
            }
        }
    }
}
