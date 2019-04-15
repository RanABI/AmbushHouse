using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush
{
    public class Enums
    {
        public enum State { OFF, ON }
        public enum MsgVars:int {  Action=1 , Component , VirtualId , Answer ,State,PhysicalId}
        public enum Direction { None = 0,Up,Middle,Down}

        public static string StateToString(State st)
        {
            switch(st)
            {
                case State.ON:
                    return "1";
                case State.OFF:
                    return "0";
            }
            return null;
        }
    }
}
