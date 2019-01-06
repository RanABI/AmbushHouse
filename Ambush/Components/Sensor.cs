using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Components
{
    class Sensor : Component
    {
        //Class representation of a sensor
        public Sensor(int id,int CPXId) : base(id,CPXId)
        {
            this.source = "SN"; //sn for Sensor
        }

         
    }
}
