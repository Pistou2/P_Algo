using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeGenerator
{
    class Maze
    {
        /// <summary>
        /// Largeur du labyrinthe
        /// </summary>
        private int width;

        /// <summary>
        /// Hauteur du labyrinthe
        /// </summary>
        private int height;

        /// <summary>
        /// Dictionnaire avec la différence en case horizontalement suivant la direction voulue
        /// </summary>
        private static Dictionary<string, int> differenceX = new Dictionary<string, int>()
        {
            { "top", 0 },
            { "bottom", 0 },
            { "right", 1 },
            { "left", -1 }
        };

        /// <summary>
        /// Dictionnaire avec la différence en case verticalement suivant la direction voulue
        /// </summary>
        private static Dictionary<string, int> differenceY = new Dictionary<string, int>()
        {
            { "top", -1 },
            { "bottom", 1 },
            { "right", 0 },
            { "left", 0 }
        };

        /// <summary>
        /// Dictionnaire avec les valeurs pour stocker les portes
        /// </summary>
        private static Dictionary<string, int> TBRL = new Dictionary<string, int>()
        {
            { "top", 1 /*0001*/ },
            { "bottom", 2 /*0010*/ },
            { "right", 4  /*0100*/ },
            { "left", 8 /*1000*/ }
        };

        /// <summary>
        /// Dictionnaire avec l'inverse des directions des portes
        /// </summary>
        private static Dictionary<string, string> REVERT_TBRL = new Dictionary<string, string>()
        {
            { "top", "bottom" },
            { "bottom", "top" },
            { "right", "left" },
            { "left", "right" }
        };

        /// <summary>
        /// Labyrinthe généré
        /// </summary>
        public int[,] maze;

        /// <summary>
        /// Random pour généré le labyrinthe
        /// </summary>
        Random random = new Random();

        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="width">Largeur du labyrinthe voulu</param>
        /// <param name="height">Hauteur du labyrinthe voulu</param>
        public Maze(int width, int height)
        {
            // Enregistre la largeur et la hauteur
            this.width = width;
            this.height = height;

            // Crée le tableau du labyrinthe
            maze = new int[width, height];

            // Génère le labyrinthe
            GenerateMaze(maze);
        }

        /// <summary>
        /// Génère un labyrinthe
        /// </summary>
        /// <param name="mazeToGenerate">Labyrinthe à générer</param>
        private void GenerateMaze(int[,] mazeToGenerate)
        {
            // Génère le labyrinthe
            GenerateMaze(0, 0, mazeToGenerate);

            // Choisi la position de l'entrée et de la sortie du labyrinthe
            int topDoorPos = random.Next(width);
            int bottomDoorPod = random.Next(width);

            // Enregistre la porte à la bonne position
            mazeToGenerate[topDoorPos, 0] |= TBRL["top"];
            mazeToGenerate[bottomDoorPod, height - 1] |= TBRL["bottom"];
        }

        /// <summary>
        /// Génère un labyrinthe avec la méthode de l'exploration exhaustive
        /// </summary>
        /// <param name="currentX">Position à l'hotizontal de la dernière case visitée</param>
        /// <param name="currentY">Position à la vertical de la dernière case visitée</param>
        /// <param name="mazeToGenerate">Labyrinthe à génèrer</param>
        private void GenerateMaze(int currentX, int currentY, int[,] mazeToGenerate)
        {
            // Crée une liste avec les différentes directions possible
            List<string> directions = new List<string> { "top", "bottom", "left", "right" };

            // Tant qu'il y a encore des directions à regarder
            while (directions.Count != 0)
            {
                // Choisi aléatoirement une des directions restantes
                int randomDir = random.Next(directions.Count);
                string direction = directions[randomDir];

                // Calcule la position de la case suivante à regarder
                int nextX = currentX + differenceX[direction];
                int nextY = currentY + differenceY[direction];

                // Si la case n'est pas au bord du labyrinthe et qu'elle n'a pas encore été visitée
                if (nextX >= 0 && nextX < width && nextY >= 0 && nextY < height && mazeToGenerate[nextX, nextY] == 0)
                {
                    // Change la valeur de la case actuel pour indiquer qu'une porte a été crée dans la direction
                    mazeToGenerate[currentX, currentY] |= TBRL[direction];

                    // Change la valeur de la case suivantes pour indiquer qu'une porte a été crée dans la direction (inversée par rapport à la case actuelle)
                    mazeToGenerate[nextX, nextY] |= TBRL[REVERT_TBRL[direction]];

                    // Continue de génèrer le labyrinthe avec la case suivante
                    GenerateMaze(nextX, nextY, mazeToGenerate);
                }

                // Supprime la direction pour de celle restante
                directions.RemoveAt(randomDir);
            }
        }

        public void SolveMaze()
        {
            int firstPositionX = 0;

            for (int i = 0; i < maze.GetLength(0); i++)
            {
                if ((maze[i, 0] & TBRL["top"]) == TBRL["top"])
                {
                    firstPositionX = i;
                }
            }

            List<int[]> solvedMaze = new List<int[]>();

            bool isEnded = false;

            SolveMaze("bottom", firstPositionX, 0, solvedMaze, ref isEnded);

            for (int i = solvedMaze.Count - 1; i >= 0; i--)
            {
                Program.printMaze(maze, solvedMaze[i][0], solvedMaze[i][1]);
                Thread.Sleep(50);
            }
        }

        private void SolveMaze(string lastDirection, int currentX, int currentY, List<int[]> solvedList, ref bool isEnded)
        {
            List<string> directions = new List<string>();

            switch (lastDirection)
            {
                case "top":
                    directions.Add("right");
                    directions.Add("top");
                    directions.Add("left");
                    break;

                case "bottom":
                    directions.Add("left");
                    directions.Add("bottom");
                    directions.Add("right");
                    break;

                case "right":
                    directions.Add("bottom");
                    directions.Add("right");
                    directions.Add("top");
                    break;

                case "left":
                    directions.Add("top");
                    directions.Add("left");
                    directions.Add("bottom");
                    break;
            }

            while(directions.Count != 0 && !isEnded)
            {
                if ((maze[currentX, currentY] & TBRL[directions[0]]) == TBRL[directions[0]])
                {
                    int nextX = currentX + differenceX[directions[0]];
                    int nextY = currentY + differenceY[directions[0]];

                    if (nextX >= 0 && nextX < maze.GetLength(0) && nextY >= 0 && nextY < maze.GetLength(1))
                    {
                        SolveMaze(directions[0], nextX, nextY, solvedList, ref isEnded);
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
    }
}
