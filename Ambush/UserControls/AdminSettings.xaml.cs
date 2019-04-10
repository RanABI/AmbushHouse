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

namespace Ambush.UserControls
{
    /// <summary>
    /// Interaction logic for AdminSettings.xaml
    /// </summary>
    public partial class AdminSettings : UserControl
    {

        public AdminSettings()
        {
            InitializeComponent();
        }
        
        private void changeBackground(string RPIid)
        {
            this.r1.Background = new SolidColorBrush(Colors.LightGray);
            this.r2.Background = new SolidColorBrush(Colors.LightGray);
            this.r3.Background = new SolidColorBrush(Colors.LightGray);
            this.r4.Background = new SolidColorBrush(Colors.LightGray);
            this.r5.Background = new SolidColorBrush(Colors.LightGray);

            switch(RPIid)
            {
                case "1":
                    { 
                        this.r1.Background = new SolidColorBrush(Colors.DarkGray);
                        break;
                    }
                case "2":
                    {
                        this.r2.Background = new SolidColorBrush(Colors.DarkGray);
                        break;
                    }
                case "3":
                    {
                        this.r3.Background = new SolidColorBrush(Colors.DarkGray);
                        break;
                    }
                case "4":
                    {
                        this.r4.Background = new SolidColorBrush(Colors.DarkGray);
                        break;
                    }
                case "5":
                    {
                        this.r5.Background = new SolidColorBrush(Colors.DarkGray);
                        break;
                    }

            }
        }

        private void R1_Click(object sender, RoutedEventArgs e)
        {
            RPISettings settings = new RPISettings();
            rpiSettings.Content = settings;
            //currentRPI = "1";
            changeBackground("1");
        }

        private void R2_Click(object sender, RoutedEventArgs e)
        {
            RPISettings settings = new RPISettings();
            rpiSettings.Content = settings;
            //settings.currentRPI = "2";
            changeBackground("2");
        }

        private void R3_Click(object sender, RoutedEventArgs e)
        {
            RPISettings settings = new RPISettings();
            rpiSettings.Content = settings;
            //settings.currentRPI = "3";
            changeBackground("3");
        }

        private void R4_Click(object sender, RoutedEventArgs e)
        {
            RPISettings settings = new RPISettings();
            rpiSettings.Content = settings;
            //settings.currentRPI = "4";
            changeBackground("4");
        }

        private void R5_Click(object sender, RoutedEventArgs e)
        {
            RPISettings settings = new RPISettings();
            rpiSettings.Content = settings;
            //settings.currentRPI = "5";
            changeBackground("5");
        }
    }
}
