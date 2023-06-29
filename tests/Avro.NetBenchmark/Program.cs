using BenchmarkDotNet.Running;

public class Program
{
    public static void Main(string[] args) 
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        System.Console.WriteLine("Press any key to exit...");
        System.Console.ReadKey();
    }
}