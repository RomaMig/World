using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    static class Utilites
    {
        public static int[,] DFS<T>(T[,] obj, Predicate<T> predicate)
        {
            int width = obj.GetUpperBound(0) + 1;
            int height = obj.Length / width;
            int[,] matrix = new int[width, height];
            int color = 1;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //matrix[i, j] = predicate(obj[i, j]) ? 1 : 0;

                    if (predicate(obj[i, j]) && matrix[i, j] == 0)
                    {
                        try
                        {
                            dfs(i, j, width, height, color, matrix, obj, predicate, 0);
                        }
                        catch (StackOverflowException e)
                        {
                            Console.WriteLine(e.Message);
                            continue;
                        }
                        color++;
                    }
                }
            }
            return matrix;
        }

        private static void dfs<T>(int i, int j, int width, int height, int color, int[,] visited, T[,] obj, Predicate<T> predicate, int deep)
        {
            if (deep > 200) throw new StackOverflowException("Stack overflow");
            if (visited[i, j] != 0) return;
            visited[i, j] = color;
            for (int n = i - 1; n < i + 2; n++)
            {
                for (int m = j - 1; m < j + 2; m++)
                {
                    if (n > -1 && n < width && m > -1 && m < height)
                    {
                        if (visited[n, m] == 0 && predicate(obj[n, m]))
                        {
                            dfs(n, m, width, height, color, visited, obj, predicate, ++deep);
                        }
                    }
                }
            }
        }
    }
}
