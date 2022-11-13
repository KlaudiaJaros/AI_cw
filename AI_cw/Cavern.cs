namespace AI_cw
{
    class Cavern
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double DistanceFromStart { get; set; }  
        public double GoalDistance { get; set; } 
        public double TotalCost { get; set; }
        public int CavernNo { get; set; }
        public Cavern CameFrom { get; set; }
        public bool Visited { get; set; }

        public Cavern()
        {
            Visited = false;
        }
        public Cavern(int x, int y, int num, double goal)
        {
            X = x;
            Y = y;
            CavernNo = num;
            GoalDistance = goal;
            Visited = false;
        }
    }
}