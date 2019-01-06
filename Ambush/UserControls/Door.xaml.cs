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
        public Door()
        {
            InitializeComponent();
        }


        public void ToggleOff()
        {
            if (UP.isToggled())
                UP.Toggle();
            if (DOWN.isToggled())
                DOWN.Toggle();
            if (MIDDLE.isToggled())
                DOWN.Toggle();
        }

        private void DOWN_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (DOWN.isToggled())
            {
                ToggleOff();
                DOWN.checkToggled();
                direction = DoorDirection.Middle;
                sendRequest(DOWN.isToggled());
            }

            else
            {
                ToggleOff();
                DOWN.Toggle();
                DOWN.checkToggled();
                direction = DoorDirection.Down;
                sendRequest(DOWN.isToggled());
            }

            }

            private void MIDDLE_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
            {
            if (MIDDLE.isToggled())
            {
                ToggleOff();
                MIDDLE.checkToggled();
                direction = DoorDirection.Middle;
                sendRequest(MIDDLE.isToggled());
            }
            else
            {
                ToggleOff();
                MIDDLE.Toggle();
                MIDDLE.checkToggled();
                direction = DoorDirection.Middle;
                sendRequest(MIDDLE.isToggled());
            }
            }

            private void UP_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
            {
            if (UP.isToggled())
            {
                ToggleOff();
                UP.checkToggled();
                direction = DoorDirection.Middle;
                sendRequest(UP.isToggled());

            }
            else
            {
                ToggleOff();
                UP.Toggle();
                UP.checkToggled();
                direction = DoorDirection.Up;
                sendRequest(UP.isToggled());
            }
            }
        public void sendRequest(bool isToggled)
        {
            StringBuilder builder = new StringBuilder();
            CPXes = MainWindow.CPXes;
            CPX cpx = CPXes.ElementAt(0);
            cpx = CPXes.ElementAt(cpx.getCPXbyDoorID(doorId - 1));
            State state;
            if (isToggled)
                state = State.ON;
            else state = State.OFF;

            cpx.components[(doorId - 1) % 8].state = state;

            builder.Append("AR:SET:DR:");
            builder.Append(this.doorId.ToString());
            builder.Append(":");
            builder.Append(direction.ToString());
            builder.Append(":");
            builder.Append(isToggled.ToString());
            using (TCPClient client = new TCPClient(builder.ToString(), cpx.ID)) ;
        }

        }
    }
}
