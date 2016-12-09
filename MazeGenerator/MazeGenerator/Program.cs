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
        static void Main(string[] args)
        {
            Console.ReadLine();
            Console.SetWindowSize(208, 123);
            while (true)
            {
                /*Console.Write("Largeur: ");
                int width = Convert.ToInt32(Console.ReadLine());

                Console.Write("Hauteur: ");
                int height = Convert.ToInt32(Console.ReadLine());
                Console.Clear();*/

                Maze maze = new Maze(/*width*/103, /*height*/60, Maze.GenerationType.Mixt, 0);

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
