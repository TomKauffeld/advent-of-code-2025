using AdventOfCode.Core;
using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day07
{
    internal static class Program
    {
        private static async Task<Field<bool>> ReadInput(bool test = false)
        {
            return await InputFileHelper.GetField<bool>(7, test);
        }
        
        private static async Task Part1(bool test = false)
        {
            Field<bool> field = await ReadInput(test);
            Queue<(int x, int y)> todo = [];
            for (int x = 0; x < field.Width; x++)
                if (field.Get(x, 0) == 'S')
                    todo.Enqueue((x, 0));
            long total = 0;
            while (todo.TryDequeue(out (int x, int y) position))
            {
                for (int y = position.y; y < field.Height; ++y)
                {
                    if (field.GetCustomData(position.x, y) is true)
                        break;
                    char c = field.Get(position.x, y);
                    field.SetCustomData(position.x, y, true);
                    if (c == '^')
                    {
                        if (position.x > 0)
                            todo.Enqueue((position.x - 1, y));
                        if (position.x < field.Width - 1)
                            todo.Enqueue((position.x + 1, y));
                        ++total;
                        break;
                    }
                }
            }
            
            Console.WriteLine($"Part 1: {total}");
        }


        private static long CountPaths(Field<bool> field, int x, int y, ref Dictionary<(int x, int y), long> cache)
        {
            if (cache.TryGetValue((x, y), out long count))
                return count;
            if (field.Get(x, y) == '^')
            {
                long left = x > 0 ? CountPaths(field, x - 1, y, ref cache) : 0;
                long right =  x < field.Width - 1 ? CountPaths(field, x + 1, y, ref cache) : 0;
                long total = left + right;
                cache[(x, y)] = total;
            }
            else if (y < field.Height - 1)
            {
                long total = CountPaths(field, x, y + 1, ref cache);
                cache[(x, y)] = total;
            }
            else
            {
                cache[(x, y)] = 1;
            }
            return cache[(x, y)];
        }
        

        private static async Task Part2(bool test = false)
        {
            Field<bool> field = await ReadInput(test);
            Queue<(int x, int y)> todo = [];
            for (int x = 0; x < field.Width; x++)
                if (field.Get(x, 0) == 'S')
                    todo.Enqueue((x, 0));

            long total = 0;
            Dictionary<(int x, int y), long> cache = new();
            while (todo.TryDequeue(out (int x, int y) position))
            {
                long result = CountPaths(field, position.x, position.y, ref cache);
                if (test)
                    Console.WriteLine($"From {position.x},{position.y}, found {result} paths");
                total += result;
            }
            
            
            Console.WriteLine($"Part 2: {total}");
        }
        
        
        private static async Task Main(string[] args)
        {
            await Part1();
            await Part2();
        }
    }
}
