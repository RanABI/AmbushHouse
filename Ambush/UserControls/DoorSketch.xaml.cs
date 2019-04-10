using Ambush.Client;
using Ambush.Components;
using Ambush.CustomEventArgs;
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
            //TODO tell client api to open door
            handler.ChangeDoorState(id, state, physicalID);
        }
       
    }
}
