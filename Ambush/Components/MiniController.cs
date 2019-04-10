using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Components
{
    public class MiniController
    {
        public string ip;
        public string id;
        public string port;
        public List<Laser> lasers;
        public string type;
        //Table microcontroller
        public MiniController(string ip, string port, List<Laser> lasers, string id,string type)
        {
            this.type = type;
            this.id = id;
            this.ip = ip;
            this.port = port;
            this.lasers = lasers;
        }

        public void setLaserList(List<Laser> LList)
        {
            this.lasers = LList;
        }

        public Laser getLaserById(int id)
        {
            foreach (Laser laser in lasers)
            {
                if (laser.id == id)
                    return laser;
            }
            return null;
        }

        public void turnOnAllLasers()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("AR:SRE:D");
            builder.Append(this.id.ToString());
            builder.Append(":");
            
        }

    }
}
