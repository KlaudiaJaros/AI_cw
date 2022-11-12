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
        public bool Visited { get; set; }
        public Cavern(int x, int y, int num, double goal)
        {
            X = x;
            Y = y;
            CavernNo = num;
            GoalDistance = goal;
            Visited = false;
        }
        public Cavern(int x, int y, int num)
        {
            X = x;
            Y = y;
            CavernNo = num;
            Visited = false;
        }
        public Cavern()
        {
            Visited = false;
        }
    }
}