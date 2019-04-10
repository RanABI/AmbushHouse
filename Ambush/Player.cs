using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush
{
    public class Player
    {
        public int score;
        public string name;

        public Player(string name)
        {
            this.name = name;
            this.score = 0;
        }
    }
}
