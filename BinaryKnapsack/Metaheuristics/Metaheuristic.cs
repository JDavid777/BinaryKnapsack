using System;
using System.Collections.Generic;
using BinaryKnapsack.Problemas;

namespace BinaryKnapsack.Metaheuristics
{
    public abstract class Metaheuristic
    {
        public int MaxEFOs;

        public Random MyAleatory;

        public Solution MyBestSolution { get; set; }

        public Knapsack MyKnapsack { get; set; }

        public int CurrentEFOs = 0;

        public List<double> Curve;

        public abstract void Execute(Knapsack theKnapsack, Random theAleatory);
    }
}
