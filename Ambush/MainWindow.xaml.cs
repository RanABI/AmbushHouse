﻿using Ambush.Components;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ambush.Utils;
using Ambush.UserControls;
using Ambush.Timers;
using Ambush.Client;
using System.Windows.Threading;
using static Ambush.Enums;

namespace Ambush
{
    /// <summary>
    /// Version 1.0 Designed for 3 DOOR STATES
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly string backgroundImagePath = "file://C://Users//User//Pictures//LOGO-AMBUSH.jpg";
        public readonly string splashImage = "/Images/LOGO-AMBUSH.jpg";
        public DateTime date { get; set; }
        public static DispatcherTimer t;
        public static bool isStop;
        private double BroadcasteventTimerInterval = 300 * 60.0; // every 60 sec
        private double ComponentseventTimerInterval = 300 * 60.0; // every 60 sec
        private double GUIeventTimerInterval = 300 * 60.0; // every 60 sec
        public MainWindow()
        {

            InitializeComponent();
   

            /* Load data from database */
            init();

            /* Start Timers */
            //startTimers();

            /* MainWindow background */
            setBackgroundImage();

            /* Initialize and run server listener */
            TCPServer server = new TCPServer();





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
            //TODO --> Change first door's state to start game
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Components.Door door = new Components.Door(2, 2, Direction.Middle);
            door.setDoorState(Direction.Down);
            //string l = "AR:SRE:D01:10:11111100";
            //string d = "AR:SRE:L01:10:11111100";
            //using (TCPClient client = new TCPClient(l, "192.168.0.28", 8080)) { }
            //using (TCPClient client = new TCPClient(d, "192.168.0.27", 8080)) { }
        }

        private void StopGame_Click(object sender, RoutedEventArgs e)
        {
            isStop = true;
        }

        public static void dmxControl()
        {
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
        }

        private void Alert_Click(object sender, RoutedEventArgs e)
        {
            //TODO --> Get buzzer IP
            using (TCPClient TcpClient = new TCPClient("ACTIVATE","192.168.0.56", 8080)) { }
        }

        private void startTimers()
        {
            BroadcastMessageTimer timer = new BroadcastMessageTimer(BroadcasteventTimerInterval);
            ComponentStateCheckAndUpdateTimer timer2 = new ComponentStateCheckAndUpdateTimer(ComponentseventTimerInterval);
            UpdateGUITimer timer3 = new UpdateGUITimer(GUIeventTimerInterval);
        }

        private void init()
        {
            Play game = Play.Instance;
            List<CPX> cPXes = Db_Utils.InitComponents();
            game.setCPXes(cPXes);
            List<MiniController> cons = Db_Utils.InitControllers();
            game.setControllers(cons);
            game.score = 0;
        }

        private void DefaultValues_Click(object sender, RoutedEventArgs e)
        {
            Play game = Play.Instance;
            if (!GeneralUtils.yesNoMessageBox("Are you sure you want to reset default component values?"))
            {
                return;
            }
            else
            {
                List<CPX> cpxes =   game.cPXes;
                foreach (CPX cpx in cpxes)
                {
                    foreach (Component cmp in cpx.components)
                    {
                        switch (cmp.source)
                        {
                            case Constants.DR:
                                {
                                    string var = new InputBox("Door #" + cmp.physicalID.ToString(), "Set default", "").ShowDialog();
                                    cmp.defaultState = var;
                                    break;
                                }
                            case Constants.TR:
                                {
                                    string var = new InputBox("Target #" + cmp.physicalID.ToString(), "Set default", "").ShowDialog();
                                    cmp.defaultState = var;
                                    Target target = (Target)cmp;
                                    var = new InputBox("Target #" + cmp.physicalID.ToString() + " sensor #"+ target.sensor.physicalID.ToString() , "Set default", "").ShowDialog();
                                    target.sensor.defaultState = var;

                                    break;
                                }
                        }
                    }
                }

                List<MiniController> nods = game.nods;
                foreach(MiniController con in nods)
                {
                    foreach (Laser laser in con.lasers)
                    {
                        string var = new InputBox("Controller #" + con.id.ToString() + " Laser #" + laser.physicalID.ToString(), "Set default", "").ShowDialog();
                        laser.defaultState = var;
                        //TODO --> ADD DETECTORS
                    }


                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window win2 = new UserControls.MicroControllerSettings();
            win2.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Play game = Play.Instance;
            List<MiniController> nods = game.nods;
            foreach (MiniController con in nods)
                Console.WriteLine(con.ToString()); 
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (GeneralUtils.yesNoMessageBox("Are you sure you want to exit?"))
            {
                Environment.Exit(0);

            }
                
            else return;
        }
    }



}
