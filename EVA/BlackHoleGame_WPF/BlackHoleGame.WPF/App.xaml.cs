using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BlackHole.ViewModel;
using BlackHole.View;
using BlackHole.Model;
using BlackHole.Persistance;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;
using Microsoft.Win32;

namespace BlackHole
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields
        private BlackHoleGameModel model = null!;
        private BlackHoleGameViewModel viewModel = null!;
        private MainWindow view = null!;
        #endregion

        #region Constructor
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        #endregion

        #region App Event Handler
        private void App_Startup(object sender, StartupEventArgs e)
        {
            // modell létrehozása
            model = new BlackHoleGameModel(new BlackHoleDataHandler());
            model.Victory += new EventHandler<int>(Model_Victory);

            // nézemodell létrehozása
            viewModel = new BlackHoleGameViewModel(model);
            //viewModel.NewGame += new EventHandler<int>(ViewModel_NewGame);
            viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            // nézet létrehozása
            view = new MainWindow();
            view.DataContext = viewModel;
            view.Closing += new System.ComponentModel.CancelEventHandler(View_Close); // eseménykezelés a bezáráshoz
            view.Show();

            viewModel.GenerateTable(5);
            model.NewGame(5);
        }
        #endregion

        #region View Event Handler
        private void View_Close(object? sender, CancelEventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to quit?", "Black Hole Game", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást
            }
        }
        #endregion

        #region ViewModel Event Handlers
        /*
        private void ViewModel_NewGame(object? sender, int size)
        {
            viewModel.GenerateTable(size);
            model.NewGame(size);
        }*/
        private async void ViewModel_LoadGame(object? sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "Load Black Hole Game Save file";
                openFileDialog.Filter = "Black Hole Table|*.bht";
                if (openFileDialog.ShowDialog() == true)
                {
                    // játék betöltése
                    await model.LoadGameAsync(openFileDialog.FileName);
                }
            }
            catch (BlackHoleDataException)
            {
                MessageBox.Show("Could not load the file", "Black Hole Game", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógusablak
                saveFileDialog.Title = "Saving Black Hole Table";
                saveFileDialog.Filter = "Black Hole Table|*.bht";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // játéktábla mentése
                        await model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (BlackHoleDataException)
                    {
                        MessageBox.Show("Could not save the game" + Environment.NewLine + "Incorrect path or the file is not editable", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Could not save the game", "Black Hole Game", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ViewModel_ExitGame(object? sender, EventArgs e)
        {
            view.Close();
        }
        #endregion

        #region Model Event Handlers
        private void Model_Victory(object? sender, int winner)
        {
            switch (winner)
            {
                case 1:
                    MessageBox.Show("Congratulations, player 'RED' has won the game!", "Victory!", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 2:
                    MessageBox.Show("Congratulations, player 'BLUE' has won the game!", "Victory!", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }
        #endregion
    }
}
