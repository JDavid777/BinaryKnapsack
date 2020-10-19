using System.IO;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace BinaryKnapsack.Problemas
{
    public class Knapsack
    {
        // private const string RootDirectory = "C:\\Users\\cobos\\Desktop\\F-M\\Binario-Restringido\\BinaryKnapsack\\bin\\Debug\\knapsack-files\\";
        //readonly string xsltFile = Application.StartupPath + @"\\knapsack-files\\";

        //string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //string xslLocation = Path.Combine(executableLocation, "docx.xsl");

        public string FileName;
        private const string RootDirectory = "knapsack-files\\";
        public int TotalItems;
        public double Capacity;
        public double OptimalKnown;
        internal List<Item> Items { get; } = new List<Item>();

        public Knapsack(string fileName)
        {
            FileName = fileName;
            ReadFile(RootDirectory + fileName);
        }

        public void ReadFile(string fullFileName)
        {
            //read the problem
            var lines = File.ReadAllLines(fullFileName);
            var firstline = lines[0].Split(' ');
            TotalItems = int.Parse(firstline[0]);
            Capacity = double.Parse(firstline[1]);

            var positionLine = 1;
            for (var i = 0; i < TotalItems; i++)
            {
                var line = lines[positionLine++].Split(' ');
                var value = double.Parse(line[0]);
                var weight = double.Parse(line[1]);
                var newVariable = new Item(i, value, weight);
                Items.Add(newVariable);
            }

            OptimalKnown = double.Parse(lines[positionLine]);
        }

        public double Density(int index)
        {
            return Items[index].Density;
        }

        public double Value(int index)
        {
            return Items[index].Value;
        }

        public double Weight(int index)
        {
            return Items[index].Weight;
        }

        public double Evaluate(int[] dim)
        {
            var summ = 0.0;
            for (var i = 0; i < TotalItems; i++)
                summ += dim[i] * Items[i].Value;

            return summ;
        }

        public override string ToString()
        {
            var result = "K:" + Capacity.ToString("##0") + "\n" +
                   "MyBestSolution:" + OptimalKnown.ToString("##0.00") + "\n";

            for (var i = 0; i < TotalItems; i++)
                result += Items[i] + "\n";
            return result;
        }
    }
}