using Org.BouncyCastle.Asn1.X509.Qualified;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public string runsQuit;
        public string runsWonOrDied;
        public string runsQuitRate;

        public string easyLocationsMinimum;
        public string easyLocationsMaximum;
        public string easyLocationsAverage;

        public string mediumLocationsMinimum;
        public string mediumLocationsMaximum;
        public string mediumLocationsAverage;

        public string hardLocationsMinimum;
        public string hardLocationsMaximum;
        public string hardLocationsAverage;

        public string cannonLocationsMinimum;
        public string cannonLocationsMaximum;
        public string cannonLocationsAverage;

        public string moduleLocationsMinimum;
        public string moduleLocationsMaximum;
        public string moduleLocationsAverage;

        public string upgradeLocationsMinimum;
        public string upgradeLocationsMaximum;
        public string upgradeLocationsAverage;

        public string relicLocationsMinimum;
        public string relicLocationsMaximum;
        public string relicLocationsAverage;

        public string shopLocationsMinimum;
        public string shopLocationsMaximum;
        public string shopLocationsAverage;

        public string levelAtEndMinimum;
        public string levelAtEndMaximum;
        public string levelAtEndAverage;

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
        public Dictionary<string, ifPair> radarAggr;
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
                ProcessBasicData();
                ProcessListData(runDataList);
            }
        }

        public List<AggregatedPropertyStats> ProcessBasicData()
        {
            float totalRuns = runDataList.Count;
            List<AggregatedPropertyStats> basicDataProps = new List<AggregatedPropertyStats>();

            RunData tempRD = new RunData();

            foreach (var prop in typeof(RunData).GetProperties())
            {
                AggregatedPropertyStats aggrData = new AggregatedPropertyStats();
                aggrData.PropertyName = GetPropertyNameReadable(prop.Name);
                if (prop.GetValue(tempRD) is int)
                {
                    List<int> values = runDataList.Select(item => Convert.ToInt32(prop.GetValue(item))).ToList();
                    if (values != null && values.Count > 0)
                    {
                        aggrData.Min = values.Min().ToString();
                        aggrData.Max = values.Max().ToString();
                        aggrData.Average = values.Average().ToString("0.##", CultureInfo.InvariantCulture);
                    }
                }
                else if (prop.GetValue(tempRD) is float)
                {
                    List<float> values = runDataList.Select(item => float.Parse(prop.GetValue(item).ToString())).ToList();
                    if (values != null && values.Count > 0)
                    {
                        aggrData.Min = values.Min().ToString("0.##", CultureInfo.InvariantCulture);
                        aggrData.Max = values.Max().ToString("0.##", CultureInfo.InvariantCulture);
                        aggrData.Average = values.Average().ToString("0.##", CultureInfo.InvariantCulture);
                    }
                }
                else if (prop.GetValue(tempRD) is bool)
                {
                    List<bool> values = runDataList.Select(item => Convert.ToBoolean(prop.GetValue(item))).ToList();
                    if (values != null && values.Count > 0)
                    {
                        aggrData.Min = values.Where(v => !v).Count().ToString();
                        aggrData.Max = values.Where(v => v).Count().ToString();
                        aggrData.Average = $"{((values.Where(v => v).Count() / totalRuns) * 100f).ToString("0.##", CultureInfo.InvariantCulture)}%";
                    }
                }
                else
                    continue;

                basicDataProps.Add(aggrData);
            }

            return basicDataProps;
        }

        private AggregatedPropertyStats GetProperyStatsFromStrings(string value, string min, string max, string avg)
        {
            return new AggregatedPropertyStats
            {
                PropertyName = GetPropertyNameReadable(value),
                Min = min,
                Max = max,
                Average = avg
            };
        }

        private string GetPropertyNameReadable(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return name;

            // Insert space before each capital letter (except the first character)
            var withSpaces = Regex.Replace(name, "(?<!^)([A-Z])", " $1");

            // Capitalize first letter
            return char.ToUpper(withSpaces[0]) + withSpaces.Substring(1);
        }

        public void ProcessListData(List<RunData> runDataList)
        {
            float totalRuns = runDataList.Count;



            Dictionary<string, ifPair> modules = new Dictionary<string, ifPair>();
            Dictionary<string, ifPair> upgrades = new Dictionary<string, ifPair>();
            Dictionary<string, ifPair> relics = new Dictionary<string, ifPair>();
            Dictionary<string, ifPair> radars = new Dictionary<string, ifPair>();
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

                foreach (var radar in radars)
                {
                    if (!radars.ContainsKey(radar.Key))
                        radars.Add(radar.Key, new ifPair { count = 0, average = 0 });

                    radars[radar.Key].count++;
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
            foreach (var radar in radars.Values)
            {
                radar.average = radar.count / totalRuns * 100;
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
            radarAggr = radars;
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

            cannonLocationsMaximum = "0";
            cannonLocationsMinimum = "0";
            cannonLocationsAverage = "0";

            moduleLocationsMinimum = "0";
            moduleLocationsMaximum = "0";
            moduleLocationsAverage = "0";

            upgradeLocationsMinimum = "0";
            upgradeLocationsMaximum = "0";
            upgradeLocationsAverage = "0";

            relicLocationsMinimum = "0";
            relicLocationsMaximum = "0";
            relicLocationsAverage = "0";

            shopLocationsMinimum = "0";
            shopLocationsMaximum = "0";
            shopLocationsAverage = "0";

            levelAtEndMinimum = "0";
            levelAtEndMaximum = "0";
            levelAtEndAverage = "0";

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

public class AggregatedPropertyStats
{
    public string PropertyName { get; set; } = "";
    public string Min { get; set; } = "";
    public string Max { get; set; } = "";
    public string Average { get; set; } = "";
}