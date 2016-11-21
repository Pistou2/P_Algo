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

        private int[] differenceX = new int[] { 1, -1, 0, 0 };
        private int[] differenceY = new int[] { 0, 0, 1, -1 };

        private int[,] maze;

        Random rnd;

        public Maze (int width, int height)
        {
            this.width = width;
            this.height = height;

            maze = new int[width, height];

            rnd = new Random();

            GenerateMaze(0, 0, maze);
            PrintMaze(0, 0, maze);
        }

        private void GenerateMaze(int lastX, int lastY, int[,] mazeGenerated)
        {
            List<int> directions = new List<int> { 0, 1, 2, 3 };

            while (directions.Count != 0)
            {
                int randomDir = rnd.Next(directions.Count);
                int nextX = lastX + differenceX[directions[randomDir]];
                int nextY = lastY + differenceY[directions[randomDir]];

                if (nextX >= 0 && nextX < width && nextY >= 0 && nextY < height && mazeGenerated[nextX, nextY] == 0)
                {
                    mazeGenerated[lastX, lastY] |= Convert.ToInt32(Math.Pow(2, directions[randomDir]));
                    mazeGenerated[nextX, nextY] |= Convert.ToInt32(Math.Pow(2, 3 - directions[randomDir]));
                    GenerateMaze(nextX, nextY, mazeGenerated);
                }

                directions.RemoveAt(randomDir);
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
                    Console.Write(((mazeToShow[x, y] & 2) != 0) ? " " : "_");

                    if ((maze[x, y] & 4) != 0)
                        Console.Write(((mazeToShow[x, y] | mazeToShow[x, y]) & 2) != 0 ? " " : "_");
                    else
                        Console.Write("|");
                }

                Console.WriteLine();
            }
        }
    }
}
