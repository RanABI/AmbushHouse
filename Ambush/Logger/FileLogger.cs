using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Logger
{
    class FileLogger : LogBase
    {
        public string filePath = "C:\\Users\\User\\source\\repos\\Ambush\\Ambush\\Logger\\log.txt";
        public override void Log(string message)
        {
            lock (_lock)
            {

                using (StreamWriter streamWriter = new StreamWriter(filePath,append: true))
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    streamWriter.WriteLine(addTimeStamp(message));
                    streamWriter.Close();
                }

            }
        }
        public string addTimeStamp(string message)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(DateTime.Today.ToString("dd-MM-yyyy"));
            builder.Append(DateTime.Now.ToString("HH:mm:ss"));
            builder.Append(" -> ");
            builder.Append(message);

            return builder.ToString();
        }
    }
}
