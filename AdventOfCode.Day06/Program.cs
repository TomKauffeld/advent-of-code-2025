using System.Runtime.InteropServices.ComTypes;
using AdventOfCode.Core;
using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day06
{
    internal static class Program
    {
        private static long Multiply(this IEnumerable<long> items)
        {
            return items.Aggregate<long, long>(1, (current, item) => current * item);
        }
        
        
        private static async Task<List<(char oper, List<long> numbers)>> ReadInputPart1(bool test = false)
        {
            List<List<long>> numbers = [];
            List<char> operators = [];
            foreach (string line in await InputFileHelper.GetLines(06, test))
            {
                List<string> parts = line
                    .Split(' ')
                    .Select(i => i.Trim())
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .ToList();
                for (int i = 0; i < parts.Count; ++i)
                {
                    if (long.TryParse(parts[i], out long number))
                    {
                        if (i < numbers.Count)
                            numbers[i].Add(number);
                        else
                            numbers.Add([number]);
                    }
                    else
                    {
                        operators.Add(parts[i][0]);
                    }
                }
            }

            List<(char oper, List<long> numbers)> items = [];
            for (int i = 0; i < numbers.Count && i < operators.Count; ++i)
                items.Add((operators[i], numbers[i]));

            return items;
        }
        
        
        
        private static async Task<List<(char oper, List<long> numbers)>> ReadInputPart2(bool test = false)
        {
            Field<bool> field = await InputFileHelper.GetField<bool>(6, test);
            List<(int from, int to)> indexes = [];
            int? startIndex = null;
            for (int x = 0; x < field.Width; ++x)
            {
                bool isEmpty = true;
                for (int y = 0; y < field.Height && isEmpty; ++y)
                {
                    if (field.Get(x, y) != ' ')
                        isEmpty = false;
                }
                if (isEmpty)
                {
                    if (!startIndex.HasValue)
                        continue;
                    indexes.Add((startIndex.Value, x));
                    startIndex = null;
                }
                else
                {
                    startIndex ??= x;
                }
            }
            if (startIndex.HasValue)
                indexes.Add((startIndex.Value, field.Width));
            List<(char oper, List<long> numbers)>  items = [];
            
            foreach ((int from, int to) in indexes)
            {
                List<long> numbers = [];
                char oper = ' ';
                for (int x = from; x < to; ++x)
                {
                    long value = 0;
                    for (int y = 0; y < field.Height - 1; ++y)
                    {
                        if (field.Get(x, y) != ' ')
                        {
                            long number = field.Get(x, y) - '0';
                            value = value * 10 + number;
                        }
                    }
                    numbers.Add(value);
                    if (field.Get(x, field.Height - 1) is '+' or '*')
                        oper = field.Get(x, field.Height - 1);
                }
                items.Add((oper, numbers));
            }
            
            
            return items;
        }
        
        private static async Task Part1(bool test = false)
        {
            List<(char oper, List<long> numbers)> input = await ReadInputPart1(test);
            long total = 0;
            foreach ((char oper, List<long> numbers) in input)
            {
                switch (oper)
                {
                    case '+':
                        total += numbers.Sum();
                        break;
                    case '*':
                        total += numbers.Multiply();
                        break;
                }
            }
            
            Console.WriteLine($"Part 1: {total}");
        }

        private static async Task Part2(bool test = false)
        {
            List<(char oper, List<long> numbers)> input = await ReadInputPart2(test);
            long total = 0;
            foreach ((char oper, List<long> numbers) in input)
            {
                switch (oper)
                {
                    case '+':
                        total += numbers.Sum();
                        break;
                    case '*':
                        total += numbers.Multiply();
                        break;
                }
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
