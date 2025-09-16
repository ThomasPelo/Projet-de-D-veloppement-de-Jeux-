using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back
{
    internal class BackPuissance4
    {
        static void Main()
        {
            int[,] grille = GenererTableau2D(6, 7);
            AfficherGrille(grille);

            Console.WriteLine(VerifierVictoire(grille, 0));
        }


        static bool VerifierVictoire(int[,] grille, int valeur)
        {
            int lignes = grille.GetLength(0);
            int colonnes = grille.GetLength(1);

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    if (grille[i, j] != valeur) continue;

                    if (j + 4 <= colonnes)
                    {
                        bool ok = true;
                        for (int k = 0; k < 4; k++)
                            if (grille[i, j + k] != valeur) { ok = false; break; }
                        if (ok) return true;
                    }

                    if (i + 4 <= lignes)
                    {
                        bool ok = true;
                        for (int k = 0; k < 4; k++)
                            if (grille[i + k, j] != valeur) { ok = false; break; }
                        if (ok) return true;
                    }

                    if (i + 4 <= lignes && j + 4 <= colonnes)
                    {
                        bool ok = true;
                        for (int k = 0; k < 4; k++)
                            if (grille[i + k, j + k] != valeur) { ok = false; break; }
                        if (ok) return true;
                    }

                    if (i + 4 <= lignes && j - 4 + 1 >= 0)
                    {
                        bool ok = true;
                        for (int k = 0; k < 4; k++)
                            if (grille[i + k, j - k] != valeur) { ok = false; break; }
                        if (ok) return true;
                    }
                }
            }
            return false;
        }
        static int[,] GenererTableau2D(int lignes, int colonnes)
        {
            int[,] tableau = new int[lignes, colonnes];
            Random rnd = new Random();

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    tableau[i, j] = rnd.Next(0, 2);
                }
            }

            return tableau;
        }

        static void AfficherGrille(int[,] grille)
        {
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    Console.Write(grille[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
