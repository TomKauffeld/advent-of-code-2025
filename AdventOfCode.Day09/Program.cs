using AdventOfCode.Core;
using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day09
{
    internal static class Program
    {
        private static async Task<List<(int x, int y)>> ReadInput(bool test = false)
        {
            return await InputFileHelper.GetDualNumbers(9, ',', test);
        }

        private static async Task Part1(bool test = false)
        {
            List<(int x, int y)> items = await ReadInput(test);
            
            
            long largestSize = 0;
            for (int i = 0; i < items.Count - 1; ++i)
            {
                for (int j = i + 1; j < items.Count; ++j)
                {
                    long dx = 1 + Math.Abs(items[i].x - items[j].x);
                    long dy = 1 + Math.Abs(items[i].y - items[j].y);
                    long size = dx * dy;
                    if (size > largestSize)
                    {
                        if (test)
                            Console.WriteLine($"Found ({items[i].x},{items[i].y} - {items[j].x},{items[j].y}) with {size} area");
                        largestSize = size;
                    }
                }
            }
            
            Console.WriteLine($"Part 1: {largestSize}");
        }


        private static bool ValidateRectangle((int x, int y)[] mappedItems, int a, int b, char?[] map, int height)
        {
            int left = Math.Min(mappedItems[a].x, mappedItems[b].x);
            int right = Math.Max(mappedItems[a].x, mappedItems[b].x);
            int top = Math.Min(mappedItems[a].y, mappedItems[b].y);
            int bottom = Math.Max(mappedItems[a].y, mappedItems[b].y);
            bool foundTopLeft = false;
            bool foundTopRight = false;
            bool foundBottomLeft = false;
            bool foundBottomRight = false;

            for (int i = 0; i < mappedItems.Length; ++i)
            {
                if (mappedItems[i].x <= left && mappedItems[i].y <= top)
                    foundTopLeft = true;
                if (mappedItems[i].x >= right && mappedItems[i].y <= top)
                    foundTopRight = true;
                if (mappedItems[i].x <= left && mappedItems[i].y >= bottom)
                    foundBottomLeft = true;
                if (mappedItems[i].x >= right && mappedItems[i].y >= bottom)
                    foundBottomRight = true;
                if (foundTopLeft && foundTopRight && foundBottomLeft && foundBottomRight)
                {
                    for (int x = left; x <= right; ++x)
                    {
                        for (int y = top; y <= bottom; ++y)
                        {
                            if (map[x * height + y] is null)
                                return false;
                        }
                    }

                    return true;
                }
            }
            return false;
        }
        

        private static async Task Part2(bool test = false)
        {
            List<(int x, int y)> items = await ReadInput(test);
            List<int> distinctX = items.Select(p => p.x).Distinct().Order().ToList();
            List<int> distinctY = items.Select(p => p.y).Distinct().Order().ToList();
            int[] xToIndex = new int[items.Max(p => p.x) + 1];
            int[] yToIndex = new int[items.Max(p => p.y) + 1];
            Parallel.For(0, xToIndex.Length, i => xToIndex[i] = -1);
            Parallel.For(0, yToIndex.Length, i => yToIndex[i] = -1);
            for (int i = 0; i < distinctX.Count; ++i)
                xToIndex[distinctX[i]] = i;
            for (int i = 0; i < distinctY.Count; ++i)
                yToIndex[distinctY[i]] = i;
            (int x, int y)[] mappedItems = new (int x, int y)[items.Count];
            Parallel.For(0, items.Count, i => mappedItems[i] = (xToIndex[items[i].x], yToIndex[items[i].y]));
            
            char?[] map =  new char?[distinctX.Count * distinctY.Count];
            for (int i = 0; i < mappedItems.Length; ++i)
            {
                int j = i > 0 ?  i - 1 : mappedItems.Length - 1 - i;
                int xs = mappedItems[j].x;
                int ys = mappedItems[j].y;
                int xe = mappedItems[i].x;
                int ye = mappedItems[i].y;
                
                if (xs < xe)
                    for (int x = xs + 1; x < xe; ++x)
                        map[x * distinctY.Count + ys] = 'r';
                if (xs > xe)
                    for (int x = xe + 1; x < xs; ++x)
                        map[x * distinctY.Count + ys] = 'l';
                if (ys < ye)
                    for (int y = ys; y <= ye; ++y)
                        map[xs * distinctY.Count + y] = 'd';
                if (ys > ye)
                    for (int y = ye; y <= ys; ++y)
                        map[xs * distinctY.Count + y] = 'u';
            }
            for (int i = 0; i < mappedItems.Length; ++i)
            {
                int x = mappedItems[i].x;
                int y = mappedItems[i].y;
                map[x * distinctY.Count + y] = map[x * distinctY.Count + y] switch
                {
                    null => '#',
                    'd' => 'D',
                    'u' => 'U',
                    _ => map[x * distinctY.Count + y]
                };
            }

            // Produces incorrect result, but is still useful
            // Some points/lines are considered inside the polygon and "shoot out" to the right
            // However the ValidateRectangle still produces the correct assessment in the end, so it's a win ?
            for (int y = 0; y < distinctY.Count; ++y)
            {
                int count = 0;
                for (int x = 0; x < distinctX.Count; ++x)
                {
                    int i = x * distinctY.Count + y;
                    switch (map[i])
                    {
                        case 'U' or 'u':
                            ++count;
                            break;
                        case 'D' or 'd':
                            --count;
                            break;
                        case null when count > 0:
                            map[i] = 'x';
                            break;
                    }
                }
            }
            
            string path = InputFileHelper.GetFilePath(9, "output.txt");
            await using (FileStream fs = new(path, FileMode.Create, FileAccess.Write))
            {
                await using (StreamWriter writer = new(fs))
                {
                    for (int y = 0; y < distinctY.Count; ++y)
                    {
                        for (int x = 0; x < distinctX.Count; ++x)
                        {
                            await writer.WriteAsync(map[x * distinctY.Count + y] ?? '.');
                        }
                        await writer.WriteLineAsync();
                    }
                    await writer.FlushAsync();
                }
            }
            
            long largestSize = 0;
            for (int i = 0; i < mappedItems.Length - 1; ++i)
            {
                int xi = distinctX[mappedItems[i].x];
                int yi = distinctY[mappedItems[i].y];
                for (int j = i + 1; j < mappedItems.Length; ++j)
                {
                    int xj = distinctX[mappedItems[j].x];
                    int yj = distinctY[mappedItems[j].y];
                    
                    long dx = 1 + Math.Abs(xi - xj);
                    long dy = 1 + Math.Abs(yi - yj);
                    long size = dx * dy;
                    if (size > largestSize)
                    {
                        if (ValidateRectangle(mappedItems, i, j, map, distinctY.Count))
                        {
                            if (test)
                                Console.WriteLine($"Found ({items[i].x},{items[i].y} - {items[j].x},{items[j].y}) with {size} area");
                            largestSize = size;
                        }
                    }
                }
            }

            Console.WriteLine($"Part 2: {largestSize}");
        }
        
        
        private static async Task Main(string[] args)
        {
            await Part1();
            await Part2();
        }
    }
}
