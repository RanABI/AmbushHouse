using Ambush.Profiles;
using Ambush.Server;
using Ambush.UserControls;
using Ambush.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ambush.Enums;

namespace Ambush.Components
{
    public class Laser : Component
    {
        public int id;
        public State state;
        public string source;
        public Laser(int id,State state) : base(id)
        {
            this.id = id;
            this.state = state;
            this.source = Constants.LS;
            ServerRequestHandler.InvokerTrigger += Play.invokeTrigger;
        }


    }
}
