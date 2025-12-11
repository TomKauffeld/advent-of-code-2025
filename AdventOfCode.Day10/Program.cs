using System.Collections.Concurrent;
using AdventOfCode.Core;
using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day10
{
    internal static class Program
    {
        private static async Task<List<(int target, int[] buttons)>> ReadInputPart1(bool test = false)
        {
            List<string> lines = await InputFileHelper.GetLines(10, test);

            List<(int target, int[] buttons)> items = [];
            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');
                string buttons = parts[0][1..^1];
                int target = 0;
                for (int i = 0; i < buttons.Length; ++i)
                    target |= buttons[i] == '#' ? 1 << i : 0;
                items.Add((
                    target,
                    parts[1..^1].Select(s => s[1..^1].Split(',').Select(int.Parse).Aggregate(0, (a, v) => a |= 1 << v)).ToArray()
                    ));
            }


            return items;
        }

        private static async Task<List<(int[] target, int[][] buttons)>> ReadInputPart2(bool test = false)
        {
            List<string> lines = await InputFileHelper.GetLines(10, test);

            List<(int[] target, int[][] buttons)> items = [];
            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');
                string buttons = parts[0][1..^1];
                items.Add((
                    parts[^1][1..^1].Split(',').Select(int.Parse).ToArray(),
                    parts[1..^1].Select(s => s[1..^1].Split(',').Select(int.Parse).ToArray()).ToArray()
                ));
            }

            return items;
        }
        
        private static bool Combinations(int[] buttons, int count, int[] ignore, int target)
        {
            if (count < 1)
                return false;
            for (int i = 0; i < buttons.Length; ++i)
            {
                if (!ignore.Contains(i))
                {
                    int current = buttons[i];
                    if (current == target)
                        return true;
                    int[] nextIgnore = [..ignore, i];
                    bool found = Combinations(buttons, count - 1, nextIgnore, target ^ current);
                    if (found)
                        return true;
                }
            }
            return false;
        }
        

        private static int MinPresses(int target, int[] buttons)
        {
            if (buttons.Any(b => b == target))
                return 1;
            int count = 2;
            while (count <= buttons.Length)
            {
                bool found = Combinations(buttons, count, [], target);
                if (found)
                    return count;
                ++count;
            }
            return 0;
        }

        private static async Task Part1(bool test = false)
        {
            List<(int target, int[] buttons)> items = await ReadInputPart1(test);
            long total = 0;
            foreach ((int target, int[] buttons) item in items)
            {
                Console.Write($"Checking: {item.target} with {item.buttons.Length}! options");
                int result = MinPresses(item.target, item.buttons);
                Console.WriteLine($" - Found: {result}");
                total += result;
            }
            Console.WriteLine($"Part 1: {total}");
        }
        

        
        private static int MinPresses(int[] target, int[][] buttons, int focus = 0, int maxPresses = int.MaxValue)
        {
            if (target.Any(i => i < 0))
                return int.MaxValue;
            if (focus >= target.Length)
                return 0;
            if (target[focus] == 0)
                return MinPresses(target, buttons.Where(b => b.All(c => c >= focus)).ToArray(), focus + 1, maxPresses);
            if (maxPresses < target.Sum())
                return int.MaxValue;
            int current = maxPresses;
            foreach (int[] option in buttons.OrderByDescending(b => b.Length))
            {
                if (option.Contains(focus) && option.All(b => b >= focus))
                {
                    int[] localTarget = target.ToArray();
                    foreach (int button in option)
                        --localTarget[button];
                    int found = MinPresses(localTarget, buttons, focus, current - 1);
                    if (found < current)
                        current = found;
                }
            }
            
            if (current >= maxPresses)
                return int.MaxValue;
            
            return current + 1;
        }

        private static void SortColumn(ref int[,] matrix, int column)
        {
            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                
            }
        }

        private static void Solver(int[][] buttons, int[] target)
        {
            int max = buttons.Max(b => b.Max());
            int[,] matrix = new int[buttons.Length + 1, max + 1];
            for (int i = 0; i < buttons.Length; ++i)
            {
                foreach (int button in buttons[i])
                {
                    matrix[i, button] = 1;
                }
            }
            for (int i = 0; i < target.Length; ++i)
            {
                matrix[buttons.Length, i] = target[i];
            }
            

            for (int i = 0; i < matrix.GetLength(1); ++i)
            {
                
            }
            
            
        }
        

        private static async Task Part2(bool test = false)
        {
            List<(int[] target, int[][] buttons)> items = await ReadInputPart2(test);
            long total = 0;
            //Parallel.ForEach(items, item =>
            foreach ((int[] target, int[][] buttons) item in items)
            {
                Solver(item.buttons, item.target);
            }
            //);
            Console.WriteLine($"Part 2: {total}");
        }
        
        
        private static async Task Main(string[] args)
        {
            await Part1();
            await Part2(true);
        }
    }
}
