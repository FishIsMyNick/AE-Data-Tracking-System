using MimeKit.Encodings;
using System.Collections.Generic;

[System.Serializable]
public class RunData
{
    public bool runWon { get; set; } // "Win" or "Loss"
    public bool runQuit { get; set; }
    public int easyLocations { get; set; }
    public int mediumLocations { get; set; }
    public int hardLocations { get; set; }
    public int cannonLocations { get; set; }
    public int moduleLocations { get; set; }
    public int upgradeLocations { get; set; }
    public int relicLocations { get; set; }
    public int shopLocations { get; set; }
    public int levelAtEnd { get; set; }
    public int scrapCollected { get; set; }
    public int scrapUsed { get; set; }
    public int scrapUsedWagons { get; set; }
    public int scrapUsedAmmo { get; set; }
    public int scrapUsedRepair { get; set; }
    public int scrapUsedUpgrades { get; set; }
    public int ammoCollected { get; set; }
    public int ammoUsed { get; set; }
    public int scrapUsedAsAmmo { get; set; }
    public int bossesKilled { get; set; }
    public float finalHull { get; set; }
    public float regularDamageTaken { get; set; }
    public float hullDamageTaken { get; set; }
    public float damageRepaired { get; set; }
    public int modulesBroken { get; set; }
    public float runDuration { get; set; } // in seconds
    public int totalRuns { get; set; }
    public int totalRunsBeaten { get; set; }
    public int currentCoreCount { get; set; }
    public Dictionary<string, float> damageByEnemy { get; set; }
    public List<string> modulesTaken { get; set; }
    public List<string> upgradesTaken { get; set; }
    public List<string> relicsTaken { get; set; }
    public List<string> radarUpgrades { get; set; }
    public List<string> visitedLocations { get; set; }

    // Empty constructor
    public RunData()
    {
        runWon = false;
        runQuit = false;
        modulesTaken = new List<string>();
        upgradesTaken = new List<string>();
        relicsTaken = new List<string>();
        radarUpgrades = new List<string>();
        visitedLocations = new List<string>();
        easyLocations = 0;
        mediumLocations = 0;
        hardLocations = 0;
        scrapCollected = 0;
        scrapUsed = 0;
        scrapUsedWagons = 0;
        scrapUsedAmmo = 0;
        scrapUsedRepair = 0;
        scrapUsedUpgrades = 0;
        ammoCollected = 0;
        ammoUsed = 0;
        bossesKilled = 0;
        finalHull = 0;
        regularDamageTaken = 0;
        hullDamageTaken = 0;
        damageRepaired = 0;
        damageByEnemy = new Dictionary<string, float>();
        modulesBroken = 0;
        runDuration = 0;
        totalRuns = 0;
        totalRunsBeaten = 0;
        currentCoreCount = 0;
    }
}
