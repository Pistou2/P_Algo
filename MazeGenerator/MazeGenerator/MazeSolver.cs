using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeGenerator
{
    class MazeSolver
    {        /// <summary>
        /// Résoud un labyrinthe
        /// </summary>
        /// <param name="mazeToSolve">Labyrinthe à résoudre</param>
        /// <returns>List des positions des cases où il faut passer pour résoudre le labyrinthe</returns>
        public static List<int[]> SolveMaze(int[,] mazeToSolve, int printTime = 0)
        {
            // Initialise une variable pour avoir la case où il y a l'entrée
            int firstPositionX = -1;

            // Regarde chaque case du haut jusqu'à ce qu'il trouve l'entrée
            for (int i = 0; i < mazeToSolve.GetLength(0) && firstPositionX == -1; i++)
            {
                // Regarde si c'est la case avec l'entrée
                if ((mazeToSolve[i, 0] & Maze.TBRL[Maze.TOP]) == Maze.TBRL[Maze.TOP])
                {
                    // Enregistre la position de l'entrée
                    firstPositionX = i;
                }
            }

            // Crée une liste où sera stocker les cases de la solution
            List<int[]> solvedMaze = new List<int[]>();

            // Booléan pour dire s'il le résolution est arrivée au bout du labyrinthe
            bool isEndedRight = false;
            bool isEndedLeft = false;

            // Résoud le labyrinthe
            Thread thRight = new Thread(x => SolveMazeRight(mazeToSolve, Maze.BOTTOM, firstPositionX, 0, solvedMaze, ref isEndedRight, printTime));

            Thread thLeft = new Thread(x => SolveMazeLeft(mazeToSolve, Maze.BOTTOM, firstPositionX, 0, solvedMaze, ref isEndedLeft, printTime));

            thRight.Start();
            thLeft.Start();


            while (thRight.IsAlive || thLeft.IsAlive) { }

            // Retourne la liste
            return solvedMaze;
        }

        /// <summary>
        /// Résoud un labyrinthe
        /// </summary>
        /// <param name="mazeToSolve">Labyrinthe à résoudre</param>
        /// <param name="lastDirection">La direction dans laquelle on est rentré dans la case</param>
        /// <param name="currentX">La position horizontale de la case actuel</param>
        /// <param name="currentY">La position verticale de la case actuel</param>
        /// <param name="solvedList">List avec les positions de solution</param>
        /// <param name="isEnded">Si on est arrivé à la fin du labyrinthe</param>
        private static void SolveMazeRight(int[,] mazeToSolve, string lastDirection, int currentX, int currentY, List<int[]> solvedList, ref bool isEnded, int printTime)
        {
            count++;

            // Crée une liste qui contiendra les directions dans l'ordre où il faut regardé
            List<string> directions = new List<string>();

            //Utilisé pour le printStep
            int lastX = 0;
            int lastY = 0;

            // Enregistre la direction des prochaines cases suivant la direction comme on est rentré dans la case actuel
            switch (lastDirection)
            {
                case Maze.TOP:
                    directions.Add(Maze.RIGHT);
                    directions.Add(Maze.TOP);
                    directions.Add(Maze.LEFT);
                    lastY = 1;
                    break;

                case Maze.BOTTOM:
                    directions.Add(Maze.LEFT);
                    directions.Add(Maze.BOTTOM);
                    directions.Add(Maze.RIGHT);
                    lastY = -1;
                    break;

                case Maze.RIGHT:
                    directions.Add(Maze.BOTTOM);
                    directions.Add(Maze.RIGHT);
                    directions.Add(Maze.TOP);
                    lastX = -1;
                    break;

                case Maze.LEFT:
                    directions.Add(Maze.TOP);
                    directions.Add(Maze.LEFT);
                    directions.Add(Maze.BOTTOM);
                    lastX = 1;
                    break;
            }

            Program.printStep(currentX, currentY, lastX, lastY, ConsoleColor.Cyan);
            Thread.Sleep(printTime);

            // Tant qu'il n'a pas regardé toutes les directions et qu'il n'a pas atteint la fin
            while (directions.Count != 0 && !isEnded)
            {
                if ((mazeToSolve[currentX, currentY] & Maze.TBRL[directions[0]]) == Maze.TBRL[directions[0]])
                {
                    int nextX = currentX + Maze.differenceX[directions[0]];
                    int nextY = currentY + Maze.differenceY[directions[0]];

                    if (nextX >= 0 && nextX < mazeToSolve.GetLength(0) && nextY >= 0 && nextY < mazeToSolve.GetLength(1))
                    {
                        SolveMazeRight(mazeToSolve, directions[0], nextX, nextY, solvedList, ref isEnded, printTime);
                    }

                    else
                    {
                        isEnded = true;
                    }
                }

                directions.RemoveAt(0);
            }

            if (isEnded)
            {
                solvedList.Add(new int[] { currentX, currentY });
                Program.printStep(currentX, currentY, lastX, lastY, ConsoleColor.Gray);
                Thread.Sleep(printTime);
            }

            else
            {
                Program.printStep(currentX, currentY, lastX, lastY, ConsoleColor.DarkCyan);
                Thread.Sleep(printTime);
            }
        }

        /// <summary>
        /// Résoud un labyrinthe
        /// </summary>
        /// <param name="mazeToSolve">Labyrinthe à résoudre</param>
        /// <param name="lastDirection">La direction dans laquelle on est rentré dans la case</param>
        /// <param name="currentX">La position horizontale de la case actuel</param>
        /// <param name="currentY">La position verticale de la case actuel</param>
        /// <param name="solvedList">List avec les positions de solution</param>
        /// <param name="isEnded">Si on est arrivé à la fin du labyrinthe</param>
        private static void SolveMazeLeft(int[,] mazeToSolve, string lastDirection, int currentX, int currentY, List<int[]> solvedList, ref bool isEnded, int printTime)
        {
            count++;

            // Crée une liste qui contiendra les directions dans l'ordre où il faut regardé
            List<string> directions = new List<string>();

            //utilisé pour le printStep
            int lastX = 0;
            int lastY = 0;

            // Enregistre la direction des prochaines cases suivant la direction comme on est rentré dans la case actuel
            switch (lastDirection)
            {
                case Maze.TOP:
                    directions.Add(Maze.LEFT);
                    directions.Add(Maze.TOP);
                    directions.Add(Maze.RIGHT);
                    lastY = 1;
                    break;

                case Maze.BOTTOM:
                    directions.Add(Maze.RIGHT);
                    directions.Add(Maze.BOTTOM);
                    directions.Add(Maze.LEFT);
                    lastY = -1;
                    break;

                case Maze.RIGHT:
                    directions.Add(Maze.TOP);
                    directions.Add(Maze.RIGHT);
                    directions.Add(Maze.BOTTOM);
                    lastX = -1;
                    break;

                case Maze.LEFT:
                    directions.Add(Maze.BOTTOM);
                    directions.Add(Maze.LEFT);
                    directions.Add(Maze.TOP);
                    lastX = 1;
                    break;
            }

            Program.printStep(currentX, currentY, lastX, lastY, ConsoleColor.Red);
            Thread.Sleep(printTime);

            // Tant qu'il n'a pas regardé toutes les directions et qu'il n'a pas atteint la fin
            while (directions.Count != 0 && !isEnded)
            {
                if ((mazeToSolve[currentX, currentY] & Maze.TBRL[directions[0]]) == Maze.TBRL[directions[0]])
                {
                    int nextX = currentX + Maze.differenceX[directions[0]];
                    int nextY = currentY + Maze.differenceY[directions[0]];

                    if (nextX >= 0 && nextX < mazeToSolve.GetLength(0) && nextY >= 0 && nextY < mazeToSolve.GetLength(1))
                    {
                        SolveMazeLeft(mazeToSolve, directions[0], nextX, nextY, solvedList, ref isEnded, printTime);
                    }

                    else
                    {
                        isEnded = true;
                    }
                }

                directions.RemoveAt(0);
            }

            if (isEnded)
            {
                solvedList.Add(new int[] { currentX, currentY });
                Program.printStep(currentX, currentY, lastX, lastY, ConsoleColor.White);
                Thread.Sleep(printTime);
            }

            else
            {
                Program.printStep(currentX, currentY, lastX, lastY, ConsoleColor.DarkRed);
                Thread.Sleep(printTime);
            }
        }

        //public static void ShowSolution(List<int[]> solvedMaze)
        //{
        //    for (int i = solvedMaze.Count - 1; i >= 0; i--)
        //    {
        //        Program.printStep(solvedMaze[i][0], solvedMaze[i][1], ConsoleColor.Yellow);
        //    }
        //}
    }
}
