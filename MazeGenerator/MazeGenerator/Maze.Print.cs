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
        /// Mutex pour écrire dans la console lors d'utilisation de threads
        /// </summary>
        static Mutex mut = new Mutex();

        /// <summary>
        /// Dictionnaire des valeurs de chaque mur
        /// </summary>
        public static Dictionary<int, string> WALL = new Dictionary<int, string>()
        {
            { 15," " /*1111*/},
            { 14,"─" /*1110*/},
            { 13,"─" /*1101*/},
            { 12,"─" /*1100*/},
            { 11,"│" /*1011*/},
            { 10,"└" /*1010*/},
            { 9, "┘" /*1001*/},
            { 8, "┴" /*1000*/},
            { 7, "│" /*0111*/},
            { 6, "┌" /*0110*/},
            { 5, "┐" /*0101*/},
            { 4, "┬" /*0100*/},
            { 3, "│" /*0011*/},
            { 2, "├" /*0010*/},
            { 1, "┤" /*0001*/},
            { 0, "┼" /*0000*/}
        };

        /// <summary>
        /// Taille des cases pour l'affichage des cases, obligatoirement à 2
        /// </summary>
        public const int SIZE = 2;

        /// <summary>
        /// Affiche un labyrinthe dans la console
        /// </summary>
        /// <param name="_maze">Labyrinthe à afficher</param>
        /// <param name="_caseCorrdsForMonoPrint">Sert au pas par pas pour changer uniquement les bords qui touche une case</param>
        /// <param name="printTime">Temps à attendre pour chaque étape de l'affichage du labyrinthe</param>
        public static void PrintMaze(int[,] _maze, int[] _caseCorrdsForMonoPrint = null, int printTime = 0)
        {
            // S'il faut regénérer une case
            if (_caseCorrdsForMonoPrint != null)
            {
                // Efface tous les murs au tours de la case
                for (int x = 0; x <= SIZE; x++)
                {
                    for (int y = 0; y <= SIZE; y++)
                    {
                        Console.SetCursorPosition(_caseCorrdsForMonoPrint[0] * SIZE + x, _caseCorrdsForMonoPrint[1] * SIZE + y);
                        Console.Write(" ");
                    }
                }

                // Affiche le mur de gauche, du haut et le coin en haut à gauche
                ShowCase(_maze, _caseCorrdsForMonoPrint[0], _caseCorrdsForMonoPrint[1]);

                // S'il y a une case en bas à droite
                if (_caseCorrdsForMonoPrint[0] < _maze.GetLength(0) - 1 && _caseCorrdsForMonoPrint[1] < _maze.GetLength(1) - 1)
                {
                    // Affiche la case d'en bas à droite pour avoir le coins en bas à droite
                    ShowCase(_maze, _caseCorrdsForMonoPrint[0] + 1, _caseCorrdsForMonoPrint[1] + 1);
                }

                // S'il y a une case à droite
                if (_caseCorrdsForMonoPrint[0] < _maze.GetLength(0) - 1)
                {
                    // Affiche la case de droite pour avoir le coins en haut à droite et le mur de droite
                    ShowCase(_maze, _caseCorrdsForMonoPrint[0] + 1, _caseCorrdsForMonoPrint[1]);
                }

                // S'il y a une case en bas
                if (_caseCorrdsForMonoPrint[1] < _maze.GetLength(1) - 1)
                {
                    // Affiche la case d'en bas pour affiche le coins en bas à gauche et le mur d'en bas
                    ShowCase(_maze, _caseCorrdsForMonoPrint[0], _caseCorrdsForMonoPrint[1] + 1);
                }
            }

            else
            {
                // Affiche toutes les case du labyrinthe
                for (int x = 0; x < _maze.GetLength(0); x++)
                {
                    for (int y = 0; y < _maze.GetLength(1); y++)
                    {
                        // Affiche la case actuel
                        ShowCase(_maze, x, y);

                        // Attends le temps demandé
                        Thread.Sleep(printTime);
                    }
                }
            }
        }

        /// <summary>
        /// Affiche les murs au tour d'un case 
        /// </summary>
        /// <param name="_maze">Labyrinthe de la case</param>
        /// <param name="posX">Position de la case en vertical</param>
        /// <param name="posY">Position de la case en horizontal</param>
        private static void ShowCase(int[,] _maze, int posX, int posY)
        {
            // Variable pour le stocker la valeur d'un coin
            int tmp = 0;

            // S'il faut afficher le mur du haut
            if ((_maze[posX, posY] & 1) == 0)
            {
                // Affiche le mur à la bonne position
                Console.SetCursorPosition(posX * SIZE + 1, posY * SIZE);
                Console.Write(WALL[12]);
            }

            // S'il faut afficher le mur de gauche
            if ((_maze[posX, posY] & 8) == 0)
            {
                // Affiche le mur à la bonne position
                Console.SetCursorPosition(posX * SIZE, posY * SIZE + 1);
                Console.Write(WALL[3]);
            }

            // Positionne le curseur pour écrire le mur d'en haut à droite
            Console.SetCursorPosition(posX * SIZE, posY * SIZE);

            // Si ce n'est pas une case dans un bord du haut et/ou de la gauche
            if (posX > 0 && posY > 0)
            {
                // Calcul le mur à afficher
                tmp = (_maze[posX, posY] & 9) + (_maze[posX - 1, posY - 1] & 6);
            }

            // Si c'est la case en haut à gauche
            else if (posX == 0 && posY == 0)
            {
                // Enregistre la valeur du coins en haut à gauche
                tmp = 6;
            }

            // Si la case se trouve en haut
            else if (posX == 0)
            {
                // Calcul le mur à afficher
                tmp = (_maze[posX, posY] & 9) + (_maze[posX, posY - 1] & 8) / 2 + 2;
            }

            // Si la case se trouve à gauche
            else if (posY == 0)
            {
                // Calcul le mur à afficher
                tmp = (_maze[posX, posY] & 9) + (_maze[posX - 1, posY] & 1) * 2 + 4;
            }

            // Affiche le mur
            Console.Write(WALL[tmp]);

            // Si la case se trouve en bas du labyrinthe
            if (posY == _maze.GetLength(1) - 1)
            {
                // Affiche le mur d'en bas s'il le faut
                if ((_maze[posX, posY] & 2) == 0)
                {
                    Console.SetCursorPosition(posX * SIZE + 1, posY * SIZE + 2);
                    Console.Write(WALL[12]);
                }

                // Position le curseur pour afficher le mur en bas à gauche
                Console.SetCursorPosition(posX * SIZE, posY * SIZE + 2);

                // Si c'est la première colone
                if (posX == 0)
                {
                    // Enregistre la valeur du coins en bas à gauche
                    tmp = 10;
                }

                else
                {
                    // Calcul le mur à afficher
                    tmp = (_maze[posX, posY] & 2) / 2 + (_maze[posX - 1, posY] & 6) + 8;
                }

                // Affiche le mur
                Console.Write(WALL[tmp]);
            }

            // Si c'est une case toute à droite
            if (posX == _maze.GetLength(0) - 1)
            {
                //Affiche le mur de droite si besoin
                if ((_maze[posX, posY] & 4) == 0)
                {
                    Console.SetCursorPosition(posX * SIZE + 2, posY * SIZE + 1);
                    Console.Write(WALL[3]);
                }

                // Positionne le curseur pour afficher le coins d'en haut à droite
                Console.SetCursorPosition(posX * SIZE + 2, posY * SIZE);

                // Si c'est la case en haut à droite
                if (posY == 0)
                {
                    // Enregistre la valeur du coins en haut à droite
                    tmp = 5;
                }

                else
                {
                    // Calcul le mur à afficher
                    tmp = (_maze[posX, posY] & 4) * 2 + (_maze[posX, posY - 1] & 6) + 1;
                }

                // Affiche le mur voulu
                Console.Write(WALL[tmp]);

                // Si c'est la case en bas à gauche
                if (posY == _maze.GetLength(1) - 1)
                {
                    // Affiche le coin
                    Console.SetCursorPosition(posX * SIZE + 2, posY * SIZE + 2);
                    Console.Write(WALL[9]);
                }
            }
        }

        /// <summary>
        /// Affiche un carré à une position voulue dans le labyrinthe
        /// </summary>
        /// <param name="_x">Position vertical de la case à afficher</param>
        /// <param name="_y">Position horizontal de la case à afficher</param>
        /// <param name="direction">Direction dans la quelle on est rentré dans la case</param>
        /// <param name="color">Couleur du carré</param>
        public static void PrintStep(int _x, int _y, string direction, ConsoleColor color)
        {
            // Attends que le mutex soit libéré
            mut.WaitOne();

            // Change la couleur de fond lors de l'écriture
            Console.BackgroundColor = color;

            // Affiche un espace avec la couleur de fond à la position voulue
            Console.SetCursorPosition(_x * SIZE + 1, _y * SIZE + 1);
            Console.Write(" ");

            // Affiche un lien entre la position actuel et la précédente si besoin
            if (direction != null)
            {
                Console.SetCursorPosition(_x * SIZE + 1 + differenceX[REVERT_TBRL[direction]], _y * SIZE + 1 + differenceY[REVERT_TBRL[direction]]);
                Console.Write(" ");
            }

            // Remet la couleur de fond à noire
            Console.BackgroundColor = ConsoleColor.Black;

            // Libère le mutex
            mut.ReleaseMutex();
        }
    }
}
