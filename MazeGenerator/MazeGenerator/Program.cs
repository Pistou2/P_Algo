using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.ReadLine();
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            Console.Write("Largeur: ");
            int width = Convert.ToInt32(Console.ReadLine());

            Console.Write("Hauteur: ");
            int height = Convert.ToInt32(Console.ReadLine());

            Console.Write("Type: ");
            int type = Convert.ToInt32(Console.ReadLine());

            Console.Clear();

            while (true)
            {
                Maze maze = new Maze(width, height, (Maze.GenerationType)type, 10);

                Console.Clear();

                Maze.PrintMaze(maze.maze);

                Maze.SolveMaze(maze, 0);

                //Maze.ShowSolution(maze, ConsoleColor.Red, 100);

                //Thread.Sleep(5000);

                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
