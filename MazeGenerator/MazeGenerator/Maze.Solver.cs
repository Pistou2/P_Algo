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
        public static List<object[]> SolveMaze(Maze mazeToSolve, int? printTime = null)
        {
            // Crée une liste où sera stocker les cases de la solution
            List<object[]> solvedMaze = new List<object[]>();
            List<object[]> solvedMaze2 = new List<object[]>();

            // Booléan pour dire s'il la résolution est arrivée au bout du labyrinthe
            bool isEndedRight = false;
            bool isEndedLeft = false;

            // Crée des thread pour résoudre le labyrinthe en suivant le mur de droite et de gauche
            Thread thRight = new Thread(x => SolveMaze(mazeToSolve, Maze.BOTTOM, mazeToSolve.enterDoorPos[0], mazeToSolve.enterDoorPos[1], solvedMaze, ref isEndedRight, true, printTime, ConsoleColor.Cyan, ConsoleColor.DarkCyan, ConsoleColor.Gray));

            Thread thLeft = new Thread(x => SolveMaze(mazeToSolve, Maze.BOTTOM, mazeToSolve.enterDoorPos[0], mazeToSolve.enterDoorPos[1], solvedMaze2, ref isEndedLeft, false, printTime, ConsoleColor.Red, ConsoleColor.DarkRed, ConsoleColor.White));

            // Démarre les threads
            thRight.Start();
            thLeft.Start();

            // Attends que les threads se temine
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
        private static void SolveMaze(Maze mazeToSolve, string lastDirection, int currentX, int currentY, List<object[]> solvedList, ref bool isEnded, bool mustGoRight, int? printTime,
            ConsoleColor inProgressColor, ConsoleColor passtColor, ConsoleColor solveColor)
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

            // S'il faut afficher le parcour de la résolution
            if (printTime != null)
            {
                // Affiche un carré à la position actuel et attend le temps voulu
                PrintStep(currentX, currentY, lastDirection, inProgressColor);
                Thread.Sleep((int)printTime);
            }

            // Tant qu'il n'a pas regardé toutes les directions et qu'il n'a pas atteint la fin
            while (directions.Count != 0 && !isEnded)
            {
                // Vérifie qu'il n'y ait pas de mur entre la case actuel et la case suivante
                if ((mazeToSolve.maze[currentX, currentY] & Maze.TBRL[directions[mustGoRight ? 0 : directions.Count - 1]]) == Maze.TBRL[directions[mustGoRight ? 0 : directions.Count - 1]])
                {
                    // Enregistre la position de la case suivante
                    int nextX = currentX + Maze.differenceX[directions[mustGoRight ? 0 : directions.Count - 1]];
                    int nextY = currentY + Maze.differenceY[directions[mustGoRight ? 0 : directions.Count - 1]];

                    // Si la case suivante existe
                    if (nextX >= 0 && nextX < mazeToSolve.maze.GetLength(0) && nextY >= 0 && nextY < mazeToSolve.maze.GetLength(1))
                    {
                        // Passe dans cette case pour continuer la résolution
                        SolveMaze(mazeToSolve, directions[mustGoRight ? 0 : directions.Count - 1], nextX, nextY, solvedList, ref isEnded, mustGoRight, printTime, inProgressColor, passtColor, solveColor);
                    }

                    else
                    {
                        // Sinon c'est qu'on a atteint la fin du labyrinthe
                        isEnded = true;
                    }
                }

                // Enlève la direction de la liste
                directions.RemoveAt(mustGoRight ? 0 : directions.Count - 1);
            }

            // Si on a atteint la fin du labyrinthe
            if (isEnded)
            {
                // Enregistre la position dans la liste de la solution
                solvedList.Add(new object[] { currentX, currentY, mazeToSolve.enterDoorPos[0] != currentX || mazeToSolve.enterDoorPos[1] != currentY ? lastDirection : null });

                // S'il faut afficher le parcour de la résolution
                if (printTime != null)
                {
                    // Affiche un carré à la position actuel et attend le temps voulu
                    PrintStep(currentX, currentY, lastDirection, solveColor);
                    Thread.Sleep((int)printTime);
                }
            }

            else
            {
                // S'il faut afficher le parcour de la résolution
                if (printTime != null)
                {
                    // Affiche un carré à la position actuel et attend le temps voulu
                    PrintStep(currentX, currentY, lastDirection, passtColor);
                    Thread.Sleep((int)printTime);
                }
            }
        }

        /// <summary>
        /// Affiche la solution du labyrinthe
        /// </summary>
        public static void ShowSolution(Maze mazeToShowSolution, ConsoleColor solutionColor, int? printTime)
        {
            // Obtient la solution du labyrinthe
            List<object[]> solvedMaze = SolveMaze(mazeToShowSolution);

            // Affiche la solution
            for (int i = solvedMaze.Count - 1; i >= 0; i--)
            {
                PrintStep((int)solvedMaze[i][0], (int)solvedMaze[i][1], (string)solvedMaze[i][2], solutionColor);

                // S'il faut attendre entre chaque étape
                if (printTime != null)
                {
                    Thread.Sleep((int)printTime);
                }
            }
        }
    }
}
