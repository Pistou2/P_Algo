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
                int topDoorPos = rnd.Next(width);

                int bottomDoorPod = rnd.Next(width);

                mazeGenerated[topDoorPos, 0] |= NSEW[0, 0];
                mazeGenerated[bottomDoorPod, height - 1] |= NSEW[1, 0];
            }
        }
    }
}
