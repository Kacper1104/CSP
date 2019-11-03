using System;
using System.Collections.Generic;
using System.Text;

namespace CSP
{
    public class Graph
    {
        //Usable for both problems
        public int problemSize { get; set; }
        public Node initialNode { get; set; }
        public List<Node> btSolutions;
        public List<Node> fcSolutions;

        public Graph(int problemSize, int problem)
        {
            //for both problems
            this.problemSize = problemSize;
            btSolutions = new List<Node>();
            fcSolutions = new List<Node>();
            //hetmans problem
            if (problem == 0)
            {
                initialNode = new Node(problemSize, 0);
                for (int i = 0; i < problemSize; i++)
                {
                    for (int j = 0; j < problemSize; j++)
                    {
                        initialNode.board[i, j] = false;
                    }
                }
            }
            else if(problem == 1)
            {
                initialNode = new Node(problemSize);
            }
        }

        #region FIBONACCI_PROBLEM

        public void FibonacciForwardChecking(int heuristicID) 
        {
            FibonacciForwardChecking(initialNode, heuristicID);
        }
        public void FibonacciForwardChecking(Node node, int heuristicID)
        {
            List<int> possibleIndexes = new List<int>();
            bool isNewSolution;
            if (node.SatisfactionCheck() == 0)//if problem not satisfied
            {
                if (heuristicID == 0)
                {
                    for (int i = 0; i < node.domain.Count; i++)
                    {
                        if (node.SatisfactionCheck(i) >= 0)
                            possibleIndexes.Add(i);
                    }
                }
                else if(heuristicID == 1)
                {
                    for(int i = node.domain.Count-1; i >= 0; i--)
                    {
                        if (node.SatisfactionCheck(i) >= 0)
                            possibleIndexes.Add(i);
                    }
                }
                foreach (var index in possibleIndexes)
                {
                    FibonacciForwardChecking(node.InsertToSolution(index), heuristicID);
                }
            }
            else if (node.SatisfactionCheck() == 1)
            {
                isNewSolution = true;
                foreach (var item in fcSolutions)
                {
                    if (item.CompareFibonacci(node))
                        isNewSolution = false;
                }
                if (isNewSolution || fcSolutions.Count == 0)
                {
                    fcSolutions.Add(node);
                    PrintFibonacciScore(node, "fc");
                }
            }
        }
        public void FibonacciBackTracking(int heuristicID)
        {
            FibonacciBackTracking(initialNode, heuristicID);
            Console.WriteLine("Backtracking finished. Found " + btSolutions.Count + " solutions");
        }
        public void FibonacciBackTracking(Node node, int heuristicID)//0 => take next; 1 => take always biggest number in domain if possible
        {
            Node next;
            bool isNewSolution;
            int satisfactionCheck;
            if (heuristicID == 0)//no heuristic, just insert next one
            {
                for (int i = 0; i < node.domain.Count; i++)
                {
                    satisfactionCheck = node.SatisfactionCheck(node.domain[i]);
                    next = node.InsertToSolution(i);
                    if (satisfactionCheck == 0)
                        FibonacciBackTracking(next, heuristicID);
                    if (satisfactionCheck == 1)
                    {
                        isNewSolution = true;
                        foreach (var item in btSolutions)
                        {
                            if (item.CompareFibonacci(next))
                                isNewSolution = false;
                        }
                        if (isNewSolution || btSolutions.Count == 0)
                        {
                            btSolutions.Add(next);
                            PrintFibonacciScore(next, "bt");
                        }
                    }
                }
            }
            else if(heuristicID == 1)
            {
                for(int i = node.domain.Count - 1; i >= 0; i--)
                {
                    satisfactionCheck = node.SatisfactionCheck(node.domain[i]);
                    next = node.InsertToSolution(i);
                    if (satisfactionCheck == 0)
                        FibonacciBackTracking(next, heuristicID);
                    if (satisfactionCheck == 1)
                    {
                        isNewSolution = true;
                        foreach (var item in btSolutions)
                        {
                            if (item.CompareFibonacci(next))
                                isNewSolution = false;
                        }
                        if (isNewSolution || btSolutions.Count == 0)
                        {
                            btSolutions.Add(next);
                            PrintFibonacciScore(next, "bt");
                        }
                    }
                }
            }
        }

        #endregion

        #region HETMANS_PROBLEM

        public void HetmansBackTracking()
        {
            HetmansBacktracking(initialNode);
            Console.WriteLine("Backtracking finished. Found "+btSolutions.Count+" solutions");
        }
        public void HetmansBacktracking(Node node)
        {
            Node next;
            bool isNewSolution;
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    if (node.SatisfactionCheck(i, j))
                    {
                        next = node.InsertHetman(i, j);
                        if (next.hetmansPlaced < problemSize)
                        {
                            HetmansBacktracking(next);
                        }
                        else
                        {
                            isNewSolution = true;
                            foreach(var item in btSolutions)
                            {
                                if (item.Compare(next))
                                    isNewSolution = false;
                            }
                            if (isNewSolution || btSolutions.Count == 0)
                            {
                                btSolutions.Add(next);
                                PrintHetmansScore(next, "bt");
                            }
                        }
                    }
                }
            }
        }
        public void HetmansForwardChecking()
        {
            HetmansForwardChecking(initialNode);
            Console.WriteLine("Forward checking finished. Found " + fcSolutions.Count + " solutions");
        }
        public void HetmansForwardChecking(Node node)
        {
            List<Tuple<int, int>> possibleIndexes = new List<Tuple<int, int>>();
            bool isNewSolution;
            if (node.hetmansPlaced < problemSize)//if problem not satisfied
            {
                for (int i = 0; i < problemSize; i++)
                {
                    for (int j = 0; j < problemSize; j++)
                    {
                        if (node.SatisfactionCheck(i, j))
                            possibleIndexes.Add(new Tuple<int, int>(i, j));
                    }
                }
                foreach (var item in possibleIndexes)
                {
                    HetmansForwardChecking(node.InsertHetman(item.Item1, item.Item2));
                }
            }
            else
            {
                isNewSolution = true;
                foreach (var item in fcSolutions)
                {
                    if (item.Compare(node))
                        isNewSolution = false;
                }
                if (isNewSolution || fcSolutions.Count == 0)
                {
                    fcSolutions.Add(node);
                    PrintHetmansScore(node, "fc");
                }
            }
        }
        #endregion

        #region PRINTING
        public void PrintHetmansScore(Node node, string algorithm)
        {
            if (algorithm == "bt")
                Console.WriteLine("\nBoard: " + btSolutions.Count);
            if (algorithm == "fc")
                Console.WriteLine("\nBoard: " + fcSolutions.Count);
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    if (node.board[i, j])
                        Console.Write("1 ");
                    else
                        Console.Write("0 ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public void PrintFibonacciScore(Node node, string algorithm)
        {
            if (algorithm == "bt")
                Console.WriteLine("\nSequence: " + btSolutions.Count);
            if (algorithm == "fc")
                Console.WriteLine("\nSequence: " + fcSolutions.Count);
            for (int i = 0; i < problemSize; i++)
            {
                Console.Write(node.solution[i]+" ");
            }
            Console.WriteLine();
        }
        #endregion
    }
}