using BinaryKnapsack.Problemas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryKnapsack.Metaheuristics.SimpleState
{
    class GuidedLocalSearch : Metaheuristic
    {
        private double RegulationParam;
        private int[] Indicators;
        private double[] Penalties;
        private double[] Utilities;
        private double[] Costs;

        public GuidedLocalSearch(int MaxEFOs, double regulationParam=0.5)
        {
            this.MaxEFOs = MaxEFOs;
            this.RegulationParam = regulationParam;
        }
        public override void Execute(Knapsack theKnapsack, Random theAleatory)
        {
            this.MyKnapsack = theKnapsack;
            this.CurrentEFOs=0;
            this.MyAleatory = theAleatory;
            Curve = new List<double>();


            int M = theKnapsack.TotalItems;
            this.Costs = new double[M];

            //Calculo del vector de costos
            for (int i = 0; i < M; i++)
            {
                this.Costs[i] = 1.0 / this.MyKnapsack.Density(i);
                //this.Costs[i] = this.MyKnapsack.Weight(i);
            }

            //Start GLS
            var s = new Solution(this);
            s.RandomInitialization();
            this.Curve.Add(s.Fitness);
            s.AlteredFitness = s.Fitness;
            this.MyBestSolution = new Solution(s);
            //Indicator Vector
            this.Indicators = new int[M];
            //Penalties Vector
            this.Penalties = new double[M];
            //Utilities Vector
            this.Utilities = new double[M];

            for (int i = 0; i < M; i++)
            {
                this.Penalties[i] = 0;
            }
            

            while (this.CurrentEFOs < this.MaxEFOs)
            {

                HillClimbing LocalSearch = new HillClimbing(this.MaxEFOsLS());

                LocalSearch.Execute(theKnapsack, this.MyAleatory, s, this.Penalties, this.RegulationParam);
                s = LocalSearch.MyBestSolution;

                this.CurrentEFOs += LocalSearch.CurrentEFOs;

                this.Indicators = s.Position;
             
                for (int i = 0; i < M; i++)
                {
                     this.Utilities[i] = this.Indicators[i] * (this.Costs[i] / (1 + this.Penalties[i]));
                }
                List<int> maxUtilities = this.MaxUtility();
                foreach (var idx in maxUtilities)
                {
                    this.Penalties[idx] += 1;
                }
                if (this.MyBestSolution.Fitness < s.Fitness)
                {
                    this.MyBestSolution = new Solution(s);
                }
                if (Math.Abs(this.MyBestSolution.Fitness - MyKnapsack.OptimalKnown) < 1e-10)
                    break;
            }
        }

        private int MaxEFOsLS()
        {
            int remainingEFOs = this.MaxEFOs - this.CurrentEFOs;
            return remainingEFOs >= 50 ? 50 : remainingEFOs;
        }

        /// <summary>
        /// Retorna el indice en el que se encuentra el valor maximo de i utilidad
        /// </summary>
        /// <param name="utilities"></param>
        /// <returns></returns>
        private List<int> MaxUtility()
        {
            double max = this.Utilities.Max();

            List<int> maxUtilities = new List<int>();
            for (int i = 0; i < this.Utilities.Length; i++)
            {
                if (this.Utilities[i] == max)
                {
                    maxUtilities.Add(i);

                }
            }
            return maxUtilities;
        }

        //public override void Execute(Knapsack theKnapsack, Random theAleatory)
        //{

        //    int k = 0;
        //    var s = new Solution(this);
        //    int m = theKnapsack.TotalItems;
        //    s.RandomInitialization();
        //    int[] p = new int[m];


        //    for (int i = 0; i < m; i++)
        //    {
        //        p[i] = 0;
        //    }

        //    //Minimizando o maximizando

        //    s.Fitness = alteredFunction(s, 0.1, p);
        //    while (CurrentEFOs < MaxEFOs)
        //    {
        //        //TODO: ASK TO THE TEACHER*

        //        //for (int i = 0; i < m; i++)
        //        //{
        //        //    sumatoria += p[i] * s.Position[i];
        //        //}
        //        //h -= lambda * sumatoria;
        //        //s.Fitness = h;

        //        HillClimbing BL = new HillClimbing(MaxEFOs);
        //        //BL.Execute(theKnapsack, theAleatory, s);
        //        var r = new Solution(BL.MyBestSolution);


        //        /*Utilities*/
        //        List<double> utilities = new List<double>();
        //        for (int i = 0; i < m; i++)
        //        {
        //            utilities.Add(r.Position[i] * (theKnapsack.Items[i].Value/(1+p[i])));
        //        }


        //        //TODO: Sublist of Maxs

        //        double maxUtility = utilities.Max();
        //        int indexMax = utilities.IndexOf(maxUtility);
        //        //Penalize the max (TODO: With the sublist)
        //        p[indexMax] += 1;

        //        /*ASK TO THE TEACHER*/
        //        s = new Solution(r);



        //    }
        //}


    }
}
