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
                new RandomSearch(maxEFOs),
                new GuidedLocalSearch(maxEFOs, 0.5)
            };


            foreach (var myMetaHeuristic in mhList)
            {
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{myMetaHeuristic.ToString(),60}");
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"|{"Problem",16} {"Best",8} {"Average Fitness",20} {"Standar Deviaton",20}{"Success Rate",20}{"Execution Time",20}|");
                Console.ResetColor();


                foreach (var myProblem in problemsList)
                {
                    Console.Write($"|{myProblem.FileName,15}|{myProblem.OptimalKnown,8:0.###}");
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
                    Console.WriteLine($"|{listaFitness.Average(),20:0.###}|{standarDeviation,20:0.#}|{succesRate,19:0.#}|{timesList.Average(),20:0.#####}|");
                }
                Console.WriteLine();
                //foreach (var item in myMetaHeuristic.Curve)
                //{
                //    string dato = string.Format("{0},",item);
                //    Console.Write(dato);
                //}
               
            }
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
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
