using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.CustomEventArgs
{
    public class InvokeTriggerEventArgs : EventArgs
    {
        public string physicalID;
        public string Direction;
    }
}
