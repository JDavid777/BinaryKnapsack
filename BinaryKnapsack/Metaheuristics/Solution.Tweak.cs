using System.Collections.Generic;

namespace BinaryKnapsack.Metaheuristics
{
    public partial class Solution
    {
        public void DefineSelectedAndUnselectedLists(List<int> selected, List<int> unselected, ref double myWeight)
        {
            selected.Clear();
            unselected.Clear();
            myWeight = 0.0;
            for (var i = 0; i < Position.Length; i++)
            {
                if (Position[i] == 1)
                {
                    selected.Add(i);
                    myWeight += _myContainer.MyKnapsack.Weight(i);
                }
                else
                    unselected.Add(i);
            }
        }

        /// Turn off (take out of the knapsack) one that was on (inside the knapsack) - Randomly selected
        public void TurnOffRandom(List<int> selected, ref double myWeight)
        {
            var pos = _myContainer.MyAleatory.Next(selected.Count);
            var posTurnOff = selected[pos];
            selected.RemoveAt(pos); // Also works: selected.Remove(posTurnOff);
            Position[posTurnOff] = 0;
            myWeight -= _myContainer.MyKnapsack.Weight(posTurnOff);
        }

        /// Turn off (take out of the knapsack) one that was on (inside the knapsack) -
        /// Randomly selected from the worst items measured by density
        public void TurnOffDensity(List<int> selected, ref double myWeight)
        {
            if (selected.Count == 0) return; // WARNING: there is no selected items

            var byDensity = new List<KeyValuePair<int, double>>();
            foreach (var posSel in selected)
            {
                var den = _myContainer.MyKnapsack.Density(posSel);
                byDensity.Add(new KeyValuePair<int, double>(posSel, den));
            }
            byDensity.Sort((x,y) => x.Value.CompareTo(y.Value));

            var restrictedListSize = (byDensity.Count / 2);
            if (restrictedListSize == 0) restrictedListSize = 1;
            var pos = _myContainer.MyAleatory.Next(restrictedListSize);
            var posTurnOff = byDensity[pos].Key;
            selected.Remove(posTurnOff);
            Position[posTurnOff] = 0;
            myWeight -= _myContainer.MyKnapsack.Weight(posTurnOff);
        }

        public void TurnOnDensity(List<int> unselected, ref double myWeight)
        {
            if (unselected.Count == 0) return;

            var byDensity = new List<KeyValuePair<int, double>>();
            foreach (var posUnsel in unselected)
            {
                var den = _myContainer.MyKnapsack.Density(posUnsel);
                byDensity.Add(new KeyValuePair<int, double>(posUnsel, den));
            }
            byDensity.Sort((x, y) => -1 * x.Value.CompareTo(y.Value));
            var restrictedListSize = (byDensity.Count / 2);
            if (restrictedListSize == 0) restrictedListSize = 1;
        
                var pos = _myContainer.MyAleatory.Next(restrictedListSize);
            
                    var posTurnOn = byDensity[pos].Key;
                    unselected.Remove(posTurnOn);
                    Position[posTurnOn] = 1;
                    myWeight += _myContainer.MyKnapsack.Weight(posTurnOn);

           
        }

        public void TurnOnRandom(List<int> unselected, ref double myWeight)
        {
            if (unselected.Count != 0) // Unselected is not empty
            {
                var pos = _myContainer.MyAleatory.Next(unselected.Count);
                var posTurnOn = unselected[pos];
                unselected.RemoveAt(pos);
                Position[posTurnOn] = 1;
                myWeight += _myContainer.MyKnapsack.Weight(posTurnOn);
            }
        }

        /// Remove from the list the items that no longer fit
        public void LeaveOnlyValidUnselectedItems(List<int> unselected, double myWeight)
        {
            var freeSpace = _myContainer.MyKnapsack.Capacity - myWeight;
            for (var i = unselected.Count - 1; i >= 0; i--)
            {
                if (_myContainer.MyKnapsack.Weight(unselected[i]) > freeSpace)
                    unselected.RemoveAt(i);
            }
        }

        public void Complete(List<int> unselected, ref double myWeight)
        {
            do
            {
                LeaveOnlyValidUnselectedItems(unselected, myWeight);
                if (unselected.Count != 0)
                    TurnOnRandom(unselected, ref myWeight);
            } while (unselected.Count != 0); // There is no valid items to include on the knapsack
        }
    }
}