using Ambush.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Utils
{
    public static class GlobalEvents
    {
        /* Event occurs when RPI returns an array of values of corrent CPX components */
        public delegate void UpdateCPXComponentsStats(object sender, IncomingStatsMessageArgs e);
        public static event UpdateCPXComponentsStats OnIncomingStatsMessage;
        public static void InvokeOnIncomingStatsMessage(object sender, IncomingStatsMessageArgs e)
        {
            OnIncomingStatsMessage(sender, e);
        }

    }
}
