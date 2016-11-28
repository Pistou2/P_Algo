using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeGenerator
{
    class MazeSolver
    {
        /// <summary>
        /// Résoud un labyrinthe
        /// </summary>
        /// <param name="mazeToSolve">Labyrinthe à résoudre</param>
        /// <returns>List des positions des cases où il faut passer pour résoudre le labyrinthe</returns>
        public static List<int[]> SolveMaze(int[,] mazeToSolve)
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
            bool isEnded = false;

            // Résoud le labyrinthe
            SolveMaze(mazeToSolve, Maze.BOTTOM, firstPositionX, 0, solvedMaze, ref isEnded);

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
        private static void SolveMaze(int[,] mazeToSolve, string lastDirection, int currentX, int currentY, List<int[]> solvedList, ref bool isEnded)
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

            // Tant qu'il n'a pas regardé toutes les directions et qu'il n'a pas atteint la fin
            while (directions.Count != 0 && !isEnded)
            {
                if ((mazeToSolve[currentX, currentY] & Maze.TBRL[directions[0]]) == Maze.TBRL[directions[0]])
                {
                    int nextX = currentX + Maze.differenceX[directions[0]];
                    int nextY = currentY + Maze.differenceY[directions[0]];

                    if (nextX >= 0 && nextX < mazeToSolve.GetLength(0) && nextY >= 0 && nextY < mazeToSolve.GetLength(1))
                    {
                        SolveMaze(mazeToSolve, directions[0], nextX, nextY, solvedList, ref isEnded);
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
            }
        }

        public static void ShowSolution (List<int[]> solvedMaze)
        {
            for (int i = solvedMaze.Count - 1; i >= 0; i--)
            {
                Program.printMaze(solvedMaze[i][0], solvedMaze[i][1]);
            }
        }
    }
}
