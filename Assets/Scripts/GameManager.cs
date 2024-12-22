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

    [Header("'Laser' Variables")]
    public int laserCount = 0;
    public float laserRange = 3f; // Radius of the turret orbit around the earth
    public float heatPerShot = 1f; // Heat generated per shot
    public float maxHeat = 10f; // Maximum heat before cooldown
    public float heatDissipationRate = 0.8f; // Rate at which heat dissipates over time
    public float laserDamage = 1; // Damage dealt by the laser
    public float laserFireRate = 1f; // Fire rate of the laser

    [Header("Turret Variables")]
    public int turretCount = 0;
    public float turretOrbitRadius = 2f;
    public float orbitSpeed = 5f;
    public float turretDetectionRange = 1f;
    public float turretFireRate = 1f;
    public float turretDamage = 1f; // Damage dealt by the turret
    public float turretBulletSpeed = 5f;

    [Header("Blaster Variables")]
    public int blasterCount = 1; // Fire rate of the blaster
    public float blasterFireRate = 1f; // Fire rate of the blaster
    public float blasterDamage = 2f; // Damage of the blaster
    public float bulletInaccuracy = 5f; // Inaccuracy of the blaster bullets
    public float blasterBulletSpeed = 5f;

    [Header("Defence Rig Variables")]
    public float rigRotationSpeed = 5f; // Speed at which the defence rig rotates towards the mouse 
    public int numberOfMinesCount = 0;
    public float mineSpawnInterval = 4f;

    private List<UpgradeMethod> obtainedUpgrades = new List<UpgradeMethod>();

    private List<UpgradeMethod> laserUpgrades = new List<UpgradeMethod>() {
        UpgradeMethod.addLaser1,
        UpgradeMethod.addLaser2,
        UpgradeMethod.addLaser3,
        UpgradeMethod.addLaser4,
        UpgradeMethod.addLaser5,
        UpgradeMethod.LaserDamageIncrease1,
        UpgradeMethod.LaserDamageIncrease2,
        UpgradeMethod.LaserFireRateIncrease1,
        UpgradeMethod.LaserFireRateIncrease2
    };

    private List<UpgradeMethod> turretUpgrades = new List<UpgradeMethod>() {
        UpgradeMethod.addTurret1,
        UpgradeMethod.addTurret2,
        UpgradeMethod.addTurret3,
        UpgradeMethod.addTurret4,
        UpgradeMethod.addTurret5,
        UpgradeMethod.TurretDamageIncrease1,
        UpgradeMethod.TurretDamageIncrease2,
        UpgradeMethod.TurretFireRateIncrease1,
        UpgradeMethod.TurretFireRateIncrease2,
        UpgradeMethod.TurretSpeedIncrease1,
        UpgradeMethod.TurretSpeedIncrease2,
        UpgradeMethod.TurretRadiusIncrease1,
        UpgradeMethod.TurretRadiusIncrease2
    };

    private List<UpgradeMethod> blasterUpgrades = new List<UpgradeMethod>() {
        UpgradeMethod.addBlaster1,
        UpgradeMethod.addBlaster2,
        UpgradeMethod.addBlaster3,
        UpgradeMethod.addBlaster4,
        UpgradeMethod.addBlaster5,
    };

    private UpgradeMethod[] availableUpgrades;

    private void Update()
    {
        if (InputHandler.Instance.TestL())
            UIHandler.UpgradeMenu();
    }

    void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InputHandler = InputHandler.Instance;
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
        blasterDamage += 1;
    }

    public void BulletDamageIncrease2()
    {
        Debug.Log("Upgraded Bullet Damage 2");
        blasterDamage += 1.5f;
    }

    public void BulletDamageIncrease3()
    {
        Debug.Log("Upgraded Bullet Damage 3");
        blasterDamage += 2;
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
        defenceSystem.SpawnLasers(laserCount);
    }

    public void FirstTurret()
    {
        Debug.Log("Unlocked First Turret");
        turretCount++;
        defenceSystem.SpawnTurrets(turretCount);
    }

    public void FirstBlaster()
    {
        Debug.Log("Unlocked First Blaster");
        blasterCount++;
        defenceSystem.SpawnBlasters(blasterCount);
    }

    public void addLaser1()
    {
        Debug.Log("Added Laser");
        laserCount++;
        defenceSystem.SpawnLasers(laserCount);
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

    public void LaserDamageIncrease1()
    {
        Debug.Log("Increased Laser Damage 1");
        laserDamage += 1;
    }

    public void LaserDamageIncrease2()
    {
        Debug.Log("Increased Laser Damage 2");
        laserDamage += 1.5f;
    }

    public void LaserFireRateIncrease1()
    {
        Debug.Log("Increased Laser Fire Rate 1");
        laserFireRate += 0.5f;
    }

    public void LaserFireRateIncrease2()
    {
        Debug.Log("Increased Laser Fire Rate 2");
        laserFireRate += 0.75f;
    }

    public void TurretDamageIncrease1()
    {
        Debug.Log("Increased Turret Damage 1");
        turretDamage += 1;
    }

    public void TurretDamageIncrease2()
    {
        Debug.Log("Increased Turret Damage 2");
        turretDamage += 1.5f;
    }

    public void TurretFireRateIncrease1()
    {
        Debug.Log("Increased Turret Fire Rate 1");
        turretFireRate += 0.5f;
    }

    public void TurretFireRateIncrease2()
    {
        Debug.Log("Increased Turret Fire Rate 2");
        turretFireRate += 0.75f;
    }

    public void TurretSpeedIncrease1()
    {
        Debug.Log("Increased Turret Speed 1");
        orbitSpeed += 1;
    }

    public void TurretSpeedIncrease2()
    {
        Debug.Log("Increased Turret Speed 2");
        orbitSpeed += 1.5f;
    }

    public void TurretRadiusIncrease1()
    {
        Debug.Log("Increased Turret Radius 1");
        turretOrbitRadius += 0.5f;
    }

    public void TurretRadiusIncrease2()
    {
        Debug.Log("Increased Turret Radius 2");
        turretOrbitRadius += 0.75f;
    }

    public void LaserHeatDissipationIncrease1()
    {
        Debug.Log("Increased Laser Heat Dissipation 1");
        heatDissipationRate += 0.2f;
    }

    public void LaserHeatDissipationIncrease2()
    {
        Debug.Log("Increased Laser Heat Dissipation 2");
        heatDissipationRate += 0.4f;
    }

    public UpgradeMethod[] GetAvailableUpgrades()
    {
        List<UpgradeMethod> available = new List<UpgradeMethod>(availableUpgrades);

        if (obtainedUpgrades.Contains(UpgradeMethod.FirstLaser))
            available.AddRange(laserUpgrades);
        if (obtainedUpgrades.Contains(UpgradeMethod.FirstTurret))
            available.AddRange(turretUpgrades);
        if (obtainedUpgrades.Contains(UpgradeMethod.FirstBlaster))
            available.AddRange(blasterUpgrades);

        available.RemoveAll(upgrade => obtainedUpgrades.Contains(upgrade));
        return available.ToArray();
    }
}
public enum UpgradeMethod
{
    // unlocks
    FirstLaser,
    FirstTurret,
    FirstBlaster,

    // Laser Upgrades
    addLaser1,
    addLaser2,
    addLaser3,
    addLaser4,
    addLaser5,
    addLaser6,
    LaserDamageIncrease1,
    LaserDamageIncrease2,
    LaserFireRateIncrease1,
    LaserFireRateIncrease2,
    LaserHeatDissipationIncrease1, // New upgrade
    LaserHeatDissipationIncrease2, // New upgrade

    // Turret Upgrades
    addTurret1,
    addTurret2,
    addTurret3,
    addTurret4,
    addTurret5,
    TurretDamageIncrease1,
    TurretDamageIncrease2,
    TurretFireRateIncrease1,
    TurretFireRateIncrease2,
    TurretSpeedIncrease1,
    TurretSpeedIncrease2,
    TurretRadiusIncrease1,
    TurretRadiusIncrease2,

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
