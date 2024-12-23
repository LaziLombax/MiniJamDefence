using UnityEngine;

public class LaserWeapon : MonoBehaviour
{
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
            SoundManager.Instance.PlayLaserSound();
        }
        else
        {
            SoundManager.Instance.StopLaserSound();
            DisableLaser();
        }

        // Dissipate heat over time
        if (currentHeat > 0 && !lineRenderer.enabled )
        {
            currentHeat -= GameManager.Instance.heatDissipationRate * Time.deltaTime;
            if (currentHeat < 0)
            {
                currentHeat = 0;
            }
        }

        // Check if cooling down
        if (currentHeat >= GameManager.Instance.maxHeat)
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
        currentHeat += GameManager.Instance.heatPerShot * Time.deltaTime;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, GameManager.Instance.laserRange);
        bool hitAsteroid = false;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Asteroid"))
            {
                Asteroid asteroid = hit.collider.GetComponent<Asteroid>();
                if (asteroid != null)
                {
                    asteroid.TakeDamage(GameManager.Instance.laserDamage, WeaponType.Laser, 0.02f);
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
            DrawLaser(transform.position, transform.position + transform.right * GameManager.Instance.laserRange);
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
