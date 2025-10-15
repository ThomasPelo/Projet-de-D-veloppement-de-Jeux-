using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Puissance4_Jeu
{
    public class CLassePuissance4 : INotifyPropertyChanged
    {
        private string adversaire;
        private int[,] matrice_map;
        private int tour;
        public string joueur_actuel { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public CLassePuissance4(int adversaire)
        {
            this.adversaire = adversaire == 0 ? "Joueur 2" : "Robot"; 
            joueur_actuel = "Joueur 1"; 
            matrice_map = new int[6, 7]; // Initialisation d'un tableau 6x7 représentant la grille, destiné à stocké la valeur de chaque case

            // Initialiser la matrice avec 2 (cases vides)
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    matrice_map[i, j] = 2; 
                }
            }
            tour = 0;
        }

        public string GetAdversaire()
        {
            return adversaire;
        }

        public int[,] GetMatrice()
        {
            return matrice_map;
        }

        public int GetTour()
        {
            return tour;
        }

        public void AQuiLeTour()
        {
            tour = (tour + 1) % 2; 
            joueur_actuel = tour == 0 ? "Joueur 1" : adversaire;
            OnPropertyChanged(nameof(joueur_actuel));
        }

        public (int, int) JouerCoup(int colonne) // Selection de la colonne ou l'on joue 
        {
            return ouVaLeJeton(colonne);
        }

        public (int, int) ouVaLeJeton(int colonne) // Renvoie la case vide la plus basse dans la colonne passé en paramètre
        {
            // Vérifier si la colonne est valide
            if (colonne < 0 || colonne >= 7) return (-1, -1);

            // Parcourir la colonne de bas en haut
            for (int ligne = 5; ligne >= 0; ligne--)
            {
                // 0 jeton joueur 1
                // 1 jeton joueur 2
                // 2 pas de jeton
                if (matrice_map[ligne, colonne] == 2)
                {
                    // Case vide trouvée, placer le jeton
                    matrice_map[ligne, colonne] = tour;

                    if (VerifierVictoire(ligne, colonne))
                    {
                        return (-2, ligne); // Victoire
                    }
                    else if (EstMatchNul())
                    {
                        return (-3, ligne); // Match nul
                    }

                    return (0, ligne); // Coup normal
                }
            }
            return (-1, -1); // Colonne pleine
        }

        public bool VerifierVictoire(int ligne, int colonne) // Vérifie les alignements autour du pions passé en paramètre
        {
            // Horizontale
            if (CompterDirection(ligne, colonne, 0, 1) + CompterDirection(ligne, colonne, 0, -1) >= 3) return true; 

            // Verticale
            if (CompterDirection(ligne, colonne, 1, 0) + CompterDirection(ligne, colonne, -1, 0) >= 3) return true;

            // Diagonale \
            if (CompterDirection(ligne, colonne, 1, 1) + CompterDirection(ligne, colonne, -1, -1) >= 3) return true;

            // Diagonale /
            if (CompterDirection(ligne, colonne, 1, -1) + CompterDirection(ligne, colonne, -1, 1) >= 3) return true;

            return false;
        }

        private int CompterDirection(int ligne, int colonne, int dLigne, int dColonne)
        {
            int count = 0; // Compteur de pions alignés
            int joueur = matrice_map[ligne, colonne]; // couleur du pion dont on part
            int r = ligne + dLigne; // Choix de la direction de la ligne ( suivante / précente / même ligne )
            int c = colonne + dColonne; // Choix de la direction de la colonne à vérifier ( suivante si dColonne vaut +1, précedente si -1 et même colonne si 0 ) 
            // On vérifie les cases tant qu'on est dans les limites de la matrice et que le pion vérifier est de la même couleur que celui du joueur
            while (r >= 0 && r < 6 && c >= 0 && c < 7 && matrice_map[r, c] == joueur) 
            {
                count++; // Incrémentation du compteur de pion 
                r += dLigne; // Passage à la ligne suivante ( en gardant la même direction dLigne )
                c += dColonne; // Passage à la colonne suivante ( en gardant la même direction dColonne )
            }

            return count;
        }

        private bool EstMatchNul()
        {
            // Vérifier si toutes les colonnes sont pleines
            for (int j = 0; j < 7; j++)
            {
                if (matrice_map[0, j] == 2) return false; // Il reste au moins une case vide
            }
            return true;
        }

        // === ALGORITHME DU ROBOT ===
        // MinMax avec élagage alpha-beta

        public int JouerCoupRobot() 
        {
            if (adversaire != "Robot") return -1;

            int meilleurCoup = TrouverMeilleurCoup();
            return meilleurCoup;
        }

        private int TrouverMeilleurCoup() // Trouve le meilleur coup possible dans la partie
        {
            List<int> coupsPossibles = GetCoupsPossibles();

            // Vérifier s'il y a un coup gagnant immédiat
            foreach (int coup in coupsPossibles)
            {
                if (EstCoupGagnant(coup, 1)) // 1 = robot
                {
                    return coup;
                }
            }

            // Vérifier si le joueur adverse a un coup gagnant à bloquer
            foreach (int coup in coupsPossibles)
            {
                if (EstCoupGagnant(coup, 0)) // 0 = joueur humain
                {
                    return coup;
                }
            }

            // Utiliser MinMax pour les coups stratégiques
            int meilleurScore = int.MinValue;
            int meilleurColonne = coupsPossibles[0];
            int profondeur = 5; // Profondeur de recherche (ajustable)

            foreach (int coup in coupsPossibles)
            {
                // Simuler le coup
                int ligne = SimulerCoup(coup, 1);
                if (ligne == -1) continue;

                // Évaluer le coup
                int score = MinMax(profondeur - 1, false, int.MinValue, int.MaxValue);

                // Annuler le coup
                matrice_map[ligne, coup] = 2;

                if (score > meilleurScore)
                {
                    meilleurScore = score;
                    meilleurColonne = coup;
                }
            }

            return meilleurColonne;
        }

        // - Si estMaximisant = true : le tour de l’IA, on cherche à maximiser le score.
        // - Si estMaximisant = false : le tour du joueur, on cherche à minimiser le score.
        // - Utilise la profondeur pour limiter la recherche et privilégier les victoires rapides.
        // - Retourne une évaluation numérique de la position courante.
        private int MinMax(int profondeur, bool estMaximisant, int alpha, int beta)
        {
            // Conditions d'arrêt
            if (profondeur == 0)
                return EvaluerPosition();

            List<int> coupsPossibles = GetCoupsPossibles();

            if (coupsPossibles.Count == 0)
                return 0; // Match nul

            if (estMaximisant)
            {
                int maxEval = int.MinValue;
                foreach (int coup in coupsPossibles)
                {
                    int ligne = SimulerCoup(coup, 1);
                    if (ligne == -1) continue;

                    // Vérifier victoire robot
                    if (VerifierVictoire(ligne, coup))
                    {
                        matrice_map[ligne, coup] = 2;
                        return 1000 + profondeur; // Préférer les victoires rapides
                    }

                    int eval = MinMax(profondeur - 1, false, alpha, beta);
                    matrice_map[ligne, coup] = 2;

                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                        break; // Élagage beta
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                foreach (int coup in coupsPossibles)
                {
                    int ligne = SimulerCoup(coup, 0);
                    if (ligne == -1) continue;

                    // Vérifier victoire joueur
                    if (VerifierVictoire(ligne, coup))
                    {
                        matrice_map[ligne, coup] = 2;
                        return -1000 - profondeur; // Pénaliser les défaites rapides
                    }

                    int eval = MinMax(profondeur - 1, true, alpha, beta);
                    matrice_map[ligne, coup] = 2;

                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                        break; // Élagage alpha
                }
                return minEval;
            }
        }

        private List<int> GetCoupsPossibles()
        {
            List<int> coups = [];
            for (int j = 0; j < 7; j++)
            {
                if (matrice_map[0, j] == 2) // Vérifier que la colonne n'est pas pleine
                {
                    coups.Add(j);
                }
            }
            return coups;
        }

        private int SimulerCoup(int colonne, int joueur) // Simule l'ajout d'un pion de "joueur" dans la colonne passé en paramètre
        {
            for (int ligne = 5; ligne >= 0; ligne--)
            {
                if (matrice_map[ligne, colonne] == 2)
                {
                    matrice_map[ligne, colonne] = joueur;
                    return ligne;
                }
            }
            return -1; // Colonne pleine
        }

        private bool EstCoupGagnant(int colonne, int joueur) // Vérifie si la matrice possède une victoire après une simulation de coup
        {
            int ligne = SimulerCoup(colonne, joueur);
            if (ligne == -1) return false;

            bool estGagnant = VerifierVictoire(ligne, colonne);
            matrice_map[ligne, colonne] = 2; // Annuler le coup
            return estGagnant;
        }

        // Évalue la position actuelle de la grille pour attribuer un score à l'IA
        private int EvaluerPosition()
        {
            int score = 0;
            int robot = 1;
            int humain = 0;

            // Évaluer le centre (les colonnes centrales sont stratégiques)
            for (int i = 0; i < 6; i++)
            {
                if (matrice_map[i, 3] == robot) score += 3;
                if (matrice_map[i, 3] == humain) score -= 3;
            }

            // Évaluer les alignements de 2 et 3 jetons
            score += CompterAlignements(robot, 2) * 2;
            score += CompterAlignements(robot, 3) * 5;
            score -= CompterAlignements(humain, 2) * 2;
            score -= CompterAlignements(humain, 3) * 5;

            return score;
        }

        // Compte le nombre d’alignements (de longueur passé en parametre ) pour un joueur sur la grille

        private int CompterAlignements(int joueur, int longueur)
        {
            int count = 0;

            // Vérifier toutes les directions
            count += CompterAlignementsDirection(joueur, longueur, 0, 1);  // Horizontal
            count += CompterAlignementsDirection(joueur, longueur, 1, 0);  // Vertical
            count += CompterAlignementsDirection(joueur, longueur, 1, 1);  // Diagonale \
            count += CompterAlignementsDirection(joueur, longueur, 1, -1); // Diagonale /

            return count;
        }
        // Parcourt la grille et compte les alignements d’une certaine longueur dans une direction donnée
        private int CompterAlignementsDirection(int joueur, int longueur, int dLigne, int dColonne)
        {
            int count = 0;

            for (int ligne = 0; ligne < 6; ligne++)
            {
                for (int colonne = 0; colonne < 7; colonne++)
                {
                    if (EstAlignementValide(ligne, colonne, joueur, longueur, dLigne, dColonne))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        // Vérifie si un alignement d’une certaine longueur est valide pour un joueur depuis une position donnée
        private bool EstAlignementValide(int ligne, int colonne, int joueur, int longueur, int dLigne, int dColonne)
        {
            // Vérifier les limites
            int finLigne = ligne + (longueur - 1) * dLigne;
            int finColonne = colonne + (longueur - 1) * dColonne;

            if (finLigne < 0 || finLigne >= 6 || finColonne < 0 || finColonne >= 7)
                return false;

            // Vérifier l'alignement
            for (int i = 0; i < longueur; i++)
            {
                int l = ligne + i * dLigne;
                int c = colonne + i * dColonne;

                if (matrice_map[l, c] != joueur)
                    return false;
            }

            return true;
        }
    }
}