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
        private byte tour;
        public string joueur_actuel { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public CLassePuissance4(int adversaire)
        {
            this.adversaire = adversaire == 0 ? "Joueur 2" : "Robot";
            joueur_actuel = "Joueur 1";
            matrice_map = new int[6, 7];
            tour = 0;
        }
        public void AQuiLeTour()
        {
            tour++;
            joueur_actuel = tour % 2 == 0 ? "Joueur 1" : adversaire;
            OnPropertyChanged(nameof(joueur_actuel));
        }

        public int ouVaLeJeton(int colonne)
        {
            // parcourir la colonne de bas en haut
            for (int i = 5; i >=0; i--)
            {
                // 0 pas de jeton
                // 1 jeton joueur 1
                // 2 jeton joueur 2
                if (matrice_map[i, colonne] == 0)
                {
                    matrice_map[i, colonne] = tour;
                    return i;
                }
            }
            return -1;
        }

        public bool VerifPuissance4()
        {
            // verif ligne et colonne 
            int nb_jeton = 0;
            return false;
        }
    }
}
