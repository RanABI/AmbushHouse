using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Logger
{
    namespace Logger { public enum LogTarget { File, EventLog } }

    public abstract class LogBase
    {
        public object _lock = new object();
        
        public abstract void Log(string message);
    }
}
