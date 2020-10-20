using System;
using System.Collections.Generic;
using System.Linq;
using BinaryKnapsack.Problemas;
using BinaryKnapsack.Metaheuristics;
using BinaryKnapsack.Metaheuristics.SimpleState;
using System.Diagnostics;

namespace BinaryKnapsack
{
    class Program
    {
        static void Main()
        {
            const int maxEFOs = 1000;
            const int maximasRepeticiones = 30;

            var problemsList = new List<Knapsack>
            {
                new Knapsack("f1.txt"),
                new Knapsack("f2.txt"),
                new Knapsack("f3.txt"),
                new Knapsack("f4.txt"),
                new Knapsack("f5.txt"),
                new Knapsack("f6.txt"),
                new Knapsack("f7.txt"),
                new Knapsack("f8.txt"),
                new Knapsack("f9.txt"),
                new Knapsack("f10.txt"),
                new Knapsack("Knapsack1.txt"),
                new Knapsack("Knapsack2.txt"),
                new Knapsack("Knapsack3.txt"),
                new Knapsack("Knapsack4.txt"),
                new Knapsack("Knapsack5.txt"),
                new Knapsack("Knapsack6.txt"),
            };

            var mhList = new List<Metaheuristic>()
            {
                new HillClimbing(maxEFOs),
                //new RandomSearch(maxEFOs),
                new GuidedLocalSearch(maxEFOs, 0.5)
            };

            Console.WriteLine($"{"Problem", 15} {"Best", 12} {"Hill Climbing", 30} {"Random Search", 35} {"Guided LS", 35}");
            Console.WriteLine("      ---------------------------------------------------------------------------------------------------------------------------");
            foreach (var myProblem in problemsList)
            {
                Console.Write($"{myProblem.FileName, 15} {myProblem.OptimalKnown, 12:0.###}| ");
                foreach (var myMetaHeuristic in mhList)
                {
                    var listaFitness = new List<double>();
                    var timesList = new List<double>();
                    var timesFoundIdeal = 0;
                    for (var rep = 0; rep < maximasRepeticiones; rep++)
                    {
                        var aleatory = new Random(rep);
                        Stopwatch timeMeasure = new Stopwatch();
                        timeMeasure.Start();
                        myMetaHeuristic.Execute(myProblem, aleatory);
                        timeMeasure.Stop();
                        timesList.Add(timeMeasure.Elapsed.TotalMilliseconds);
                        listaFitness.Add(myMetaHeuristic.MyBestSolution.Fitness);
                        if (Math.Abs(myMetaHeuristic.MyBestSolution.Fitness - myProblem.OptimalKnown) < 1e-10)
                            timesFoundIdeal++;
                    }
                    var succesRate = timesFoundIdeal * 100.0 / maximasRepeticiones;
                    var standarDeviation = StandarDeviation(listaFitness);
                    Console.Write($"{listaFitness.Average(), 10:0.###} {standarDeviation,8:0.###} ({succesRate,4:0.#}) {timesList.Average(),11:0.#####}|");
                }
                Console.WriteLine();
            }
            Console.WriteLine("      --------------------------------------------------------------------------------------------------");
            Console.ReadKey();
        }
        private static double StandarDeviation(List<double> fitnessList)
        {
            int n = fitnessList.Count;
            double sum = 0;
            double average = fitnessList.Average();
            for (int i = 0; i < n; i++)
            {
                sum += Math.Pow((fitnessList[i] - average), 2);
            }
            return Math.Sqrt((1.0 / (double)n) * sum);
        }

    }
}
