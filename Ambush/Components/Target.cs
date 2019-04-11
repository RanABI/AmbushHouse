using Ambush.CustomEventArgs;
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
    class Target : Component,IProfile
    {
        public Sensor sensor;
        public delegate void TargetStateChanged(object sender, TargetStateChangeArgs e);
        public event TargetStateChanged OnTargetStateChange;
        TargetClientHandler handler;

        public Target(int virtualID,Sensor sensor,int physicalID) : base(virtualID, physicalID)
        {
            this.sensor = sensor;
            this.source = Constants.TR;
            this.OnTargetStateChange += HandleTargetStateChange;
            ServerRequestHandler.InvokerTrigger += Play.invokeTrigger;
        }

        public void HandleTargetStateChange(Object sender, TargetStateChangeArgs e)
        {
            handler = new TargetClientHandler();
            Direction state = e.state;
            int id = e.targetId;
            int physicalID = e.physicalID;
            handler.ChangeTargetState(id, state, physicalID);
        }
        public void setTargetState(Direction newState)
        {
            TargetStateChangeArgs args = new TargetStateChangeArgs();
            args.targetId = this.virtualID;
            args.state = newState;
            args.physicalID = this.physicalID;
            OnTargetStateChange(this, args);
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
            builder.Append("    Assosiated Sensor : ");
            builder.Append("\n");
            builder.Append("        PhysicalID :" + this.sensor.physicalID.ToString());
            builder.Append("\n");
            builder.Append("        VirtualID :" + this.sensor.virtualID.ToString());
            builder.Append("\n");
            builder.Append("        State :" + this.sensor.isOn.ToString());
            builder.Append("\n");
            return builder.ToString();

        }

        public void Invoke(string nextState)
        {
            Direction dir = Conversions.stringToDirection(nextState);
            setTargetState(dir);
        }
    }
 
}
