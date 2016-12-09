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
        /// <summary>
        /// Les différents type de génération d'un labyrinthe
        /// </summary>
        public enum GenerationType { RecursiveBacktracking, GenerateMazeByKruskal, Mixt }

        /// <summary>
        /// Largeur du labyrinthe
        /// </summary>
        private int width;

        /// <summary>
        /// Hauteur du labyrinthe
        /// </summary>
        private int height;

        /// <summary>
        /// Positin de la porte d'entrée du labyrinthe
        /// </summary>
        public int[] enterDoorPos;

        /// <summary>
        /// Positin de la porte de sortie du labyrinthe
        /// </summary>
        public int[] outDoorPos;

        /// <summary>
        /// Constante pour stocker les noms des différentes directions possible
        /// </summary>
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
        /// <param name="withDoors">Défini si il faut généré un labyrinthe avec les portes d'entrée et de sortie</param>
        public Maze(int width, int height, GenerationType type = GenerationType.GenerateMazeByKruskal, int? stepByStepLength = null)
        {
            // Enregistre la largeur et la hauteur
            this.width = width;
            this.height = height;

            // Crée le tableau du labyrinthe
            maze = new int[width, height];

            // L'affiche une première fois pour un step by step
            if (stepByStepLength != null)
            {
                PrintMaze(maze);
            }

            // Génère le labyrinthe
            GenerateMaze(maze, type, stepByStepLength);
        }

        /// <summary>
        /// Génère un labyrinthe
        /// </summary>
        /// <param name="mazeToGenerate">Labyrinthe à générer</param>
        /// <param name="stepByStep">Lance le labyrinthe en mode "Démo", et montre chaque étape de la génération</param>
        public void GenerateMaze(int[,] mazeToGenerate, GenerationType type = GenerationType.GenerateMazeByKruskal, int? stepByStepLength = null)
        {
            // Génère le labyrinthe avec la méthode voulue
            switch (type)
            {
                case GenerationType.GenerateMazeByKruskal:
                    GenerateMazeByKruskal(mazeToGenerate, stepByStepLength);
                    break;

                case GenerationType.RecursiveBacktracking:
                    GenerateMazeByRecursiveBacktracking(random.Next(width), random.Next(height), mazeToGenerate, stepByStepLength);
                    break;

                case GenerationType.Mixt:
                    GenrateMazeMixt(mazeToGenerate, stepByStepLength);
                    break;
            }

            // Choisi la position de l'entrée et de la sortie du labyrinthe
            int topDoorPos = random.Next(width);
            int bottomDoorPod = random.Next(width);
            enterDoorPos = new int[] { topDoorPos, 0 };
            outDoorPos = new int[] { bottomDoorPod, height - 1 };

            // Enregistre la porte à la bonne position
            mazeToGenerate[topDoorPos, 0] |= TBRL[TOP];
            mazeToGenerate[bottomDoorPod, height - 1] |= TBRL[BOTTOM];
        }

        private void GenrateMazeMixt(int[,] mazeToGenerate, int? stepByStepLength)
        {
            // Divise le labyrinthe en 4 petits labyrinthes
            int[][,] subMazes = new int[4][,];

            subMazes[0] = new int[width / 2 + (width % 2), height / 2 + (height % 2)];
            subMazes[1] = new int[width / 2, height / 2 + (height % 2)];
            subMazes[2] = new int[width / 2 + (width % 2), height / 2];
            subMazes[3] = new int[width / 2, height / 2];

            // Génère les 4 petits labyrinthes avec les deux méthodes de génération
            GenerateMazeByKruskal(subMazes[0], stepByStepLength);
            GenerateMazeByRecursiveBacktracking(random.Next(subMazes[1].GetLength(0)), random.Next(subMazes[1].GetLength(1)), subMazes[1], stepByStepLength);
            GenerateMazeByRecursiveBacktracking(random.Next(subMazes[2].GetLength(0)), random.Next(subMazes[2].GetLength(0)), subMazes[2], stepByStepLength);
            GenerateMazeByKruskal(subMazes[3], stepByStepLength);

            for (int i = 0; i < 3; i++)
            {
                int tmp = random.Next(subMazes[i].GetLength(i % 2));

                if (i == 0)
                {
                    subMazes[0][tmp, height / 2 + (height % 2) - 1] |= TBRL[BOTTOM];
                    subMazes[2][tmp, 0] |= TBRL[TOP];
                }
                else if (i == 1)
                {
                    subMazes[0][width / 2 + (width % 2) - 1, tmp] |= TBRL[RIGHT];
                    subMazes[1][0, tmp] |= TBRL[LEFT];
                }
                else if (i == 2)
                {
                    subMazes[1][tmp, height / 2 + (height % 2) - 1] |= TBRL[BOTTOM];
                    subMazes[3][tmp, 0] |= TBRL[TOP];
                }
            }

            // Regroupe les 4 petits labyrinthe dans le labyrinthe final
            for (int i = 0; i < subMazes.Length; i++)
            {
                for (int x = 0; x < subMazes[i].GetLength(0); x++)
                {
                    for (int y = 0; y < subMazes[i].GetLength(1); y++)
                    {
                        maze[x + ((i % 2) * width / 2 + (width % 2 & i % 2)), y + (i / 2) * height / 2 + (height % 2 & i / 2)] = subMazes[i][x, y];
                    }
                }
            }
        }

        /// <summary>
        /// Génère un labyrinthe avec la méthode de l'exploration exhaustive
        /// </summary>
        /// <param name="currentX">Position à l'hotizontal de la dernière case visitée</param>
        /// <param name="currentY">Position à la vertical de la dernière case visitée</param>
        /// <param name="mazeToGenerate">Labyrinthe à génèrer</param>
        /// <param name="stepByStep">Lance le labyrinthe en mode "Démo", et montre chaque étape de la génération</param>
        private void GenerateMazeByRecursiveBacktracking(int currentX, int currentY, int[,] mazeToGenerate, int? stepByStepLength)
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
                if (nextX >= 0 && nextX < mazeToGenerate.GetLength(0) && nextY >= 0 && nextY < mazeToGenerate.GetLength(1) && mazeToGenerate[nextX, nextY] == 0)
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

        /// <summary>
        /// Génère un labyrinthe avec la méthode de Kruskal
        /// </summary>
        /// <param name="mazeToGenerate">Labyrinthe à génèrer</param>
        /// <param name="stepByStepLength">Lance le labyrinthe en mode "Démo", et montre chaque étape de la génération</param>
        private void GenerateMazeByKruskal(int[,] mazeToGenerate, int? stepByStepLength)
        {
            // Crée une tableau de la taille du labyrinthe à générer pour stocker les nombres de chaque case
            int[,] mazeNumber = new int[mazeToGenerate.GetLength(0), mazeToGenerate.GetLength(1)];

            // Initilise le tableau avec le numéro de la case
            for (int i = 0; i < mazeNumber.GetLength(0); i++)
            {
                for (int j = 0; j < mazeNumber.GetLength(1); j++)
                {
                    mazeNumber[i, j] = i * mazeNumber.GetLength(0) + j;
                }
            }

            // Crée un tableau avec les 4 directions possible
            string[] directions = new string[] { TOP, BOTTOM, LEFT, RIGHT };

            // Fait la boucle tant que le labyrinthe n'est pas fini
            while (!MazeIsGeneratedByKruskal(mazeNumber))
            {
                // Choisi des positions pour une case à ouvrir
                int posX = random.Next(mazeNumber.GetLength(0));
                int posY = random.Next(mazeNumber.GetLength(1));

                // Choisi une direction aléatoire
                string direction = directions[random.Next(4)];

                // Calcul la position de la deuxième case suivant la direction voulue
                int secondPosX = posX + differenceX[direction];
                int secondPosY = posY + differenceY[direction];

                // Vérifie que la position de la deuxième case se trouve dans les limites du labyrinthe et que le numéro des deux cases ne sont pas égale
                if (secondPosX >= 0 && secondPosX < mazeNumber.GetLength(0) && secondPosY >= 0 && secondPosY < mazeNumber.GetLength(1) && mazeNumber[posX, posY] != mazeNumber[secondPosX, secondPosY])
                {
                    // Enregistre la position de la porte ouverte dans les deux cases
                    mazeToGenerate[posX, posY] |= TBRL[direction];
                    mazeToGenerate[secondPosX, secondPosY] |= TBRL[REVERT_TBRL[direction]];

                    // Enregistre le numéro de la deuxième case pour changer les numéros de chaque case égale
                    int tmp = mazeNumber[secondPosX, secondPosY];

                    // Vérifie le numéro de chaque case et change celle voulue
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

                    // Affiche le pas par pas du labyrinthe si voulu
                    if (stepByStepLength != null)
                    {
                        PrintMaze(mazeToGenerate, new int[] { posX, posY });
                        Thread.Sleep((int)stepByStepLength);
                    }
                }
            }
        }

        /// <summary>
        /// Vérifie qu'un labyrinthe généré avec la méthode Kruskal est fini
        /// </summary>
        /// <param name="mazeNumberToCheck">Tableau des numéros des cases d'un labyrinthe généré avec Kruskal</param>
        /// <returns>Si le labyrinthe est terminé</returns>
        private bool MazeIsGeneratedByKruskal(int[,] mazeNumberToCheck)
        {
            // Vérifie pour chaque case si le numéro et égale à celui de la première
            for (int i = 0; i < mazeNumberToCheck.GetLength(0); i++)
            {
                for (int j = 0; j < mazeNumberToCheck.GetLength(1); j++)
                {
                    if (mazeNumberToCheck[i, j] != mazeNumberToCheck[0, 0])
                    {
                        // Pour finir la méthode dès qu'on trouve une case non reliée
                        return false;
                    }
                }
            }

            // Retourne que le labyrinthe est terminé
            return true;
        }
    }
}
