using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeGenerator
{
    partial class Maze
    {
        public enum GenerationType { RecursiveBacktracking, GenerateMazeByKruskal, Mixt }

        /// <summary>
        /// Largeur du labyrinthe
        /// </summary>
        private int width;

        /// <summary>
        /// Hauteur du labyrinthe
        /// </summary>
        private int height;

        public int[] enterDoorPos;
        public int[] outDoorPos;

        // Constante pour stocker les noms des différentes directions
        public const string TOP = "top";
        public const string BOTTOM = "bottom";
        public const string RIGHT = "right";
        public const string LEFT = "left";

        /// <summary>
        /// Dictionnaire avec la différence en case horizontalement suivant la direction voulue
        /// </summary>
        public static Dictionary<string, int> differenceX = new Dictionary<string, int>()
        {
            { TOP, 0 },
            { BOTTOM, 0 },
            { RIGHT, 1 },
            { LEFT, -1 }
        };

        /// <summary>
        /// Dictionnaire avec la différence en case verticalement suivant la direction voulue
        /// </summary>
        public static Dictionary<string, int> differenceY = new Dictionary<string, int>()
        {
            { TOP, -1 },
            { BOTTOM, 1 },
            { RIGHT, 0 },
            { LEFT, 0 }
        };

        /// <summary>
        /// Dictionnaire avec les valeurs pour stocker les portes
        /// </summary>
        public static Dictionary<string, int> TBRL = new Dictionary<string, int>()
        {
            { TOP, 1 /*0001*/ },
            { BOTTOM, 2 /*0010*/ },
            { RIGHT, 4  /*0100*/ },
            { LEFT, 8 /*1000*/ }
        };

        /// <summary>
        /// Dictionnaire avec l'inverse des directions des portes
        /// </summary>
        public static Dictionary<string, string> REVERT_TBRL = new Dictionary<string, string>()
        {
            { TOP, BOTTOM },
            { BOTTOM, TOP },
            { RIGHT, LEFT },
            { LEFT, RIGHT }
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
        /// <param name="stepByStep">Lance le labyrinthe en mode "Démo", et montre chaque étape de la génération</param>
        public Maze(int width, int height, GenerationType type = GenerationType.GenerateMazeByKruskal, int? stepByStepLength = null, bool withDoors = true)
        {
            // Enregistre la largeur et la hauteur
            this.width = width;
            this.height = height;

            // Crée le tableau du labyrinthe
            maze = new int[width, height];

            // Génère le labyrinthe
            //L'affiche une première fois pour un step by step
            if (stepByStepLength != null)
            {
                PrintMaze(maze);
            }
            GenerateMaze(maze, type, stepByStepLength, withDoors);
        }

        /// <summary>
        /// Génère un labyrinthe
        /// </summary>
        /// <param name="mazeToGenerate">Labyrinthe à générer</param>
        /// <param name="stepByStep">Lance le labyrinthe en mode "Démo", et montre chaque étape de la génération</param>
        public void GenerateMaze(int[,] mazeToGenerate, GenerationType type = GenerationType.GenerateMazeByKruskal, int? stepByStepLength = null, bool withDoors = true)
        {
            // Génère le labyrinthe
            if (type == GenerationType.GenerateMazeByKruskal)
            {
                GenerateMazeByKruskal(mazeToGenerate, stepByStepLength);


            }
            else if (type == GenerationType.RecursiveBacktracking)
            {
                GenerateMazeByRecursiveBacktracking(random.Next(width), random.Next(height), mazeToGenerate, stepByStepLength);
            }

            else if (type == GenerationType.Mixt)
            {
                //TODO
                //divide the maze in 4 little mazes
                Maze[] subMazes = new Maze[4];

                subMazes[0] = new Maze(width / 2, height / 2, GenerationType.GenerateMazeByKruskal, stepByStepLength, false);
                subMazes[1] = new Maze(width / 2, height / 2, GenerationType.RecursiveBacktracking, stepByStepLength, false);
                subMazes[2] = new Maze(width / 2, height / 2, GenerationType.GenerateMazeByKruskal, stepByStepLength, false);
                subMazes[3] = new Maze(width / 2, height / 2, GenerationType.RecursiveBacktracking, stepByStepLength, false);

                Console.Clear();

                //Regroup all the mazes in an unique maze

                for (int i = 0; i < subMazes.Length; i++)
                {
                    for (int x = 0; x < subMazes[i].maze.GetLength(0); x++)
                    {
                        for (int y = 0; y < subMazes[i].maze.GetLength(1); y++)
                        {
                            maze[x + ((i % 2) * width / 2), y + (i / 2) * height / 2] = subMazes[i].maze[x, y];
                        }
                    }
                }
            }

            if (withDoors)
            {
                // Choisi la position de l'entrée et de la sortie du labyrinthe
                int topDoorPos = random.Next(width);
                int bottomDoorPod = random.Next(width);
                enterDoorPos = new int[] { topDoorPos, 0 };
                outDoorPos = new int[] { bottomDoorPod, height - 1 };

                // Enregistre la porte à la bonne position
                mazeToGenerate[topDoorPos, 0] |= TBRL[TOP];
                mazeToGenerate[bottomDoorPod, height - 1] |= TBRL[BOTTOM];
            }
        }

        /// <summary>
        /// Génère un labyrinthe avec la méthode de l'exploration exhaustive
        /// </summary>
        /// <param name="currentX">Position à l'hotizontal de la dernière case visitée</param>
        /// <param name="currentY">Position à la vertical de la dernière case visitée</param>
        /// <param name="mazeToGenerate">Labyrinthe à génèrer</param>
        /// <param name="stepByStep">Lance le labyrinthe en mode "Démo", et montre chaque étape de la génération</param>
        public void GenerateMazeByRecursiveBacktracking(int currentX, int currentY, int[,] mazeToGenerate, int? stepByStepLength)
        {
            // Crée une liste avec les différentes directions possible
            List<string> directions = new List<string> { TOP, BOTTOM, LEFT, RIGHT };

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

                    //Step by step
                    if (stepByStepLength != null)
                    {
                        //écrit la case
                        PrintMaze(mazeToGenerate, new int[] { currentX, currentY });
                        Thread.Sleep((int)stepByStepLength);
                    }

                    // Continue de génèrer le labyrinthe avec la case suivante
                    GenerateMazeByRecursiveBacktracking(nextX, nextY, mazeToGenerate, stepByStepLength);
                }

                // Supprime la direction pour de celle restante
                directions.RemoveAt(randomDir);
            }
        }

        private void GenerateMazeByKruskal(int[,] mazeToGenerate, int? stepByStepLength)
        {
            int[,] mazeNumber = new int[mazeToGenerate.GetLength(0), mazeToGenerate.GetLength(1)];

            for (int i = 0; i < mazeNumber.GetLength(0); i++)
            {
                for (int j = 0; j < mazeNumber.GetLength(1); j++)
                {
                    mazeNumber[i, j] = i * mazeNumber.GetLength(0) + j;
                }
            }

            string[] directions = new string[] { TOP, BOTTOM, LEFT, RIGHT };

            while (!MazeIsGeneratedByKruskal(mazeNumber))
            {
                int posX = random.Next(mazeNumber.GetLength(0));
                int posY = random.Next(mazeNumber.GetLength(1));

                string direction = directions[random.Next(4)];

                int secondPosX = posX + differenceX[direction];
                int secondPosY = posY + differenceY[direction];

                if (secondPosX >= 0 && secondPosX < mazeNumber.GetLength(0) && secondPosY >= 0 && secondPosY < mazeNumber.GetLength(1) && mazeNumber[posX, posY] != mazeNumber[secondPosX, secondPosY])
                {
                    mazeToGenerate[posX, posY] |= TBRL[direction];
                    mazeToGenerate[secondPosX, secondPosY] |= TBRL[REVERT_TBRL[direction]];

                    int tmp = mazeNumber[secondPosX, secondPosY];

                    for (int i = 0; i < mazeNumber.GetLength(0); i++)
                    {
                        for (int j = 0; j < mazeNumber.GetLength(1); j++)
                        {
                            if (mazeNumber[i, j] == tmp)
                            {
                                mazeNumber[i, j] = mazeNumber[posX, posY];
                            }
                        }
                    }

                    if (stepByStepLength != null)
                    {
                        PrintMaze(mazeToGenerate, new int[] { posX, posY });
                        Thread.Sleep((int)stepByStepLength);
                    }
                }
            }
        }

        private bool MazeIsGeneratedByKruskal(int[,] mazeToCheck)
        {
            for (int i = 0; i < mazeToCheck.GetLength(0); i++)
            {
                for (int j = 0; j < mazeToCheck.GetLength(1); j++)
                {
                    if (mazeToCheck[i, j] != mazeToCheck[0, 0])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void PrintMaze(int[,] _maze, int[] _caseCorrdsForMonoPrint = null, int printTime = 0)
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
                        //Slow print if nedded
                    }
                }

                ShowCase(_maze, _caseCorrdsForMonoPrint[0], _caseCorrdsForMonoPrint[1]);

                if (_caseCorrdsForMonoPrint[0] < _maze.GetLength(0) - 1 && _caseCorrdsForMonoPrint[1] < _maze.GetLength(1) - 1)
                {
                    ShowCase(_maze, _caseCorrdsForMonoPrint[0] + 1, _caseCorrdsForMonoPrint[1] + 1);
                }

                if (_caseCorrdsForMonoPrint[0] < _maze.GetLength(0) - 1)
                {
                    ShowCase(_maze, _caseCorrdsForMonoPrint[0] + 1, _caseCorrdsForMonoPrint[1]);
                }

                if (_caseCorrdsForMonoPrint[1] < _maze.GetLength(1) - 1)
                {
                    ShowCase(_maze, _caseCorrdsForMonoPrint[0], _caseCorrdsForMonoPrint[1] + 1);
                }

                Thread.Sleep(printTime);
            }

            else
            {
                //go through all the cases
                for (int x = 0; x < _maze.GetLength(0); x++)
                {
                    for (int y = 0; y < _maze.GetLength(1); y++)
                    {
                        ShowCase(_maze, x, y);
                        Thread.Sleep(printTime);
                    }
                }
            }
        }
    }
}
