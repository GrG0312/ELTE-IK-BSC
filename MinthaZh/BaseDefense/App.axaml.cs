using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using BaseDefense.Model;
using BaseDefense.ViewModels;
using BaseDefense.Views;
using System;
using System.IO;

namespace BaseDefense;

public partial class App : Application
{
    private MainViewModel mainViewModel;
    private GameModel gameModel;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        gameModel = new GameModel();
        mainViewModel = new MainViewModel(gameModel);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            desktop.Exit += Desktop_Exit;
            desktop.Startup += Desktop_Startup;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void Desktop_Startup(object? sender, ControlledApplicationLifetimeStartupEventArgs e)
    {
        // új játék automatikusan kezdődik a MainViewModel konstruktorban
        // ezért azt nem kell itt elindítani
        try
        {
            // lekérdezzük a dokumentum mappának a helyét, megnézzük, hogy azon belül létezik-e SuspendedGame
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SuspendedGame");
            gameModel.LoadGame(path);
        }
        catch { /* Amennyiben hibát kaptunk, úgy nem létezik az elmentett fájl */ }
    }

    private void Desktop_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        // mentsük el a játék állását
        try
        {
            // a mentés helye legyen a dokumentumok mappa, mert oda garantáltan van jogunk írni
            // betöltésnél itt keressük majd a fájlt
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SuspendedGame");
            gameModel.SaveGame(path);
        }
        catch 
        { 
            /* Ha hibát kapunk, úgy valszeg olyan helyet választottunk, ahova nincs engedélyünk írni, 
             * esetleg a fájlkezelés során történt valami 
             * Ha nem vagyunk biztosak benne, használjunk több catch-blokkot és több fajta exceptiont!
             */ 
        }
    }
}
