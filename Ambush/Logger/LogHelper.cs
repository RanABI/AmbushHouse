using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Logger
{
    public static class LogHelper
    {
        private static LogBase logger = null;


        public static void Log(Logger.LogTarget target, string message)
        {
            switch (target)
            {
                case Logger.LogTarget.File:
                    logger = new FileLogger();
                    logger.Log(message);
                    break;
                default: 
                    return;
            }
        }
    }
}
