using Ambush.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Components
{
    public class Sensor : Component
    {
        public int targetID;
        //Class representation of a sensor
        public Sensor(int virtualID,int physicalID,int targetID) : base(virtualID, physicalID)
        {
            this.source = Constants.SN; //sn for Sensor
            this.targetID = targetID;
            this.isOn = Enums.State.ON;
        }

         
    }
}
