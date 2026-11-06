namespace ParallelComputingLab;

public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Parallel Computing Lab (П1 + П2) ===");

        // TODO 1: Добавлен размер массива 10 000 000
        int[] sizes = { 10_000, 100_000, 1_000_000, 5_000_000, 10_000_000 };

        foreach (int size in sizes)
        {
            int[] numbers = DataGenerator.Generate(size, seed: 12345);

            Console.WriteLine($"\n--- Размер: {size:N0} ---");
            BenchmarkRunner.Run(numbers);
            BenchmarkRunner.CompareParallelism(numbers);
        }

        Console.WriteLine("\n=== Выводы (TODO 5-10) ===");
        Console.WriteLine("5. Параллельная обработка стала быстрее последовательной начиная с размера ~100 000 - 1 000 000 элементов.");
        Console.WriteLine("6. Лучший результат обычно показывает Default (-1) или значение, равное количеству физических ядер CPU (часто 4 или 8).");
        Console.WriteLine("7. Результаты могут незначительно отличаться при перезапуске из-за фоновых процессов ОС.");

        // TODO 8: Почему 4 может быть хуже 2?
        Console.WriteLine("8. MaxDegreeOfParallelism = 4 может быть хуже 2, если:");
        Console.WriteLine("   - Задача слишком простая, и накладные расходы на переключение 4 потоков превышают выгоду.");
        Console.WriteLine("   - Происходит конкуренция за кэш процессора или память.");
        Console.WriteLine("   - На компьютере всего 2 физических ядра, и 4 потока создают излишнюю конкуренцию за ресурсы.");

        // TODO 10: Итоговый вывод
        Console.WriteLine("10. Вывод: Параллельная обработка не всегда быстрее. " +
            "Для малых объемов данных накладные расходы делают её медленнее. " +
            "Распараллеливание выгодно только для ресурсоемких задач на больших массивах данных.");

        Console.WriteLine("\nРабота завершена.");
    }
    static void TestScenario(string name, int[] numbers)
    {
        var seq = Calculator.CalculateSequential(numbers);
        var par = Calculator.CalculateParallel(numbers);
        if (seq != par) throw new Exception($"Ошибка в тесте: {name}");
    }
}