using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.CustomEventArgs
{
    public class BroadcastRplyEventArgs : EventArgs
    {
        public BroadcastRplyEventArgs(string s) { Id = s; }
        public String Id { get; } // readonly
    }
}
