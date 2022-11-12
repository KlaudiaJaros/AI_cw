using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_cw
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Concat(args[0], ".cav"));
            string outputfilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Concat(args[0], ".csn"));
            string[] input = File.ReadAllLines(filePath);
            string[] split = input[0].Split(',');
            int[] inputNo = Array.ConvertAll(split, int.Parse);
            int numOfVacs = inputNo[0];
            double goal;
            int lastX = inputNo[numOfVacs * 2 - 1];
            int lastY = inputNo[numOfVacs * 2];

            List<Cavern> cavernsGlobal = new List<Cavern>();

            // create a list with all the nodes and coordinates:
            int count = 1;
            for (int i = 1; i < numOfVacs * 2; i += 2)
            {
                goal = CalculateCost(inputNo[i], inputNo[i + 1], lastX, lastY); // ? maybe do that someplace else?
                Cavern c1 = new Cavern(inputNo[i], inputNo[i + 1], count, goal);
                cavernsGlobal.Add(c1);
                count++;
            }


            List<int> bestOutput = new List<int>();
            List<Cavern> cavernOptions = new List<Cavern>();
            List<Cavern> visitedOptions = new List<Cavern>();
            Cavern currentCav = new Cavern();
            currentCav = cavernsGlobal.ElementAt(0);


            cavernOptions.Add(currentCav);
            while(cavernOptions.Any())
            {
                cavernOptions = cavernOptions.OrderBy(x => x.TotalCost).ToList();
                currentCav = cavernOptions.First();
                cavernOptions.Remove(currentCav);

                var walkableCaverns= WhereToGo(currentCav.CavernNo, inputNo, cavernsGlobal).OrderBy(x => x.TotalCost);
                foreach (Cavern c in walkableCaverns)
                {

                    if (c.State == 2)
                        continue;

                    double costToTravelTo = CalculateCost(currentCav.X, currentCav.Y, c.X, c.Y);
                    if (c.DistanceFromStart == 0 || currentCav.DistanceFromStart + costToTravelTo < c.DistanceFromStart )
                    {
                        c.DistanceFromStart = currentCav.DistanceFromStart + costToTravelTo;
                        c.CameFrom = currentCav;
                        cavernsGlobal.ElementAt(c.CavernNo - 1).DistanceFromStart = c.DistanceFromStart;
                        cavernsGlobal.ElementAt(c.CavernNo - 1).CameFrom = currentCav;
                        c.TotalCost = c.DistanceFromStart + c.GoalDistance;
                        cavernsGlobal.ElementAt(c.CavernNo - 1).TotalCost = c.TotalCost;
                        if (!cavernOptions.Contains(c))
                        {
                            cavernOptions.Add(c);
                        }
                    }
                }
                currentCav.State = 2;
                if (currentCav.CavernNo==numOfVacs)
                {
                    break;
                }
            }



            Cavern traverse = cavernsGlobal.ElementAt(numOfVacs - 1);
            Console.WriteLine(traverse.DistanceFromStart);
            bestOutput.Add(traverse.CavernNo);
            while (traverse.CameFrom != null)
            {
                traverse = traverse.CameFrom;
                bestOutput.Add(traverse.CavernNo);
            }
            bestOutput.Reverse();
            if (bestOutput.Count == 1)
            {
                Console.WriteLine("No Path");
                using (StreamWriter file = new StreamWriter(outputfilePath, true))
                {
                    file.WriteLine("No Path");
                }
            }
            else
            {
                Console.WriteLine(string.Join(" ", bestOutput));

                using (StreamWriter file = new StreamWriter(outputfilePath, true))
                {
                    file.WriteLine(string.Join(" ", bestOutput));
                }
            }

        }
        /// <summary>
        /// Loops through the input list to find out which cavern can be visited from the provided cavern
        /// </summary>
        /// <param name="cavernNo">current cavern</param>
        /// <param name="input">input list</param>
        /// <returns>list of options of where to go</returns>
        public static List<Cavern> WhereToGo(int cavernNo, int[] input, List<Cavern> caverns)
        {
            List<Cavern> order = new List<Cavern>();
            int numOfVacs = input[0];
            int offset = numOfVacs * 2;
            int cavernIndex = offset + cavernNo;
            double totalCost = 0;
            for (int i = 1; i < numOfVacs + 1; i++)
            {
                if (input[cavernIndex] == 1 && i != cavernNo)
                {
                    totalCost = CalculateCost(caverns.ElementAt(cavernNo - 1).X, caverns.ElementAt(cavernNo - 1).Y, caverns.ElementAt(i - 1).X, caverns.ElementAt(i - 1).Y);
                    caverns.ElementAt(i - 1).TotalCost = caverns.ElementAt(i - 1).GoalDistance + totalCost;
                    order.Add(caverns.ElementAt(i - 1));
                }
                cavernIndex += numOfVacs;
            }

            return order;
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