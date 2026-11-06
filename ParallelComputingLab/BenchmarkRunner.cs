using System.Diagnostics;

namespace ParallelComputingLab;

public static class BenchmarkRunner
{
    // TODO 3: Изменяем количество измерений на 10
    private const int MeasurementCount = 10;
    public static void Run(int[] numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);
        Console.WriteLine($"\nРазмер массива: {numbers.Length:N0}");

        Calculator.CalculateSequential(numbers);
        Calculator.CalculateParallel(numbers);

        var sequentialTimes = new List<double>();
        var parallelTimes = new List<double>();
        CalculationResult? expectedResult = null;

        for (int i = 0; i < MeasurementCount; i++)
        {
            var sequential = Measure(() => Calculator.CalculateSequential(numbers));
            var parallel = Measure(() => Calculator.CalculateParallel(numbers));

            expectedResult ??= sequential.Result;

            // TODO 9: Проверка совпадения результатов
            if (sequential.Result != parallel.Result)
                throw new InvalidOperationException("Результаты не совпадают!");

            sequentialTimes.Add(sequential.ElapsedMilliseconds);
            parallelTimes.Add(parallel.ElapsedMilliseconds);
        }

        Console.WriteLine($"Сумма:           {expectedResult!.Sum:N0}");
        Console.WriteLine($"Количество:      {expectedResult.Count:N0}");
        Console.WriteLine($"Максимум:        {expectedResult.MaxValue:N0}");
        Console.WriteLine($"Последовательно: {Median(sequentialTimes):F3} мс");
        Console.WriteLine($"Параллельно:{Median(parallelTimes):F3} мс");
    }

    public static void CompareParallelism(int[] numbers)
    {
        // TODO 2: Добавляем степень параллелизма 3
        int[] degrees = { 1, 2, 3, 4, -1 };

        Console.WriteLine("\nСтепень параллелизма | Время, мс | Ускорение");
        Console.WriteLine("--------------------------------------------");
        var seqMeasure = Measure(() => Calculator.CalculateSequential(numbers));
        double seqMedian = seqMeasure.ElapsedMilliseconds;

        foreach (int degree in degrees)
        {
            var times = new List<double>();
            CalculationResult? expected = null;

            for (int i = 0; i < MeasurementCount; i++)
            {
                var measurement = Measure(() => Calculator.CalculateParallel(numbers, degree));

                // TODO 9: Проверка совпадения для каждого варианта
                expected ??= measurement.Result;
                if (expected != measurement.Result)
                    throw new InvalidOperationException($"Несовпадение при degree={degree}");

                times.Add(measurement.ElapsedMilliseconds);
            }

            double median = Median(times);
            // TODO 4: Добавляем расчет ускорения
            double speedup = seqMedian / median;

            string degreeStr = degree == -1 ? "Default (-1)" : degree.ToString();
            Console.WriteLine($"{degreeStr,20} | {median,9:F3} | {speedup,9:F2}x");
        }
    }

    public static (CalculationResult Result, double ElapsedMilliseconds) Measure(Func<CalculationResult> action)
    {
        var stopwatch = Stopwatch.StartNew();
        CalculationResult result = action();
        stopwatch.Stop();
        return (result, stopwatch.Elapsed.TotalMilliseconds);
    }

    private static double Median(List<double> values)
    {
        double[] sorted = values.OrderBy(x => x).ToArray();
        int middle = sorted.Length / 2;
        return sorted.Length % 2 == 1 ? sorted[middle] : (sorted[middle - 1] + sorted[middle]) / 2;
    }
}