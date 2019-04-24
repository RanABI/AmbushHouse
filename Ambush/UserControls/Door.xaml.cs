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

        public void Toggle(Direction dir)
        {
            this.ToggleAllOff();
            this.toggels[dir].Toggle();
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
            this.Dispatcher.Invoke(() =>
            {
                this.toggels[dir].Wait();
                
            });
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
            Play game = Play.Instance;
            CPX currentCPX = game.getCpxByPhysicalId(physicalID);
            foreach (Component cmp in currentCPX.components)
            { 
                    if(cmp.physicalID == physicalID)
                    {
                        return cmp.virtualID;
                    }
            }
            return -1;
            
        }
      


    }
    




}
