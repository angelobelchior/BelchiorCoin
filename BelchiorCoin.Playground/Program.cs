using Blockchain;
using System;
using ServiceStack.Text;
using System.Reflection;

namespace BelchiorCoin.Playground
{
    public class Program
    {
        static void Main(string[] args)
        {
            byte effort = 0;
            var chain = new Chain(effort);
            var block1 = chain.Add(new Coin(300F));
            var block2 = chain.Add(new Coin(400F));
            var block3 = chain.Add(new Coin(500F));

            var separator = (new string('-', 10)) + "\n";

            Console.WriteLine("Genesis Block");
            Console.WriteLine(chain.GenesisBlock.Dump());
            Console.WriteLine(separator);

            foreach (var block in chain.Blocks)
            {
                Console.WriteLine(block.Dump());
                Console.WriteLine("---");
            }

            Console.WriteLine($"Check Integrity: {chain.CheckIntegrity()}");
            Console.WriteLine(separator);

            Console.WriteLine("Simulates a change in the immutable structure of Block");
            Console.WriteLine("Set block1.Data to new Coin(666F)");
            SetPrivatePropertyValue(block1, "Data", new Coin(666F));
            Console.WriteLine(separator);

            Console.WriteLine($"Check Integrity: {chain.CheckIntegrity()}");
            Console.WriteLine(separator);

            Console.Read();
        }

        public static void SetPrivatePropertyValue<T>(Block block, string propertyName, T value)
        {
            var t = block.GetType();
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance;
            t.InvokeMember(propertyName, flags, null, block, new object[] { value });
        }
    }
}
