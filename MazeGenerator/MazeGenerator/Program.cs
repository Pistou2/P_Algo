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





        static void Main(string[] args)
        {
            Console.ReadLine();
            Console.SetWindowSize(200, 123);
            while (true)
            {
                /*Console.Write("Largeur: ");
                int width = Convert.ToInt32(Console.ReadLine());

                Console.Write("Hauteur: ");
                int height = Convert.ToInt32(Console.ReadLine());*/
                Console.Clear();

                Maze maze = new Maze(/*width*/49, /*height*/49, Maze.GenerationType.Mixt, 2);

                Console.Clear();

                Maze.PrintMaze(maze.maze, null);

                //Maze.SolveMaze(maze);

                //Thread.Sleep(5000);
                
                Console.ReadLine();
            }
        }
    }
}
