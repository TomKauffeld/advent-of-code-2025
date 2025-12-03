using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day02
{
    internal static class Program
    {
        private static async Task<List<(long low, long high)>> ReadItems(bool test = false)
        {
            string file;
            await using (FileStream stream = InputFileHelper.GetInputFile(02, test))
            {
                using StreamReader reader = new(stream);
                file = await reader.ReadToEndAsync();
            }
            List<(long low, long high)> items = [];
            foreach (string item in file.Split(','))
            {
                string[] parts = item.Split('-');
                if (parts.Length != 2)
                    throw new Exception($"Invalid item: {item}");
                if (long.TryParse(parts[0], out long low) &&  long.TryParse(parts[1], out long high))
                    items.Add((low, high));
                else
                    throw new Exception($"Invalid item: {item}");
            }
            return items;
        }

        private static int GetNumberOfDigits(long value)
        {
            return (int)Math.Floor(Math.Log10(value)) + 1;
        }

        private static long GetSmallestNumberForNumberOfDigits(int digits)
        {
            return (long)Math.Pow(10, digits - 1);
        }
        
        private static long GetLargestNumberForNumberOfDigits(int digits)
        {
            return (long)Math.Pow(10, digits) - 1;
        }
        
        private static async Task Part1(bool test = false)
        {
            List<(long low, long high)> items = await ReadItems(test);
            List<long> invalids = [];
            foreach ((long low, long high) in items)
            {
                int nbL = GetNumberOfDigits(low);
                int nbH = GetNumberOfDigits(high);
                long lowBound = nbL % 2 == 0 ? low : GetSmallestNumberForNumberOfDigits(nbL + 1);
                long highBound = nbH % 2 == 0 ? high : GetLargestNumberForNumberOfDigits(nbH - 1);
                /* Should have been the smarter way, but didn't work...
                if (highBound >= lowBound)
                {
                    nbL = nbL % 2 == 0 ? nbL : nbL + 1;
                    nbH = nbH % 2 == 0 ? nbH : nbH - 1;
                    int halfNbL = nbL / 2;
                    int halfNbH = nbH / 2;
                    long secondsHalfStart = lowBound % (long)Math.Pow(10, halfNbL);
                    long secondsHalfEnd = highBound % (long)Math.Pow(10, halfNbH);
                    for (int halfNb = halfNbL; halfNb <= halfNbH; ++halfNb)
                    {
                        long firstHalfStart = halfNb == halfNbL 
                            ? lowBound / (long)Math.Pow(10, halfNbL) 
                            : GetSmallestNumberForNumberOfDigits(halfNb);
                        long firstHalfEnd = halfNb == halfNbH
                            ? highBound / (long)Math.Pow(10, halfNbH)
                            : GetLargestNumberForNumberOfDigits(halfNb);
                        if (halfNb == halfNbL && firstHalfStart < secondsHalfStart)
                            firstHalfStart = secondsHalfStart;
                        if (halfNb == halfNbH && firstHalfEnd > secondsHalfEnd)
                            firstHalfEnd = secondsHalfEnd;
                        for (long v = firstHalfStart; v <= firstHalfEnd; ++v)
                        {
                            long value = v * (long)Math.Pow(10, halfNb) + v;
                            invalids.Add(value);
                        }
                    }
                }*/
                // So brute force it is
                for (long v = lowBound; v <= highBound; ++v)
                {
                    int nb = GetNumberOfDigits(v) / 2;
                    long firstHalf = v / (long)Math.Pow(10, nb);
                    long secondHalf = v % (long)Math.Pow(10, nb);
                    if (firstHalf == secondHalf)
                    {
                        invalids.Add(v);
                    }
                }
            }
            Console.WriteLine($"Part 1: {invalids.Sum()}");
        }

        private static async Task Part2(bool test = false)
        {
            List<(long low, long high)> items = await ReadItems(test);
            List<long> invalids = [];
            foreach ((long low, long high) in items)
            {
                for (long v = low; v <= high; ++v)
                {
                    int nb = GetNumberOfDigits(v);
                    string stringValue = v.ToString();
                    for (int i = 1; i <= nb / 2; ++i)
                    {
                        if (nb % i == 0)
                        {
                            int amount = nb / i;
                            string part = stringValue[..(i)];
                            string total = "";
                            for (int j = 0; j < amount; ++j)
                                total += part;
                            if (total == stringValue)
                            {
                                if (!invalids.Contains(v))
                                    invalids.Add(v);
                            }
                        }
                        
                    }
                }
            }
            Console.WriteLine($"Part 2: {invalids.Sum()}");
        }
        
        
        private static async Task Main(string[] args)
        {
            await Part1();
            await Part2();
        }
    }
}
