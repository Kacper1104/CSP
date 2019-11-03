using System;
using System.Collections.Generic;
using System.Linq;

namespace CSP
{
    public class Node
    {
        //for both problems
        //for hetmans problem
        public bool[,] board { get; set; }
        public int hetmansPlaced { get; set; }

        //fibonacci problem
        public List<int> domain { get; set; }
        public List<int> solution { get; set; }
        public int goal { get; set; }

        //constructor for hetmans problem
        public Node(int problemSize, int hetmansPlaced)
        {
            board = new bool[problemSize, problemSize];
            this.hetmansPlaced = hetmansPlaced;
        }
        //constructor for fibonacci problem
        public Node(int goal)
        {
            domain = EstablishDomain(goal);
            solution = new List<int>();
            this.goal = goal;
        }

        #region FIBONACCI_PROBLEM
        public List<int> EstablishDomain(int n)
        {
            List<int> sequence = new List<int>();
            int a = 0;
            sequence.Add(a);
            int b = 1;
            sequence.Add(b);
            int c;
            while (a + b < n)
            {
                c = a + b;
                a = b;
                b = c;
                sequence.Add(c);
            }
            return sequence;
        }
        public Node InsertToSolution (int index)
        {
            Node node = new Node(goal);
            domain.ForEach((item) => { node.domain.Add(item); });
            solution.ForEach((item) => { node.solution.Add(item); });
            node.solution.Add(node.domain[index]);
            node.domain.RemoveAt(index);
            return node;
        }

        public int SatisfactionCheck(int nextNumber) //-1 => error; 0 => OK; 1 => satisfied;
        {
            if(solution.Sum() + nextNumber > goal)
                return -1;
            else if (solution.Sum() + nextNumber < goal)
                return 0;
            return 1;
        }
        public int SatisfactionCheck()
        {
            if (solution.Sum() > goal)
                return -1;
            else if (solution.Sum() == goal)
                return 1;
            else return 0;
        }
        public bool CompareFibonacci(Node node)
        {
            foreach(var number in solution)
            {
                int index = node.solution.FindIndex(item => item == number);
                if (index == -1)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region HETMANS_PROBELM
        public bool SatisfactionCheck(int x, int y)//check satisfaction on specified field on board
        {
            for(int i = 0; i < board.GetLength(0); i++)
            {
                //check every value in column y
                if (board[i, y])
                    return false;
                //check every value in row x
                if (board[x, i])
                    return false;
                //check cross
                if (x - i >= 0 && y - i  >= 0)
                {
                    if (board[x - i, y - i])
                        return false;
                }
                if (x - i >= 0 && y + i < board.GetLength(1))
                {
                    if (board[x - i, y + i])
                        return false;
                }
                if (x + i < board.GetLength(0) && y + i < board.GetLength(1))
                {
                    if (board[x + i, y + i])
                        return false;
                }
                if (x + i < board.GetLength(0) && y - i >= 0)
                {
                    if (board[x + i, y - i])
                        return false;
                }
            }
            return true;
        }
        public Node InsertHetman(int x, int y)
        {
            Node node = new Node(board.GetLength(0), hetmansPlaced+1);
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                    node.board[i, j] = board[i, j];
            }
            node.board[x, y] = true;
            return node;
        }

        public bool Compare(Node node)
        {
            for(int i = 0; i < board.GetLength(0); i++)
            {
                for(int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] != node.board[i, j])
                        return false;
                }
            }
            return true;
        }
        #endregion
    }
}