using Ambush.CustomEventArgs;
using Ambush.Server;
using Ambush.Timers;
using Ambush.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static Ambush.Server.ServerRequestHandler;

namespace Ambush.UserControls
{
    /// <summary>
    /// Interaction logic for ComponentStatus.xaml
    /// </summary>
    public partial class RPIStatus : UserControl
    {

        //BackgroundWorker backgroundWorker = new BackgroundWorker();

        public RPIStatus()
        {
            InitializeComponent();
            ServerRequestHandler.BroadcastRply += updateStatusArray;
        }



        public static void changeButtonBackgroundColor()
        {

        }

        public Button[] GetButtons()
        {
            return new Button[] { rpi1, rpi2, rpi3, rpi4, rpi5 };
        }


        public void updateStatusArray(object sender, BroadcastRplyEventArgs e)
        {
            //bool[] cpxes = Play.cpxStates;
            //cpxes[Int32.Parse(e.Id) - 1] = true;
            //-----------------Move to new timer that loops over bool[] states and colors accordinly
            this.Dispatcher.Invoke(() =>
            {
                foreach (Button button in GetButtons())
                {
                    if (button.Name == e.Id.ToString())
                        button.Background = (Brush)Brushes.Green;
                }
            });

        }

    }
}
