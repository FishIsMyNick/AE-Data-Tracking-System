using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Security.RightsManagement;
using System.Text;
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
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public AgregatedData agregatedData;

    public List<RunData> RunData;

    public MainWindow()
    {
        InitializeComponent();
    }

    private const string email = "luxlupus1997@gmail.com";
    private const string appPassword = "ydbi vrdr lkul yggq";
    private const string saveDirectory = @"C:\CSV_Dump\";

    public event PropertyChangedEventHandler? PropertyChanged;

    public void CollectCsvAttachments()
    {
        using (var client = new ImapClient())
        {
            client.Connect("imap.gmail.com", 993, true);
            client.Authenticate(email, appPassword);

            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadWrite);

            var query = SearchQuery.DeliveredAfter(StartDatePicker.SelectedDate == null ? new DateTime(2025, 1, 1) : (DateTime)StartDatePicker.SelectedDate)
                .And(SearchQuery.DeliveredBefore(EndDatePicker.SelectedDate == null ? DateTime.Now : (DateTime)EndDatePicker.SelectedDate))
                .And(SearchQuery.SubjectContains("RunData"));

            foreach (var uid in inbox.Search(query))
            {
                var message = inbox.GetMessage(uid);

                foreach (var attachment in message.Attachments)
                {
                    if (attachment is MimePart part && part.FileName.EndsWith(".csv"))
                    {
                        var filePath = System.IO.Path.Combine(saveDirectory, part.FileName);
                        using (var stream = File.Create(filePath))
                        {
                            part.Content.DecodeTo(stream);
                        }
                        Console.WriteLine($"Saved CSV: {filePath}");
                    }
                }
            }

            client.Disconnect(true);
        }
    }

    private void LoadData_Click(object sender, RoutedEventArgs e)
    {
        StatusLabel.Content = "Loading...";

        if (Directory.Exists(saveDirectory))
        {
            foreach (string file in Directory.GetFiles(saveDirectory))
            {
                File.Delete(file);
            }
        }

        GetDataFromEmail();

        // All data
        AllDataListView.ItemsSource = RunData;

        // Agregated data
        UpdateBasicAgregatedData(agregatedData);
        UpdateListData(agregatedData);

        RunsProcessedLabel.Content = $"Runs processed: {agregatedData.runDataList.Count}";

        StatusLabel.Content = "";
    }

    private void GetDataFromEmail()
    {
        CollectCsvAttachments();

        string[] csvs = LoadAllFilesFromDirectory();

        RunData = new List<RunData>();

        foreach (string csv in csvs)
            RunData.Add(GetRunDataFromCsv(csv));

        agregatedData = new AgregatedData();
        agregatedData.InitializeData(RunData);
        this.DataContext = agregatedData;

        runsLostTB.Text = agregatedData.runsLost;

    }

    private void UpdateBasicAgregatedData(AgregatedData data)
    {
        // Win Rate group
        runsLostTB.Text = data.runsLost;
        runsWonTB.Text = data.runsWon;
        winRateTB.Text = data.winRate;

        // Easy Locations
        easyLocationsMinTB.Text = data.easyLocationsMinimum;
        easyLocationsMaxTB.Text = data.easyLocationsMaximum;
        easyLocationsAvgTB.Text = data.easyLocationsAverage;

        // Medium Locations
        mediumLocationsMinTB.Text = data.mediumLocationsMinimum;
        mediumLocationsMaxTB.Text = data.mediumLocationsMaximum;
        mediumLocationsAvgTB.Text = data.mediumLocationsAverage;

        // Hard Locations
        hardLocationsMinTB.Text = data.hardLocationsMinimum;
        hardLocationsMaxTB.Text = data.hardLocationsMaximum;
        hardLocationsAvgTB.Text = data.hardLocationsAverage;

        // Scrap Collected
        scrapCollectedMinTB.Text = data.scrapCollectedMinimum;
        scrapCollectedMaxTB.Text = data.scrapCollectedMaximum;
        scrapCollectedAvgTB.Text = data.scrapCollectedAverage;

        // Ammo Collected
        ammoCollectedMinTB.Text = data.ammoCollectedMinimum;
        ammoCollectedMaxTB.Text = data.ammoCollectedMaximum;
        ammoCollectedAvgTB.Text = data.ammoCollectedAverage;

        // Scrap Used - Wagons
        scrapUsedWagonsMinTB.Text = data.scrapUsedWagonsMinimum;
        scrapUsedWagonsMaxTB.Text = data.scrapUsedWagonsMaximum;
        scrapUsedWagonsAvgTB.Text = data.scrapUsedWagonsAverage;

        // Scrap Used - Ammo
        scrapUsedAmmoMinTB.Text = data.scrapUsedAmmoMinimum;
        scrapUsedAmmoMaxTB.Text = data.scrapUsedAmmoMaximum;
        scrapUsedAmmoAvgTB.Text = data.scrapUsedAmmoAverage;

        // Scrap Used - Repair
        scrapUsedRepairMinTB.Text = data.scrapUsedRepairMinimum;
        scrapUsedRepairMaxTB.Text = data.scrapUsedRepairMaximum;
        scrapUsedRepairAvgTB.Text = data.scrapUsedRepairAverage;

        // Scrap Used - Upgrades
        scrapUsedUpgradesMinTB.Text = data.scrapUsedUpgradesMinimum;
        scrapUsedUpgradesMaxTB.Text = data.scrapUsedUpgradesMaximum;
        scrapUsedUpgradesAvgTB.Text = data.scrapUsedUpgradesAverage;

        // Ammo Used
        ammoUsedMinTB.Text = data.ammoUsedMinimum;
        ammoUsedMaxTB.Text = data.ammoUsedMaximum;
        ammoUsedAvgTB.Text = data.ammoUsedAverage;

        // Bosses Killed
        bossesKilledMinTB.Text = data.bossesKilledMinimum;
        bossesKilledMaxTB.Text = data.bossesKilledMaximum;
        bossesKilledAvgTB.Text = data.bossesKilledAverage;

        // Final Hull
        finalHullMinTB.Text = data.finalHullMinimum;
        finalHullMaxTB.Text = data.finalHullMaximum;
        finalHullAvgTB.Text = data.finalHullAverage;

        // Regular Damage Taken
        regularDamageTakenMinTB.Text = data.regularDamageTakenMinimum;
        regularDamageTakenMaxTB.Text = data.regularDamageTakenMaximum;
        regularDamageTakenAvgTB.Text = data.regularDamageTakenAverage;

        // Hull Damage Taken
        hullDamageTakenMinTB.Text = data.hullDamageTakenMinimum;
        hullDamageTakenMaxTB.Text = data.hullDamageTakenMaximum;
        hullDamageTakenAvgTB.Text = data.hullDamageTakenAverage;

        // Damage Repaired
        damageRepairedMinTB.Text = data.damageRepairedMinimum;
        damageRepairedMaxTB.Text = data.damageRepairedMaximum;
        damageRepairedAvgTB.Text = data.damageRepairedAverage;

        // Modules Broken
        modulesBrokenMinTB.Text = data.modulesBrokenMinimum;
        modulesBrokenMaxTB.Text = data.modulesBrokenMaximum;
        modulesBrokenAvgTB.Text = data.modulesBrokenAverage;

        // Run Duration
        runDurationMinTB.Text = data.runDurationMinimum;
        runDurationMaxTB.Text = data.runDurationMaximum;
        runDurationAvgTB.Text = data.runDurationAverage;

        // Total Runs
        totalRunsMinTB.Text = data.totalRunsMinimum;
        totalRunsMaxTB.Text = data.totalRunsMaximum;
        totalRunsAvgTB.Text = data.totalRunsAverage;

        // Total Runs Beaten
        totalRunsBeatenMinTB.Text = data.totalRunsBeatenMinimum;
        totalRunsBeatenMaxTB.Text = data.totalRunsBeatenMaximum;
        totalRunsBeatenAvgTB.Text = data.totalRunsBeatenAverage;

        // Current Cores
        currentCoresMinTB.Text = data.currentCoresMinimum;
        currentCoresMaxTB.Text = data.currentCoresMaximum;
        currentCoresAvgTB.Text = data.currentCoresAverage;
    }

    private void UpdateListData(AgregatedData data)
    {
        AggrModules.ItemsSource = data.modulesAggr;
        AggrUpgrades.ItemsSource = data.upgradesAggr;
        AggrRelics.ItemsSource = data.relicsAggr;
        AggrLocations.ItemsSource = data.locationsAggr;
        AggrDamageByEnemy.ItemsSource = data.damageByEnemyAggr;
    }

    private void SaveData_Click(object sender, RoutedEventArgs e)
    {
        StatusLabel.Content = "Save";
    }
    private void ClearData_Click(object sender, RoutedEventArgs e)
    {

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
        Dictionary<string, string> kvps = new Dictionary<string, string>();
        for (int i = 0; i < keys.Length; i++)
        {
            kvps.Add(keys[i], values[i]);
        }
        string dataLine = csv.Split('\n')[1];

        string[] data = dataLine.Split(',');

        RunData runData = new RunData()
        {
            runWon = kvps["RunWon"] == "True" ? true : false,
            modulesTaken = new List<string>(),
            upgradesTaken = new List<string>(),
            relicsTaken = new List<string>(),
            radarUpgrades = new List<string>(),
            visitedLocations = new List<string>(),
            easyLocations = int.Parse(kvps["EasyLocations"]),
            mediumLocations = int.Parse(kvps["NormalLocations"]),
            hardLocations = int.Parse(kvps["HardLocations"]),
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
            runData.modulesTaken.Add(module);

        foreach (string upgrade in kvps["Upgrades"].Split('|'))
            runData.upgradesTaken.Add(upgrade);

        foreach (string relic in kvps["Relics"].Split('|'))
            runData.relicsTaken.Add(relic);

        foreach (string radar in kvps["RadarUpgrades"].Split('|'))
            runData.radarUpgrades.Add(radar);

        foreach (string location in kvps["VisitedLocations"].Split('|'))
            runData.visitedLocations.Add(location);

        foreach (string damage in kvps["DamageByEnemy"].Split('|'))
        {
            string[] kvp = damage.Split(":");
            runData.damageByEnemy.Add(kvp[0], float.Parse(kvp[1]));
        }

        return runData;
    }

    private string[] LoadAllFilesFromDirectory()
    {
        if (!Directory.Exists(saveDirectory))
            throw new DirectoryNotFoundException($"Directory not found: {saveDirectory}");

        string[] files = Directory.GetFiles(saveDirectory);
        string[] allContents = new string[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            string content = File.ReadAllText(files[i]);
            allContents[i] = content;
        }

        return allContents;
    }
}