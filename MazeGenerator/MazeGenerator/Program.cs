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


            Console.ReadLine();
        }

        public static void printMaze(int[,] _maze)
        {
            Console.SetCursorPosition(0, 0);
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
                        Console.SetCursorPosition(x  * SIZE , y * SIZE + 1);
                        Console.Write(WALL_STRINGS[1]);
                    }

                    //if this is the last case of the row
                    if(y == _maze.GetLength(1) - 1)
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

        }
    }
}
