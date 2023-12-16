using AoC.Common;

var data = new List<string>();
var grids = new List<Grid<char>>();
foreach (var row in File.ReadAllLines("input.txt").Append(""))
{
    if (string.IsNullOrEmpty(row))
    {
        grids.Add(Grid.FromRows(data, c => c));
        data.Clear();
        continue;
    }

    data.Add(row);
}

var value1 = 0;
var value2 = 0;
var gridCount = 0;
foreach (var grid in grids)
{
    gridCount++;
    foreach (var part2 in new[] { false, true })
    {
        var smudgeFound = false;

        bool IsEqual(int index1, int index2, Func<int, int, bool> exactComparer, Func<int, int, bool> fuzzyComparer)
        {
            if ((!part2 || smudgeFound) && exactComparer(index1, index2))
            {
                return true;
            }

            if (part2 && fuzzyComparer(index1, index2))
            {
                smudgeFound = true;
                return true;
            }

            return false;
        }
        var limit = Math.Max(grid.Width, grid.Height);
        var found = false;

        smudgeFound = false;
        var reflectionRow = -10;
        var reflectionCol = -10;

        for (var rowOrColumn = 0; rowOrColumn < limit; rowOrColumn++)
        {
            if (found)
            {
                break;
            }

            var shouldTestColumn = rowOrColumn < grid.Width - 1;

            if (shouldTestColumn && IsEqual(rowOrColumn, rowOrColumn + 1, grid.CompareColumns, (x1, x2) => grid.CountColumnsEqual(x1, x2) == grid.Height - 1))
            {
                var r1 = rowOrColumn;
                var r2 = grid.Width - (rowOrColumn + 2);
                var steps = Math.Min(r1, r2);
                var eq = true;

                for (var offset = 0; offset < steps; offset++)
                {
                    var o1 = rowOrColumn - 1 - offset;
                    var o2 = rowOrColumn + 2 + offset;

                    if (!IsEqual(o1, o2, grid.CompareColumns, (x1, x2) => grid.CountColumnsEqual(x1, x2) == grid.Height - 1))
                    {
                        eq = false;
                        break;
                    }
                }

                if (eq)
                {
                    reflectionCol = rowOrColumn;
                    if (!part2)
                    {
                        value1 += rowOrColumn + 1;
                    }
                    else
                    {
                        value2 += rowOrColumn + 1;
                    }
                    found = true;
                }
            }

            var shouldTestRow = !found && rowOrColumn < grid.Height - 1;

            if (shouldTestRow && IsEqual(rowOrColumn, rowOrColumn + 1, grid.CompareRows, (x1, x2) => grid.CountRowsEqual(x1, x2) == grid.Width - 1))
            {
                var r1 = rowOrColumn;
                var r2 = grid.Height - (rowOrColumn + 2);
                var steps = Math.Min(r1, r2);
                var eq = true;

                for (var offset = 0; offset < steps; offset++)
                {
                    var o1 = rowOrColumn - 1 - offset;
                    var o2 = rowOrColumn + 2 + offset;

                    if (!IsEqual(o1, o2, grid.CompareRows, (x1, x2) => grid.CountRowsEqual(x1, x2) == grid.Width - 1))
                    {
                        eq = false;
                        break;
                    }
                }

                if (eq)
                {
                    reflectionRow = rowOrColumn;
                    if (!part2)
                    {
                        value1 += (rowOrColumn + 1) * 100;
                    }
                    else
                    {
                        value2 += (rowOrColumn + 1) * 100;
                    }
                    found = true;
                }
            }
        }

        Console.WriteLine($" Grid {gridCount} -- {(part2 ? "2" : "1")}");
        Console.Write("   ");
        for (var col = 0; col < grid.Width; col++)
        {
            if (col == reflectionCol)
            {
                Console.Write("<< ");
            }
            else if (col == reflectionCol + 1)
            {
                Console.Write(">> ");
            }
            else
            {
                Console.Write($"{col + 1:00} ");
            }
        }
        Console.WriteLine();
        for (var row = 0; row < grid.Height; row++)
        {
            var sigil = row == reflectionRow ? "^^" : row == reflectionRow + 1 ? "vv" : $"{row + 1:00}";
            Console.WriteLine($" {sigil} {string.Join("  ", grid.GetRow(row))}");
        }
        Console.WriteLine();
    }

    Console.WriteLine();
}

Render.Result("Part 1", value1);
Render.Result("Part 2", value2);
