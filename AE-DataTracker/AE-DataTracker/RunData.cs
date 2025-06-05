using MimeKit.Encodings;
using System.Collections.Generic;

[System.Serializable]
public class RunData
{
    public bool runWon; // "Win" or "Loss"
    public bool runQuit;
    public List<string> modulesTaken;
    public List<string> upgradesTaken;
    public List<string> relicsTaken;
    public List<string> radarUpgrades;
    public List<string> visitedLocations;
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
    public float finalHull;
    public float regularDamageTaken;
    public float hullDamageTaken;
    public float damageRepaired;
    public Dictionary<string, float> damageByEnemy;
    public int modulesBroken;
    public float runDuration; // in seconds
    public int totalRuns;
    public int totalRunsBeaten;
    public int currentCoreCount;

    public string runWonString => runWon ? "Win" : "Loss";
    public string runQuitString => runQuit ? "Quit" : (runWon ? "Win" : "Loss");
    public string modulesTakenString => string.Join(", ", modulesTaken);
    public string upgradesTakenString => string.Join(", ", upgradesTaken);
    public string relicsTakenString => string.Join(", ", relicsTaken);
    public string radarUpgradesString => string.Join(", ", radarUpgrades);
    public string visitedLocationsString => string.Join(", ", visitedLocations);
    public string easyLocationsString => easyLocations.ToString();
    public string mediumLocationsString => mediumLocations.ToString();
    public string hardLocationsString => hardLocations.ToString();
    public string cannonLocationsString => cannonLocations.ToString();
    public string moduleLocationsString => moduleLocations.ToString();
    public string upgradeLocationsString => upgradeLocations.ToString();
    public string relicLocationsString => relicLocations.ToString();
    public string shopLocationsString => shopLocations.ToString();
    public string levelAtEndString => levelAtEnd.ToString();
    public string scrapCollectedString => scrapCollected.ToString();
    public string ammoCollectedString => ammoCollected.ToString();
    public string scrapUsedString => scrapUsed.ToString();
    public string scrapUsedWagonsString => scrapUsedWagons.ToString();
    public string scrapUsedAmmoString => scrapUsedAmmo.ToString();
    public string scrapUsedRepairString => scrapUsedRepair.ToString();
    public string scrapUsedUpgradesString => scrapUsedUpgrades.ToString();
    public string ammoUsedString => ammoUsed.ToString();
    public string bossesKilledString => bossesKilled.ToString();
    public string finalHullString => finalHull.ToString();
    public string regularDamageTakenString => regularDamageTaken.ToString();
    public string hullDamageTakenString => hullDamageTaken.ToString();
    public string damageRepairedString => damageRepaired.ToString();
    public string damageByEnemyString => string.Join(", ", damageByEnemy.OrderBy(kvp => kvp.Value).Select(kvp => $"{kvp.Key}: {kvp.Value}"));
    public string modulesBrokenString => modulesBroken.ToString();
    public string runDurationString => runDuration.ToString();
    public string totalRunsString => totalRuns.ToString();
    public string totalRunsBeatenString => totalRunsBeaten.ToString();
    public string currentCoreCountString => currentCoreCount.ToString();

    

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
