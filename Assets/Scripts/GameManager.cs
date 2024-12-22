using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [HideInInspector] public InputHandler InputHandler;
    [HideInInspector] public UIHandler UIHandler;

    private DefenseSystem defenceSystem;
    public int resources = 0; // Resource count
    public int baseResourcesNeeded = 5;
    public int currentResourcesNeeded;

    public float health = 100; // Health of the earth

    public float bulletDamageMultiplier = 1f;

    [Header("'Laser' Variables")]
    public int laserCount = 0;
    public float laserRange = 3f; // Radius of the turret orbit around the earth
    public float heatPerShot = 1; // Heat generated per shot
    public float maxHeat = 100f; // Maximum heat before cooldown
    public float heatDissipationRate = 5f; // Rate at which heat dissipates over time
    public float laserDamage = 1; // Damage dealt by the laser


    [Header("Turret Variables")]
    public int turretCount = 0;
    public float orbitRadius = 2f;
    public float orbitSpeed = 5f;
    public float turretDetectionRange = 1f;
    public float turretFireRate = 1f;
    public float turretRadius = 3f; // Radius of the turret orbit around the earth


    [Header("Blaster Variables")]
    public int blasterCount = 1; // Fire rate of the blaster
    public float blasterFireRate = 1f; // Fire rate of the blaster
    public float blasterDamage = 10f; // Damage of the blaster

    [Header("Defence Rig Variables")]
    public float rigRotationSpeed = 5f; // Speed at which the defence rig rotates towards the mouse 
    public int numberOfMinesCount = 0;
    public float mineSpawnInterval = 4f;

    private List<UpgradeMethod> obtainedUpgrades = new List<UpgradeMethod>();

    private List<UpgradeMethod> laserUpgrades = new List<UpgradeMethod>() {
        UpgradeMethod.addTurret1,
        UpgradeMethod.addTurret2,
        UpgradeMethod.addTurret3,
        UpgradeMethod.addTurret4,
        UpgradeMethod.addTurret5
    };

    private List<UpgradeMethod> turretUpgrades = new List<UpgradeMethod>() {
        UpgradeMethod.addTurret1,
        UpgradeMethod.addTurret2,
        UpgradeMethod.addTurret3,
        UpgradeMethod.addTurret4,
        UpgradeMethod.addTurret5,
    };

    private UpgradeMethod[] availableUpgrades;

    private void Update()
    {
        if ((Input.GetButtonDown("L")))
        {
            UIHandler.UpgradeMenu();
        }
    }

    void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InputHandler = gameObject.AddComponent<InputHandler>();
            availableUpgrades = (UpgradeMethod[])System.Enum.GetValues(typeof(UpgradeMethod));
            defenceSystem = GameObject.FindGameObjectWithTag("DefenceRig").GetComponent<DefenseSystem>();
        }
        else
        {
            Destroy(gameObject);
        }
        currentResourcesNeeded = baseResourcesNeeded;
    }

    // Method to add resources
    public void AddResources(int amount)
    {
        resources += amount;

        // If the player has the required amount of resources, upgrade
        if (currentResourcesNeeded <= resources)
        {
            resources -= currentResourcesNeeded;
            currentResourcesNeeded += baseResourcesNeeded;
            UIHandler.UpgradeMenu();
        }
        UIHandler.UpdateResourceBar(currentResourcesNeeded, resources);
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
        obtainedUpgrades.Add(upgrade);
    }

    public void BulletDamageIncrease1()
    {
        Debug.Log("Upgraded Bullet Damage 1");
        bulletDamageMultiplier += 1;
    }

    public void BulletDamageIncrease2()
    {
        Debug.Log("Upgraded Bullet Damage 2");
        bulletDamageMultiplier += 1.5f;
    }

    public void BulletDamageIncrease3()
    {
        Debug.Log("Upgraded Bullet Damage 3");
        bulletDamageMultiplier += 2;
    }

    public void BlasterFireRateIncrease1()
    {
        Debug.Log("Increased Blaster Fire Rate 1");
        blasterFireRate += 0.5f;
    }

    public void BlasterFireRateIncrease2()
    {
        Debug.Log("Increased Blaster Fire Rate 2");
        blasterFireRate += 0.75f;
    }

    public void BlasterFireRateIncrease3()
    {
        Debug.Log("Increased Blaster Fire Rate 3");
        blasterFireRate += 1f;
    }

    public void BlasterFireRateIncrease4()
    {
        Debug.Log("Increased Blaster Fire Rate 4");
        blasterFireRate += 1.25f;
    }

    public void FirstLaser()
    {
        Debug.Log("Unlocked First Laser");
        laserCount++;
        defenceSystem.SpawnLasers(laserCount, 1);
    }

    public void FirstTurret()
    {
        Debug.Log("Unlocked First Turret");
        turretCount++;
        defenceSystem.SpawnTurrets(turretCount);
    }

    public void addLaser1()
    {
        Debug.Log("Added Laser");
        laserCount++;
        defenceSystem.SpawnLasers(laserCount, 1);
    }

    public void addLaser2()
    {
        addLaser1();
    }

    public void addLaser3()
    {
        addLaser1();

    }

    public void addLaser4()
    {
        addLaser1();
    }

    public void addLaser5()
    {
        addLaser1();
    }

    public void addLaser6()
    {
        addLaser1();
    }

    public void addTurret1()
    {
        Debug.Log("Added Turret");
        turretCount++;
        defenceSystem.SpawnTurrets(turretCount);
    }

    public void addTurret2()
    {
        addTurret1();
    }

    public void addTurret3()
    {
        addTurret1();
    }

    public void addTurret4()
    {
        addTurret1();
    }

    public void addTurret5()
    {
        addTurret1();
    }

    public void BulletDamageIncrease()
    {
        Debug.Log("Upgraded Bullets");
        bulletDamageMultiplier += 1;
    }

    public void BlasterFireRateIncrease()
    {
        Debug.Log("Increased Blaster Fire Rate");
        blasterFireRate += 0.5f;
    }
    public void addBlaster1()
    {
        Debug.Log("Added Blaster");
        blasterCount++;
        defenceSystem.SpawnBlasters(blasterCount);
    }

    public void addBlaster2()
    {
        addBlaster1();
    }

    public void addBlaster3()
    {
        addBlaster1();
    }

    public void addBlaster4()
    {
        addBlaster1();
    }

    public void addBlaster5()
    {
        addBlaster1();
    }

    public UpgradeMethod[] GetAvailableUpgrades()
    {
        List<UpgradeMethod> available = new List<UpgradeMethod>(availableUpgrades);

        if (obtainedUpgrades.Contains(UpgradeMethod.FirstLaser))
            available.AddRange(laserUpgrades);
        if (obtainedUpgrades.Contains(UpgradeMethod.FirstTurret))
            available.AddRange(turretUpgrades);

        available.RemoveAll(upgrade => obtainedUpgrades.Contains(upgrade));
        return available.ToArray();
    }
}

public enum UpgradeMethod
{
    // unlocks
    FirstLaser,
    FirstTurret,

    // Laser Upgrades
    addLaser1,
    addLaser2,
    addLaser3,
    addLaser4,
    addLaser5,
    addLaser6,

    // Turret Upgrades
    addTurret1,
    addTurret2,
    addTurret3,
    addTurret4,
    addTurret5,

    // Blaster Upgrades
    addBlaster1,
    addBlaster2,
    addBlaster3,
    addBlaster4,
    addBlaster5,

    BulletDamageIncrease1,
    BulletDamageIncrease2,
    BulletDamageIncrease3,
    BlasterFireRateIncrease1,
    BlasterFireRateIncrease2,
    BlasterFireRateIncrease3,
    BlasterFireRateIncrease4,
}
