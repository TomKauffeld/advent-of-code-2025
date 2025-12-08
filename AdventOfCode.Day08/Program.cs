using AdventOfCode.Core;
using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day08
{
    internal static class Program
    {
        private static async Task<List<(int x, int y, int z)>> ReadInput(bool test = false)
        {
            return (await InputFileHelper.GetSeparatedNumbers(8, test, ','))
                .Select(items => (x: items[0], y: items[1], z: items[2]))
                .ToList();
        }

        private static double Distance((int x, int y, int z) a, (int x, int y, int z) b)
        {
            double dx = a.x - b.x;
            double dy = a.y - b.y;
            double dz = a.z - b.z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        
        private static async Task Part1(bool test = false)
        {
            int connectionsToMake = test ? 10 : 1000;
            
            List<(int x, int y, int z)> items = await ReadInput(test);
            List<(int a, int b, double d)> links = [];
            
            for (int i = 0; i < items.Count - 1; ++i)
            {
                for (int j = i + 1; j < items.Count; ++j)
                {
                    links.Add((i, j, Distance(items[i], items[j])));
                }
            }
            links = links
                .OrderBy(l => l.d)
                .ToList();
            
            Dictionary<int, int> indexToGroup = [];
            Dictionary<int, List<int>> groups = [];

            for (int i = 0; i < items.Count; ++i)
            {
                indexToGroup.Add(i, i);
                groups.Add(i, [i]);
            }
            
            for (int i = 0; i < connectionsToMake; ++i)
            {
                (int a, int b, double d) link = links[i];
                
                int groupA = indexToGroup[link.a];
                int groupB = indexToGroup[link.b];
                if (groupA != groupB)
                {
                    (int x, int y, int z) itemA = items[link.a];
                    (int x, int y, int z) itemB = items[link.b];
                    foreach (int index in groups[groupB])
                    {
                        groups[groupA].Add(index);
                        indexToGroup[index] = groupA;
                    }
                    if (test)
                        Console.WriteLine($"Linking {itemA.x},{itemA.y},{itemA.z} and {itemB.x},{itemB.y},{itemB.z}, in group: {groups[groupA].Count}");
                    
                    groups.Remove(groupB);
                }
            }
            

            long result = groups
                .Select(p => p.Value.Count)
                .OrderDescending()
                .Take(3)
                .Aggregate(1L, (a, b) => a * b);
            
            Console.WriteLine($"Part 1: {result}");
        }
        
        

        private static async Task Part2(bool test = false)
        {
            List<(int x, int y, int z)> items = await ReadInput(test);
            List<(int a, int b, double d)> links = [];
            
            for (int i = 0; i < items.Count - 1; ++i)
            {
                for (int j = i + 1; j < items.Count; ++j)
                {
                    links.Add((i, j, Distance(items[i], items[j])));
                }
            }
            links = links
                .OrderBy(l => l.d)
                .ToList();
            
            Dictionary<int, int> indexToGroup = [];
            Dictionary<int, List<int>> groups = [];

            for (int i = 0; i < items.Count; ++i)
            {
                indexToGroup.Add(i, i);
                groups.Add(i, [i]);
            }
            int lastLinkIndex = -1;
            for (int i = 0; i < links.Count; ++i)
            {
                (int a, int b, double d) link = links[i];
                
                int groupA = indexToGroup[link.a];
                int groupB = indexToGroup[link.b];
                if (groupA != groupB)
                {
                    (int x, int y, int z) itemA = items[link.a];
                    (int x, int y, int z) itemB = items[link.b];
                    foreach (int index in groups[groupB])
                    {
                        groups[groupA].Add(index);
                        indexToGroup[index] = groupA;
                    }
                    if (test)
                        Console.WriteLine($"Linking {itemA.x},{itemA.y},{itemA.z} and {itemB.x},{itemB.y},{itemB.z}, in group: {groups[groupA].Count}");
                    lastLinkIndex = i;
                    groups.Remove(groupB);
                    if (groups.Count < 2)
                        break;
                }
            }
            (int a, int b, double d) lastConnectedLink = links[lastLinkIndex];
            (int x, int y, int z) lastItemA = items[lastConnectedLink.a];
            (int x, int y, int z) lastItemB = items[lastConnectedLink.b];
            
            long result = (long)lastItemA.x * lastItemB.x;
            
            Console.WriteLine($"Part 2: {result}");
        }
        
        
        private static async Task Main(string[] args)
        {
            await Part1();
            await Part2();
        }
    }
}
