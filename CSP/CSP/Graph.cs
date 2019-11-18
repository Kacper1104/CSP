using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace CSP
{
    public class Graph
    {
        //Usable for both problems
        public int problemSize { get; set; }
        public Node initialNode { get; set; }
        public List<Node> btSolutions;
        public List<Node> fcSolutions;

        //Diagnostics
        long nodesVisited;
        long time;
        Stopwatch stopwatch;
        long generalTime;
        Stopwatch generalStopwatch;

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

        public void FibonacciForwardChecking(int heuristicID, int solutionsLimit) 
        {
            //Diagnostics
            stopwatch = new Stopwatch();
            time = 0;
            nodesVisited = 0;
            stopwatch.Start();
            generalStopwatch = new Stopwatch();
            generalTime = 0;
            generalStopwatch.Start();
            //Diagnostics end
            FibonacciForwardChecking(initialNode, heuristicID, solutionsLimit);
            Console.WriteLine("Forward Checking finished. Found " + fcSolutions.Count + " solutions");
            //Diagnostics
            generalStopwatch.Stop();
            generalTime = generalStopwatch.ElapsedMilliseconds;
            Console.WriteLine("Diagnostics for Hetmans Problem: \nTime until first solution: {0}\nNodes visited: {1}\nTime in general: {2}", time, nodesVisited, generalTime);
            //Diagnostics End
        }
        public void FibonacciForwardChecking(Node node, int heuristicID, int solutionsLimit)
        {
            if (fcSolutions.Count == solutionsLimit && solutionsLimit != -1)
                return;
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
                    //Diagnostics
                    nodesVisited++;
                    //Diagnostics End
                    FibonacciForwardChecking(node.InsertToSolution(index), heuristicID, solutionsLimit);
                }
            }
            else if (node.SatisfactionCheck() == 1)//if problem satisfied
            {
                if(time == 0)//Diagnostics
                {
                    stopwatch.Stop();
                    time = stopwatch.ElapsedMilliseconds;
                }//Diagnostics End

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
        public void FibonacciBackTracking(int heuristicID, int solutionsLimit)
        {
            //Diagnostics
            stopwatch = new Stopwatch();
            time = 0;
            nodesVisited = 0;
            stopwatch.Start();
            generalStopwatch = new Stopwatch();
            generalTime = 0;
            generalStopwatch.Start();
            //Diagnostics end
            FibonacciBackTracking(initialNode, heuristicID, solutionsLimit);
            Console.WriteLine("Backtracking finished. Found " + btSolutions.Count + " solutions");
            //Diagnostics
            generalStopwatch.Stop();
            generalTime = generalStopwatch.ElapsedMilliseconds;
            Console.WriteLine("Diagnostics for Hetmans Problem: \nTime until first solution: {0}\nNodes visited: {1}\nTime in general: {2}", time, nodesVisited, generalTime);
            //Diagnostics End
        }
        public void FibonacciBackTracking(Node node, int heuristicID, int solutionsLimit)//0 => take next; 1 => take always biggest number in domain if possible
        {
            if (btSolutions.Count == solutionsLimit && solutionsLimit != -1)
                return;
            Node next;
            bool isNewSolution;
            int satisfactionCheck;
            if (heuristicID == 0)//no heuristic, just insert next one
            {
                for (int i = 0; i < node.domain.Count; i++)
                {
                    //Diagnostics
                    nodesVisited++;
                    //Diagnostics end
                    satisfactionCheck = node.SatisfactionCheck(node.domain[i]);
                    next = node.InsertToSolution(i);
                    if (satisfactionCheck == 0)
                        FibonacciBackTracking(next, heuristicID, solutionsLimit);
                    if (satisfactionCheck == 1)
                    {
                        if (time == 0)//Diagnostics
                        {
                            stopwatch.Stop();
                            time = stopwatch.ElapsedMilliseconds;
                        }//Diagnostics End

                        isNewSolution = true;
                        foreach (var item in btSolutions)
                        {
                            if (item.CompareFibonacci(next))
                                isNewSolution = false;
                        }
                        if (isNewSolution || btSolutions.Count == 0)
                        {
                            btSolutions.Add(next);
                            //PrintFibonacciScore(next, "bt");
                        }
                    }
                }
            }
            else if(heuristicID == 1)
            {
                for(int i = node.domain.Count - 1; i >= 0; i--)
                {
                    //Diagnostics
                    nodesVisited++;
                    //Diagnostics end
                    satisfactionCheck = node.SatisfactionCheck(node.domain[i]);
                    next = node.InsertToSolution(i);
                    if (satisfactionCheck == 0)
                        FibonacciBackTracking(next, heuristicID, solutionsLimit);
                    if (satisfactionCheck == 1)
                    {
                        if (time == 0)//Diagnostics
                        {
                            stopwatch.Stop();
                            time = stopwatch.ElapsedMilliseconds;
                        }//Diagnostics End
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

        public void HetmansBackTracking(int solutionsLimit)
        {
            //Diagnostics
            stopwatch = new Stopwatch();
            time = 0;
            nodesVisited = 0;
            stopwatch.Start();
            generalStopwatch = new Stopwatch();
            generalTime = 0;
            generalStopwatch.Start();
            //Diagnostics end
            HetmansBacktracking(initialNode, solutionsLimit);
            Console.WriteLine("Backtracking finished. Found " + btSolutions.Count + " solutions");
            //Diagnostics
            generalStopwatch.Stop();
            generalTime = generalStopwatch.ElapsedMilliseconds;
            Console.WriteLine("Diagnostics for Hetmans Problem: \nTime until first solution: {0}\nNodes visited: {1}\nTime in general: {2}", time, nodesVisited, generalTime);
            //Diagnostics End
        }
        public void HetmansBacktracking(Node node, int solutionsLimit)
        {
            if (btSolutions.Count == solutionsLimit)
                return;
            Node next;
            bool isNewSolution;
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    //Diagnotics
                    nodesVisited++;
                    //Diagnostics end
                    if (node.SatisfactionCheck(i, j))
                    {
                        next = node.InsertHetman(i, j);
                        if (next.hetmansPlaced < problemSize)
                        {
                            HetmansBacktracking(next, solutionsLimit);
                        }
                        else
                        {
                            if (time == 0)//Diagnostics
                            {
                                stopwatch.Stop();
                                time = stopwatch.ElapsedMilliseconds;
                            }//Diagnostics End
                            isNewSolution = true;
                            foreach(var item in btSolutions)
                            {
                                if (item.Compare(next))
                                    isNewSolution = false;
                            }
                            if (isNewSolution || btSolutions.Count == 0)
                            {
                                btSolutions.Add(next);
                                //PrintHetmansScore(next, "bt");
                            }
                        }
                    }
                }
            }
        }
        public void HetmansForwardChecking(int solutionsLimit)
        {
            //Diagnostics
            stopwatch = new Stopwatch();
            time = 0;
            nodesVisited = 0;
            stopwatch.Start();
            generalStopwatch = new Stopwatch();
            generalTime = 0;
            generalStopwatch.Start();
            //Diagnostics end
            HetmansForwardChecking(initialNode, solutionsLimit);
            Console.WriteLine("Forward checking finished. Found " + fcSolutions.Count + " solutions");
            //Diagnostics
            generalStopwatch.Stop();
            generalTime = generalStopwatch.ElapsedMilliseconds;
            Console.WriteLine("Diagnostics for Hetmans Problem: \nTime until first solution: {0}\nNodes visited: {1}\nTime in general: {2}", time, nodesVisited, generalTime);
            //Diagnostics End
        }
        public void HetmansForwardChecking(Node node, int solutionsLimit)
        {
            if (fcSolutions.Count == solutionsLimit)
                return;
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
                    //Diagnotics
                    nodesVisited++;
                    //Diagnostics end
                    HetmansForwardChecking(node.InsertHetman(item.Item1, item.Item2), solutionsLimit);
                }
            }
            else
            {
                if (time == 0)//Diagnostics
                {
                    stopwatch.Stop();
                    time = stopwatch.ElapsedMilliseconds;
                }//Diagnostics End
                isNewSolution = true;
                foreach (var item in fcSolutions)
                {
                    if (item.Compare(node))
                        isNewSolution = false;
                }
                if (isNewSolution || fcSolutions.Count == 0)
                {
                    fcSolutions.Add(node);
                    //PrintHetmansScore(node, "fc");
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
            for (int i = 0; i < node.solution.Count; i++)
            {
                Console.Write(node.solution[i]+" ");
            }
            Console.WriteLine();
        }
        #endregion
    }
}