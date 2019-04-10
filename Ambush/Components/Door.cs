using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ambush;
using Ambush.Client;
using Ambush.UserControls;
using static Ambush.Enums;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Ambush.Server;
using static Ambush.Server.ServerRequestHandler;
using Ambush.CustomEventArgs;
using Ambush.Profiles;
using Ambush.Utils;

namespace Ambush.Components
{
    /* Class representation of a door . */
    [Table(Name = "Door")]
    class Door : Component,IProfile
    {
        public Direction direction;
        public delegate void DoorStateChanged(object sender, CustomEventArgs.DoorStateChangeArgs e);
        public event DoorStateChanged OnDoorStateChange;
        //-------------------------------------------------------------------------

        DoorClientHandler handler;




        public Door(int virtualId, int physicalID) : base(virtualId, physicalID)
        {
            this.source = Constants.DR;
            direction = Direction.Middle;
            this.OnDoorStateChange += HandleDoorStateChange;
        }
        public Door(int virtualId, int physicalID, Direction dir) : base(virtualId, physicalID)
        {
            this.source = Constants.DR;
            this.direction = dir;
            this.OnDoorStateChange += HandleDoorStateChange;

        }


        public void HandleDoorStateChange(Object sender, CustomEventArgs.DoorStateChangeArgs e)
        {
            handler = new DoorClientHandler();
            Direction state = e.state;
            int id = e.doorId;
            int physicalId = e.physicalId;
            //TODO tell client api to open door
            handler.ChangeDoorState(id, state, physicalId);
        }

        public void setDoorState(Direction direction)
        {
            CustomEventArgs.DoorStateChangeArgs args = new CustomEventArgs.DoorStateChangeArgs();
            args.doorId = this.virtualID;
            args.state = direction;
            args.physicalId = this.physicalID;
            OnDoorStateChange(this, args);
        }

        public void Invoke(string nextState)
        {
            Direction dir = Conversions.stringToDirection(nextState);
            setDoorState(dir);
        }
    }
}
