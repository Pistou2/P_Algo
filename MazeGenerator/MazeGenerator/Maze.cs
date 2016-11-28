using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    class Maze
    {
        private int width;
        private int height;

        private int[] differenceX = new int[] { 0, 0, 1, -1 };
        private int[] differenceY = new int[] { -1, 1, 0, 0 };

        private int[,] NSEW = new int[,] { { 1, 2, 4, 8 }, { 2, 1, 8, 4 } };

        public int[,] maze;

        Random rnd;

        public Maze(int width, int height)
        {
            this.width = width;
            this.height = height;

            maze = new int[width, height];

            rnd = new Random();

            GenerateMaze(0, 0, maze);
        }

        private void GenerateMaze(int lastX, int lastY, int[,] mazeGenerated, bool mustDrawEntry = true)
        {
            List<int> directions = new List<int> { 0, 1, 2, 3 };

            while (directions.Count != 0)
            {
                int randomDir = rnd.Next(directions.Count);
                int nextX = lastX + differenceX[directions[randomDir]];
                int nextY = lastY + differenceY[directions[randomDir]];

                if (nextX >= 0 && nextX < width && nextY >= 0 && nextY < height && mazeGenerated[nextX, nextY] == 0)
                {
                    mazeGenerated[lastX, lastY] |= NSEW[0,directions[randomDir]];

                    mazeGenerated[nextX, nextY] |= NSEW[1, directions[randomDir]];

                    GenerateMaze(nextX, nextY, mazeGenerated, false);
                }

                directions.RemoveAt(randomDir);
            }

            if (mustDrawEntry)
            {

            }
        }

        private void PrintMaze(int mazePositionX, int mazePositionY, int[,] mazeToShow)
        {
            Console.Write(" ");

            for (int i = 0; i < width * 2 - 1; i++)
            {
                Console.Write("_");
            }

            Console.WriteLine();

            for (int y = 0; y < height; y++)
            {
                Console.Write("|");

                for (int x = 0; x < width; x++)
                {
                    Console.Write(((mazeToShow[y, x] & 2) != 0) ? " " : "_");

                    if ((maze[x, y] & 4) != 0)
                        if (x + 1 < mazeToShow.GetLength(1))
                            Console.Write(((mazeToShow[y, x] | mazeToShow[y, x + 1]) & 2) != 0 ? " " : "_");
                        else
                            Console.Write(((mazeToShow[y, x]) & 2) != 0 ? " " : "_");
                    else
                        Console.Write("|");
                }

                Console.WriteLine();
            }
        }
    }
}
