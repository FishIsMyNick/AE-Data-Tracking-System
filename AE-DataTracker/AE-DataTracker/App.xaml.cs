using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;

namespace AE_DataTracker;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        CultureInfo culture = new CultureInfo("en-US"); // or "sv-SE", "fr-FR", etc.
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}

