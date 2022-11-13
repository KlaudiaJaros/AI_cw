using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AI_cw
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // get the input file and turn the input into int array:
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Concat(args[0], ".cav"));
            string outputfilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Concat(args[0], ".csn"));
            string[] input = File.ReadAllLines(filePath);
            string[] split = input[0].Split(',');
            int[] inputNo = Array.ConvertAll(split, int.Parse);

            // get the total number of caverns and the last cavern (*2 because they're pairs):
            int numberOfCaverns = inputNo[0];
            int lastX = inputNo[numberOfCaverns * 2 - 1];
            int lastY = inputNo[numberOfCaverns * 2];

            // create a list of type Cavern and populate it with all the locations and target costs:
            List<Cavern> cavernsGlobal = new List<Cavern>();
            int count = 1;
            for (int i = 1; i < numberOfCaverns * 2; i += 2)
            {
                double goal = CalculateCost(inputNo[i], inputNo[i + 1], lastX, lastY);
                Cavern c1 = new Cavern(inputNo[i], inputNo[i + 1], count, goal);
                cavernsGlobal.Add(c1);
                count++;
            }

            List<Cavern> cavernQueue = new List<Cavern>();

            // get the first cavern and add it to the queue:
            Cavern currentCav = new Cavern();
            currentCav = cavernsGlobal.ElementAt(0);
            cavernQueue.Add(currentCav);

            // while there are options:
            while (cavernQueue.Any())
            {
                // reorder by total cost and pop the lowest cost cavern:
                cavernQueue = cavernQueue.OrderBy(x => x.TotalCost).ToList();
                currentCav = cavernQueue.First();
                cavernQueue.Remove(currentCav);

                // find cavern connections and explore:
                var connectedCaverns = WhereToGo(currentCav.CavernNo, inputNo, cavernsGlobal).OrderBy(x => x.TotalCost);
                foreach (Cavern c in connectedCaverns)
                {
                    if (c.Visited == true)
                        continue;

                    // distance to this connection:
                    double costToTravelTo = CalculateCost(currentCav.X, currentCav.Y, c.X, c.Y);
                    // if the connection had its distance from start calculated or if the current cavern distance and the cost to travel is lower to get there:
                    if (c.DistanceFromStart == 0 || currentCav.DistanceFromStart + costToTravelTo < c.DistanceFromStart )
                    {
                        // save the new distance and total cost or update it and save the new parent:
                        // saving in the all caverns list:
                        c.DistanceFromStart = currentCav.DistanceFromStart + costToTravelTo;
                        c.CameFrom = currentCav;
                        c.TotalCost = c.DistanceFromStart + c.GoalDistance;
                        cavernsGlobal.ElementAt(c.CavernNo - 1).DistanceFromStart = c.DistanceFromStart;
                        cavernsGlobal.ElementAt(c.CavernNo - 1).CameFrom = currentCav;
                        cavernsGlobal.ElementAt(c.CavernNo - 1).TotalCost = c.TotalCost;

                        // if the connection is not in the queue already, save it:
                        if (!cavernQueue.Contains(c))
                        {
                            cavernQueue.Add(c);
                        }
                    }
                }
                currentCav.Visited = true;
                // check if at destination:
                if (currentCav.CavernNo==numberOfCaverns)
                {
                    break;
                }
            }

            // get the path by traversing back from the last cavern:
            List<int> bestPath = new List<int>();
            Cavern traverse = cavernsGlobal.ElementAt(numberOfCaverns - 1);
            Console.WriteLine(traverse.DistanceFromStart);
            bestPath.Add(traverse.CavernNo);
            while (traverse.CameFrom != null)
            {
                traverse = traverse.CameFrom;
                bestPath.Add(traverse.CavernNo);
            }
            bestPath.Reverse();

            // count = 1 only if there is no path and the only item is the last cavern:
            if (bestPath.Count == 1)
            {
                Console.WriteLine("0");
                using (StreamWriter file = new StreamWriter(outputfilePath, true))
                {
                    file.WriteLine("0");
                }
            }
            else
            {
                Console.WriteLine(string.Join(" ", bestPath));
                using (StreamWriter file = new StreamWriter(outputfilePath, true))
                {
                    file.WriteLine(string.Join(" ", bestPath));
                }
            }
        }
        /// <summary>
        /// Loops through the input list grid to find out which cavern can be visited from the provided cavern
        /// </summary>
        /// <param name="cavernNo">current cavern</param>
        /// <param name="input">input grid list</param>
        /// <param name="caverns">the list of all caverns</param>
        /// <returns>list of options of where to go</returns>
        public static List<Cavern> WhereToGo(int cavernNo, int[] input, List<Cavern> caverns)
        {
            List<Cavern> options = new List<Cavern>();
            int numberOfCavs = input[0];
            // offset to skip the cavern locations:
            int offset = numberOfCavs * 2; 
            // grid column: matrix start -1 + cavern number:
            int cavernIndex = offset + cavernNo; 

            // loop through each row of the grid: 
            for (int i = 1; i < numberOfCavs + 1; i++)
            {
                // if connection found, caculate costs and add it to the list:
                if (input[cavernIndex] == 1)
                {
                    options.Add(caverns.ElementAt(i - 1));
                }
                cavernIndex += numberOfCavs; // go to the next row
            }
            return options;
        }
        /// <summary>
        /// Calculates the distance between two caverns using Euclidean distance
        /// </summary>
        /// <param name="x">cavern1 X</param>
        /// <param name="y">cavern1 Y</param>
        /// <param name="a">cavern2 X</param>
        /// <param name="b">cavern2 Y</param>
        /// <returns>distance</returns>
        public static double CalculateCost(int x, int y, int a, int b)
        {
            double cost = Math.Sqrt((a - x) * (a - x) + (b - y) * (b - y));
            return cost;
        }
    }
}