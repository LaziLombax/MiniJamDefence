using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
	public GameObject asteroidPrefab; // Prefab of the asteroid to spawn
	public float spawnInterval = 1f; // Interval between spawns in seconds
	public float difficultyIncrement = 1f; // Amount by which difficulty increases every second
	public float initSpawnDistance = 10f; // Distance from the screen edge to spawn asteroids
	public float baseHealth = 3f; // Base health for the smallest asteroid
	public float baseSize = 0.5f; // Base size for the smallest asteroid

	[SerializeField]
	private float difficulty = 1f; // Initial difficulty level, visible in the editor

	private float nextSpawnTime;

	void Start()
	{
		nextSpawnTime = Time.time + spawnInterval;
	}

	void Update()
	{
		// Increment difficulty over time
		difficulty += difficultyIncrement * Time.deltaTime;

		// Check if it's time to spawn asteroids
		if (Time.time >= nextSpawnTime)
		{
			SpawnAsteroids();
			nextSpawnTime = Time.time + spawnInterval;
		}
	}
    void SpawnAsteroids()
    {
        // Calculate the total health of existing asteroids
        float existingAsteroidsHealth = CalculateExistingAsteroidsHealth();

        // Calculate the remaining health to distribute among new asteroids
        float remainingHealth = difficulty - existingAsteroidsHealth;

        if (remainingHealth <= 0)
        {
            return; // No need to spawn new asteroids if the existing ones already meet or exceed the difficulty
        }

        // Determine the maximum number of asteroids to spawn based on remaining health and base health
        int maxAsteroidCount = Mathf.FloorToInt(remainingHealth / baseHealth);

        // Ensure at least one asteroid is spawned if possible
        int asteroidCount = Mathf.Max(1, maxAsteroidCount);

        for (int i = 0; i < asteroidCount; i++)
        {
            // Instantiate the asteroid
            GameObject asteroid = Instantiate(asteroidPrefab, Vector3.left * 1000, Quaternion.identity); // spawn outside of range
            Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();

            if (asteroidScript != null)
            {
                // Calculate health and size for the asteroid
                float asteroidHealth;
                if (i == asteroidCount - 1)
                {
                    // Assign remaining health to the last asteroid
                    asteroidHealth = remainingHealth;
                }
                else
                {
                    // Ensure each asteroid has at least baseHealth
                    asteroidHealth = Mathf.Min(baseHealth, remainingHealth - baseHealth * (asteroidCount - i - 1));
                }

                float asteroidSize = baseSize * (asteroidHealth / baseHealth);
                asteroid.transform.position = GetRandomSpawnPosition(asteroidSize / 2f); // set position

                // Set the asteroid's health and size
                asteroidScript.health = asteroidHealth;
                asteroidScript.initHealth = asteroidHealth;
                asteroid.transform.localScale = new Vector3(asteroidSize, asteroidSize, asteroidSize);

                // Reduce the remaining total health
                remainingHealth -= asteroidHealth;
            }
        }
    }


    float CalculateExistingAsteroidsHealth()
	{
		float totalHealth = 0f;
		Asteroid[] existingAsteroids = FindObjectsOfType<Asteroid>();

		foreach (Asteroid asteroid in existingAsteroids)
		{
			totalHealth += asteroid.health;
		}

		return totalHealth;
	}

	Vector3 GetRandomSpawnPosition(float size)
	{
		// Get the screen bounds
		Camera mainCamera = Camera.main;
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = mainCamera.orthographicSize * 2;
		float cameraWidth = cameraHeight * screenAspect;
		float spawnDistance = initSpawnDistance + size;

		// Randomly choose a side to spawn the asteroid
		int side = Random.Range(0, 4);
		Vector3 spawnPosition = Vector3.zero;

		switch (side)
		{
			case 0: // Top
				spawnPosition = new Vector3(Random.Range(-cameraWidth / 2, cameraWidth / 2), mainCamera.orthographicSize + spawnDistance, 0);
				break;
			case 1: // Bottom
				spawnPosition = new Vector3(Random.Range(-cameraWidth / 2, cameraWidth / 2), -mainCamera.orthographicSize - spawnDistance, 0);
				break;
			case 2: // Left
				spawnPosition = new Vector3(-cameraWidth / 2 - spawnDistance, Random.Range(-mainCamera.orthographicSize, mainCamera.orthographicSize), 0);
				break;
			case 3: // Right
				spawnPosition = new Vector3(cameraWidth / 2 + spawnDistance, Random.Range(-mainCamera.orthographicSize, mainCamera.orthographicSize), 0);
				break;
		}

		return spawnPosition;
	}
}
