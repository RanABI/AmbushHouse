using Ambush.Client;
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
using static Ambush.Enums;

namespace Ambush.UserControls
{
    /// <summary>
    /// Interaction logic for Door.xaml
    /// </summary>
    public partial class Door : UserControl
    {

        public int doorId;
        List<CPX> CPXes;
        public DoorDirection direction;
        public delegate void DoorStateChanged(object sender, DoorStateChangeArgs e);
        public event DoorStateChanged OnDoorStateChange;
        private Dictionary<DoorDirection, ToggleSwitch> toggels;


        public Door()
        {
            InitializeComponent();
            mapToggels();
        }

        private void mapToggels()
        {
            toggels = new Dictionary<DoorDirection, ToggleSwitch>();
            toggels.Add(DoorDirection.Down, DOWN);
            toggels.Add(DoorDirection.Up, UP);
            toggels.Add(DoorDirection.Middle, MIDDLE);
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
            HandleToggleStateChange(DoorDirection.Down);


        }

        private void MIDDLE_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            HandleToggleStateChange(DoorDirection.Middle);


        }

        private void UP_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            HandleToggleStateChange(DoorDirection.Up);
        }


        private void HandleToggleStateChange(DoorDirection dir)
        {
            ToggleAllOff();
            toggels[dir].Toggle();
            direction = dir;
            DoorStateChangeArgs args = new DoorStateChangeArgs();
            args.doorId = doorId;
            args.state = direction;
            OnDoorStateChange(this, args);
            // TODO replace following line with call to Client.Instance
            //sendRequest(MIDDLE.isToggled());
        }

      

    }

    

    public class DoorStateChangeArgs: EventArgs
    {
        public  DoorDirection state;
        public  int doorId;
    }

}
