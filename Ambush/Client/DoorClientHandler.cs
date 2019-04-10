using Ambush.Components;
using Ambush.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Ambush.Enums;

namespace Ambush.Client
{
    class DoorClientHandler
    {
        

        public void ChangeDoorState(int doorId, Direction dir,int physicalID)
        {//AR:SET:DR:VIRTUALID:DIR:PHYSID

            CPX cpx = Play.getCpxByPhysicalId(physicalID);

            if (doorId == 4 || doorId == 8 || doorId == 13 || doorId == 18)
            {
                if (dir == Direction.Down)
                    dir = Direction.Up;
                else if (dir == Direction.Up)
                    dir = Direction.Down;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("AR:SET:DR:");
            builder.Append(doorId.ToString());
            builder.Append(":");
            builder.Append((int)dir);
            builder.Append(":");
            builder.Append(physicalID);
            MessageBox.Show(builder.ToString());
            using (TCPClient client = new TCPClient(builder.ToString(), cpx)) { }

        }


    }
}
