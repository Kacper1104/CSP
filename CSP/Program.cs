
namespace CSP
{
    class Program
    {
        static void Main(string[] args)
        {
            //Graph hetman = new Graph(7, 0); //0 means that its hetmans problem
            //hetman.HetmansForwardChecking();
            
            Graph fibonacci = new Graph(59, 1);
            fibonacci.FibonacciForwardChecking(1);
        }
    }
}
