using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_cw
{
    class Cavern
    {
        public int X {get; set;}
        public int Y { get; set; }
        public double TotalCost { get; set; }
        public double GoalCost { get; set; }
        public int CavernNo { get; set; }
        public Cavern CameFrom { get; set; }
        public Cavern(int x, int y, int num, double goal)
        {
            X = x;
            Y = y;
            CavernNo = num;
            GoalCost = goal;
        }
        public Cavern(int x, int y, int num)
        {
            X = x;
            Y = y;
            CavernNo = num;
        }
        public Cavern()
        {

        }
    }
}
