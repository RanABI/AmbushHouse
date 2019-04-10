using Ambush.Client;
using Ambush.Components;
using Ambush.CustomEventArgs;
using Ambush.Server;
using Ambush.Utils;
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
using static Ambush.Enums;

namespace Ambush.UserControls
{
    /// <summary>
    /// Interaction logic for Door.xaml
    /// </summary>
    public partial class Door : UserControl
    {

        public int physicalID;
        public int doorId;
        public Direction direction;
        public delegate void DoorStateChanged(object sender, DoorStateChangeArgs e);
        public event DoorStateChanged OnDoorStateChange;
        private Dictionary<Direction, ToggleSwitch> toggels;


        public Door()
        {
            InitializeComponent();
            mapToggels();
            ServerRequestHandler.ChangeDoorUI += ToggleAfterReceivedMessage;
        }

        private void mapToggels()
        {
            toggels = new Dictionary<Direction, ToggleSwitch>();
            toggels.Add(Direction.Down, DOWN);
            toggels.Add(Direction.Up, UP);
            toggels.Add(Direction.Middle, MIDDLE);
        }

        public void ToggleAllOff()
        {
            if (UP.isToggled())
                UP.Toggle();
            if (DOWN.isToggled())
                DOWN.Toggle();
            if (MIDDLE.isToggled())
                MIDDLE.Toggle();
        }

        private void DOWN_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            HandleToggleStateChange(Direction.Down);


        }

        private void MIDDLE_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            HandleToggleStateChange(Direction.Middle);


        }

        private void UP_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            HandleToggleStateChange(Direction.Up);
        }


        private void HandleToggleStateChange(Direction dir)
        {
            //ToggleAllOff();
            //toggels[dir].Toggle();
            direction = dir;
            DoorStateChangeArgs args = new DoorStateChangeArgs();
            this.doorId = getDoorVirtualId(physicalID);
            args.doorId = this.doorId;
            args.physicalId = physicalID;
            args.state = direction;
            OnDoorStateChange(this, args);
        }

      public int getDoorVirtualId(int physicalID)
        {
            CPX currentCPX = Play.getCpxByPhysicalId(physicalID);
            foreach (Component cmp in currentCPX.components)
            { 
                    if(cmp.physicalID == physicalID)
                    {
                        return cmp.virtualID;
                    }
            }
            return -1;
            
        }
      
       public void ToggleAfterReceivedMessage(object sender, DoorStateChangeArgs e)
        {
            UserControls.Door door = null;
            Application.Current.Dispatcher.Invoke((Action)delegate {
                DoorSketch d = new DoorSketch();
                door = d.FindName("door" + e.physicalId.ToString()) as UserControls.Door;  // This is the canvas in your UserControl
       
            });
            
  
            this.Dispatcher.Invoke(() =>
            {
                
                ToggleAllOff();
                //this.toggels[e.state].Toggle();
                if (e.state == Direction.Down)
                    door.DOWN.Toggle();
                else if (e.state == Direction.Up)
                    door.UP.Toggle();
                else if (e.state == Direction.Middle)
                    door.MIDDLE.Toggle();
                
            });
        }

    }
    




}
