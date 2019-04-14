using Ambush.CustomEventArgs;
using Ambush.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Components
{
    public class CPX
    {
        public int id;
        public string ip;
        public static int port = 8085;
        public List<Component> components;
        public MiniController miniController;
        public bool isOn = false;
        public CPX(int id, string ip, List<Component> components)
        {
            this.id = id;
            this.ip = ip;
            this.components = components;
            this.isOn = true;
            Utils.GlobalEvents.OnIncomingStatsMessage += updateCpxComponentsStatus;

        }
        override
        public string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("CPX id: ");
            builder.Append(this.id.ToString());
            builder.Append(", CPX ip : ");
            builder.Append(this.ip.ToString());
            builder.Append("\n");
            foreach (Component cmp in this.components)
                builder.Append(cmp.ToString());

            return builder.ToString();

        }
        public string constructGetStatusMessage()
        {
            ////TODO SET RPI SCRIPT ACCORDINGLY
            StringBuilder builder = new StringBuilder();
            builder.Append("AR:GET:CPX:");
            builder.Append(this.id);
            return builder.ToString();
        }

        public void updateCpxComponentsStatus(object sender, IncomingStatsMessageArgs e)
        {
            string[] states = e.values;
            string id = e.cpxID;
            List<CPX> cPXes = Play.cPXes;
            CPX cpx = Play.getCpxByPhysicalId(Int32.Parse(id));
            int i = 0;
            foreach (Component cmp in cpx.components)
            {
                cmp.state = Utils.Conversions.stringToDirection(states[i++]);
            }
              
        }


    }
}
