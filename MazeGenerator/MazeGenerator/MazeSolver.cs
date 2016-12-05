using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeGenerator
{
    partial class Maze
    {        /// <summary>
        /// Résoud un labyrinthe
        /// </summary>
        /// <param name="mazeToSolve">Labyrinthe à résoudre</param>
        /// <returns>List des positions des cases où il faut passer pour résoudre le labyrinthe</returns>
        public static List<object[]> SolveMaze(Maze mazeToSolve, int printTime = 0)
        {
            // Crée une liste où sera stocker les cases de la solution
            List<object[]> solvedMaze = new List<object[]>();
            List<object[]> solvedMaze2 = new List<object[]>();

            // Booléan pour dire s'il le résolution est arrivée au bout du labyrinthe
            bool isEndedRight = false;
            bool isEndedLeft = false;

            // Résoud le labyrinthe
            Thread thRight = new Thread(x => SolveMazeRight(mazeToSolve.maze, Maze.BOTTOM, mazeToSolve.enterDoorPos[0], mazeToSolve.enterDoorPos[1], solvedMaze, ref isEndedRight, printTime));

            Thread thLeft = new Thread(x => SolveMazeLeft(mazeToSolve.maze, Maze.BOTTOM, mazeToSolve.enterDoorPos[0], mazeToSolve.enterDoorPos[1], solvedMaze2, ref isEndedLeft, printTime));

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
        private static void SolveMazeRight(int[,] mazeToSolve, string lastDirection, int currentX, int currentY, List<object[]> solvedList, ref bool isEnded, int printTime)
        {
            // Crée une liste qui contiendra les directions dans l'ordre où il faut regardé
            List<string> directions = new List<string>();

            // Enregistre la direction des prochaines cases suivant la direction comme on est rentré dans la case actuel
            switch (lastDirection)
            {
                case Maze.TOP:
                    directions.Add(Maze.RIGHT);
                    directions.Add(Maze.TOP);
                    directions.Add(Maze.LEFT);
                    break;

                case Maze.BOTTOM:
                    directions.Add(Maze.LEFT);
                    directions.Add(Maze.BOTTOM);
                    directions.Add(Maze.RIGHT);
                    break;

                case Maze.RIGHT:
                    directions.Add(Maze.BOTTOM);
                    directions.Add(Maze.RIGHT);
                    directions.Add(Maze.TOP);
                    break;

                case Maze.LEFT:
                    directions.Add(Maze.TOP);
                    directions.Add(Maze.LEFT);
                    directions.Add(Maze.BOTTOM);
                    break;
            }

            PrintStep(currentX, currentY, lastDirection, ConsoleColor.Cyan);
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
                solvedList.Add(new object[] { currentX, currentY, lastDirection });
                PrintStep(currentX, currentY, lastDirection, ConsoleColor.Gray);
                Thread.Sleep(printTime);
            }

            else
            {
                PrintStep(currentX, currentY, lastDirection, ConsoleColor.DarkCyan);
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
        private static void SolveMazeLeft(int[,] mazeToSolve, string lastDirection, int currentX, int currentY, List<object[]> solvedList, ref bool isEnded, int printTime)
        {
            // Crée une liste qui contiendra les directions dans l'ordre où il faut regardé
            List<string> directions = new List<string>();

            // Enregistre la direction des prochaines cases suivant la direction comme on est rentré dans la case actuel
            switch (lastDirection)
            {
                case Maze.TOP:
                    directions.Add(Maze.LEFT);
                    directions.Add(Maze.TOP);
                    directions.Add(Maze.RIGHT);
                    break;

                case Maze.BOTTOM:
                    directions.Add(Maze.RIGHT);
                    directions.Add(Maze.BOTTOM);
                    directions.Add(Maze.LEFT);
                    break;

                case Maze.RIGHT:
                    directions.Add(Maze.TOP);
                    directions.Add(Maze.RIGHT);
                    directions.Add(Maze.BOTTOM);
                    break;

                case Maze.LEFT:
                    directions.Add(Maze.BOTTOM);
                    directions.Add(Maze.LEFT);
                    directions.Add(Maze.TOP);
                    break;
            }

            PrintStep(currentX, currentY, lastDirection, ConsoleColor.Red);
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
                solvedList.Add(new object[] { currentX, currentY });
                PrintStep(currentX, currentY, lastDirection, ConsoleColor.White);
                Thread.Sleep(printTime);
            }

            else
            {
                PrintStep(currentX, currentY, lastDirection, ConsoleColor.DarkRed);
                Thread.Sleep(printTime);
            }
        }

        public static void ShowSolution(List<object[]> solvedMaze)
        {
            PrintStep((int)solvedMaze[solvedMaze.Count - 1][0], (int)solvedMaze[solvedMaze.Count - 1][1], BOTTOM, ConsoleColor.Yellow);

            for (int i = solvedMaze.Count - 2; i >= 0; i--)
            {
                PrintStep((int)solvedMaze[i][0], (int)solvedMaze[i][1], (string)solvedMaze[i][2], ConsoleColor.Yellow);
            }
        }
    }
}
