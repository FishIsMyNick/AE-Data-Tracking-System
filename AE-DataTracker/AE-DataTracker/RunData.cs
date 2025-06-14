using MimeKit.Encodings;
using System.Collections.Generic;

[System.Serializable]
public class RunData
{
    public bool runWon; // "Win" or "Loss"
    public bool runQuit;
    public int easyLocations;
    public int mediumLocations;
    public int hardLocations;
    public int cannonLocations;
    public int moduleLocations;
    public int upgradeLocations;
    public int relicLocations;
    public int shopLocations;
    public int levelAtEnd;
    public int scrapCollected;
    public int scrapUsed;
    public int scrapUsedWagons;
    public int scrapUsedAmmo;   
    public int scrapUsedRepair;
    public int scrapUsedUpgrades;
    public int ammoCollected;
    public int ammoUsed;
    public int scrapUsedAsAmmo;
    public int bossesKilled;
    public float finalHull { get; set; }
    public float regularDamageTaken { get; set; }
    public float hullDamageTaken { get; set; }
    public float damageRepaired { get; set; }
    public int modulesBroken;
    public float runDuration { get; set; } // in seconds
    public int totalRuns;
    public int totalRunsBeaten;
    public int currentCoreCount;        
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
