using System;
using System.Drawing;
using System.Windows.Forms;

namespace Puissance4_Jeu
{
    public partial class Jeu : Form
    {
        private CLassePuissance4 classe_puissance4;
        private int diametre = 35;
        private Panel boardPanel;
        private const int colonnes = 7;
        private const int lignes = 6;
        private const int marge = 10;

        public Jeu(int adversaire)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None; // si tu veux cacher la barre Windows

            classe_puissance4 = new(adversaire);
            this.textBox1.DataBindings.Add("Text", classe_puissance4, "joueur_actuel", false, DataSourceUpdateMode.OnPropertyChanged);

            boardPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Blue
            };

            boardPanel.Paint += BoardPanel_Paint;
            boardPanel.MouseClick += BoardPanel_MouseClick;
            boardPanel.Resize += (s, e) => boardPanel.Invalidate();

            this.Controls.Add(boardPanel);
            boardPanel.SendToBack();

            //this.DoubleBuffered = true;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void Jeu_Load(object sender, EventArgs e)
        {
            boardPanel.Invalidate();
        }

        private (int,int,int) getOffsets()
        {
            int cellW = (boardPanel.ClientSize.Width - (colonnes - 1) * marge) / colonnes;
            int cellH = (boardPanel.ClientSize.Height - (lignes - 1) * marge) / lignes;
            int diametre = Math.Min(cellW, cellH);

            int largeurPlateau = colonnes * diametre + (colonnes - 1) * marge;
            int hauteurPlateau = lignes * diametre + (lignes - 1) * marge;
            int offsetX = (boardPanel.ClientSize.Width - largeurPlateau) / 2;
            int offsetY = (boardPanel.ClientSize.Height - hauteurPlateau) / 2;
            return (offsetX, offsetY, diametre);
        }

        private void BoardPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            (int, int, int) offsets = getOffsets();

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    int x = offsets.Item1 + j * (offsets.Item3 + marge);
                    int y = offsets.Item2 + i * (offsets.Item3 + marge);

                    Brush b = classe_puissance4.GetMatrice()[i, j] switch
                    {
                        1 => Brushes.Yellow,
                        0 => Brushes.Red,
                        _ => Brushes.White,
                    };

                    g.FillEllipse(b, x, y, offsets.Item3, offsets.Item3);
                    g.DrawEllipse(Pens.Black, x, y, offsets.Item3, offsets.Item3);
                }
            }
        }

        private void BoardPanel_MouseClick(object sender, MouseEventArgs e)
        {
            (int,int,int) offsets = getOffsets();
            

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    Rectangle r = new(
                        offsets.Item1 + j * (offsets.Item3 + marge),
                        offsets.Item2 + i * (offsets.Item3 + marge),
                        offsets.Item3, offsets.Item3
                    );

                    if (r.Contains(e.Location))
                    {
                        // on a appuyé sur une colonne mais ou va le jeton ?
                        (int, int) i_jeton = classe_puissance4.ouVaLeJeton(j);
                        if (i_jeton.Item1 != -1)
                        {
                            // il y a de la place
                            Rectangle jeton = new(
                                offsets.Item1 + j * (offsets.Item3 + marge),
                                offsets.Item2 + i_jeton.Item2 * (offsets.Item3 + marge),
                                offsets.Item3, offsets.Item3
                            );
                            // redessine juste la case cliquée
                            boardPanel.Invalidate(jeton);
                            if (i_jeton.Item1 == -2)
                            {
                                MessageBox.Show("Le " + textBox1.Text + " a gagné");
                                Application.OpenForms[0].Show();
                                this.Close();
                            }
                            else
                            {
                                classe_puissance4.AQuiLeTour();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Colonne pleine !");
                        }

                        // si robot
                        if (classe_puissance4.GetAdversaire() == "Robot")
                        {
                            // tour de l'ordi
                            int col_ordi = classe_puissance4.JouerCoupRobot();
                            (int, int) coord = classe_puissance4.JouerCoup(col_ordi);
                            if (coord.Item1 != 1)
                            {
                                r = new Rectangle(
                                    offsets.Item1 + col_ordi * (offsets.Item3 + marge),
                                    offsets.Item2 + coord.Item2 * (offsets.Item3 + marge),
                                    offsets.Item3, offsets.Item3
                                );
                                boardPanel.Invalidate(r);
                                if (coord.Item1 == -2)
                                {
                                    // victoire
                                    MessageBox.Show("L'ordinateur a gagné");
                                    Application.OpenForms[0].Show();
                                    this.Close();
                                }
                                else
                                {
                                    // prochain tour
                                    classe_puissance4.AQuiLeTour();
                                }
                            }
                        }
                        return;
                    }
                }
            }
        }
    }
}