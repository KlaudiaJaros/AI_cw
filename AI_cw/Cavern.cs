using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_cw
{
    class Cavern
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double DistanceFromStart { get; set; }  // cost to get to this node from start on the current path 
        public double GoalDistance { get; set; } // cost to get from this cavern to the goal
        public double TotalCost { get; set; }
        public int CavernNo { get; set; }
        public Cavern CameFrom { get; set; }
        public int State { get; set; } // 0- unknown, 1 - open, 2- closed 
        public bool IsWalkable { get; set; }
        public Cavern(int x, int y, int num, double goal)
        {
            State = 0;
            X = x;
            Y = y;
            CavernNo = num;
            GoalDistance = goal;
            IsWalkable = true;
        }
        public Cavern(int x, int y, int num)
        {
            X = x;
            Y = y;
            CavernNo = num;
            State = 0;
            IsWalkable = true;
        }
        public Cavern()
        {
            State = 0;
            IsWalkable = true;
        }
    }
}