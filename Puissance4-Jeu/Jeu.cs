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
        private int adversaire;
        public Jeu(int adversaire)
        {
            InitializeComponent();
            this.adversaire = adversaire;
        }

        private void Jeu_Load(object sender, EventArgs e)
        {
            // faire une class
            //int[,] matrice = new int[6, 7];
            this.textBox1.Text = adversaire == 0 ? "Joueur" : "Robot";
            affichage();
        }

        private void affichage()
        {

        }

        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {

            PictureBox pb = sender as PictureBox;

            if (pb != null)
            {
                PictureBox nouveau = new();
                nouveau.Image = Properties.Resources.jeton_rouge;
                nouveau.SizeMode = PictureBoxSizeMode.Zoom;
                nouveau.Width = 50;    
                nouveau.Height = 50;
                nouveau.Location = new Point(e.X, e.Y);

                this.Controls.Add(nouveau);
                nouveau.BringToFront();
            }   
        }
    }
}
