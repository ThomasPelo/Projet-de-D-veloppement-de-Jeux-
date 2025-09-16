using System;
using System.Drawing;
using System.Windows.Forms;

namespace Puissance4_Jeu
{
    public partial class Jeu : Form
    {
        private CLassePuissance4 classe_puissance4;
        private int diametre = 35;
        private int[,] grille;

        private Panel boardPanel;
        private const int colonnes = 7;
        private const int lignes = 6;
        private const int marge = 10;
        public Jeu(int adversaire)
        {
            InitializeComponent();
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

            grille = new int[lignes, colonnes];
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    grille[i, j] = 2;
                }
            }
        }

        private void Jeu_Load(object sender, EventArgs e)
        {
            boardPanel.Invalidate();
        }

        private void BoardPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int cellW = (boardPanel.ClientSize.Width - (colonnes - 1) * marge) / colonnes;
            int cellH = (boardPanel.ClientSize.Height - (lignes - 1) * marge) / lignes;
            diametre = Math.Max(4, Math.Min(cellW, cellH));

            int largeurPlateau = colonnes * diametre + (colonnes - 1) * marge;
            int hauteurPlateau = lignes * diametre + (lignes - 1) * marge;
            int offsetX = (boardPanel.ClientSize.Width - largeurPlateau) / 2;
            int offsetY = (boardPanel.ClientSize.Height - hauteurPlateau) / 2;

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    int x = offsetX + j * (diametre + marge);
                    int y = offsetY + i * (diametre + marge);

                    Brush b = grille[i, j] switch
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
            // Même calcul que dans Paint (important : utiliser e.Location relatif au panel)
            int cellW = (boardPanel.ClientSize.Width - (colonnes - 1) * marge) / colonnes;
            int cellH = (boardPanel.ClientSize.Height - (lignes - 1) * marge) / lignes;
            int d = Math.Min(cellW, cellH);

            int largeurPlateau = colonnes * d + (colonnes - 1) * marge;
            int hauteurPlateau = lignes * d + (lignes - 1) * marge;
            int offsetX = (boardPanel.ClientSize.Width - largeurPlateau) / 2;
            int offsetY = (boardPanel.ClientSize.Height - hauteurPlateau) / 2;

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    Rectangle r = new(
                        offsetX + j * (d + marge),
                        offsetY + i * (d + marge),
                        d, d);

                    if (r.Contains(e.Location))
                    {
                        // toggle blanc <-> rouge
                        grille[i, j] = classe_puissance4.AQuiLeTour();
                        boardPanel.Invalidate(r); // redessine juste la case cliquée
                        return;
                    }
                }
            }
        }
    }
}
