using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE_DataTracker
{
    public class AgregatedData
    {
        public List<RunData> runDataList;
        public DateTime firstEntry;
        public DateTime lastEntry;

        public string runsWon;
        public string runsLost;
        public string winRate;

        public string easyLocationsMinimum;
        public string easyLocationsMaximum;
        public string easyLocationsAverage;

        public string mediumLocationsMinimum;
        public string mediumLocationsMaximum;
        public string mediumLocationsAverage;

        public string hardLocationsMinimum;
        public string hardLocationsMaximum;
        public string hardLocationsAverage;

        public string scrapCollectedMinimum;
        public string scrapCollectedMaximum;
        public string scrapCollectedAverage;

        public string ammoCollectedMinimum;
        public string ammoCollectedMaximum;
        public string ammoCollectedAverage;

        public string scrapUsedWagonsMinimum;
        public string scrapUsedWagonsMaximum;
        public string scrapUsedWagonsAverage;

        public string scrapUsedAmmoMinimum;
        public string scrapUsedAmmoMaximum;
        public string scrapUsedAmmoAverage;

        public string scrapUsedRepairMinimum;
        public string scrapUsedRepairMaximum;
        public string scrapUsedRepairAverage;

        public string scrapUsedUpgradesMinimum;
        public string scrapUsedUpgradesMaximum;
        public string scrapUsedUpgradesAverage;

        public string ammoUsedMinimum;
        public string ammoUsedMaximum;
        public string ammoUsedAverage;

        public string bossesKilledMinimum;
        public string bossesKilledMaximum;
        public string bossesKilledAverage;

        public string finalHullMinimum;
        public string finalHullMaximum;
        public string finalHullAverage;

        public string regularDamageTakenMinimum;
        public string regularDamageTakenMaximum;
        public string regularDamageTakenAverage;

        public string hullDamageTakenMinimum;
        public string hullDamageTakenMaximum;
        public string hullDamageTakenAverage;

        public string damageRepairedMinimum;
        public string damageRepairedMaximum;
        public string damageRepairedAverage;

        public string modulesBrokenMinimum;
        public string modulesBrokenMaximum;
        public string modulesBrokenAverage;

        public string runDurationMinimum;
        public string runDurationMaximum;
        public string runDurationAverage;

        public string totalRunsMinimum;
        public string totalRunsMaximum;
        public string totalRunsAverage;

        public string totalRunsBeatenMinimum;
        public string totalRunsBeatenMaximum;
        public string totalRunsBeatenAverage;

        public string currentCoresMinimum;
        public string currentCoresMaximum;
        public string currentCoresAverage;

        public Dictionary<string, ifPair> modulesAggr;
        public Dictionary<string, ifPair> upgradesAggr;
        public Dictionary<string, ifPair> relicsAggr;
        public Dictionary<string, ifPair> locationsAggr;
        public Dictionary<string, ffPair> damageByEnemyAggr;

        public AgregatedData()
        {
            Populate0Values();
        }

        public void InitializeData(List<RunData> runDataList)
        {
            this.runDataList = runDataList;
            if (runDataList == null || runDataList.Count == 0)
            {
                Populate0Values();
            }
            else
            {
                ProcessBasicData(runDataList);
                ProcessListData(runDataList);
            }
        }

        public void ProcessBasicData(List<RunData> runDataList)
        {
            int rw = runDataList.Count(r => r.runWon);
            int rl = runDataList.Count - rw;
            runsWon = "Runs won: " + rw.ToString();
            runsLost = "Runs lost: " + rl.ToString();
            winRate = "Win rate: " + (((float)rw / (rw + rl)) * 100f).ToString("F2") + "%";

            easyLocationsMinimum = runDataList.Min(r => r.easyLocations).ToString();
            easyLocationsMaximum = runDataList.Max(r => r.easyLocations).ToString();
            easyLocationsAverage = runDataList.Average(r => r.easyLocations).ToString("F2");

            mediumLocationsMinimum = runDataList.Min(r => r.mediumLocations).ToString();
            mediumLocationsMaximum = runDataList.Max(r => r.mediumLocations).ToString();
            mediumLocationsAverage = runDataList.Average(r => r.mediumLocations).ToString("F2");

            hardLocationsMinimum = runDataList.Min(r => r.hardLocations).ToString();
            hardLocationsMaximum = runDataList.Max(r => r.hardLocations).ToString();
            hardLocationsAverage = runDataList.Average(r => r.hardLocations).ToString("F2");

            scrapCollectedMinimum = runDataList.Min(r => r.scrapCollected).ToString();
            scrapCollectedMaximum = runDataList.Max(r => r.scrapCollected).ToString();
            scrapCollectedAverage = runDataList.Average(r => r.scrapCollected).ToString("F2");

            ammoCollectedMinimum = runDataList.Min(r => r.ammoCollected).ToString();
            ammoCollectedMaximum = runDataList.Max(r => r.ammoCollected).ToString();
            ammoCollectedAverage = runDataList.Average(r => r.ammoCollected).ToString("F2");

            scrapUsedWagonsMinimum = runDataList.Min(r => r.scrapUsedWagons).ToString();
            scrapUsedWagonsMaximum = runDataList.Max(r => r.scrapUsedWagons).ToString();
            scrapUsedWagonsAverage = runDataList.Average(r => r.scrapUsedWagons).ToString("F2");

            scrapUsedAmmoMinimum = runDataList.Min(r => r.scrapUsedAmmo).ToString();
            scrapUsedAmmoMaximum = runDataList.Max(r => r.scrapUsedAmmo).ToString();
            scrapUsedAmmoAverage = runDataList.Average(r => r.scrapUsedAmmo).ToString("F2");

            scrapUsedRepairMinimum = runDataList.Min(r => r.scrapUsedRepair).ToString();
            scrapUsedRepairMaximum = runDataList.Max(r => r.scrapUsedRepair).ToString();
            scrapUsedRepairAverage = runDataList.Average(r => r.scrapUsedRepair).ToString("F2");

            scrapUsedUpgradesMinimum = runDataList.Min(r => r.scrapUsedUpgrades).ToString();
            scrapUsedUpgradesMaximum = runDataList.Max(r => r.scrapUsedUpgrades).ToString();
            scrapUsedUpgradesAverage = runDataList.Average(r => r.scrapUsedUpgrades).ToString("F2");

            ammoUsedMinimum = runDataList.Min(r => r.ammoUsed).ToString();
            ammoUsedMaximum = runDataList.Max(r => r.ammoUsed).ToString();
            ammoUsedAverage = runDataList.Average(r => r.ammoUsed).ToString("F2");

            bossesKilledMinimum = runDataList.Min(r => r.bossesKilled).ToString();
            bossesKilledMaximum = runDataList.Max(r => r.bossesKilled).ToString();
            bossesKilledAverage = runDataList.Average(r => r.bossesKilled).ToString("F2");

            finalHullMinimum = runDataList.Min(r => r.finalHull).ToString();
            finalHullMaximum = runDataList.Max(r => r.finalHull).ToString();
            finalHullAverage = runDataList.Average(r => r.finalHull).ToString("F2");

            regularDamageTakenMinimum = runDataList.Min(r => r.regularDamageTaken).ToString();
            regularDamageTakenMaximum = runDataList.Max(r => r.regularDamageTaken).ToString();
            regularDamageTakenAverage = runDataList.Average(r => r.regularDamageTaken).ToString("F2");

            hullDamageTakenMinimum = runDataList.Min(r => r.hullDamageTaken).ToString();
            hullDamageTakenMaximum = runDataList.Max(r => r.hullDamageTaken).ToString();
            hullDamageTakenAverage = runDataList.Average(r => r.hullDamageTaken).ToString("F2");

            damageRepairedMinimum = runDataList.Min(r => r.damageRepaired).ToString();
            damageRepairedMaximum = runDataList.Max(r => r.damageRepaired).ToString();
            damageRepairedAverage = runDataList.Average(r => r.damageRepaired).ToString("F2");

            modulesBrokenMinimum = runDataList.Min(r => r.modulesBroken).ToString();
            modulesBrokenMaximum = runDataList.Max(r => r.modulesBroken).ToString();
            modulesBrokenAverage = runDataList.Average(r => r.modulesBroken).ToString("F2");

            runDurationMinimum = runDataList.Min(r => r.runDuration).ToString();
            runDurationMaximum = runDataList.Max(r => r.runDuration).ToString();
            runDurationAverage = runDataList.Average(r => r.runDuration).ToString("F2");

            totalRunsMinimum = runDataList.Min(r => r.totalRuns).ToString();
            totalRunsMaximum = runDataList.Max(r => r.totalRuns).ToString();
            totalRunsAverage = runDataList.Average(r => r.totalRuns).ToString("F2");

            totalRunsBeatenMinimum = runDataList.Min(r => r.totalRunsBeaten).ToString();
            totalRunsBeatenMaximum = runDataList.Max(r => r.totalRunsBeaten).ToString();
            totalRunsBeatenAverage = runDataList.Average(r => r.totalRunsBeaten).ToString("F2");

            currentCoresMinimum = runDataList.Min(r => r.currentCoreCount).ToString();
            currentCoresMaximum = runDataList.Max(r => r.currentCoreCount).ToString();
            currentCoresAverage = runDataList.Average(r => r.currentCoreCount).ToString("F2");
        }

        public void ProcessListData(List<RunData> runDataList)
        {
            float totalRuns = runDataList.Count;

            Dictionary<string, ifPair> modules = new Dictionary<string, ifPair>();
            Dictionary<string, ifPair> upgrades = new Dictionary<string, ifPair>();
            Dictionary<string, ifPair> relics = new Dictionary<string, ifPair>();
            Dictionary<string, ifPair> locations = new Dictionary<string, ifPair>();
            Dictionary<string, List<float>> damageByEnemy = new Dictionary<string, List<float>>();
            Dictionary<string, ffPair> damageByEnemyAgr = new Dictionary<string, ffPair>();

            foreach (var run in runDataList)
            {
                foreach (var runModule in run.modulesTaken)
                {
                    if (!modules.ContainsKey(runModule))
                        modules.Add(runModule, new ifPair { count = 0, average = 0 });

                    modules[runModule].count++;
                }

                foreach (var runUpgrade in run.upgradesTaken)
                {
                    if (!upgrades.ContainsKey(runUpgrade))
                        upgrades.Add(runUpgrade, new ifPair { count = 0, average = 0 });

                    upgrades[runUpgrade].count++;
                }

                foreach (var runRelic in run.relicsTaken)
                {
                    if (!relics.ContainsKey(runRelic))
                        relics.Add(runRelic, new ifPair { count = 0, average = 0 });

                    relics[runRelic].count++;
                }

                foreach (var runLocation in run.visitedLocations)
                {
                    if (!locations.ContainsKey(runLocation))
                        locations.Add(runLocation, new ifPair { count = 0, average = 0 });

                    locations[runLocation].count++;
                }
            }
            foreach (var module in modules.Values)
            {
                module.average = module.count / totalRuns * 100;
            }
            foreach (var upgrade in upgrades.Values)
            {
                upgrade.average = upgrade.count / totalRuns * 100;
            }
            foreach (var relic in relics.Values)
            {
                relic.average = relic.count / totalRuns * 100;
            }
            foreach (var location in locations.Values)
            {
                location.average = location.count / totalRuns * 100;
            }

            foreach (var run in runDataList)
            {
                foreach (var damage in run.damageByEnemy)
                {
                    if (!damageByEnemy.ContainsKey(damage.Key))
                        damageByEnemy.Add(damage.Key, new List<float>());

                    damageByEnemy[damage.Key].Add(damage.Value);
                }
            }
            foreach (var damage in damageByEnemy)
            {
                damageByEnemyAgr.Add(damage.Key, new ffPair { count = damage.Value.Sum(), average = damage.Value.Average() });
            }

            modulesAggr = modules;
            upgradesAggr = upgrades;
            relicsAggr = relics;
            locationsAggr = locations;
            damageByEnemyAggr = damageByEnemyAgr;
        }

        private void Populate0Values()
        {
            runsWon = "N/A";
            runsLost = "N/A";
            winRate = "N/A";

            easyLocationsMinimum = "0";
            easyLocationsMaximum = "0";
            easyLocationsAverage = "0";

            mediumLocationsMinimum = "0";
            mediumLocationsMaximum = "0";
            mediumLocationsAverage = "0";

            hardLocationsMinimum = "0";
            hardLocationsMaximum = "0";
            hardLocationsAverage = "0";

            scrapCollectedMinimum = "0";
            scrapCollectedMaximum = "0";
            scrapCollectedAverage = "0";

            ammoCollectedMinimum = "0";
            ammoCollectedMaximum = "0";
            ammoCollectedAverage = "0";

            scrapUsedWagonsMinimum = "0";
            scrapUsedWagonsMaximum = "0";
            scrapUsedWagonsAverage = "0";

            scrapUsedAmmoMinimum = "0";
            scrapUsedAmmoMaximum = "0";
            scrapUsedAmmoAverage = "0";

            scrapUsedRepairMinimum = "0";
            scrapUsedRepairMaximum = "0";
            scrapUsedRepairAverage = "0";

            scrapUsedUpgradesMinimum = "0";
            scrapUsedUpgradesMaximum = "0";
            scrapUsedUpgradesAverage = "0";

            ammoUsedMinimum = "0";
            ammoUsedMaximum = "0";
            ammoUsedAverage = "0";

            bossesKilledMinimum = "0";
            bossesKilledMaximum = "0";
            bossesKilledAverage = "0";

            finalHullMinimum = "0";
            finalHullMaximum = "0";
            finalHullAverage = "0";

            regularDamageTakenMinimum = "0";
            regularDamageTakenMaximum = "0";
            regularDamageTakenAverage = "0";

            hullDamageTakenMinimum = "0";
            hullDamageTakenMaximum = "0";
            hullDamageTakenAverage = "0";

            damageRepairedMinimum = "0";
            damageRepairedMaximum = "0";
            damageRepairedAverage = "0";

            modulesBrokenMinimum = "0";
            modulesBrokenMaximum = "0";
            modulesBrokenAverage = "0";

            runDurationMinimum = "0";
            runDurationMaximum = "0";
            runDurationAverage = "0";

            totalRunsMinimum = "0";
            totalRunsMaximum = "0";
            totalRunsAverage = "0";

            totalRunsBeatenMinimum = "0";
            totalRunsBeatenMaximum = "0";
            totalRunsBeatenAverage = "0";

            currentCoresMinimum = "0";
            currentCoresMaximum = "0";
            currentCoresAverage = "0";
        }
    }
}

public class ifPair
{
    public int count;
    public float average;

    public string countString => count.ToString("F2");
    public string averageString => average.ToString("F2") + "%";
}

public class ffPair
{
    public float count;
    public float average;

    public string countString => count.ToString("F2");
    public string averageString => average.ToString("F2");
}