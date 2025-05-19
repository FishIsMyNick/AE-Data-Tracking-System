using MimeKit.Encodings;
using System.Collections.Generic;

[System.Serializable]
public class RunData
{
    public bool runWon; // "Win" or "Loss"
    public string runWonString => runWon ? "Win" : "Loss";
    public List<string> modulesTaken;
    public string modulesTakenString => string.Join(", ", modulesTaken);
    public List<string> upgradesTaken;
    public string upgradesTakenString => string.Join(", ", upgradesTaken);
    public List<string> relicsTaken;
    public string relicsTakenString => string.Join(", ", relicsTaken);
    public List<string> radarUpgrades;
    public string radarUpgradesString => string.Join(", ", radarUpgrades);
    public List<string> visitedLocations;
    public string visitedLocationsString => string.Join(", ", visitedLocations);
    public int easyLocations;
    public string easyLocationsString => easyLocations.ToString();
    public int mediumLocations;
    public string mediumLocationsString => mediumLocations.ToString();
    public int hardLocations;
    public string hardLocationsString => hardLocations.ToString();
    public int scrapCollected;

    public int cannonLocations; 
    public string cannonLocationsString => cannonLocations.ToString();
    public int moduleLocations;
    public string moduleLocationsString => moduleLocations.ToString();
    public int upgradeLocations;
    public string upgradeLocationsString => upgradeLocations.ToString();
    public int relicLocations;
    public string relicLocationsString => relicLocations.ToString();
    public int shopLocations;
    public string shopLocationsString => shopLocations.ToString();
    public string scrapCollectedString => scrapCollected.ToString();
    public int ammoCollected;
    public string ammoCollectedString => ammoCollected.ToString();
    public int scrapUsed;
    public string scrapUsedString => scrapUsed.ToString();
    public int scrapUsedWagons;
    public string scrapUsedWagonsString => scrapUsedWagons.ToString();
    public int scrapUsedAmmo;
    public string scrapUsedAmmoString => scrapUsedAmmo.ToString();
    public int scrapUsedRepair;
    public string scrapUsedRepairString => scrapUsedRepair.ToString();
    public int scrapUsedUpgrades;
    public string scrapUsedUpgradesString => scrapUsedUpgrades.ToString();
    public int ammoUsed;
    public string ammoUsedString => ammoUsed.ToString();
    public int bossesKilled;
    public string bossesKilledString => bossesKilled.ToString();
    public float finalHull;
    public string finalHullString => finalHull.ToString();
    public float regularDamageTaken;
    public string regularDamageTakenString => regularDamageTaken.ToString();
    public float hullDamageTaken;
    public string hullDamageTakenString => hullDamageTaken.ToString();
    public float damageRepaired;
    public string damageRepairedString => damageRepaired.ToString();
    public Dictionary<string, float> damageByEnemy;
    public string damageByEnemyString => string.Join(", ", damageByEnemy.OrderBy(kvp => kvp.Value).Select(kvp => $"{kvp.Key}: {kvp.Value}"));
    public int modulesBroken;
    public string modulesBrokenString => modulesBroken.ToString();
    public float runDuration; // in seconds
    public string runDurationString => runDuration.ToString();
    public int totalRuns;
    public string totalRunsString => totalRuns.ToString();
    public int totalRunsBeaten;
    public string totalRunsBeatenString => totalRunsBeaten.ToString();
    public int currentCoreCount;
    public string currentCoreCountString => currentCoreCount.ToString();

    

    // Empty constructor
    public RunData()
    {
        runWon = false;
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
