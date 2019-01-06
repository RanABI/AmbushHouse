using Ambush.Components;
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
        List<CPX> CPXes;
        public void ChangeDoorState(int doorId, DoorDirection dir)
        {
            if(doorId == 4 || doorId == 8 || doorId == 13 || doorId == 18)
            {
                if (dir == DoorDirection.Down)
                    dir = DoorDirection.Up;
                else if (dir == DoorDirection.Up)
                    dir = DoorDirection.Down;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("AR:SET:DR:");
            builder.Append(doorId.ToString());
            builder.Append(":");
            builder.Append(dir.ToString());
            MessageBox.Show(builder.ToString());
            //using (TCPClient client = new TCPClient(builder.ToString(), cpx.ID)) ;

        }


    }
}
