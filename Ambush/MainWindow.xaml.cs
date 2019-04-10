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
using MySql.Data.MySqlClient;
using static Ambush.Enums;
using System.Data;
using Ambush.UserControls;
using Ambush.Timers;
using System.Net;
using System.Net.Sockets;
using Ambush.Client;
using System.Diagnostics;
using System.Collections;
using Proxymity.QuickDmx.FtdiUsb;
using System.Threading;
using Proxymity.QuickDmx;
using System.Windows.Threading;
using System.IO.Ports;
using Proxymity.QuickDmx.VellemanP8062;
using System.Windows.Forms;

namespace Ambush
{
    /// <summary>
    /// Version 1.0 Designed for 3 DOOR STATES
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly string backgroundImagePath = "file://C://Users//User//Downloads//LOGO-AMBUSH.jpg";
        public readonly string splashImage = "/Images/LOGO-AMBUSH.jpg";
        public DateTime date { get; set; }
        public static DispatcherTimer t;
        public static bool isStop;
        SplashScreen splashScreen;
        private double eventTimerInterval = 300 * 60.0; // every 60 sec


        public MainWindow()
        {

            InitializeComponent();
            Play game = new Play();
            List<CPX> cPXes = Db_Utils.InitComponents();
            Play.setCPXes(cPXes);

            

            /*<<---------------DMX CONTROL--------------->>
            using (uDMX dmx = new uDMX())
            {
                if (dmx.IsOpen)
                {
                    while (true)
                    {
                        // Set channel 0 to value 127
                        Console.WriteLine(dmx.SetSingleChannel(1, 127).ToString());
                        Console.WriteLine(dmx.SetSingleChannel(2, 127).ToString());
                        Console.WriteLine(dmx.SetSingleChannel(3, 127).ToString()); 
                        dmx.SetSingleChannel(4, 50);
                        dmx.SetSingleChannel(4, 80);
                        dmx.SetSingleChannel(4, 110);
                        dmx.SetSingleChannel(4, 140);
                        dmx.SetSingleChannel(4, 180);
                        dmx.SetSingleChannel(4, 255);
                        dmx.SetSingleChannel(4, 0);
                        dmx.SetSingleChannel(5, 50);
                        dmx.SetSingleChannel(5, 80);
                        dmx.SetSingleChannel(5, 110);
                        dmx.SetSingleChannel(5, 140);
                        dmx.SetSingleChannel(5, 180);
                        dmx.SetSingleChannel(5, 255);
                        dmx.SetSingleChannel(5, 0);


                    }
                    // Set three channels, starting with channel 0
                    //byte[] values = new byte[] { 0xFF, 0xAA, 0x05 };

                    //dmx.SetChannelRange(0, values);
                }
            }
            ---------------DMX CONTROL---------------*/

            //using (TCPClient TcpClient = new TCPClient("ACTIVATE","192.168.0.56", 8080)) { }


            /* MainWindow background */
            setBackgroundImage();
            //bg();
            /* Initialize and run server listener */
            TCPServer server = new TCPServer();
            //EventTimer.Timer =  new System.Timers.Timer(1000 * 5.0); // 60 sec interval
            //ComponentStateCheckAndUpdate timer = new ComponentStateCheckAndUpdate(eventTimerInterval);

            // BroadcastMessageTimer timer2 = new BroadcastMessageTimer(5000.0);

            // using (TCPClient client = new TCPClient(Constants.Broadcast_Message, cpx)) { }

            ////TODO : Broadcast message at startup
            ///every X minutes send broadcast message  - ask who is online (RPI & ben's stuff)
            ///each device will return unique id and get IP from message
            ///
        }


        public void setBackgroundImage()
        {
            ImageBrush myBrush = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(@backgroundImagePath));
            myBrush.ImageSource = image.Source;

            this.Background = myBrush;
            this.Background.Opacity = 0.5;
        }
        private void t_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            string time = Convert.ToString((DateTime.Now - date));
            TimerDisplay.Text = time.Substring(0, 8);


            if (TimerDisplay.Text != "")
            {
                string[] times = TimerDisplay.Text.Split(':');
                if (Int32.Parse(times[1]) == Constants.RoundMinutes)
                {
                    //TODO -- > Activate ALARM --> Turn on lights --> open door path to exit 

                    timer.Stop();
                    TimerDisplay.Text = "";
                    //Play.SetComponentsToDefault();
                    GeneralUtils.InfoMessageBox("Timer stopped");
                }
            }
            if (isStop)
            {
                timer.Stop();
                TimerDisplay.Text = "";
            }

        }

        public void bg()
        {
            var bc = new BrushConverter();

            this.Background = (Brush)bc.ConvertFrom("#FFB3B3B3");
        }

        private void SetTriggers_Click(object sender, RoutedEventArgs e)
        {
            Window win2 = new UserControls.Trigger();
            win2.Show();
        }

        private void RPISettings_Click(object sender, RoutedEventArgs e)
        {

            Window win2 = new UserControls.RPISettingsWindowxaml();
            win2.Show();

        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            isStop = false;
            t = new DispatcherTimer(new TimeSpan(0, 0, 0, 1, 0), DispatcherPriority.Background, t_Tick, Dispatcher.CurrentDispatcher);
            t.IsEnabled = true;
            this.date = DateTime.Now;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Components.Door door = new Components.Door(2, 2, Direction.Middle);
            door.setDoorState(Direction.Down);
        }

        private void StopGame_Click(object sender, RoutedEventArgs e)
        {
            isStop = true;
        }
    }



}
