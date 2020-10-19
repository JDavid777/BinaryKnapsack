using System;
using System.Collections.Generic;
using BinaryKnapsack.Problemas;

namespace BinaryKnapsack.Metaheuristics.SimpleState
{
    internal class RandomSearch : Metaheuristic
    {
        public RandomSearch(int maxEFOs)
        {
            MaxEFOs = maxEFOs;
        }

        public override void Execute(Knapsack theKnapsack, Random theAleatory,Solution s=null)
        {
            MyKnapsack = theKnapsack;
            MyAleatory = theAleatory;
            CurrentEFOs = 0;
            Curve = new List<double>();

            s = new Solution(this);
            s.RandomInitialization();
            Curve.Add(s.Fitness);

            while (CurrentEFOs < MaxEFOs)
            {
                var r = new Solution(this);
                r.RandomInitialization();

                if (r.Fitness > s.Fitness)
                    s = new Solution(r);

                Curve.Add(s.Fitness);

                if (Math.Abs(s.Fitness - MyKnapsack.OptimalKnown) < 1e-10)
                    break;
            }
            MyBestSolution = s;
        }

        public override string ToString()
        {
            return "Random Search";
        }
    }
}