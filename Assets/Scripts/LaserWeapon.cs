using UnityEngine;

public class LaserWeapon : MonoBehaviour
{
    public float laserRange = 10f; // Range of the laser
    public float heatPerShot = 1; // Heat generated per shot
    public float maxHeat = 100f; // Maximum heat before cooldown
    public float heatDissipationRate = 5f; // Rate at which heat dissipates over time
    public float laserDamage = 1; // Damage dealt by the laser
    public LineRenderer lineRenderer; // Reference to the LineRenderer component

    public float currentHeat = 0f;
    private bool isCoolingDown = false;

    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                Debug.LogError("LineRenderer component missing from the laser prefab.");
            }
        }
    }

    void Update()
    {
        if (!isCoolingDown)
        {
            ShootLaser();
        }
        else
        {
            DisableLaser();
        }

        // Dissipate heat over time
        if (currentHeat > 0 && !lineRenderer.enabled )
        {
            currentHeat -= heatDissipationRate * Time.deltaTime;
            if (currentHeat < 0)
            {
                currentHeat = 0;
            }
        }

        // Check if cooling down
        if (currentHeat >= maxHeat)
        {
            isCoolingDown = true;
        }
        else if (currentHeat == 0)
        {
            isCoolingDown = false;
        }
    }

    void ShootLaser()
    {
        currentHeat += heatPerShot * Time.deltaTime;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, laserRange);
        bool hitAsteroid = false;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Asteroid"))
            {
                Asteroid asteroid = hit.collider.GetComponent<Asteroid>();
                if (asteroid != null)
                {
                    asteroid.TakeDamage(laserDamage, WeaponType.Laser, 0.02f);
                }

                // Draw the laser line
                DrawLaser(transform.position, hit.point);
                hitAsteroid = true;
                break;
            }
        }

        if (!hitAsteroid)
        {
            // Draw the laser line to the maximum range
            DrawLaser(transform.position, transform.position + transform.right * laserRange);
        }
    }

    void DrawLaser(Vector3 start, Vector3 end)
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            lineRenderer.enabled = true;
        }
    }

    void DisableLaser()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }
}
