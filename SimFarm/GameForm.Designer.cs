namespace SimFarm
{
    partial class GameForm
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
            this.btnDanielForm = new System.Windows.Forms.Button();
            this.btnAngelaForm = new System.Windows.Forms.Button();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDanielForm
            // 
            this.btnDanielForm.Location = new System.Drawing.Point(90, 101);
            this.btnDanielForm.Name = "btnDanielForm";
            this.btnDanielForm.Size = new System.Drawing.Size(75, 23);
            this.btnDanielForm.TabIndex = 0;
            this.btnDanielForm.Text = "10 X 10";
            this.btnDanielForm.UseVisualStyleBackColor = true;
            this.btnDanielForm.Click += new System.EventHandler(this.btnDanielForm_Click);
            // 
            // btnAngelaForm
            // 
            this.btnAngelaForm.Location = new System.Drawing.Point(90, 150);
            this.btnAngelaForm.Name = "btnAngelaForm";
            this.btnAngelaForm.Size = new System.Drawing.Size(75, 23);
            this.btnAngelaForm.TabIndex = 1;
            this.btnAngelaForm.Text = "20 X 5";
            this.btnAngelaForm.UseVisualStyleBackColor = true;
            this.btnAngelaForm.Click += new System.EventHandler(this.btnAngelaForm_Click);
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(38, 50);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(183, 13);
            this.lblPrompt.TabIndex = 2;
            this.lblPrompt.Text = "Which grid would you like to play on?";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 239);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.btnAngelaForm);
            this.Controls.Add(this.btnDanielForm);
            this.Name = "GameForm";
            this.Text = "GameForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDanielForm;
        private System.Windows.Forms.Button btnAngelaForm;
        private System.Windows.Forms.Label lblPrompt;
    }
}