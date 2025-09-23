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
        private const int margeVerticale = 30; // marge en haut et en bas

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

            this.DoubleBuffered = true;
        }

        private void Jeu_Load(object sender, EventArgs e)
        {
            boardPanel.Invalidate();
        }

        private void BoardPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Prend en compte la marge verticale pour calculer la taille
            int cellW = (boardPanel.ClientSize.Width - (colonnes - 1) * marge) / colonnes;
            int cellH = (boardPanel.ClientSize.Height - (lignes - 1) * marge - 2 * margeVerticale) / lignes;
            diametre = Math.Max(4, Math.Min(cellW, cellH));

            int largeurPlateau = colonnes * diametre + (colonnes - 1) * marge;
            int hauteurPlateau = lignes * diametre + (lignes - 1) * marge;
            int offsetX = (boardPanel.ClientSize.Width - largeurPlateau) / 2;
            int offsetY = margeVerticale; // fixe la marge en haut

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    int x = offsetX + j * (diametre + marge);
                    int y = offsetY + i * (diametre + marge);

                    Brush b = classe_puissance4.GetMatrice()[i, j] switch
                    {
                        1 => Brushes.Yellow,
                        0 => Brushes.Red,
                        _ => Brushes.White,
                    };

                    g.FillEllipse(b, x, y, diametre, diametre);
                    g.DrawEllipse(Pens.Black, x, y, diametre, diametre);
                }
            }
        }

        private void BoardPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // Même calcul que dans Paint
            int cellW = (boardPanel.ClientSize.Width - (colonnes - 1) * marge) / colonnes;
            int cellH = (boardPanel.ClientSize.Height - (lignes - 1) * marge - 2 * margeVerticale) / lignes;
            int d = Math.Min(cellW, cellH);

            int largeurPlateau = colonnes * d + (colonnes - 1) * marge;
            int hauteurPlateau = lignes * d + (lignes - 1) * marge;
            int offsetX = (boardPanel.ClientSize.Width - largeurPlateau) / 2;
            int offsetY = margeVerticale; // même marge en haut

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    Rectangle r = new(
                        offsetX + j * (d + marge),
                        offsetY + i * (d + marge),
                        d, d
                    );

                    if (r.Contains(e.Location))
                    {
                        (int, int) i_jeton = classe_puissance4.ouVaLeJeton(j);
                        if (i_jeton.Item1 != -1)
                        {
                            // il y a de la place
                            r = new Rectangle(
                                offsetX + j * (d + marge),
                                offsetY + i_jeton.Item2 * (d + marge),
                                d, d
                            );

                            boardPanel.Invalidate(r);
                            if (i_jeton.Item1 == -2)
                            {
                                MessageBox.Show("Le " + textBox1.Text + " a gagné");
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
                        return;
                    }
                }
            }
        }
    }
}
