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
            mazeToGenerate[topDoorPos, 0] |= TBRL[TOP];
            mazeToGenerate[bottomDoorPod, height - 1] |= TBRL[BOTTOM];
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

                    // Continue de génèrer le labyrinthe avec la case suivante
                    GenerateMaze(nextX, nextY, mazeToGenerate);
                }

                // Supprime la direction pour de celle restante
                directions.RemoveAt(randomDir);
            }
        }
    }
}
