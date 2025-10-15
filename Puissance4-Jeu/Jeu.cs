using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Puissance4_Jeu
{
    public partial class Jeu : Form
    {
        private readonly CLassePuissance4 classe_puissance4;
        private readonly Panel boardPanel;
        private const int colonnes = 7;
        private const int lignes = 6;
        private const int marge = 10;
        private bool partieTerminee = false;
        private System.Windows.Forms.Timer tourTimer;
        private int tempsRestant;
        private const int tempsParTour = 10;
        private bool timerActif;

        public Jeu(int adversaire)
        {
            InitializeComponent();

            this.ActiveControl = null;

            // data binding
            classe_puissance4 = new(adversaire);
            this.textBox1.DataBindings.Add("Text", classe_puissance4, "joueur_actuel", false, DataSourceUpdateMode.OnPropertyChanged);

            // affichage
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
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

            // timer 
            timerActif = true;
            tempsRestant = tempsParTour;
            tourTimer = new()
            {
                Interval = 1000
            };
            tourTimer.Tick += TourTimer_Tick;
            tourTimer.Start();
        }

        private void TourTimer_Tick(object sender, EventArgs e)
        {
            if (timerActif) tempsRestant--;

            labelTimer.Text = $"Temps restant : {tempsRestant}s";
            labelTimer.ForeColor = tempsRestant switch
            {
                > 5 => Color.Green,
                > 2 => Color.Orange,
                _ => Color.Red,
            };

            if (tempsRestant <= 0)
            {
                timerActif = false;
                tempsRestant = tempsParTour;
                MessageBox.Show("Temps écoulé ! Tour suivant.");
                classe_puissance4.AQuiLeTour();
                timerActif = true;
            }
        }


        private void Jeu_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool retourMenu = true;

            if (e.CloseReason == CloseReason.UserClosing && !partieTerminee)
            {
                DialogResult result = MessageBox.Show(
                    "Voulez-vous vraiment quitter ?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    retourMenu = false;
                }
            }

            if (retourMenu && Application.OpenForms.Count > 0)
            {
                TimerStop();
                Application.OpenForms[0].Show();
            }
        }

        private void TimerStop()
        {
            if (tourTimer != null && tourTimer.Enabled)
            {
                tourTimer.Stop();
            }
        }


        private void Jeu_Load(object sender, EventArgs e)
        {
            boardPanel.Invalidate();
        }

        private (int, int, int) GetOffsets()
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

            (int, int, int) offsets = GetOffsets();

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

        private async void BoardPanel_MouseClick(object sender, MouseEventArgs e)
        {
            (int, int, int) offsets = GetOffsets();

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
                            r = new(
                                offsets.Item1 + j * (offsets.Item3 + marge),
                                offsets.Item2 + i_jeton.Item2 * (offsets.Item3 + marge),
                                offsets.Item3, offsets.Item3
                            );
                            tempsRestant = tempsParTour;
                            // redessine juste la case cliquée
                            boardPanel.Invalidate(r);
                            if (i_jeton.Item1 == -2)
                            {
                                TimerStop();
                                MessageBox.Show("Le " + textBox1.Text + " a gagné");
                                Application.OpenForms[0].Show();
                                partieTerminee = true;
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
                            // On désactive le plateau pour que le joueur ne puisse pas cliquer
                            // pendant que le robot "réfléchit".
                            boardPanel.Enabled = false;

                            // On attend 2 secondes (2000 millisecondes) de manière asynchrone.
                            // L'interface ne se bloque pas pendant ce temps.
                            await Task.Delay(2000);

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
                                tempsRestant = tempsParTour;
                                boardPanel.Invalidate(r);
                                if (coord.Item1 == -2)
                                {
                                    // victoire
                                    TimerStop();
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

                            // On réactive le plateau pour le prochain tour du joueur.
                            boardPanel.Enabled = true;
                        }
                        return;
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}