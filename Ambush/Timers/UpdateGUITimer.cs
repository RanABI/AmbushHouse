using Ambush.UserControls;
using Ambush.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Ambush.Timers
{
    public class UpdateGUITimer
    {

        public static System.Timers.Timer Timer = null;
        public UpdateGUITimer(double eventTimerInterval)

        {
            try
            {
                Timer = new System.Timers.Timer(eventTimerInterval);
                Timer.Enabled = true;
                Timer.Elapsed += new System.Timers.ElapsedEventHandler(updateGuiByValue);
                Timer.AutoReset = true;
                UpdateGUITimer.Timer.Start();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        }

        static void updateGuiByValue(object sender, ElapsedEventArgs e)
        {
           // bool[] cpxStates = Play.cpxStates;
        }


    }
}
