using BenchmarkDotNet.Running;

public class Program
    {
        public static void Main(string[] args)
        {
            // This will discover all [Benchmark]-attributed types in the assembly
            BenchmarkSwitcher
              .FromAssembly(typeof(Program).Assembly)
              .Run(args);
        }
    }