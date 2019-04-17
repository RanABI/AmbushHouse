using Ambush.Client;
using Ambush.Components;
using Ambush.CustomEventArgs;
using Ambush.Server;
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
        DoorClientHandler handler;

        public DoorSketch()
        {
            InitializeComponent();
            AssignDoorIds();
            ServerRequestHandler.ChangeDoorUI += ToggleAfterReceivedMessage;
        }
        


        private void AssignDoorIds()
        {
            for(int i = 1; FindName("door"+i.ToString()) != null; i++)
            {
                Door door = FindName("door" + i.ToString()) as Door;
                door.physicalID = i;
                door.OnDoorStateChange += HandleDoorStateChange;
                
            }
        }

        public void HandleDoorStateChange(Object sender, DoorStateChangeArgs e)
        {
            handler = new DoorClientHandler();
            Direction state = e.state;
            int id = e.doorId;
            int physicalID = e.physicalId;
            handler.ChangeDoorState(id, state, physicalID);
        }

        public void ToggleAfterReceivedMessage(object sender, DoorStateChangeArgs e)
        {
            Direction dir = e.state;
            if (e.physicalId == 4 || e.physicalId == 8 || e.physicalId == 13 || e.physicalId == 18)
            {
                if (dir == Direction.Down)
                    dir = Direction.Up;
                else if (dir == Direction.Up)
                    dir = Direction.Down;
            }

            this.Dispatcher.Invoke(() =>
            {
                Door door = FindName("door" + e.physicalId.ToString()) as Door;
                door?.Toggle(dir);

            });
        }

    }
}
