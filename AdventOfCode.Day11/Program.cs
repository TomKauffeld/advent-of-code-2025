using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day11
{
    internal static class Program
    {
        private static async Task<Dictionary<string,string[]>> ReadInput(bool test = false)
        {
            List<string> lines = await InputFileHelper.GetLines(11, test);
            Dictionary<string, string[]> items = [];
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                items[parts[0]] = parts[1]
                    .Split(' ')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();
            }


            return items;
        }

        private static long CountPaths(Dictionary<string, string[]> items, string current, string[] path, string target, ref Dictionary<string, long> cache)
        {
            if (cache.TryGetValue(current, out long paths))
                return paths;
            long total = 0;
            if (items.TryGetValue(current, out string[]? values))
            {
                foreach (string value in values)
                {
                    if (value == target)
                    {
                        ++total;
                    }
                    else if (!path.Contains(value))
                    {
                        total += CountPaths(items, value, [..path, value], target, ref cache);
                    }
                }
            }
            cache[current] = total;
            return total;
        }

        private static async Task Part1(bool test = false)
        {
            Dictionary<string, string[]> items = await ReadInput(test);
            Dictionary<string, long> cache = new();
            long total = CountPaths(items, "you", ["you"], "out", ref cache);
            Console.WriteLine($"Part 1: {total}");
        }
        

        private static async Task Part2(bool test = false)
        {
            Dictionary<string, string[]> items = await ReadInput(test);
            Dictionary<string, long> cache = new();
            long pathsFftDac = CountPaths(items, "fft", ["fft"], "dac", ref cache);
            cache.Clear();
            long pathsDacFft = CountPaths(items, "dac", ["dac"], "fft", ref cache);
            string stop1 = pathsFftDac > pathsDacFft ? "fft" : "dac";
            string stop2 =  pathsFftDac > pathsDacFft ? "dac" : "fft";
            long pathsStop1Stop2 = Math.Max(pathsFftDac, pathsDacFft);
            
            cache.Clear();
            long pathsSvrStop1 = CountPaths(items, "svr", ["svr"], stop1, ref cache);
            
            cache.Clear();
            long pathsStop2Out = CountPaths(items, stop2, [stop2], "out", ref cache);
            long total = pathsSvrStop1 * pathsStop1Stop2 * pathsStop2Out;
            Console.WriteLine($"Part 2: {total}");
        }
        
        
        private static async Task Main(string[] args)
        {
            await Part1();
            await Part2();
        }
    }
}
