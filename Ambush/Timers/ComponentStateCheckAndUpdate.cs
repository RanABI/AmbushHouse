using Ambush.Client;
using Ambush.Components;
using Ambush.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Timers
{
    class ComponentStateCheckAndUpdateTimer
    {
        public static System.Timers.Timer Timer = null;
        public ComponentStateCheckAndUpdateTimer(double eventTimerInterval)
        {
            try
            {
                Timer = new System.Timers.Timer(eventTimerInterval);
                Timer.Enabled = true;
                Timer.Elapsed += new System.Timers.ElapsedEventHandler(checkComponentState);
                Timer.AutoReset = true;
                ComponentStateCheckAndUpdateTimer.Timer.Start();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        }

        static void checkComponentState(object Sender, System.Timers.ElapsedEventArgs e)
        {
            Play game = Play.Instance;
            ///CHANGE TO 1 MESSAGE PER CPX
            List<CPX> cPXes = game.cPXes;
            foreach (CPX cpx in cPXes)
            {
                using (TCPClient TcpClient = new TCPClient(cpx.constructGetStatusMessage(), cpx)) { }
            }
            return;
        }
    }
}
