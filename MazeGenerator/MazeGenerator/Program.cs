using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    // 1 1 1 1
    class Program
    {
        public/*const x.x*/ static string[] WALL_STRINGS = new string[] { "─", "│", "├", "┤", "┬", "┴", "┌", "┐", "└", "┘", "┼" };

        /// <summary>
        /// const used to display the maze, minimum value = 2
        /// </summary>
        public const int SIZE = 2;


        static void Main(string[] args)
        {
            Maze maze = new Maze(10, 10);
            int[,] temp = new int[,] { { 15, 15 }, { 15, 15 }, { 15, 15 } };

            Console.Clear();

            printMaze(temp);
            Console.ReadLine();
        }

        public static void printMaze(int[,] _maze)
        {
            #region cases
            //go through all the cases
            for (int x = 0; x < _maze.GetLength(0); x++)
            {
                for (int y = 0; y < _maze.GetLength(1); y++)
                {
                    //if the first bit is lit
                    if ((_maze[x, y] & 1) == 1)
                    {
                        //write the top wall
                        Console.SetCursorPosition(x * SIZE + 1, y * SIZE);
                        Console.Write(WALL_STRINGS[0]);
                    }

                    //if the forth bit is lit
                    if ((_maze[x, y] & 8) == 8)
                    {
                        //write the front wall
                        Console.SetCursorPosition(x * SIZE, y * SIZE + 1);
                        Console.Write(WALL_STRINGS[1]);
                    }

                    //if this is the last case of the row
                    if (y == _maze.GetLength(1) - 1)
                    {
                        //sets the bottom wall if needed
                        //if the second bit is lit
                        if ((_maze[x, y] & 2) == 2)
                        {
                            //write the bottom wall
                            Console.SetCursorPosition(x * SIZE + 1, y * SIZE + 2);
                            Console.Write(WALL_STRINGS[0]);
                        }
                    }

                    //if this is the last row
                    if (x == _maze.GetLength(0) - 1)
                    {
                        //sets the right wall if needed

                        //if the third bit is lit
                        if ((_maze[x, y] & 4) == 4)
                        {
                            //write the right wall
                            Console.SetCursorPosition(x * SIZE + 2, y * SIZE + 1);
                            Console.Write(WALL_STRINGS[1]);
                        }
                    }
                }
            }
            #endregion

            #region Corners
            //write all the corners
            Console.SetCursorPosition(0, 0);
            Console.Write(WALL_STRINGS[6]);

            Console.SetCursorPosition(_maze.GetLength(0) * SIZE, 0);
            Console.Write(WALL_STRINGS[7]);

            Console.SetCursorPosition(0, _maze.GetLength(1) * SIZE);
            Console.Write(WALL_STRINGS[8]);

            Console.SetCursorPosition(_maze.GetLength(0) * SIZE, _maze.GetLength(1) * SIZE);
            Console.Write(WALL_STRINGS[9]);

            #endregion

            #region Borders
            //go thgough all the visible chars
            for (int x = 0; x < _maze.GetLength(0) * SIZE; x++)
            {
                for (int y = 0; y < _maze.GetLength(1) * SIZE; y++)
                {
                    //check what string need to be wroten
                    //left walls                   
                    if (x == 0 && y > 0 && y % 2 == 0)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(WALL_STRINGS[2]);
                    }

                    //right walls
                    else if (x == _maze.GetLength(0) * SIZE - 1 && y > 0 && y % 2 == 0)
                    {
                        Console.SetCursorPosition(x + 1, y);
                        Console.Write(WALL_STRINGS[3]);
                    }

                    //top walls
                    else if (y == 0 && x > 0 && x % 2 == 0)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(WALL_STRINGS[4]);
                    }

                    //bottom walls
                    else if (y == _maze.GetLength(1) * SIZE - 1 && x > 0 && x % 2 == 0)
                    {
                        Console.SetCursorPosition(x, y + 1);
                        Console.Write(WALL_STRINGS[5]);
                    }
                    //corner
                    else if (x > 0 && y > 0 && x < _maze.GetLength(0) * SIZE - 1 && y < _maze.GetLength(1) * SIZE - 1 && x % 2 == 0 && y % 2 == 0)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(WALL_STRINGS[10]);
                    }
                }
            }
            #endregion
        }
    }
}
