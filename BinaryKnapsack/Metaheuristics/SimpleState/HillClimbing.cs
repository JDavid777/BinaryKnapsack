using System;
using System.Collections.Generic;
using BinaryKnapsack.Problemas;

namespace BinaryKnapsack.Metaheuristics.SimpleState
{
    internal class HillClimbing : Metaheuristic
    {
        public HillClimbing(int maxEFOs)
        {
            this.MaxEFOs = maxEFOs;
        }

        public override void Execute(Knapsack theKnapsack, Random theAleatory)
        {
            this.MyKnapsack = theKnapsack;
            this.MyAleatory = theAleatory;
            this.CurrentEFOs = 0;
            Curve = new List<double>();

            // Hill Climbing
            var s = new Solution(this);

            s.RandomInitialization();
            Curve.Add(s.Fitness);

            while (CurrentEFOs < MaxEFOs)
            {
                var r = new Solution(s);
                r.Tweak();

                if (r.Fitness > s.Fitness)
                    s = new Solution(r);

                Curve.Add(s.Fitness);

                if (Math.Abs(s.Fitness - MyKnapsack.OptimalKnown) < 1e-10)
                    break;
            }
            MyBestSolution = s;
        }
        public void Execute(Knapsack theKnapsack, Random theAleatory, Solution s, double[] penalties, double regulationParam)
        {
            this.MyKnapsack = theKnapsack;
            this.MyAleatory = theAleatory;
            this.CurrentEFOs = 0;
            Curve = new List<double>();
            s._myContainer = this;
            // Hill Climbing
            Curve.Add(s.Fitness);

            while (this.CurrentEFOs < MaxEFOs) 
            {
                var r = new Solution(s);
                r.Tweak();
                r.AlteredFunction(penalties, regulationParam);
                if (r.AlteredFitness > s.AlteredFitness)
                    s = new Solution(r);

                Curve.Add(s.Fitness);

                if (Math.Abs(s.Fitness - MyKnapsack.OptimalKnown) < 1e-10)
                    break;
            }
            MyBestSolution = s;
        }
        

    public override string ToString()
        {
            return "Hill Climbing";
        }
    }
}