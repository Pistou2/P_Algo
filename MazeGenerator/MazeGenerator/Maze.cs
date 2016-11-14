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

        private bool[,] visitedRoom;
        private bool[,] verticalDoors;
        private bool[,] horizontalDoors;

        Random rnd;

        public Maze (int width, int height)
        {
            this.width = width;
            this.height = height;

            visitedRoom = new bool[width, height];
            verticalDoors = new bool[width - 1, height];
            horizontalDoors = new bool[width, height - 1];

            rnd = new Random();
        }

        private void GenerateMaze(bool[,] visitedRoom, int[] lastPosition)
        {

        }
    }
}
