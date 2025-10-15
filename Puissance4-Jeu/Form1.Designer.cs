namespace Puissance4_Jeu
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            textBox1 = new TextBox();
            textBox3 = new TextBox();
            radioButtonJoueur = new RadioButton();
            radioButton2 = new RadioButton();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Cursor = Cursors.Hand;
            button1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            button1.ForeColor = Color.Black;
            button1.Location = new Point(319, 285);
            button1.Name = "button1";
            button1.Size = new Size(180, 74);
            button1.TabIndex = 0;
            button1.Text = "Jouer";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            textBox1.Location = new Point(259, 56);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(300, 30);
            textBox1.TabIndex = 1;
            textBox1.Text = "Puissance 4";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            textBox3.BackColor = SystemColors.Control;
            textBox3.BorderStyle = BorderStyle.FixedSingle;
            textBox3.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            textBox3.Location = new Point(259, 111);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(300, 30);
            textBox3.TabIndex = 3;
            textBox3.Text = "Choissisez votre adversaire :";
            textBox3.TextAlign = HorizontalAlignment.Center;
            // 
            // radioButtonJoueur
            // 
            radioButtonJoueur.AutoSize = true;
            radioButtonJoueur.Checked = true;
            radioButtonJoueur.Cursor = Cursors.Hand;
            radioButtonJoueur.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            radioButtonJoueur.Location = new Point(307, 183);
            radioButtonJoueur.Name = "radioButtonJoueur";
            radioButtonJoueur.Size = new Size(100, 29);
            radioButtonJoueur.TabIndex = 4;
            radioButtonJoueur.TabStop = true;
            radioButtonJoueur.Text = "Joueur";
            radioButtonJoueur.UseVisualStyleBackColor = true;
            radioButtonJoueur.CheckedChanged += radioButton_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Cursor = Cursors.Hand;
            radioButton2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            radioButton2.Location = new Point(441, 183);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(89, 29);
            radioButton2.TabIndex = 5;
            radioButton2.Text = "Robot";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(800, 450);
            Controls.Add(radioButton2);
            Controls.Add(radioButtonJoueur);
            Controls.Add(textBox3);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private TextBox textBox3;
        private RadioButton radioButtonJoueur;
        private RadioButton radioButton2;
    }
}