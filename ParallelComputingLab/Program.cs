namespace ParallelComputingLab;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== ParallelComputingLab ===\n");

        // TODO 3: Массив из одного элемента
        // TODO 4: Пустой массив
        // TODO 5: Передача null  
        TestScenario("Один элемент", new[] { 13 });
        TestScenario("Пустой массив", Array.Empty<int>());
        Console.WriteLine("Тест с null:");
        try
        {
            Calculator.CalculateParallel(null!);
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("Корректно выброшено ArgumentNullException.\n");
        }

        int[] numbers = DataGenerator.Generate(size: 1_000_000, seed: 12345);
        BenchmarkRunner.Run(numbers);

        Console.WriteLine("\nРабота завершена.");
    }

    static void TestScenario(string name, int[] numbers)
    {
        Console.WriteLine($"Тест: {name}");
        var seq = Calculator.CalculateSequential(numbers);
        var par = Calculator.CalculateParallel(numbers);

        Console.WriteLine($"  Seq: Sum={seq.Sum}, Count={seq.Count}, Max={seq.MaxValue}");
        Console.WriteLine($"  Par: Sum={par.Sum}, Count={par.Count}, Max={par.MaxValue}");
        Console.WriteLine($"  Совпадение: {seq == par}\n");
    }
}