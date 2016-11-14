using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.ReadLine();
        }

        public static void printMaze(bool[] _horizontalDoors, bool[] _verticalDoors)
        {
            //horizontal top and bottom lines
            Console.SetCursorPosition(0, 0);
            for(int i = 0; i < _horizontalDoors.GetLength(0); i++)
            {
                Console.Write("_");
            }
        }
    }
}
