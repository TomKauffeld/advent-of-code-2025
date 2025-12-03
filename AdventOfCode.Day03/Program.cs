using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day03
{
    internal static class Program
    {
        private static Task<List<List<int>>> ReadItems(bool test = false)
        {
            return InputFileHelper.GetDigits(03, test);
        }

        
        private static async Task Part1(bool test = false)
        {
            List<List<int>> items = await ReadItems(test);
            List<int> result = [];
            foreach (List<int> list in items)
            {
                int largest = 0;
                for (int i = 0; i < list.Count - 1; ++i)
                {
                    if (list[i] * 10 + 9 < largest)
                        continue;
                    for (int j = i + 1; j < list.Count; ++j)
                    {
                        int number = list[i] * 10 + list[j];
                        if (number > largest)
                            largest = number;
                    }
                }
                result.Add(largest);
            }
            Console.WriteLine($"Part 1: {result.Sum()}");
        }

        private static async Task Part2(bool test = false)
        {
            List<List<int>> items = await ReadItems(test);
            List<long> result = [];
            
            foreach (List<int> list in items)
            {
                long current = 0;
                int startIndex = 0;
                for (int i = 0; i < 12; ++i)
                {
                    int index = startIndex;
                    int maxIndex = list.Count - 12 + i;
                    for (int j = startIndex + 1; j <= maxIndex; ++j)
                    {
                        if (list[j] > list[index])
                            index = j;
                    }
                    current = current * 10 + list[index];
                    startIndex = index + 1;
                }
                result.Add(current);
            }
            Console.WriteLine($"Part 2: {result.Sum()}");
        }
        
        
        private static async Task Main(string[] args)
        {
            await Part1();
            await Part2();
        }
    }
}
