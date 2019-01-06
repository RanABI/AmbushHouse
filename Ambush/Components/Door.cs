using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambush;
using Ambush.Client;
using static Ambush.Enums;

namespace Ambush.Components
{
    
    class Door : Component
    {
        //Class representation of a door
        public DoorDirection direction;

        public Door(int doorID,int CPXId) : base(doorID,CPXId)
        {
            this.source = "DR"; // dr for Door
            direction = DoorDirection.Middle;
        }
    
    }
}
