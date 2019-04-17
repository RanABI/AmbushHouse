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
    public class Laser : Component, IProfile
    {
        public Laser(int physicalId, int virtualId) : base(virtualId, physicalId,Constants.LS)
        {
            ServerRequestHandler.InvokerTrigger += Play.invokeTrigger;
        }
        public Laser(int physicalId, int virtualId,State state) : base(virtualId, physicalId, Constants.LS)
        {
            this.isOn = state;
            ServerRequestHandler.InvokerTrigger += Play.invokeTrigger;
        }

        public void Invoke(string nextState)
        {
            throw new NotImplementedException();
        }
        
    }
}
