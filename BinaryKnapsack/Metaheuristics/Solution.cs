using System.Collections.Generic;

namespace BinaryKnapsack.Metaheuristics
{
    public partial class Solution
    {
        public int[] Position;
        public double Fitness;
        public double Weight;

        public double AlteredFitness;  //use Only for GLS 
        public   Metaheuristic _myContainer; //TODO: Cambiar indentificar de acceso
       
       
        public Solution(Metaheuristic theOwner)
        {
            _myContainer = theOwner;
            Position = new int[_myContainer.MyKnapsack.TotalItems];
        }

        public Solution(Solution original)
        {
            _myContainer = original._myContainer;

            Position = new int[_myContainer.MyKnapsack.TotalItems];
            for (var d = 0; d < _myContainer.MyKnapsack.TotalItems; d++)
                Position[d] = original.Position[d];
            Fitness = original.Fitness;
            AlteredFitness = original.AlteredFitness;
        }

        // Random Initialization using the new methods for Tweak
        public void RandomInitialization()
        {
            var selected = new List<int>();
            var unselected = new List<int>();
            var myWeight = 0.0;

            DefineSelectedAndUnselectedLists(selected, unselected, ref myWeight);
            Complete(unselected, ref myWeight);
            Evaluate();
        }
        

        public void Tweak()
        {
            // Define selected and unselect items. Also calculate the current weight of the knapsack
            var selected = new List<int>();
            var unselected = new List<int>();
            var myWeight = 0.0;

            DefineSelectedAndUnselectedLists(selected, unselected, ref myWeight);

            if (_myContainer.MyAleatory.NextDouble() < 0.2)
                TurnOffRandom(selected, ref myWeight);
            else
                TurnOffDensity(selected, ref myWeight);

            LeaveOnlyValidUnselectedItems(unselected, myWeight);

            // Here was the ERROR ... I corrected it by replacing "selected" with "unselected"
            if (_myContainer.MyAleatory.NextDouble() < 0.2)
                TurnOnRandom(unselected, ref myWeight);
            else
                TurnOnDensity(unselected, ref myWeight); //Prender las sol que estan apagadas que quepan en la mochilay tengan mayor density
            Complete(unselected, ref myWeight);

            Evaluate();
        }

        public void CalculateWeight()
        {
            Weight = 0.0;
            for (var i = 0; i < Position.Length; i++)
            {
                if (Position[i] == 1)
                    Weight += _myContainer.MyKnapsack.Weight(i);
            }
        }

        public void Evaluate()
        {
            _myContainer.CurrentEFOs++;
            CalculateWeight();
            if (Weight > _myContainer.MyKnapsack.Capacity)
                Fitness = 0;
            else
                Fitness = _myContainer.MyKnapsack.Evaluate(Position);
        }

        public override string ToString()
        {
            var result = "P [ ";
            for (var d = 0; d < _myContainer.MyKnapsack.TotalItems; d++)
            {
                result += Position[d] + " ";
            }
            result += "] F [" + Fitness + " ]";
                 result += "] AF [" + AlteredFitness + " ]" ;
            return result;
        }
        /// <summary>
        /// Calculo de la funcion objetivo disminuida
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public void AlteredFunction(double[] penalties, double regulationParam)
        {
            int m = penalties.Length;
            
            double sumatoria = 0;
            for (int i = 0; i < m; i++)
            {
                sumatoria += penalties[i] * this.Position[i];
            }
            double h = this.Fitness - regulationParam * sumatoria;
            this.AlteredFitness=h;
        }
    }
}