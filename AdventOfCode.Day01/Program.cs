using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day01
{
    internal static class Program
    {
        private static async Task<List<int>> ReadDirections(bool test = false)
        {
            List<string> lines = await InputFileHelper.GetLines(01, test);
            List<int> items = [];
            foreach (string line in lines)
            {
                switch (line[0])
                {
                    case 'L' when int.TryParse(line[1..], out int l):
                        items.Add(-l);
                        break;
                    case 'R' when int.TryParse(line[1..], out int r):
                        items.Add(r);
                        break;
                    default:
                        throw new Exception($"Invalid direction: {line}");
                }
            }
            return items;
        }
        
        private static async Task Part1(bool test = false)
        {
            const int startDialPosition = 50;
            const int maxDialPosition = 99;
            const int mod = maxDialPosition + 1;
            List<int> items = await ReadDirections(test);
            int current = startDialPosition;
            int times = 0;
            foreach (int item in items)
            {
                current += item;
                while (current < 0)
                    current += mod;
                while (current >= mod)
                    current -= mod;
                if (current == 0)
                    ++times;
            }
            Console.WriteLine($"Part 1: {times}");
        }

        private static async Task Part2(bool test = false)
        {
            const int startDialPosition = 50;
            const int maxDialPosition = 99;
            const int mod = maxDialPosition + 1;
            List<int> items = await ReadDirections(test);
            int current = startDialPosition;
            int times = 0;
            foreach (int item in items)
            {
                for (int i = 0; i < Math.Abs(item); ++i)
                {
                    current += (item > 0) ? 1 : -1;
                    while (current < 0)
                        current += mod;
                    while (current >= mod)
                        current -= mod;
                    if (current == 0)
                        ++times;
                }
            }
            
            Console.WriteLine($"Part 2: {times}");
        }
        
        
        private static async Task Main(string[] args)
        {
            await Part1();
            await Part2();
        }
    }
}
