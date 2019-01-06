using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Components
{
    public class CPX
    {
        public int ID;
        public string IP;
        public int port;
        public List<Component> components;
        public const int doorNum = 5;

        public CPX(int id,string ip,int idRangeStart,int idRangeEnd)
        {
            this.port = 8085;
            this.ID = id;
            this.IP = ip;
            components = new List<Component>();
            int j = 0;
            for(int i = idRangeStart; i<idRangeEnd+1;i++)
            {
                if(j < doorNum)
                {
                    components.Add(new Door(i,id));
                    
                }
                else
                {
                    components.Add(new Target(i,id));
                }
                j++;
            }
        }

        public int getCPXbyDoorID(int doorID)
        {
            if (doorID < 5)
                return 0;
            else if (doorID < 13)
                return 1;
            else if (doorID < 21)
                return 2;
            else if (doorID < 29)
                return 3;
            else if (doorID < 38)
                return 4;
            return 0;
        }

    }
}
