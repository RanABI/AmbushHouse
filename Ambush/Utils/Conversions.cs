using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ambush.Enums;

namespace Ambush.Utils
{
    public static class Conversions
    {

        public static Direction stringToDirection(string dir)
        {
            int direction = Int32.Parse(dir);
            Direction dire = Direction.None;
            switch(direction)
            {
                case 1:
                    {
                        dire = Direction.Up;
                        return dire;
                    }
                case 2:
                    {
                        dire = Direction.Middle;
                        return dire;
                    }
                case 3:
                    {
                        dire = Direction.Down;
                        return dire;
                    }
                default:
                    return dire;
            }

        }
    }

}
