﻿using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AE_DataTracker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool useLocalData = false;
    public AgregatedData agregatedData;
    public DateTime firstEntry;
    public DateTime lastEntry;

    public List<RunData> RunData;

    public MainWindow()
    {
        InitializeComponent();

        if (!Directory.Exists(csvDumpDirectory))
        {
            Directory.CreateDirectory(csvDumpDirectory);
        }

        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
    }

    private const string email = "llamawaredatatracking@gmail.com";
    private const string fishEmail = "luxlupus1997@gmail.com";
    private const string appPassword = "pkhn xyru dpnt tsvx ";
    private const string fishAppPassword = "ovaw ejhk liis ohai ";

    private const string csvDumpDirectory = @"C:\CSV_Dump\";
    private const string saveDirectory = @"C:\AggregatedData\";

    private void LoadData_Click(object sender, RoutedEventArgs e)
    {
        StatusLabel.Content = "Loading...";

        if (Directory.Exists(csvDumpDirectory))
        {
            if (!useLocalData)
            {
                foreach (string file in Directory.GetFiles(csvDumpDirectory))
                {
                    File.Delete(file);
                }
            }
        }

        try
        {
            if (!useLocalData)
                GetDataFromEmail();

            LoadAndParseLocalData();

            // Agregated data
            agregatedData = new AgregatedData();
            agregatedData.InitializeData(RunData);
            ProcessRadioButtonFilters();
            agregatedData.firstEntry = firstEntry;
            agregatedData.lastEntry = lastEntry;
            UpdateAggregatedData(agregatedData);
            // All data
            UpdateAllDataTable();

            RunsProcessedLabel.Content = $"Runs processed: {agregatedData.runDataList.Count}";
        }
        catch (AggregateException ex)
        {
            MessageBox.Show(ex.Message, "CSV parsing error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        StatusLabel.Content = "";
    }


    private void GetDataFromEmail()
    {
        CollectCsvAttachments();
    }

    private void LoadAndParseLocalData()
    {
        string[] csvs = LoadAllFilesFromDirectory();

        RunData = new List<RunData>();

        foreach (string csv in csvs)
            RunData.Add(GetRunDataFromCsv(csv));
    }

    public void CollectCsvAttachments()
    {
        using (var client = new ImapClient())
        {
            client.Connect("imap.gmail.com", 993, true);
            client.Authenticate(email, appPassword);

            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadWrite);

            var query =
                        SearchQuery.DeliveredAfter(StartDatePicker.SelectedDate == null ? new DateTime(2025, 5, 17) : (DateTime)StartDatePicker.SelectedDate)
                    .And(
                        SearchQuery.DeliveredBefore(EndDatePicker.SelectedDate == null ? DateTime.Now.AddDays(1) : (DateTime)EndDatePicker.SelectedDate.Value.AddDays(1)))
                    .And(
                        SearchQuery.SubjectContains("RunData"));

            firstEntry = new DateTime(2025, 1, 1);
            lastEntry = DateTime.Now;

            var queriedInbox = inbox.Search(query);
            foreach (var uid in queriedInbox)
            {
                var message = inbox.GetMessage(uid);

                foreach (var attachment in message.Attachments)
                {
                    if (attachment is MimePart part && part.FileName.EndsWith(".csv"))
                    {
                        if (message.Date < firstEntry)
                            firstEntry = message.Date.DateTime;
                        if (message.Date > lastEntry)
                            lastEntry = message.Date.DateTime;

                        var filePath = System.IO.Path.Combine(csvDumpDirectory, $"RunData_{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}_{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}.csv");
                        using (var stream = File.Create(filePath))
                        {
                            part.Content.DecodeTo(stream);
                        }
                    }
                }
            }

            client.Disconnect(true);
            return;
        }
    }


    private void SaveData_Click(object sender, RoutedEventArgs e)
    {
        StatusLabel.Content = "Saving...";

        string fileName = $"RunData_Report_{firstEntry.ToString("yyyyMMdd_HHmmss")}-{lastEntry.ToString("yyyyMMdd_HHmmss")}.csv";
        string filePath = System.IO.Path.Combine(saveDirectory, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"Run Data Report; From: {agregatedData.firstEntry.Date.ToString("yyyy.MM.dd.")}; To: {agregatedData.lastEntry.Date.ToString("yyyy.MM.dd.")}");
            writer.WriteLine();
            writer.WriteLine($"Metric; Min; Max; Avg;", FontStyles.Oblique);
            writer.WriteLine();
            writer.WriteLine($"Win rate;{agregatedData.runsWon}; {agregatedData.runsLost}; {agregatedData.winRate}%");
            writer.WriteLine();
            writer.WriteLine($"Easy locations;min={agregatedData.easyLocationsMinimum}; max={agregatedData.easyLocationsMaximum}; avg={agregatedData.easyLocationsAverage}");
            writer.WriteLine($"Medium locations;min={agregatedData.mediumLocationsMinimum}; max={agregatedData.mediumLocationsMaximum}; avg={agregatedData.mediumLocationsAverage}");
            writer.WriteLine($"Hard locations;min={agregatedData.hardLocationsMinimum}; max={agregatedData.hardLocationsMaximum}; avg={agregatedData.hardLocationsAverage}");
            writer.WriteLine();
            writer.WriteLine($"Scrap collected;min={agregatedData.scrapCollectedMinimum}; max={agregatedData.scrapCollectedMaximum}; avg={agregatedData.scrapCollectedAverage}");
            writer.WriteLine($"Ammo collected;min={agregatedData.ammoCollectedMinimum}; max={agregatedData.ammoCollectedMaximum}; avg={agregatedData.ammoCollectedAverage}");
            writer.WriteLine();
            writer.WriteLine($"Scrap used on wagons;min={agregatedData.scrapUsedWagonsMinimum}; max={agregatedData.scrapUsedWagonsMaximum}; avg={agregatedData.scrapUsedWagonsAverage}");
            writer.WriteLine($"Scrap used on ammo;min={agregatedData.scrapUsedAmmoMinimum}; max={agregatedData.scrapUsedAmmoMaximum}; avg={agregatedData.scrapUsedAmmoAverage}");
            writer.WriteLine($"Scrap used on repairs;min={agregatedData.scrapUsedRepairMinimum}; max={agregatedData.scrapUsedRepairMaximum}; avg={agregatedData.scrapUsedRepairAverage}");
            writer.WriteLine($"Scrap used on upgrades;min={agregatedData.scrapUsedUpgradesMinimum}; max={agregatedData.scrapUsedUpgradesMaximum}; avg={agregatedData.scrapUsedUpgradesAverage}");
            writer.WriteLine($"Ammo used;min={agregatedData.ammoUsedMinimum}; max={agregatedData.ammoUsedMaximum}; avg={agregatedData.ammoUsedAverage}");
            writer.WriteLine();
            writer.WriteLine($"Bosses killed;min={agregatedData.bossesKilledMinimum}; max={agregatedData.bossesKilledMaximum}; avg={agregatedData.bossesKilledAverage}");
            writer.WriteLine();
            writer.WriteLine($"Final hull;min={agregatedData.finalHullMinimum}; max={agregatedData.finalHullMaximum}; avg={agregatedData.finalHullAverage}");
            writer.WriteLine($"Regular damage taken;min={agregatedData.regularDamageTakenMinimum}; max={agregatedData.regularDamageTakenMaximum}; avg={agregatedData.regularDamageTakenAverage}");
            writer.WriteLine($"Hull damage taken;min={agregatedData.hullDamageTakenMinimum}; max={agregatedData.hullDamageTakenMaximum}; avg={agregatedData.hullDamageTakenAverage}");
            writer.WriteLine($"Damage repaired;min={agregatedData.damageRepairedMinimum}; max={agregatedData.damageRepairedMaximum}; avg={agregatedData.damageRepairedAverage}");
            writer.WriteLine();
            writer.WriteLine($"Modules broken;min={agregatedData.modulesBrokenMinimum}; max={agregatedData.modulesBrokenMaximum}; avg={agregatedData.modulesBrokenAverage}");
            writer.WriteLine();
            writer.WriteLine($"Run duration;min={agregatedData.runDurationMinimum}; max={agregatedData.runDurationMaximum}; avg={agregatedData.runDurationAverage}");

            writer.WriteLine($"Total runs;min={agregatedData.totalRunsMinimum}; max={agregatedData.totalRunsMaximum}; avg={agregatedData.totalRunsAverage}");
            writer.WriteLine($"Total runs beaten;min={agregatedData.totalRunsBeatenMinimum}; max={agregatedData.totalRunsBeatenMaximum}; avg={agregatedData.totalRunsBeatenAverage}");
            writer.WriteLine();
            writer.WriteLine($"Current cores;min={agregatedData.currentCoresMinimum}; max={agregatedData.currentCoresMaximum}; avg={agregatedData.currentCoresAverage}");

            // Modules Table
            writer.WriteLine();
            writer.WriteLine("Modules Taken;");
            writer.WriteLine("Module Name;Times Taken;% Taken");
            foreach (var entry in agregatedData.modulesAggr)
            {
                writer.WriteLine($"{entry.Key};{entry.Value.count};{entry.Value.average:0.##}%");
            }

            // Upgrades Table
            writer.WriteLine();
            writer.WriteLine("Upgrades Taken;");
            writer.WriteLine("Upgrade Name;Times Taken;% Taken");
            foreach (var entry in agregatedData.upgradesAggr)
            {
                writer.WriteLine($"{entry.Key};{entry.Value.count};{entry.Value.average:0.##}%");
            }

            // Relics Table
            writer.WriteLine();
            writer.WriteLine("Relics Taken;");
            writer.WriteLine("Relic Name;Times Taken;% Taken");
            foreach (var entry in agregatedData.relicsAggr)
            {
                writer.WriteLine($"{entry.Key};{entry.Value.count};{entry.Value.average:0.##}%");
            }

            // Locations Table
            writer.WriteLine();
            writer.WriteLine("Locations Visited;");
            writer.WriteLine("Location Name;Times Taken;% Taken");
            foreach (var entry in agregatedData.locationsAggr)
            {
                writer.WriteLine($"{entry.Key};{entry.Value.count};{entry.Value.average:0.##}%");
            }

        }

        StatusLabel.Content = "";
    }

    private void ClearData_Click(object sender, RoutedEventArgs e)
    {
        if (Directory.Exists(csvDumpDirectory))
        {
            foreach (string file in Directory.GetFiles(csvDumpDirectory))
            {
                File.Delete(file);
            }
        }

        agregatedData = null;
        UpdateAggregatedData(new AgregatedData());
        DataTableGrid.ItemsSource = new List<RunData>();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private RunData GetRunDataFromCsv(string csv)
    {
        string[] kvs = csv.Split('\n');
        string[] keys = kvs[0].Trim('\r').Split(',');
        string[] values = kvs[1].Trim('\r').Split(',');

        if (keys.Length != values.Length)
        {
            throw new AggregateException("Keys and values can't be mapped due to different sizes. CSV is not in a correct format.");
        }
        FieldInfo[] props = RunData.GetType().GetFields();
        foreach (var field in props)
        {
            if (!keys.Contains(field.Name))
                throw new AggregateException($"CSV keys and RunData fields do no match. Conflicting RunData field: {field.Name}.");
        }

        Dictionary<string, string> kvps = new Dictionary<string, string>();
        for (int i = 0; i < keys.Length; i++)
        {
            kvps.Add(keys[i], values[i]);
        }
        string dataLine = csv.Split('\n')[1];

        string[] data = dataLine.Split(',');

        RunData runData = new RunData()
        {
            runWon = kvps["runWon"] == "True" ? true : false,
            modulesTaken = new List<string>(),
            upgradesTaken = new List<string>(),
            relicsTaken = new List<string>(),
            radarUpgrades = new List<string>(),
            visitedLocations = new List<string>(),
            easyLocations = int.Parse(kvps["EasyLocations"]),
            mediumLocations = int.Parse(kvps["NormalLocations"]),
            hardLocations = int.Parse(kvps["HardLocations"]),
            cannonLocations = int.Parse(kvps["CannonLocations"]),
            moduleLocations = int.Parse(kvps["ModuleLocations"]),
            upgradeLocations = int.Parse(kvps["UpgradeLocations"]),
            relicLocations = int.Parse(kvps["RelicLocations"]),
            shopLocations = int.Parse(kvps["ShopLocations"]),
            levelAtEnd = int.Parse(kvps["LevelAtEnd"]),
            scrapCollected = int.Parse(kvps["ScrapCollected"]),
            scrapUsed = int.Parse(kvps["ScrapUsed"]),
            scrapUsedWagons = int.Parse(kvps["ScrapUsedWagons"]),
            scrapUsedAmmo = int.Parse(kvps["ScrapUsedAmmo"]),
            scrapUsedRepair = int.Parse(kvps["ScrapUsedRepair"]),
            scrapUsedUpgrades = int.Parse(kvps["ScrapUsedUpgrades"]),
            ammoCollected = int.Parse(kvps["AmmoCollected"]),
            ammoUsed = int.Parse(kvps["AmmoUsed"]),
            bossesKilled = int.Parse(kvps["BossesKilled"]),
            finalHull = float.Parse(kvps["FinalHull"]),
            regularDamageTaken = float.Parse(kvps["TotalDamageTaken"]),
            hullDamageTaken = float.Parse(kvps["HullDamageTaken"]),
            damageRepaired = float.Parse(kvps["DamageRepaired"]),
            modulesBroken = int.Parse(kvps["ModulesBroken"]),
            runDuration = float.Parse(kvps["RunDuration"]),
            totalRuns = int.Parse(kvps["TotalRuns"]),
            totalRunsBeaten = int.Parse(kvps["TotalRunsBeaten"]),
            currentCoreCount = int.Parse(kvps["CurrentCoreCount"]),
            damageByEnemy = new Dictionary<string, float>()
        };

        foreach (string module in kvps["Modules"].Split('|'))
            if (module != "")
                runData.modulesTaken.Add(module);

        foreach (string upgrade in kvps["Upgrades"].Split('|'))
            if (upgrade != "")
                runData.upgradesTaken.Add(upgrade);

        foreach (string relic in kvps["Relics"].Split('|'))
            if (relic != "")
                runData.relicsTaken.Add(relic);

        foreach (string radar in kvps["RadarUpgrades"].Split('|'))
            if (radar != "")
                runData.radarUpgrades.Add(radar);

        foreach (string damage in kvps["DamageByEnemy"].Split('|'))
        {
            string[] kvp = damage.Split(":");
            if (kvp.Length != 2)
                continue;
            runData.damageByEnemy.Add(kvp[0], float.Parse(kvp[1]));
        }

        return runData;
    }

    private string[] LoadAllFilesFromDirectory()
    {
        if (!Directory.Exists(csvDumpDirectory))
            throw new DirectoryNotFoundException($"Directory not found: {csvDumpDirectory}");

        string[] files = Directory.GetFiles(csvDumpDirectory);
        string[] allContents = new string[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            string content = File.ReadAllText(files[i]);
            allContents[i] = content;
        }

        return allContents;
    }


    #region Update UI Elements

    public DataTable ConvertToDataTable<T>(List<T> items)
    {
        var table = new DataTable();
        var type = typeof(T);

        // Use fields instead of properties
        var props = type.GetProperties();

        // Create columns
        foreach (var prop in props)
        {
            table.Columns.Add(prop.Name, typeof(string));
        }

        // Create rows
        foreach (var item in items)
        {
            var row = table.NewRow();
            foreach (var prop in props)
            {
                var value = prop.GetValue(item);
                if (value is int iValue)
                {
                    row[prop.Name] = iValue.ToString();
                }
                else if (value is float fValue)
                {
                    row[prop.Name] = fValue.ToString("0.##", CultureInfo.InvariantCulture);
                }
                else if (value is bool bValue)
                {
                    row[prop.Name] = bValue.ToString();
                }
                else if (value is List<string> lValue)
                {
                    if (lValue.Count > 0)
                        row[prop.Name] = string.Join(',', lValue);
                    else
                        row[prop.Name] = "";
                }
                else if (value is Dictionary<string, float> dValue)
                {
                    if (dValue.Count > 0)
                        row[prop.Name] = string.Join(", ", dValue.Select(k => $"{k.Key}: {k.Value}"));
                    else
                        row[prop.Name] = "";
                }
            }
            table.Rows.Add(row);
        }

        return table;
    }

    private void UpdateAggregatedData(AgregatedData data)
    {
        UpdateBasicAgregatedData(data);
        UpdateListData(data);
    }

    private void UpdateBasicAgregatedData(AgregatedData data)
    {
        StatsDataGrid.ItemsSource = data.ProcessBasicData();
    }

    private void UpdateListData(AgregatedData data)
    {
        AggrModules.ItemsSource = GetSortedCollectionFromDict(data.modulesAggr);
        AggrUpgrades.ItemsSource = GetSortedCollectionFromDict(data.upgradesAggr);
        AggrRelics.ItemsSource = GetSortedCollectionFromDict(data.relicsAggr);
        AggrRadarUpgrades.ItemsSource = GetSortedCollectionFromDict(data.radarAggr);
        AggrDamageByEnemy.ItemsSource = GetSortedCollectionFromDict(data.damageByEnemyAggr);
    }

    private void UpdateAllDataTable()
    {
        DataTableGrid.ItemsSource = ConvertToDataTable<RunData>(RunData).DefaultView;
    }

    private void UpdateAllDataTable(List<RunData> data)
    {
        DataTableGrid.ItemsSource = ConvertToDataTable<RunData>(data).DefaultView;
    }
    #endregion

    #region Radio Buttons
    private void ProcessRadioButtonFilters()
    {
        if (RunData == null || RunData.Count == 0)
            return;

        List<RunData> filteredList = new List<RunData>(RunData);
        if ((bool)WinRB.IsChecked)
        {
            filteredList = filteredList.Where(e => e.runWon).ToList();
        }
        else if ((bool)LossRB.IsChecked)
        {
            filteredList = filteredList.Where(e => !e.runWon).ToList();
        }
        if ((bool)QuitRB.IsChecked)
        {
            filteredList = filteredList.Where(e => e.runQuit).ToList();
        }
        else if ((bool)DiedRB.IsChecked)
        {
            filteredList = filteredList.Where(e => !e.runQuit).ToList();
        }
        agregatedData.InitializeData(filteredList);
        UpdateAggregatedData(agregatedData);
        UpdateAllDataTable(filteredList);
    }

    private void AllRB_Click(object sender, RoutedEventArgs e)
    {
        ProcessRadioButtonFilters();
    }

    private void WinRB_Click(object sender, RoutedEventArgs e)
    {
        ProcessRadioButtonFilters();
    }

    private void LossRB_Click(object sender, RoutedEventArgs e)
    {
        ProcessRadioButtonFilters();
    }

    private void AllQRB_Click(object sender, RoutedEventArgs e)
    {
        ProcessRadioButtonFilters();
    }
    private void QuitRB_Click(object sender, RoutedEventArgs e)
    {
        ProcessRadioButtonFilters();
    }
    private void DiedRB_Click(object sender, RoutedEventArgs e)
    {
        ProcessRadioButtonFilters();
    }

    #endregion

    #region Sorting and Filtering
    private List<List3ValueViewModel> GetSortedCollectionFromDict(Dictionary<string, ifPair> dict)
    {
        List<List3ValueViewModel> list = new List<List3ValueViewModel>();
        foreach (var kvp in dict)
        {
            list.Add(new List3ValueViewModel(kvp.Key, kvp.Value.count.ToString(), kvp.Value.average.ToString("0.##")));
        }

        return list.OrderByDescending(item =>
        {
            // Attempt numeric sort
            if (double.TryParse(item.Value3, out double num))
                return num;

            // Fallback to lexicographic order
            return double.MaxValue;
        })
        .ThenBy(item => item.Value3) // Secondary sort for non-numeric
        .ToList();
    }

    private List<List3ValueViewModel> GetSortedCollectionFromDict(Dictionary<string, ffPair> dict)
    {
        List<List3ValueViewModel> list = new List<List3ValueViewModel>();
        foreach (var kvp in dict)
        {
            list.Add(new List3ValueViewModel(kvp.Key, kvp.Value.count.ToString("0.##"), kvp.Value.average.ToString("0.##")));
        }

        return list.OrderByDescending(item =>
        {
            // Attempt numeric sort
            if (double.TryParse(item.Value3, out double num))
                return num;

            // Fallback to lexicographic order
            return double.MaxValue;
        })
        .ThenBy(item => item.Value3) // Secondary sort for non-numeric
        .ToList();
    }

    private void UpgradeFilterTB_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (agregatedData is null || agregatedData.upgradesAggr is null || agregatedData.upgradesAggr.Count == 0)
            return;

        if (e.Key == Key.Enter)
        {
            string filterString = UpgradeFilterTB.Text.ToLower();
            if (string.IsNullOrEmpty(filterString))
            {
                AggrUpgrades.ItemsSource = null;
                AggrUpgrades.ItemsSource = GetSortedCollectionFromDict(agregatedData.upgradesAggr);
            }
            else
            {
                var filteredList = agregatedData.upgradesAggr.Where(x => x.Key.ToLower().Contains(filterString)).ToDictionary();
                AggrUpgrades.ItemsSource = null;
                AggrUpgrades.ItemsSource = GetSortedCollectionFromDict(filteredList);
            }
        }
    }

    #endregion
}