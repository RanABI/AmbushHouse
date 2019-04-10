using Ambush.Client;
using Ambush.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ambush.Enums;

namespace Ambush.Components
{
    public class Component
    {
        public int virtualID { get; set; }

        public int physicalID { get; set; }
        public Direction state { get; set; }

        public State isOn { get; set; }
        public int failCount;
        public string source { get; set; }

        public string defaultState { get; set;  }
        public Component(int id, int physicalID)
        {
            this.virtualID = id;
            this.state = Direction.None;
            this.physicalID = physicalID;
        }
        public Component(int id)
        {
            this.physicalID = id;
            this.failCount = 0;
        }

        public void setRequest(Direction newState)
        {
            //Build a string in the correct form of a SET request
            CPX cpx = Play.getCpxByPhysicalId(this.physicalID);

            StringBuilder builder = new StringBuilder();
            builder.Append("AR:");
            builder.Append("SET:");
            builder.Append(source);
            builder.Append(":");
            builder.Append(virtualID.ToString());
            builder.Append(":");
            builder.Append(newState.ToString());
            builder.Append(":");
            builder.Append(this.physicalID.ToString());
            using (TCPClient client = new TCPClient(builder.ToString(), cpx)) { }
        }
        public void getRequest()
        {
            //Build a string in the correct form of a GET request
            CPX cpx = Play.getCpxByPhysicalId(this.physicalID);

            StringBuilder builder = new StringBuilder();
            builder.Append("AR:");
            builder.Append("GET:");
            builder.Append(source);
            builder.Append(":");
            builder.Append(virtualID.ToString());
            builder.Append(":");
            builder.Append("null:");
            builder.Append(this.physicalID.ToString());

            using (TCPClient client = new TCPClient(builder.ToString(), cpx)) { }

        }



        override
        public string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Component type: ");
            builder.Append(this.source);
            builder.Append("\n");
            builder.Append("    PhysicalID : ");
            builder.Append(this.physicalID.ToString());
            builder.Append("\n");
            builder.Append("    VirtualID : ");
            builder.Append(this.virtualID.ToString());
            builder.Append("\n");

            return builder.ToString();
        }




    }
}
