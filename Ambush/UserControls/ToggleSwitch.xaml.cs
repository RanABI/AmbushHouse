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
    /// Interaction logic for ToggleSwitch.xaml
    /// </summary>
    public partial class ToggleSwitch : UserControl
    {
        Thickness LeftSide = new Thickness(-59, 0, 0, 0);
        Thickness RightSide = new Thickness(0, 0, -59, 0);
        SolidColorBrush Off = new SolidColorBrush(Color.FromRgb(160, 160, 160));
        SolidColorBrush On = new SolidColorBrush(Color.FromRgb(130, 190, 125));
        private bool Toggled = false;
        public ToggleSwitch()
        {
            InitializeComponent();
            DotBackGround.Fill = Off;
            Toggled = false;
            //Dot.Margin = LeftSide;
        }
        
        public bool Toggled1 { get => Toggled; set => Toggled = value; }

        private void Dot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Toggle();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Toggle();
        }

        public void Toggle()
        {
            if (!Toggled)
            {
                DotBackGround.Fill = On;
                Toggled = true;

                //Dot.Margin = RightSide;
            }
            else
            {
                DotBackGround.Fill = Off;
                Toggled = false;
                //Dot.Margin = LeftSide;
            }
        }

        public bool isToggled()
        {
            if (Toggled)
                return true;
            else return false;
        }

        public void checkToggled()
        {
            //if (Toggled && Dot.Margin == LeftSide)
            //    Dot.Margin = RightSide;
            //else if (!Toggled && Dot.Margin == RightSide)
            //    Dot.Margin = LeftSide;
        }
    }
}
