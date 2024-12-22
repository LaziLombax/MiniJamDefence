using UnityEngine;
using System.Collections;

public class DefenseSystem : MonoBehaviour
{
    public int health = 10; // Health of the defense system
    public float rotationSpeed = 5f; // Speed at which the defense system rotates towards the mouse
    public GameObject laserPrefab; // Prefab of the laser to spawn
    public GameObject turretPrefab; // Prefab of the turret to spawn
    public GameObject minePrefab; // Prefab of the mine to spawn
    public float mineSpawnInterval = 5f; // Interval in seconds between mine spawns

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        SpawnLasers(10, 1f);
        SpawnTurrets(10);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
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

    public void SpawnLasers(int numberOfLasers, float radius)
    {
        DestroyExistingLasers();
        for (int i = 0; i < numberOfLasers; i++)
        {
            // Calculate the angle for each laser
            float angle = i * Mathf.PI * 2 / numberOfLasers;
            Vector3 laserPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

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
            yield return new WaitForSeconds(mineSpawnInterval);
            SpawnMine();
        }
    }

    void SpawnMine()
    {
        // Instantiate the mine at the defense system's position
        GameObject mine = Instantiate(minePrefab, transform.position, Quaternion.identity);
        mine.transform.SetParent(transform);
    }
}
