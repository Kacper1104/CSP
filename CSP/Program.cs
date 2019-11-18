
using System;

namespace CSP
{
    class Program
    {
        static void Main(string[] args)
        {
            //Hetmans
            //Graph hetmans;
            //int[] hetmansTestData = new int[8] { 4, 5, 6, 7, 8, 9, 10, 11 };
            //int[] hetmansSolution = new int[8] { 2, 10, 4, 40, 92, 352, 724, 2680 };
            //for (int i = 5; i < 8; i++)
            //{
            //    Console.WriteLine("Hetmans problem for N={0}", hetmansTestData[i]);
            //    hetmans = new Graph(hetmansTestData[i], 0);
            //    Console.WriteLine();
            //    if (i != 5)
            //    {
            //        Console.WriteLine("Forward checking");
            //        hetmans.HetmansForwardChecking(hetmansSolution[i]);
            //        Console.WriteLine();
            //    }
            //    Console.WriteLine("Backtracking");
            //    hetmans.HetmansBackTracking(hetmansSolution[i]);
            //    Console.WriteLine();
            //}

            //Fibonacci
            Graph fibonacci;
            int[] fibonacciTestData = new int[6] { 59, 617, 1447, 2137, 10177, 104009 };
            //for problem size 1447
            Console.WriteLine("Fibonacci problem for N={0}", fibonacciTestData[4]);
            
            //fibonacci = new Graph(fibonacciTestData[2], 1);
            //Console.WriteLine();
            //Console.WriteLine("Forward checking with heuristic 0");
            //fibonacci.FibonacciForwardChecking(0, 11);
            
            fibonacci = new Graph(fibonacciTestData[4], 1);
            Console.WriteLine();
            Console.WriteLine("Forward checking with heuristic 1");
            fibonacci.FibonacciForwardChecking(1, 54);
            
            //fibonacci = new Graph(fibonacciTestData[2], 1);
            //Console.WriteLine();
            //Console.WriteLine("Backtracking with heuristic 0");
            //fibonacci.FibonacciBackTracking(0, 7);
            
            fibonacci = new Graph(fibonacciTestData[4], 1);
            Console.WriteLine();
            Console.WriteLine("Backtracking with heuristic 1");
            fibonacci.FibonacciBackTracking(1, 54);
            Console.WriteLine();
        }
    }
}
