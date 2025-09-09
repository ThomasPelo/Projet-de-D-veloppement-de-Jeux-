namespace Puissance4_Jeu
{
    public partial class Form1 : Form
    {
        private int adversaire;
        public Form1()
        {
            InitializeComponent();
            adversaire = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Jeu jeu = new(adversaire);
            jeu.Show();
            this.Hide();
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rb && rb.Checked)
            {
                adversaire = rb.Name == "radioButtonJoueur" ? 0 : 1;
            }
        }
    }
}
