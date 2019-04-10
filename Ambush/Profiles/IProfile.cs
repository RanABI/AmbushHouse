using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Profiles
{
    interface IProfile
    {
        void Invoke(string nextState);
    }
}
