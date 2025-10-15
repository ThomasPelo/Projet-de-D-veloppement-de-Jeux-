using Puissance4_Jeu;
namespace TestClassePuissance4
{
    [TestClass]
    public sealed class ClassePuissance4Tests
    {
        [TestMethod]
        public void TourInit()
        {
            CLassePuissance4 cp4 = new(1);
            // le tour commence à 0 donc c'est le joueur 1
            Assert.AreEqual(0, cp4.GetTour());
        }

        [TestMethod]
        public void TourJ1()
        {
            CLassePuissance4 cp4 = new(1);
            // le tour commence à 0 donc c'est le joueur 1
            cp4.AQuiLeTour();
            // après cette méthode c'est le tour du joueur 2 donc tour = 1
            cp4.AQuiLeTour();
            // tour du J1
            Assert.AreEqual(0, cp4.GetTour());
        }

        [TestMethod]
        public void TourJ2()
        {
            CLassePuissance4 cp4 = new(1);
            // le tour commence à 0 donc c'est le joueur 1
            cp4.AQuiLeTour();
            // après cette méthode c'est le tour du joueur 2 donc tour = 1
            Assert.AreEqual(1, cp4.GetTour());
        }

        [TestMethod]
        public void OuVaLeJetonColonneVide()
        {
            CLassePuissance4 cp4 = new(1);
            (int,int) result = cp4.JouerCoup(0);
            // Item1 : code de retour 0 = coup normal pas de victoire, colonne pleine ou match nul
            Assert.AreEqual(0, result.Item1);
            // colonne vide donc le jeton va en bas à la ligne 5
            Assert.AreEqual(5, result.Item2);
        }

        [TestMethod]
        public void OuVaLeJetonColonneNonVide()
        {
            CLassePuissance4 cp4 = new(1);
            // on joue une fois
            cp4.JouerCoup(0);
            // on rejoue dans la même colonne
            (int, int) result = cp4.JouerCoup(0);
            Assert.AreEqual(0, result.Item1);
            // colonne non vide donc le jeton va en bas à la ligne 4
            Assert.AreEqual(4, result.Item2);
        }

        [TestMethod]
        public void OuVaLeJetonColonnePleine()
        {
            CLassePuissance4 cp4 = new(1);
            // on joue 6 fois
            for (int i = 0; i < 6; i++){
                cp4.JouerCoup(0);
            }
            // on rejoue dans la même colonne
            (int, int) result = cp4.JouerCoup(0);
            // Item1 : code de retour -1 = colonne pleine
            Assert.AreEqual(-1, result.Item1);
            // colonne pleine = -1
            Assert.AreEqual(-1, result.Item2);
        }

        [TestMethod]
        public void Victoire()
        {
            CLassePuissance4 cp4 = new(1);
            // on joue 3 fois
            for (int i = 0; i < 3; i++)
            {
                cp4.JouerCoup(0);
            }
            // on rejoue dans la même colonne pour le puissance 4
            (int, int) result = cp4.JouerCoup(0);
            // Item1 : code de retour -2 = victoire
            Assert.AreEqual(-2, result.Item1);
            // colonne du jeton qui fait puissance 4
            Assert.AreEqual(2, result.Item2);
        }

        [TestMethod]
        public void MatchNul()
        {
            
        }

        [TestMethod]
        public void MeilleurCoupRobotDefense()
        {
            CLassePuissance4 cp4 = new(1);
            // on joue 3 fois
            for (int i = 0; i < 3; i++)
            {
                cp4.JouerCoup(0);
            }
            // le robot doit jouer en 0 pour bloquer le puissance 4
            // renvoie la colonne du meilleur coup
            int result = cp4.JouerCoupRobot();
            Assert.AreEqual(2, result);

        }
    }
}
