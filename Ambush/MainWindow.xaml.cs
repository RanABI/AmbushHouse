using Ambush.Components;
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
using Ambush.Utils;
namespace Ambush
{
    /// <summary>
    /// Version 1.0 Designed for 3 DOOR STATES
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int CPX_Quantity = 5;
        public string[] ipAddresses = { "10.0.0.6", "10.0.0.6", "10.0.0.6", "10.0.0.6", "10.0.0.6" };
        public static int DoorAmount = 19;

        public readonly string backgroundImagePath = "file://C://Users//User//Downloads//LOGO-AMBUSH.jpg";
        public static List<CPX> CPXes { get; set; }
        DateTime date { get; }
        public MainWindow()
        {

            InitializeComponent();
            //MainWindow background 
            setBackgroundImage();

            //Initialize CPXes
            InitializeCPX();

            //Initialize server
            TCPServer server = new TCPServer();

            ////TODO : Broadcast message at startup
            ///every X minutes send broadcast message  - ask who is online (RPI & ben's stuff)
            ///each device will return unique id and get IP from message
        }

        public static List<CPX> getCPXList()
        {
            return CPXes;
        }

        public void setBackgroundImage()
        {
            ImageBrush myBrush = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(@backgroundImagePath));
            myBrush.ImageSource = image.Source;
            this.Background = myBrush;
        }

        public void InitializeCPX()
        {
            CPXes = new List<CPX>();
            int start = 0;
            int end = 7;
            for (int i = 0; i < 5; i++)
            {
                CPXes.Add(new CPX(i, ipAddresses[i], start, end));
                start += 8;
                end += 8;
            }
        }

        private void DoorSketch_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
