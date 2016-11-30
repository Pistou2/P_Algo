using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            Maze maze = new Maze(100, 55, 1);
            Console.Clear();

            printMaze(maze.maze);
            
            Console.ReadLine();
        }

        public static void printMaze(int[,] _maze, int[] _caseCorrdsForMonoPrint = null)
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
                    }
                }

            }

            #region cases

            bool[] tempCornerWalls = new bool[4];
            string tempCornerText;
            //go through all the cases
            for (int x = 0; x < _maze.GetLength(0); x++)
            {
                for (int y = 0; y < _maze.GetLength(1); y++)
                {
                    //if this is a monoPrint, rewrite only the case in question
                    if (_caseCorrdsForMonoPrint != null)
                    {
                        x = _caseCorrdsForMonoPrint[0];
                        y = _caseCorrdsForMonoPrint[1];
                    }

                    //if the first bit is lit
                    if ((_maze[x, y] & 1) == 0)
                    {
                        //write the top wall
                        Console.SetCursorPosition(x * SIZE + 1, y * SIZE);
                        Console.Write(WALL_STRINGS[0]);
                    }

                    //if the forth bit is lit
                    if ((_maze[x, y] & 8) == 0)
                    {
                        //write the left wall
                        Console.SetCursorPosition(x * SIZE, y * SIZE + 1);
                        Console.Write(WALL_STRINGS[1]);
                    }

                    //if this is the last case of the row, or a monoPrint
                    if (y == _maze.GetLength(1) - 1 | _caseCorrdsForMonoPrint != null)
                    {
                        //sets the bottom wall if needed
                        //if the second bit is lit
                        if ((_maze[x, y] & 2) == 0)
                        {
                            //write the bottom wall
                            Console.SetCursorPosition(x * SIZE + 1, y * SIZE + 2);
                            Console.Write(WALL_STRINGS[0]);
                        }
                    }

                    //if this is the last row, or a monoPrint
                    if (x == _maze.GetLength(0) - 1 | _caseCorrdsForMonoPrint != null)
                    {
                        //sets the right wall if needed

                        //if the third bit is lit
                        if ((_maze[x, y] & 4) == 0)
                        {
                            //write the right wall
                            Console.SetCursorPosition(x * SIZE + 2, y * SIZE + 1);
                            Console.Write(WALL_STRINGS[1]);
                        }
                    }

                    //get out of the fors directly if this is a monoPrint
                    if (_caseCorrdsForMonoPrint != null)
                    {
                        x = _maze.GetLength(0);
                        y = _maze.GetLength(1);
                    }

                }
            }
            #endregion

            #region Case Corners
            int xCorner, yCorner, maxXCorner, maxYCorner;
            //if this is a monoPrint, only handle the corners around the changed case
            if (_caseCorrdsForMonoPrint != null)
            {
                xCorner = _caseCorrdsForMonoPrint[0];
                yCorner = _caseCorrdsForMonoPrint[1];


                if (_maze.GetLength(0) - 1 == xCorner)
                {
                    maxXCorner = xCorner + 1;
                }
                else
                {
                    maxXCorner = xCorner + 2;
                }

                if (_maze.GetLength(1) - 1 == yCorner)
                {
                    maxYCorner = yCorner + 1;
                }
                else
                {
                    maxYCorner = yCorner + 2;
                }

            }
            else
            {
                xCorner = 0;
                yCorner = 0;

                maxXCorner = _maze.GetLength(0);
                maxYCorner = _maze.GetLength(1);
            }


            //go through all the cases
            for (int x = xCorner; x < maxXCorner; x++)
            {
                for (int y = yCorner; y < maxYCorner; y++)
                {

                    if (x > 0 && y > 0)
                    {

                        //TODO
                        Console.SetCursorPosition(x * SIZE, y * SIZE);


                        //set the temp bool (Top, bottom, right, left)
                        tempCornerWalls[0] = ((_maze[x, y - 1] & 8) == 0);
                        tempCornerWalls[1] = ((_maze[x, y] & 8) == 0);
                        tempCornerWalls[2] = ((_maze[x, y] & 1) == 0);
                        tempCornerWalls[3] = ((_maze[x - 1, y] & 1) == 0);

                        #region Char test

                        if (tempCornerWalls[0])
                        #region Top + ?
                        {
                            if (tempCornerWalls[1])
                            #region Top + Bottom + ?
                            {
                                if (tempCornerWalls[2])
                                #region Top + Bottom + Right + ?
                                {
                                    if (tempCornerWalls[3])
                                    #region Top + Bottom + Right + Left
                                    {
                                        tempCornerText = WALL_STRINGS[10];
                                    }
                                    #endregion
                                    else
                                    #region Top + Bottom + Right
                                    {
                                        tempCornerText = WALL_STRINGS[2];
                                    }
                                    #endregion

                                }
                                #endregion
                                else
                                #region Top + Bottom + ?
                                {
                                    if (tempCornerWalls[3])
                                    #region Top + Bottom + Left
                                    {
                                        tempCornerText = WALL_STRINGS[3];
                                    }
                                    #endregion
                                    else
                                    #region Top + Bottom
                                    {
                                        tempCornerText = WALL_STRINGS[1];
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion
                            else
                            #region Top + ?
                            {
                                if (tempCornerWalls[2])
                                #region Top + Right + ?
                                {
                                    if (tempCornerWalls[3])
                                    #region Top + Right + Left
                                    {
                                        tempCornerText = WALL_STRINGS[5];
                                    }
                                    #endregion
                                    else
                                    #region Top + Right
                                    {
                                        tempCornerText = WALL_STRINGS[8];
                                    }
                                    #endregion

                                }
                                #endregion
                                else
                                #region Top + ?
                                {
                                    if (tempCornerWalls[3])
                                    #region Top + Left
                                    {
                                        tempCornerText = WALL_STRINGS[9];
                                    }
                                    #endregion
                                    else
                                    #region Top
                                    {
                                        tempCornerText = WALL_STRINGS[1];
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion
                        }
                        #endregion
                        else
                        #region + ?
                        {
                            if (tempCornerWalls[1])
                            #region Bottom + ?
                            {
                                if (tempCornerWalls[2])
                                #region Bottom + Right + ?
                                {
                                    if (tempCornerWalls[3])
                                    #region Bottom + Right + Left
                                    {
                                        tempCornerText = WALL_STRINGS[4];
                                    }
                                    #endregion
                                    else
                                    #region Bottom + Right
                                    {
                                        tempCornerText = WALL_STRINGS[6];
                                    }
                                    #endregion

                                }
                                #endregion
                                else
                                #region Bottom + ?
                                {
                                    if (tempCornerWalls[3])
                                    #region Bottom + Left
                                    {
                                        tempCornerText = WALL_STRINGS[7];
                                    }
                                    #endregion
                                    else
                                    #region Bottom
                                    {
                                        tempCornerText = WALL_STRINGS[1];
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion
                            else
                            #region + ?
                            {
                                if (tempCornerWalls[2])
                                #region Right + ?
                                {
                                    if (tempCornerWalls[3])
                                    #region Right + Left
                                    {
                                        tempCornerText = WALL_STRINGS[0];
                                    }
                                    #endregion
                                    else
                                    #region Right
                                    {
                                        tempCornerText = WALL_STRINGS[0];
                                    }
                                    #endregion

                                }
                                #endregion
                                else
                                #region + ?
                                {
                                    if (tempCornerWalls[3])
                                    #region Left
                                    {
                                        tempCornerText = WALL_STRINGS[0];
                                    }
                                    #endregion
                                    else
                                    #region NOTHING ?!?!?!??
                                    {
                                        Debug.Fail("LE labyrinthe n'est pas parfait ._.");
                                        tempCornerText = "";
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion
                        }
                        #endregion
                        #endregion

                        Console.Write(tempCornerText);
                    }

                }
            }
            #endregion

            #region Tab Corners
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
                        //check if there's a wall at right to write the correct char

                        if ((_maze[x / SIZE, y / SIZE - 1] & 2) == 0)
                        {
                            Console.Write(WALL_STRINGS[2]);
                        }
                        else
                        {
                            Console.Write(WALL_STRINGS[1]);
                        }
                    }

                    //right walls
                    else if (x == _maze.GetLength(0) * SIZE - 1 && y > 0 && y % 2 == 0)
                    {
                        Console.SetCursorPosition(x + 1, y);
                        //check if there's a wall at left to write the correct char

                        if ((_maze[x / SIZE, y / SIZE - 1] & 2) == 0)
                        {
                            Console.Write(WALL_STRINGS[3]);
                        }
                        else
                        {
                            Console.Write(WALL_STRINGS[1]);
                        }
                    }

                    //top walls
                    else if (y == 0 && x > 0 && x % 2 == 0)
                    {
                        Console.SetCursorPosition(x, y);
                        //check if there's a wall under to write the correct char

                        if ((_maze[x / SIZE, y / SIZE] & 8) == 0)
                        {
                            Console.Write(WALL_STRINGS[4]);
                        }
                        else
                        {
                            Console.Write(WALL_STRINGS[0]);
                        }
                    }

                    //bottom walls
                    else if (y == _maze.GetLength(1) * SIZE - 1 && x > 0 && x % 2 == 0)
                    {

                        Console.SetCursorPosition(x, y + 1);
                        //check if there's a wall on top to write the correct char

                        if ((_maze[x / SIZE, y / SIZE] & 8) == 0)
                        {
                            Console.Write(WALL_STRINGS[5]);
                        }
                        else
                        {
                            Console.Write(WALL_STRINGS[0]);
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Debug Method
        /// </summary>
        public static void printMaze(int[,] _maze, int _lastX, int _lastY)
        {
            //printMaze(_maze);
            Console.SetCursorPosition(_lastX * SIZE + 1, _lastY * SIZE + 1);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
