using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeGenerator
{
    partial class Maze
    {
        static Mutex mut = new Mutex();

        public static Dictionary<int, string> WALL = new Dictionary<int, string>()
        {
            { 14,"─" },
            { 13,"─" },
            { 12,"─" },
            { 11,"│" },
            { 10,"└" },
            { 9, "┘" },
            { 8, "┴" },
            { 7, "│" },
            { 6, "┌" },
            { 5, "┐" },
            { 4, "┬" },
            { 3, "│" },
            { 2, "├" },
            { 1, "┤" },
            { 0, "┼" }
        };

        /// <summary>
        /// const used to display the maze, minimum value = 2
        /// </summary>
        public const int SIZE = 2;

        private static void ShowCase(int[,] _maze, int posX, int posY)
        {
            int tmp;

            //if the first bit is lit
            if ((_maze[posX, posY] & 1) == 0)
            {
                //write the top wall
                Console.SetCursorPosition(posX * SIZE + 1, posY * SIZE);
                Console.Write(WALL[12]);
            }

            //if the forth bit is lit
            if ((_maze[posX, posY] & 8) == 0)
            {
                //write the left wall
                Console.SetCursorPosition(posX * SIZE, posY * SIZE + 1);
                Console.Write(WALL[3]);
            }

            Console.SetCursorPosition(posX * SIZE, posY * SIZE);

            if (posX > 0 && posY > 0)
            {
                tmp = (_maze[posX, posY] & 9) + (_maze[posX - 1, posY - 1] & 6);

                Console.Write(WALL[tmp]);
            }

            else if (posX == 0 && posY == 0)
            {
                Console.Write(WALL[6]);
            }

            else if (posX == 0)
            {
                tmp = (_maze[posX, posY] & 9) + (_maze[posX, posY - 1] & 8) / 2 + 2;

                Console.Write(WALL[tmp]);
            }

            else if (posY == 0)
            {
                tmp = (_maze[posX, posY] & 9) + (_maze[posX - 1, posY] & 1) * 2 + 4;

                Console.Write(WALL[tmp]);
            }


            //if this is the last case of the row
            if (posY == _maze.GetLength(1) - 1)
            {
                //sets the bottom wall if needed
                //if the second bit is lit
                if ((_maze[posX, posY] & 2) == 0)
                {
                    //write the bottom wall
                    Console.SetCursorPosition(posX * SIZE + 1, posY * SIZE + 2);
                    Console.Write(WALL[12]);
                }

                Console.SetCursorPosition(posX * SIZE, posY * SIZE + 2);

                if (posX == 0)
                {
                    Console.Write(WALL[10]);
                }

                else
                {
                    tmp = (_maze[posX, posY] & 2) / 2 + (_maze[posX - 1, posY] & 6) + 8;

                    Console.Write(WALL[tmp]);
                }
            }

            //if this is the last row
            if (posX == _maze.GetLength(0) - 1)
            {
                //sets the right wall if needed

                //if the third bit is lit
                if ((_maze[posX, posY] & 4) == 0)
                {
                    //write the right wall
                    Console.SetCursorPosition(posX * SIZE + 2, posY * SIZE + 1);
                    Console.Write(WALL[3]);
                }

                Console.SetCursorPosition(posX * SIZE + 2, posY * SIZE);

                if (posY == 0)
                {
                    Console.Write(WALL[5]);
                }

                else
                {
                    tmp = (_maze[posX, posY] & 4) * 2 + (_maze[posX, posY - 1] & 6) + 1;
                    Console.Write(WALL[tmp]);
                }

                if (posY == _maze.GetLength(1) - 1)
                {
                    Console.SetCursorPosition(posX * SIZE + 2, posY * SIZE + 2);
                    Console.Write(WALL[9]);
                }
            }
        }

        /// <summary>
        /// Debug Method
        /// </summary>
        public static void PrintStep(int _x, int _y, string direction, ConsoleColor color)
        {
            mut.WaitOne();

            Console.BackgroundColor = color;
            //write the step
            Console.SetCursorPosition(_x * SIZE + 1, _y * SIZE + 1);
            Console.Write(" ");

            //Write the middle step
            _x = _x * SIZE + 1;
            _y = _y * SIZE + 1;



            Console.SetCursorPosition(_x + differenceX[REVERT_TBRL[direction]], _y + differenceY[REVERT_TBRL[direction]]);
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            mut.ReleaseMutex();
        }
    }
}
