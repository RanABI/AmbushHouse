using Ambush.UserControls;
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
        public List<Laser> lasers;
        public List<Detector> detectors;
        public string type;
        public readonly int port = 8080;
        //Table microcontroller
        public MiniController(string ip,string id)
        {
            this.id = id;
            this.ip = ip;
        }
        public MiniController(string ip, List<Laser> lasers, string id,string type)
        {
            this.type = type;
            this.id = id;
            this.ip = ip;
            this.lasers = lasers;
        }
        public MiniController(string ip,  List<Detector> detectors, string id, string type)
        {
            this.type = type;
            this.id = id;
            this.ip = ip;
            this.detectors = detectors;
        }

        public void setLaserList(List<Laser> LList)
        {
            this.lasers = LList;
        }

        public void setDetectorList(List<Detector> DList)
        {
            this.detectors = DList;
        }

        public Detector getDetectorById(int id)
        {
            foreach(Detector det in detectors)
            {
                if (det.physicalID == id)
                    return det;
            }
            return null;
        }

        public Laser getLaserById(int id)
        {
            foreach (Laser laser in lasers)
            {
                if (laser.physicalID == id)
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
            builder.Append("11111111");

            
        }

        override
        public string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Controller Id : ");
            builder.Append(this.id.ToString());
            builder.Append(Environment.NewLine);
            builder.Append("Controller type : ");
            builder.Append(this.type.ToString());
            builder.Append(Environment.NewLine);
            if(this.type == Constants.LS)
            {
                foreach (Laser laser in this.lasers)
                {
                    builder.Append(laser.ToString());
                    builder.Append(Environment.NewLine);
                }
            }
            else if (this.type == Constants.DT)
                foreach(Detector det in this.detectors)
                {
                    builder.Append(det.ToString());
                    builder.Append(Environment.NewLine);
                }
            return builder.ToString();
        }




    }
}
