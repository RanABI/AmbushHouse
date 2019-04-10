using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ambush.Enums;

namespace Ambush.CustomEventArgs
{
    public class TargetStateChangeArgs : EventArgs
    {
        public Direction state;
        public int targetId;
        public int physicalID;
    }
}
