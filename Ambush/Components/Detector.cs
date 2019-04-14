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
    public class Detector : Component, IProfile
    {
        public Detector(int physicalId,int virtualId) : base(virtualId,physicalId,Constants.DT)
        {
            ServerRequestHandler.InvokerTrigger += Play.invokeTrigger;
        }

        public void Invoke(string nextState)
        {
            throw new NotImplementedException();
        }
    }
}
