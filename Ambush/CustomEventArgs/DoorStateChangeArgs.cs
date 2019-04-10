using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ambush.Enums;

namespace Ambush.CustomEventArgs
{
    public class DoorStateChangeArgs : EventArgs
    {
        public Direction state;
        public int doorId;
        public int physicalId;
    }
}
