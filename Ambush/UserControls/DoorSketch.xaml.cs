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
        DoorClientHandler handler;

        public Brush On { get; private set; }
        public bool Toggled { get; private set; }
        public object Dot { get; private set; }
        public object RightSide { get; private set; }

        public DoorSketch()
        {

            InitializeComponent();
            AssignDoorIds();


        }
        
        private void AssignDoorIds()
        {
            for(int i = 1; FindName("door"+i.ToString()) != null; i++)
            {
                Door door = FindName("door" + i.ToString()) as Door;
                door.doorId = i ;
                door.OnDoorStateChange += HandleDoorStateChange;
            }
        }

        public void HandleDoorStateChange(Object sender, DoorStateChangeArgs e)
        {
            handler = new DoorClientHandler();
            DoorDirection state = e.state;
            int id = e.doorId;
            //TODO tell client api to open door
            handler.ChangeDoorState(id, state);
        }
       
    }
}
