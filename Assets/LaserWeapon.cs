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

    void Update()
    {
        if (Input.GetMouseButton(0) && !isCoolingDown)
        {
            ShootLaser();
        }
        else
        {
            DisableLaser();
        }

        // Dissipate heat over time
        if (currentHeat > 0)
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
        if (currentHeat + heatPerShot <= maxHeat)
        {
            currentHeat += heatPerShot * Time.deltaTime;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, laserRange);
            if (hit.collider != null && hit.collider.CompareTag("Asteroid"))
            {
                Asteroid asteroid = hit.collider.GetComponent<Asteroid>();
                if (asteroid != null)
                {
                    asteroid.TakeDamage(laserDamage, WeaponType.Laser, 0.02f);
                }

                // Draw the laser line
                DrawLaser(transform.position, hit.point);
            }
            else
            {
                // Draw the laser line to the maximum range
                DrawLaser(transform.position, transform.position + transform.right * laserRange);
            }
        }
    }

    void DrawLaser(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.enabled = true;

    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;
    }
}
