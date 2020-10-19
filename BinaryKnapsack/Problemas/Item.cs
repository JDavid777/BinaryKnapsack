namespace BinaryKnapsack.Problemas
{
    internal class Item
    {
        public int Position;
        public double Value;
        public double Weight;
        public double Density => Value / Weight;

        public Item(int p, double v, double w)
        {
            Position = p;
            Value = v;
            Weight = w;
        }

        public Item(Item original)
        {
            Position = original.Position;
            Value = original.Value;
            Weight = original.Weight;
        }

        public override string ToString()
        {
            return "P: " + $"{Position,3:##0}" +
                   " V: " + $"{Value,6:##0.0}" +
                   " W: " + $"{Weight,6:##0.0}" +
                   " D: " + $"{Density,6:##0.0}";
        }
    }
}
