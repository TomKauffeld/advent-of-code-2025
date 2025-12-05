using AdventOfCode.Core;
using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day05
{
    internal static class Program
    {
        private static async Task<(List<(long low, long high)> ranges, List<long> items)> ReadInput(bool test = false)
        {
            List<(long low, long high)> ranges = [];
            List<long> items = [];

            foreach (string line in await InputFileHelper.GetLines(05, test))
            {
                string[] parts = line.Split('-');
                switch (parts.Length)
                {
                    case 2:
                    {
                        long low = long.Parse(parts[0]);
                        long high = long.Parse(parts[1]);
                        ranges.Add((low, high));
                        break;
                    }
                    case 1:
                    {
                        long item = long.Parse(parts[0]);
                        items.Add(item);
                        break;
                    }
                }
            }


            return (ranges, items);
        }

        private static bool IsInside(List<(long low, long high)> ranges, long item)
        {
            foreach ((long low, long high) in ranges)
            {
                if (item >= low && item <= high)
                    return true;
                if (item < low)
                    return false;
            }
            return false;
        }
        
        private static async Task Part1(bool test = false)
        {
            (List<(long low, long high)> ranges, List<long> items) = await ReadInput(test);
            ranges = ranges.OrderBy(r => r.low).ToList();
            long totalFound = 0;
            foreach (long item in items)
            {
                if (IsInside(ranges, item))
                    ++totalFound;
            }
            Console.WriteLine($"Part 1: {totalFound}");
        }

        private static async Task Part2(bool test = false)
        {
            (List<(long low, long high)> ranges, _) = await ReadInput(test);
            ranges = ranges.OrderBy(r => r.low).ToList();
            for (int i = 0; i < ranges.Count - 1; ++i)
            {
                if (ranges[i].low > ranges[i].high)
                {
                    ranges.RemoveAt(i);
                    --i;
                }else if (ranges[i].high >= ranges[i + 1].low)
                {
                    if (ranges[i].high <= ranges[i + 1].high)
                        ranges[i] =  (ranges[i].low, ranges[i + 1].high);
                    ranges.RemoveAt(i + 1);
                    --i;
                }
            }
            
            long sum = ranges.Sum(r => r.high - r.low + 1);
            
            Console.WriteLine($"Part 2: {sum}");
        }
        
        
        private static async Task Main(string[] args)
        {
            await Part1();
            await Part2();
        }
    }
}
