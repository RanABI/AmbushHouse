using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ambush.Enums;

namespace Ambush.Components
{
    class TargetHandler
    {
        private int id;
        public TargetHandler(int id)
        {
            this.id = id;
        }
        //Turn on led lights Source:Id:True/False (send to MicroController)
        //Invoke sound
        //Add points to player (according to strength of hit)
        //Wait few seconds and turn off led lights

        public void TargetLedControl(State state)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("TR:");
            builder.Append(this.id.ToString());
            builder.Append(":");
            builder.Append(state.ToString());
            //SEND TO MICROCONTROLLER TARGET STRING TO TURN ON LEDS

        }

        public void InvokeSound()
        {
            //Invoke sound in case of HIT
        }


    }
}
