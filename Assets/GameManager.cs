using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int resources = 0; // Resource count

    public float health = 100; // Health of the earth

    public float bulletDamageMultiplier = 1f;

    void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to add resources
    public void AddResources(int amount)
    {
        resources += amount;
        Debug.Log("Resources: " + resources);
    }

    public void RecordDamage(GameObject asteroid)
    {
        // Calculate damage based on the size of the asteroid
        float asteroidSize = asteroid.transform.localScale.x;
        float damage = asteroidSize * 10; // Example: damage is 10 times the size of the asteroid

        // Reduce the health of the earth
        health -= damage;

        Debug.Log("Damage recorded for asteroid: " + asteroid.name + ", Damage: " + damage + ", Earth Health: " + health);
    }

    public void Upgrade(UpgradeMethod upgrade)
    {
        Invoke(upgrade.ToString(), 0f);
    }

    public void BulletDamageIncrease()
    {
        bulletDamageMultiplier += 1;
    }

}


public enum UpgradeMethod
{
    BulletDamageIncrease,

}