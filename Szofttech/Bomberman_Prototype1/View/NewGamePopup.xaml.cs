using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bomberman_Prototype1.View
{
    /// <summary>
    /// Interaction logic for NewGamePopup.xaml
    /// </summary>
    public partial class NewGamePopup : Window
    {
        #region Fields
        private int playerNumber;
        private int roundNumber;
        private int mapID;
        private bool isBattleRoyale;
        #endregion

        #region Properties
        /// <summary>
        /// This is the Property that will summarize the result of the User's selected options,
        /// in other words, the result of this window.
        /// </summary>
        public (int playerNumber,int roundNumber, int mapID, bool isBattleRoyale) Params
        {
            get
            {
                return (playerNumber,roundNumber, mapID, isBattleRoyale);
            }
        }
        #endregion
        public NewGamePopup()
        {
            InitializeComponent();
        }

        #region Input Event Handlers
        private void RadioButton_PlayerNumber(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            if (radio != null) 
            {
                if (radio.Content.ToString() == "Resources/Menu/one.png") playerNumber = 1;
                else if(radio.Content.ToString() == "Resources/Menu/two.png") playerNumber = 2;
                else { playerNumber = 3; }
                //playerNumber = Convert.ToInt32(radio.Content.ToString());
            }
        }
        private void RoundSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            roundNumber = (int)Math.Round(slider!.Value);
            SliderLabel.Content= (int)Math.Round(slider!.Value);
        }
        private void RadioButton_MapID(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            if (radio != null) 
            {
                if (radio.Content.ToString() == "Resources/Cyber/cyberground.png") mapID = 1;
                else if(radio.Content.ToString() == "Resources/Desert/desertground.png") mapID= 2;
                else { mapID= 3; }
            }
        }
        #endregion

        #region Window Event Handlers
        /// <summary>
        /// Event handler for the possibility of the User changing windows.
        /// We can't let this happen as this window MUST have a result.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            //soo creative omg
            this.Focus();
        }
        #endregion

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            isBattleRoyale = (bool)BattleRoyalRadio.IsChecked!;
            DialogResult = true;
        }
    }
}
