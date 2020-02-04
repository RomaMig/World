using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    static class Utilites
    {
        public static int[,] DFS<T>(int width, int height, T[,] obj, Predicate<T> predicate)
        {
            int[,] matrix = new int[width, height];
            int color = 1;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (predicate(obj[i, j]) && matrix[i, j] == 0)
                    {
                        dfs(i, j, width, height, color, matrix, obj, predicate);
                        color++;
                    }

                }
            }
            return matrix;
        }
        private static void dfs<T>(int i, int j, int width, int height, int color, int[,] visited, T[,] obj, Predicate<T> predicate)
        {
            visited[i, j] = color;
            for (int n = i - 1; n < i + 2; n++)
            {
                for (int m = j - 1; m < j + 2; m++)
                {
                    if (n > -1 && n < width && m > -1 && m < height && n != i & m != j && visited[n, m] == 0 && predicate(obj[n, m]))
                    {
                        dfs(n, m, width, height, color, visited, obj, predicate);
                    }
                }
            }
        }
    }
}
