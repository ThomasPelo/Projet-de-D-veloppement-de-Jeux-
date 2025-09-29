namespace Puissance4_Jeu
{
    partial class Jeu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            labelTimer = new Label();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            textBox1.Location = new Point(50, 68);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(223, 30);
            textBox1.TabIndex = 0;
            textBox1.TabStop = false;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            textBox2.Location = new Point(50, 25);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(223, 30);
            textBox2.TabIndex = 2;
            textBox2.TabStop = false;
            textBox2.Text = "Au tour de :";
            // 
            // labelTimer
            // 
            labelTimer.AutoSize = true;
            labelTimer.BackColor = Color.Transparent;
            labelTimer.Location = new Point(50, 118);
            labelTimer.Name = "labelTimer";
            labelTimer.Size = new Size(59, 20);
            labelTimer.TabIndex = 3;
            labelTimer.Text = "Temps :";
            labelTimer.Click += label1_Click;
            // 
            // Jeu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Highlight;
            ClientSize = new Size(1902, 1033);
            Controls.Add(labelTimer);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Name = "Jeu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Jeu";
            FormClosing += Jeu_FormClosing;
            Load += Jeu_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Label labelTimer;
    }
}