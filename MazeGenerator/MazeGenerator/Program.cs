using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeGenerator
{
    // 1 1 1 1
    class Program
    {
        public/*const x.x*/ static string[] WALL_STRINGS = new string[] { "─", "│", "├", "┤", "┬", "┴", "┌", "┐", "└", "┘", "┼" };

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

        static Mutex mut = new Mutex();


        static void Main(string[] args)
        {
            Console.ReadLine();
            Console.SetWindowSize(208, 123);
            int number = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < number; i++)
            {
                /*Console.Write("Largeur: ");
                int width = Convert.ToInt32(Console.ReadLine());

                Console.Write("Hauteur: ");
                int height = Convert.ToInt32(Console.ReadLine());*/
                Console.Clear();

                Maze maze = new Maze(/*width*/50, /*height*/50, 10);

                Console.Clear();

                printMaze(maze.maze, null);

                MazeSolver.SolveMaze(maze.maze);

                //Thread.Sleep(5000);

                //MazeSolver.ShowSolution(MazeSolver.SolveMaze(maze.maze));

                Console.ReadLine();
            }
            Console.ReadLine();
        }

        public static void printMaze(int[,] _maze, int[] _caseCorrdsForMonoPrint = null, int printTime = 0)
        {
            //check if this is a monoPrint (only generate 1 case)
            if (_caseCorrdsForMonoPrint != null)
            {
                //clear the case
                for (int x = 0; x <= SIZE; x++)
                {
                    for (int y = 0; y <= SIZE; y++)
                    {
                        Console.SetCursorPosition(_caseCorrdsForMonoPrint[0] * SIZE + x, _caseCorrdsForMonoPrint[1] * SIZE + y);
                        Console.Write(" ");
                        //Slow print if nedded
                    }
                }

                ShowCase(_maze, _caseCorrdsForMonoPrint[0], _caseCorrdsForMonoPrint[1]);

                if (_caseCorrdsForMonoPrint[0] < _maze.GetLength(0) - 1 && _caseCorrdsForMonoPrint[1] < _maze.GetLength(1) - 1)
                {
                    ShowCase(_maze, _caseCorrdsForMonoPrint[0] + 1, _caseCorrdsForMonoPrint[1] + 1);
                }

                if (_caseCorrdsForMonoPrint[0] < _maze.GetLength(0) - 1)
                {
                    ShowCase(_maze, _caseCorrdsForMonoPrint[0] + 1, _caseCorrdsForMonoPrint[1]);
                }

                if (_caseCorrdsForMonoPrint[1] < _maze.GetLength(1) - 1)
                {
                    ShowCase(_maze, _caseCorrdsForMonoPrint[0], _caseCorrdsForMonoPrint[1] + 1);
                }

                Thread.Sleep(printTime);
            }

            else
            {
                //go through all the cases
                for (int x = 0; x < _maze.GetLength(0); x++)
                {
                    for (int y = 0; y < _maze.GetLength(1); y++)
                    {
                        ShowCase(_maze, x, y);
                    }
                }
            }
        }

        private static void ShowCase (int[,] _maze, int posX, int posY)
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
        public static void printMaze(/*int[,] _maze,*/ int _lastX, int _lastY, ConsoleColor color)
        {
            mut.WaitOne();
            //printMaze(_maze);
            Console.SetCursorPosition(_lastX * SIZE + 1, _lastY * SIZE + 1);
            Console.BackgroundColor = color;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            mut.ReleaseMutex();
        }
    }
}
