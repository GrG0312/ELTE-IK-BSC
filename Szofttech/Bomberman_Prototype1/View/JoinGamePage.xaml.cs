using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bomberman_Prototype1.View
{
    /// <summary>
    /// Interaction logic for JoinGamePage.xaml
    /// </summary>
    public partial class JoinGamePage : Window
    {
        public string Address { get; private set; } = string.Empty;
        public JoinGamePage()
        {
            InitializeComponent();
        }

        private void JoinBtn_Click(object sender, RoutedEventArgs e)
        {
            Address = ipAddress.Text;
            DialogResult = true;
        }
    }
}
