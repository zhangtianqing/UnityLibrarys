using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dll.UnityUtils
{
    /// <summary>
    /// 概率生成器
    /// </summary>
    public class ProbabilityGenerator<T>
    {
        public List<T> itemTypes = new List<T>();
        private List<double> probabilities = new List<double>();
        private System.Random random = new System.Random();
        public int GetCount() { return itemTypes.Count; }
        public double GetProbalities(T id,Func<T,T, bool> check) {
        
            for (int i = 0; i < itemTypes.Count; i++)
            {
                if (check.Invoke (itemTypes[i],id))
                {
                    return probabilities[i];
                 
                }
            }
            return 0;

        }
        private void Simple() {
            ProbabilityGenerator<ItemType> itemGenerator = new ProbabilityGenerator<ItemType>();

            // Add item types and their probabilities
            itemGenerator.AddItemType(new ItemType("Item 1"), 0.2);
            itemGenerator.AddItemType(new ItemType("Item 2"), 0.4);
            itemGenerator.AddItemType(new ItemType("Item 3"), 0.3);
            itemGenerator.AddItemType(new ItemType("Item 4"), 0.1);

            // Generate F items
            int F = 10;
            for (int i = 0; i < F; i++)
            {
                ItemType item = itemGenerator.GenerateItem();
                Console.WriteLine("Generated Item: " + item.Name);
            }
        }
        public void AddItemType(T itemType, double probability)
        {
            itemTypes.Add(itemType);
            probabilities.Add(probability); 
            totalProbability += probability;
        }
        public void Clear() {
            itemTypes.Clear();
            probabilities.Clear();
            totalProbability = 0;
        }

        double totalProbability = 0;
        public T GenerateItem()
        {
            double randomValue = random.NextDouble() * totalProbability;

            double cumulativeProbability = 0;
            for (int i = 0; i < itemTypes.Count; i++)
            {
                cumulativeProbability += probabilities[i];
                if (randomValue < cumulativeProbability)
                {
                    return itemTypes[i];
                }
            }

            //  如果概率被错误地指定，应该永远不会执行到这里
            throw new Exception("Invalid probabilities");
        }
    }

    class ItemType
    {
        public string Name { get; private set; }

        public ItemType(string name)
        {
            Name = name;
        }
    }
}
