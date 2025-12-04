using AdventOfCode.Core;
using AdventOfCode.Core.Helpers;

namespace AdventOfCode.Day04
{
    internal static class Program
    {
        private static Task<Field<bool>> ReadField(bool test = false)
        {
            return InputFileHelper.GetField<bool>(04, test);
        }

        private static void RenderField(Field<bool> field)
        {
            for (int y = 0; y < field.Height; ++y)
            {
                for (int x = 0; x < field.Width; ++x)
                {
                    Console.Write(field.GetCustomData(x, y) is true ? 'x' : field.Get(x, y));
                }
                Console.WriteLine();
            }
        }
        
        
        private static async Task Part1(bool test = false)
        {
            Field<bool> field = await ReadField(test);
            int takable = 0;
            for (int x = 0; x < field.Width; ++x)
            {
                for (int y = 0; y < field.Height; ++y)
                {
                    if (field.Get(x, y) == '@')
                    {
                        char[] around =
                        [
                            field.Get(x - 1, y - 1),
                            field.Get(x, y - 1),
                            field.Get(x + 1, y - 1),
                            
                            field.Get(x - 1, y + 1),
                            field.Get(x, y + 1),
                            field.Get(x + 1, y + 1),

                            field.Get(x - 1, y),
                            field.Get(x + 1, y)
                        ];
                        int amount = around.Count(c => c == '@');
                        if (amount < 4)
                        {
                            ++takable;
                            field.SetCustomData(x, y, true);
                        }
                        else
                        {
                            field.SetCustomData(x, y, false);
                        }
                    }
                }
            }
            RenderField(field);
            Console.WriteLine();
            Console.WriteLine($"Part 1: {takable}");
            Console.WriteLine();
        }

        private static async Task Part2(bool test = false)
        {
            Field<bool> field = await ReadField(test);
            int localRemoved;
            long totalRemoved = 0;
            Field<bool> nextField = field.Clone();
            do
            {
                localRemoved = 0;
                for (int x = 0; x < field.Width; ++x)
                {
                    for (int y = 0; y < field.Height; ++y)
                    {
                        if (field.Get(x, y) == '@')
                        {
                            char[] around =
                            [
                                field.Get(x - 1, y - 1),
                                field.Get(x, y - 1),
                                field.Get(x + 1, y - 1),
                            
                                field.Get(x - 1, y + 1),
                                field.Get(x, y + 1),
                                field.Get(x + 1, y + 1),

                                field.Get(x - 1, y),
                                field.Get(x + 1, y)
                            ];
                            int amount = around.Count(c => c == '@');
                            if (amount < 4)
                            {
                                ++localRemoved;
                                nextField.Set(x,  y, 'x');
                            }
                        }
                    }
                }
                field = nextField;
                nextField = field.Clone();
                Console.WriteLine($"Removed: {localRemoved}");
                totalRemoved += localRemoved;
            } while (localRemoved > 0);
            
            Console.WriteLine($"Part 2: {totalRemoved}");
        }
        
        
        private static async Task Main(string[] args)
        {
            await Part1();
            await Part2();
        }
    }
}
