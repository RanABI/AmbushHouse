using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Components
{
    class Target : Component
    {
        public Target(int id,int CPXId) : base(id,CPXId)
        {
            this.source = "TR";
        }
            
    }
}
