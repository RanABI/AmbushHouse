using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.CustomEventArgs
{
    public class IncomingStatsMessageArgs : EventArgs
    {
        public string cpxID;
        public string[] values;

        public IncomingStatsMessageArgs(string id, string[] values)
        {
            this.cpxID = id;
            this.values = values;
        }

    }
}
