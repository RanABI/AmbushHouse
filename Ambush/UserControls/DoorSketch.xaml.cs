using Ambush.Client;
using Ambush.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using static Ambush.Enums;

namespace Ambush.UserControls
{
    /// <summary>
    /// Interaction logic for DoorSketch.xaml
    /// </summary>
    public partial class DoorSketch : UserControl
    {
        //Constants
        private const String CRLF = "\r\n";
        private const string LOCALHOST = "10.0.0.2";
        private const int DEFAULT_PORT = 8080;


        //Fields
        private IPAddress _serverIpAddress;
        private int _port;
        private TCPClient _tcpClient;
        List<CPX> CPXes;

        public Brush On { get; private set; }
        public bool Toggled { get; private set; }
        public object Dot { get; private set; }
        public object RightSide { get; private set; }

        public DoorSketch()
        {
            InitializeComponent();

          
        }
        

        private void Door1Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            checkToggle(1, "Down");
        }

        private void Door1UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(1, "Up");

        }

        private void Door1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(1, "Middle");
        }


        private void Door2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(2, "Middle");
        }

        private void Door2UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(2, "Up");

        }

        private void Door2Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(2, "Down");

        }

        private void Door3UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(3, "Up");

        }

        private void Door3Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(3, "Down");

        }

        private void Door3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(3, "Middle");
        }

        private void Door4UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(4, "Up");

        }

        private void Door4Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(4, "Down");

        }

        private void Door4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(4, "Middle");
        }

        private void Door5UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(5, "Up");

        }

        private void Door5Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(5, "Down");

        }

        private void Door5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(5, "Middle");
        }

        private void Door6UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(6, "Up");

        }

        private void Door6Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(6, "Down");

        }

        private void Door6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(6, "Middle");
        }

        private void Door7UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(7, "Up");

        }

        private void Door7Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(7, "Down");

        }

        private void Door7_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(7, "Middle");
        }

        private void Door8UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(8, "Up");

        }

        private void Door8Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(8, "Down");

        }

        private void Door8_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(8, "Middle");
        }

        private void Door9UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(9, "Up");

        }

        private void Door9Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(9, "Down");

        }

        private void Door9_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(9, "Middle");
        }

        private void Door10UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(10, "Up");

        }

        private void Door10Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(10, "Down");

        }

        private void Door10_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(10, "Middle");
        }

        private void Door11UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(11, "Up");

        }

        private void Door11Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(11, "Down");

        }

        private void Door11_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(11, "Middle");
        }

        private void Door12UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(12, "Up");

        }

        private void Door12Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(12, "Down");

        }

        private void Door12_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(12, "Middle");
        }

        private void Door13UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(13, "Up");

        }

        private void Door13Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(13, "Down");

        }

        private void Door13_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(13, "Middle");
        }

        private void Door14UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(14, "Up");

        }

        private void Door14Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(14, "Down");

        }

        private void Door14_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(14, "Middle");
        }

        private void Door15UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(15, "Up");

        }

        private void Door15Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(15, "Down");

        }

        private void Door15_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(15, "Middle");
        }

        private void Door16UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(16, "Up");

        }

        private void Door16Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(16, "Down");

        }

        private void Door16_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(16, "Middle");
        }

        private void Door17UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(17, "Up");

        }

        private void Door17Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(17, "Down");

        }

        private void Door17_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(17, "Middle");
        }

        private void Door18UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(18, "Up");

        }

        private void Door18Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(18, "Down");

        }


        private void Door18_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(18, "Middle");
        }

        private void Door19UP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(19, "Up");

        }

        private void Door19Down_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(19, "Down");

        }

        private void Door19_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkToggle(19, "Middle");
        }


       

        public void checkToggle(int doorId, string direction)
        {
            switch (doorId)
            {
                case 1:
                    switch (direction)
                    {
                        case "Up":
                            {
                                //if (Door1Down.isToggled())
                              //      Door1Down.Toggle();
                              //  if (Door1.isToggled())
                              //      Door1.Toggle();
                              //  Door1.checkToggled();
                             //   sendRequest(doorId, direction,Door1UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                             //   if (Door1UP.isToggled())
                             //       Door1UP.Toggle();
                             //   if (Door1.isToggled())
                             //       Door1.Toggle();
                             //   Door1.checkToggled();
                             //   sendRequest(doorId, direction, Door1Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                            //    if (Door1Down.isToggled())
                             //       Door1Down.Toggle();
                             //   if (Door1UP.isToggled())
                             //       Door1UP.Toggle();
                             //   Door1.checkToggled();
                             //   sendRequest(doorId, direction, Door1.isToggled());
                                break;
                            }
                    }
                    break;
                case 2:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door2Down.isToggled())
                                    Door2Down.Toggle();
                                if (Door2.isToggled())
                                    Door2.Toggle();
                                Door2.checkToggled();
                                sendRequest(doorId, direction, Door2UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door2UP.isToggled())
                                    Door2UP.Toggle();
                                if (Door2.isToggled())
                                    Door2.Toggle();
                                Door2.checkToggled();
                                sendRequest(doorId, direction, Door2Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door2Down.isToggled())
                                    Door2Down.Toggle();
                                if (Door2UP.isToggled())
                                    Door2UP.Toggle();
                                Door2.checkToggled();
                                sendRequest(doorId, direction, Door2.isToggled());
                                break;
                            }
                    }
                    break;
                case 3:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door3Down.isToggled())
                                    Door3Down.Toggle();
                                if (Door3.isToggled())
                                    Door3.Toggle();
                                Door3.checkToggled();
                                sendRequest(doorId, direction, Door3UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door3UP.isToggled())
                                    Door3UP.Toggle();
                                if (Door3.isToggled())
                                    Door3.Toggle();
                                Door3.checkToggled();
                                sendRequest(doorId, direction, Door3Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door3Down.isToggled())
                                    Door3Down.Toggle();
                                if (Door3UP.isToggled())
                                    Door3UP.Toggle();
                                Door3.checkToggled();
                                sendRequest(doorId, direction, Door3.isToggled());
                                break;
                            }
                    }
                    break;
                case 4:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door4Down.isToggled())
                                    Door4Down.Toggle();
                                if (Door4.isToggled())
                                    Door4.Toggle();
                                Door4.checkToggled();
                                sendRequest(doorId, direction, Door4UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door4UP.isToggled())
                                    Door4UP.Toggle();
                                if (Door4.isToggled())
                                    Door4.Toggle();
                                Door4.checkToggled();
                                sendRequest(doorId, direction, Door4Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door4Down.isToggled())
                                    Door4Down.Toggle();
                                if (Door4UP.isToggled())
                                    Door4UP.Toggle();
                                Door4.checkToggled();
                                sendRequest(doorId, direction, Door4.isToggled());
                                break;
                            }
                    }
                    break;
                case 5:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door5Down.isToggled())
                                    Door5Down.Toggle();
                                if (Door5.isToggled())
                                    Door5.Toggle();
                                Door5.checkToggled();
                                sendRequest(doorId, direction, Door5UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door5UP.isToggled())
                                    Door5UP.Toggle();
                                if (Door5.isToggled())
                                    Door5.Toggle();
                                Door5.checkToggled();
                                sendRequest(doorId, direction, Door5Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door5Down.isToggled())
                                    Door5Down.Toggle();
                                if (Door5UP.isToggled())
                                    Door5UP.Toggle();
                                Door5.checkToggled();
                                sendRequest(doorId, direction, Door5.isToggled());
                                break;
                            }
                    }
                    break;
                case 6:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door6Down.isToggled())
                                    Door6Down.Toggle();
                                if (Door6.isToggled())
                                    Door6.Toggle();
                                Door6.checkToggled();
                                sendRequest(doorId, direction, Door6UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door6UP.isToggled())
                                    Door6UP.Toggle();
                                if (Door6.isToggled())
                                    Door6.Toggle();
                                Door6.checkToggled();
                                sendRequest(doorId, direction, Door6Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door6Down.isToggled())
                                    Door6Down.Toggle();
                                if (Door6UP.isToggled())
                                    Door6UP.Toggle();
                                Door6.checkToggled();
                                sendRequest(doorId, direction, Door6.isToggled());
                                break;
                            }
                    }
                    break;
                case 7:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door7Down.isToggled())
                                    Door7Down.Toggle();
                                if (Door7.isToggled())
                                    Door7.Toggle();
                                Door7.checkToggled();
                                sendRequest(doorId, direction, Door7UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door7UP.isToggled())
                                    Door7UP.Toggle();
                                if (Door7.isToggled())
                                    Door7.Toggle();
                                Door7.checkToggled();
                                sendRequest(doorId, direction, Door7Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door7Down.isToggled())
                                    Door7Down.Toggle();
                                if (Door7UP.isToggled())
                                    Door7UP.Toggle();
                                Door7.checkToggled();
                                sendRequest(doorId, direction, Door7.isToggled());
                                break;
                            }
                    }
                    break;
                case 8:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door8Down.isToggled())
                                    Door8Down.Toggle();
                                if (Door8.isToggled())
                                    Door8.Toggle();
                                Door8.checkToggled();
                                sendRequest(doorId, direction, Door8UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door8UP.isToggled())
                                    Door8UP.Toggle();
                                if (Door8.isToggled())
                                    Door8.Toggle();
                                Door8.checkToggled();
                                sendRequest(doorId, direction, Door8Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door8Down.isToggled())
                                    Door8Down.Toggle();
                                if (Door8UP.isToggled())
                                    Door8UP.Toggle();
                                Door8.checkToggled();
                                sendRequest(doorId, direction, Door8.isToggled());
                                break;
                            }
                    }
                    break;
                case 9:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door9Down.isToggled())
                                    Door9Down.Toggle();
                                if (Door9.isToggled())
                                    Door9.Toggle();
                                Door9.checkToggled();
                                sendRequest(doorId, direction, Door9UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door9UP.isToggled())
                                    Door9UP.Toggle();
                                if (Door9.isToggled())
                                    Door9.Toggle();
                                Door9.checkToggled();
                                sendRequest(doorId, direction, Door9Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door9Down.isToggled())
                                    Door9Down.Toggle();
                                if (Door9UP.isToggled())
                                    Door9UP.Toggle();
                                Door9.checkToggled();
                                sendRequest(doorId, direction, Door9.isToggled());
                                break;
                            }
                    }
                    break;
                case 10:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door10Down.isToggled())
                                    Door10Down.Toggle();
                                if (Door10.isToggled())
                                    Door10.Toggle();
                                Door10.checkToggled();
                                sendRequest(doorId, direction, Door10UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door10UP.isToggled())
                                    Door10UP.Toggle();
                                if (Door10.isToggled())
                                    Door10.Toggle();
                                Door10.checkToggled();
                                sendRequest(doorId, direction, Door10Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door10Down.isToggled())
                                    Door10Down.Toggle();
                                if (Door10UP.isToggled())
                                    Door10UP.Toggle();
                                Door10.checkToggled();
                                sendRequest(doorId, direction, Door10.isToggled());
                                break;
                            }
                    }
                    break;
                case 11:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door11Down.isToggled())
                                    Door11Down.Toggle();
                                if (Door11.isToggled())
                                    Door11.Toggle();
                                Door11.checkToggled();
                                sendRequest(doorId, direction, Door11UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door11UP.isToggled())
                                    Door11UP.Toggle();
                                if (Door11.isToggled())
                                    Door11.Toggle();
                                Door11.checkToggled();
                                sendRequest(doorId, direction, Door11Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door11Down.isToggled())
                                    Door11Down.Toggle();
                                if (Door11UP.isToggled())
                                    Door11UP.Toggle();
                                Door11.checkToggled();
                                sendRequest(doorId, direction, Door11.isToggled());
                                break;
                            }
                    }
                    break;
                case 12:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door12Down.isToggled())
                                    Door12Down.Toggle();
                                if (Door12.isToggled())
                                    Door12.Toggle();
                                Door12.checkToggled();
                                sendRequest(doorId, direction, Door12UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door12UP.isToggled())
                                    Door12UP.Toggle();
                                if (Door12.isToggled())
                                    Door12.Toggle();
                                Door12.checkToggled();
                                sendRequest(doorId, direction, Door12Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door12Down.isToggled())
                                    Door12Down.Toggle();
                                if (Door12UP.isToggled())
                                    Door12UP.Toggle();
                                Door12.checkToggled();
                                sendRequest(doorId, direction, Door12.isToggled());
                                break;
                            }
                    }
                    break;
                case 13:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door13Down.isToggled())
                                    Door13Down.Toggle();
                                if (Door13.isToggled())
                                    Door13.Toggle();
                                Door13.checkToggled();
                                sendRequest(doorId, direction, Door13UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door13UP.isToggled())
                                    Door13UP.Toggle();
                                if (Door13.isToggled())
                                    Door13.Toggle();
                                Door13.checkToggled();
                                sendRequest(doorId, direction, Door13Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door13Down.isToggled())
                                    Door13Down.Toggle();
                                if (Door13UP.isToggled())
                                    Door13UP.Toggle();
                                Door13.checkToggled();
                                sendRequest(doorId, direction, Door13.isToggled());
                                break;
                            }
                    }
                    break;
                case 14:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door14Down.isToggled())
                                    Door14Down.Toggle();
                                if (Door14.isToggled())
                                    Door14.Toggle();
                                Door14.checkToggled();
                                sendRequest(doorId, direction, Door14UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door14UP.isToggled())
                                    Door14UP.Toggle();
                                if (Door14.isToggled())
                                    Door14.Toggle();
                                Door14.checkToggled();
                                sendRequest(doorId, direction, Door14Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door14Down.isToggled())
                                    Door14Down.Toggle();
                                if (Door14UP.isToggled())
                                    Door14UP.Toggle();
                                Door14.checkToggled();
                                sendRequest(doorId, direction, Door14.isToggled());
                                break;
                            }
                    }
                    break;
                case 15:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door15Down.isToggled())
                                    Door15Down.Toggle();
                                if (Door15.isToggled())
                                    Door15.Toggle();
                                Door15.checkToggled();
                                sendRequest(doorId, direction, Door15UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door15UP.isToggled())
                                    Door15UP.Toggle();
                                if (Door15.isToggled())
                                    Door15.Toggle();
                                Door15.checkToggled();
                                sendRequest(doorId, direction, Door15Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door15Down.isToggled())
                                    Door15Down.Toggle();
                                if (Door15UP.isToggled())
                                    Door15UP.Toggle();
                                Door15.checkToggled();
                                sendRequest(doorId, direction, Door15.isToggled());
                                break;
                            }
                    }
                    break;
                case 16:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door16Down.isToggled())
                                    Door16Down.Toggle();
                                if (Door16.isToggled())
                                    Door16.Toggle();
                                Door16.checkToggled();
                                sendRequest(doorId, direction, Door16UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door16UP.isToggled())
                                    Door16UP.Toggle();
                                if (Door16.isToggled())
                                    Door16.Toggle();
                                Door16.checkToggled();
                                sendRequest(doorId, direction, Door16Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door16Down.isToggled())
                                    Door16Down.Toggle();
                                if (Door16UP.isToggled())
                                    Door16UP.Toggle();
                                Door16.checkToggled();
                                sendRequest(doorId, direction, Door16.isToggled());
                                break;
                            }
                    }
                    break;
                case 17:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door17Down.isToggled())
                                    Door17Down.Toggle();
                                if (Door17.isToggled())
                                    Door17.Toggle();
                                Door17.checkToggled();
                                sendRequest(doorId, direction, Door17UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door17UP.isToggled())
                                    Door17UP.Toggle();
                                if (Door17.isToggled())
                                    Door17.Toggle();
                                Door17.checkToggled();
                                sendRequest(doorId, direction, Door17Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door17Down.isToggled())
                                    Door17Down.Toggle();
                                if (Door17UP.isToggled())
                                    Door17UP.Toggle();
                                Door17.checkToggled();
                                sendRequest(doorId, direction, Door17.isToggled());
                                break;
                            }
                    }
                    break;
                case 18:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door18Down.isToggled())
                                    Door18Down.Toggle();
                                if (Door18.isToggled())
                                    Door18.Toggle();
                                Door18.checkToggled();
                                sendRequest(doorId, direction, Door18UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door18UP.isToggled())
                                    Door18UP.Toggle();
                                if (Door18.isToggled())
                                    Door18.Toggle();
                                Door18.checkToggled();
                                sendRequest(doorId, direction, Door18Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door18Down.isToggled())
                                    Door18Down.Toggle();
                                if (Door18UP.isToggled())
                                    Door18UP.Toggle();
                                Door18.checkToggled();
                                sendRequest(doorId, direction, Door18.isToggled());
                                break;
                            }
                    }
                    break;
                case 19:
                    switch (direction)
                    {
                        case "Up":
                            {
                                if (Door19Down.isToggled())
                                    Door19Down.Toggle();
                                if (Door19.isToggled())
                                    Door19.Toggle();
                                Door19.checkToggled();
                                sendRequest(doorId, direction, Door19UP.isToggled());
                                break;

                            }
                        case "Down":
                            {
                                if (Door19UP.isToggled())
                                    Door19UP.Toggle();
                                if (Door19.isToggled())
                                    Door19.Toggle();
                                Door19.checkToggled();
                                sendRequest(doorId, direction, Door19Down.isToggled());
                                break;
                            }
                        case "Middle":
                            {
                                if (Door19Down.isToggled())
                                    Door19Down.Toggle();
                                if (Door19UP.isToggled())
                                    Door19UP.Toggle();
                                Door19.checkToggled();
                                sendRequest(doorId, direction, Door19.isToggled());
                                break;
                            }
                    }
                    break;
            }
        }
        
        public void sendRequest(int doorId,string direction,bool isToggled)
        {
            StringBuilder builder = new StringBuilder();
            CPXes = MainWindow.CPXes;
            CPX cpx = CPXes.ElementAt(0);
            cpx = CPXes.ElementAt(cpx.getCPXbyDoorID(doorId-1));
            State state;
            if (isToggled)
                state = State.ON;
            else state = State.OFF;

            cpx.components[(doorId - 1) % 8].state = state; 

            if(direction == "Middle")
            {//AR:GET/SET:DR/SN:ID:DIRECTION:ON/OFF
                builder.Append("AR:SET:DR:");
                builder.Append(doorId.ToString());
                builder.Append(":");
                builder.Append("MIDDLE:");
                builder.Append("1");
            }
            else
            {
                builder.Append("AR:SET:DR:");
                builder.Append(doorId.ToString());
                builder.Append(":");
                builder.Append(direction.ToUpper());
                builder.Append(":");
                builder.Append(isToggled.ToString());
            }
            using (TCPClient client = new TCPClient(builder.ToString(), cpx.ID));

        }

        private void Door_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            door1.doorId = 1;
        }
    }
}
