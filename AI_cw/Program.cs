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

            List<Cavern> caverns = new List<Cavern>();
            int count = 1;
            for (int i = 1; i < numOfVacs * 2; i += 2)
            {
                goal = CalculateCost(inputNo[i], inputNo[i + 1], lastX, lastY);
                Cavern c1 = new Cavern(inputNo[i], inputNo[i + 1], count, goal);
                caverns.Add(c1);
                count++;
            }

            string output = string.Empty;
            List<int> bestOutput = new List<int>();
            List<int> pathfinding = new List<int>();
            List<int> visitedCavs = new List<int>();
            List<int> options = new List<int>();
            bool cont = true;
            bool goBack = false;
            int currentCavern = 1;
            int removeOption = 0;
            double totalCost = 0;
            double distance = 0;
            double currentCost = double.MaxValue;
            double currentDistance = 0;
            double distanceCost = 0;
            Cavern currentCav = new Cavern();
            currentCav = caverns.ElementAt(0);
            output = string.Concat(output, currentCavern, " ");
            bestOutput.Add(currentCavern);
            visitedCavs.Add(currentCavern);
            while (cont)
            {
                options = WhereToGo(currentCavern, inputNo);
                if (goBack)
                {
                    options.Remove(removeOption);
                    if (options.Count==0)
                    {
                        output = "No path";
                        break;
                    }
                }
                /*
                foreach (int visit in visitedCavs)
                {
                    if (options.Contains(visit))
                    {
                        options.Remove(visit);
                    }
                }*/
                if (options.Count > 0)
                {
                    foreach (int option in options)
                    {
                        distance = CalculateCost(caverns.ElementAt(currentCavern - 1).X, caverns.ElementAt(currentCavern - 1).Y, caverns.ElementAt(option - 1).X, caverns.ElementAt(option - 1).Y);
                        totalCost = distance + caverns.ElementAt(option - 1).GoalCost;
                        if (totalCost < currentCost && !visitedCavs.Contains(option))
                        {
                            currentCost = totalCost;
                            currentDistance = distance;
                            pathfinding.Insert(0, option);
                        }
                        else
                        {
                            pathfinding.Add(option);
                        }
                    }

                    currentCavern = pathfinding.ElementAt(0);
                    distanceCost += currentDistance;
                    pathfinding.RemoveAt(0);
                    currentCost = int.MaxValue;
                    output = string.Concat(output, currentCavern, " ");
                    bestOutput.Add(currentCavern);
                    visitedCavs.Add(currentCavern);
                    goBack = false;
                }
                else
                {
                    output = string.Concat(output, "0", " ");
                    goBack = true;
                    removeOption = currentCavern;
                    bestOutput.Remove(currentCavern);
                    bestOutput.RemoveAt(bestOutput.Count - 1);
                    currentCavern = visitedCavs[visitedCavs.Count - 2];
                    output = string.Concat(output, currentCavern, " ");
                    visitedCavs.Add(currentCavern);
                }

                if (currentCavern == numOfVacs)
                {
                    cont = false;
                }
            }

            Console.WriteLine(output);
            Console.WriteLine(distanceCost);
            Console.WriteLine(string.Join(" ", bestOutput));
            using (StreamWriter file = new StreamWriter(outputfilePath, true))
            {
                file.WriteLine(output);
            }
        }
        /// <summary>
        /// Loops through the input list to find out which cavern can be visited from the provided cavern
        /// </summary>
        /// <param name="cavernNo">current cavern</param>
        /// <param name="input">input list</param>
        /// <returns>list of options of where to go</returns>
        public static List<int> WhereToGo(int cavernNo, int[] input)
        {
            List<int> options = new List<int>();
            int numOfVacs = input[0];
            int offset = numOfVacs * 2 ;
            int cavernIndex = offset + cavernNo;
            for (int i=1; i< numOfVacs+1;i++)
            {
                if(input[cavernIndex]==1 && i!=cavernNo)
                {
                    options.Add(i);
                }
                cavernIndex += numOfVacs;
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
