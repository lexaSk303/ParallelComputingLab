namespace ParallelComputingLab;

public static class Calculator
{
    public static CalculationResult CalculateSequential(int[] numbers)
    {
        // TODO 5: Проверка на null
        ArgumentNullException.ThrowIfNull(numbers);

        long sum = 0;
        int count = 0;
        int maxValue = int.MinValue;

        foreach (int number in numbers)
        {
            // TODO 1: Делимость на 13
            if (number % 13 == 0)
            {
                sum += number;
                count++;
                if (number > maxValue) maxValue = number;
            }
        }

        return new CalculationResult(sum, count, count > 0 ? maxValue : 0);
    }

    public static CalculationResult CalculateParallel(
        int[] numbers,
        int maxDegreeOfParallelism = -1)
    {
        // TODO 5: Проверка на null
        ArgumentNullException.ThrowIfNull(numbers);

        if (maxDegreeOfParallelism != -1 && maxDegreeOfParallelism < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism));
        }

        long sum = 0;
        int count = 0;
        int maxValue = int.MinValue;
        object maxLock = new object();

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
        };

        // TODO 7: Используем локальное состояние, чтобы избежать гонки данных
        Parallel.For(
            0,
            numbers.Length,
            options,
            () => (LocalSum: 0L, LocalCount: 0, LocalMax: int.MinValue), 
            (i, state, local) =>
            {
                int number = numbers[i];
                if (number % 13 == 0)
                {
                    local.LocalSum += number;
                    local.LocalCount++;
                    if (number > local.LocalMax) local.LocalMax = number;
                }
                return local;
            },
            local =>
            {
                Interlocked.Add(ref sum, local.LocalSum);
                Interlocked.Add(ref count, local.LocalCount);

                lock (maxLock)
                {
                    if (local.LocalMax > maxValue) maxValue = local.LocalMax;
                }
            });

        return new CalculationResult(sum, count, count > 0 ? maxValue : 0);
    }
}