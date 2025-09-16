using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puissance4_Jeu
{
    internal class CLassePuissance4 : INotifyPropertyChanged
    {
        private string adversaire {  get; }
        private int[,] matrice_map;
        private int tour;
        public string joueur_actuel { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        internal CLassePuissance4(int adversaire)
        {
            this.adversaire = adversaire == 0 ? "Joueur 2" : "Robot";
            joueur_actuel = "Joueur 1";
            matrice_map = new int[6, 7];
            // 2 partout pour set blanc côté front
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    matrice_map[i, j] = 2;
                }
            }
            // joueur 1 commence
            tour = 0;
        }
        public int[,] GetMatrice()
        {
            return matrice_map;
        }

        public void AQuiLeTour()
        {
            tour = (tour+1)%2;
            joueur_actuel = tour == 1 ? "Joueur 1" : adversaire;
            OnPropertyChanged(nameof(joueur_actuel));
        }

        public (int,int) ouVaLeJeton(int colonne)
        {
            // parcourir la colonne de bas en haut
            for (int ligne = 5; ligne >= 0; ligne--)
            {
                // 0 jeton joueur 1
                // 1 jeton joueur 2
                // 2 pas de jeton
                if (matrice_map[ligne, colonne] == 2)
                {
                    // ligne dispo dans une colonne donc on met le jeton
                    matrice_map[ligne, colonne] = tour;
                    if (VerifierVictoire(ligne, colonne))
                    {
                        // victoire
                        return (-2, ligne);
                    }
                    return (0,ligne);
                }
            }
            // colonne pleine
            return (-1, -1);
        }

        public bool VerifierVictoire(int ligne, int colonne)
        {
            // Horizontale
            if (VerifierDirection(ligne, colonne, 0, 1)) return true;

            // Verticale
            if (VerifierDirection(ligne, colonne, 1, 0)) return true;

            // Diagonale \
            if (VerifierDirection(ligne, colonne, 1, 1)) return true;

            // Diagonale /
            if (VerifierDirection(ligne, colonne, 1, -1)) return true;

            return false;
        }

        private bool VerifierDirection(int ligne, int colonne, int dLigne, int dColonne)
        {
            // on compte déjà le pion posé
            int nb_jeton = 1; 

            // Compter dans le sens (dLigne, dColonne)
            nb_jeton += CompterDirection(ligne, colonne, dLigne, dColonne);

            // Compter dans le sens opposé (-dLigne, -dColonne)
            nb_jeton += CompterDirection(ligne, colonne, -dLigne, -dColonne);

            return nb_jeton >= 4;
        }
        private int CompterDirection(int ligne, int colonne, int dLigne, int dColonne)
        {
            int count = 0;
            int r = ligne + dLigne;
            int c = colonne + dColonne;

            while (r >= 0 && r < 6 && c >= 0 && c < 7 && matrice_map[r, c] == tour)
            {
                count++;
                r += dLigne;
                c += dColonne;
            }

            return count;
        }
    }
}
