using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace Puissance4_Jeu
{
    public partial class Jeu : Form
    {
        private CLassePuissance4 classe_puissance4;
        public Jeu(int adversaire)
        {
            InitializeComponent();
            classe_puissance4 = new(adversaire);
            this.textBox1.DataBindings.Add("Text", classe_puissance4, "joueur_actuel", false, DataSourceUpdateMode.OnPropertyChanged);

        }

        private void Jeu_Load(object sender, EventArgs e)
        {
            affichage();
        }

        private void affichage()
        {
            // faire 7 groupboxs qui contiennent 6 pictureboxs
            // comme ça je sais que chaque groupbox est une colonne
        }

        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            if (sender is PictureBox)
            {
                classe_puissance4.AQuiLeTour();
                PictureBox nouveau = new()
                {
                    Image = this.textBox1.Text == "Joueur 1" ? Properties.Resources.jeton_rouge : Properties.Resources.jeton_jaune,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 50,
                    Height = 50,
                    Location = new Point(e.X, e.Y)
                };

                this.Controls.Add(nouveau);
                nouveau.BringToFront();
            }
        }
    }
}
