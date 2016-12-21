namespace stahovanie
{
    partial class DlzniciForm
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
            this.Zrušiť = new System.Windows.Forms.Button();
            this.Stahovať = new System.Windows.Forms.Button();
            this.Ddmoznost = new System.Windows.Forms.ComboBox();
            this.lblMoznost = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.KurzListok = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Zrušiť
            // 
            this.Zrušiť.Location = new System.Drawing.Point(259, 120);
            this.Zrušiť.Name = "Zrušiť";
            this.Zrušiť.Size = new System.Drawing.Size(121, 23);
            this.Zrušiť.TabIndex = 0;
            this.Zrušiť.Text = "Zrušiť";
            this.Zrušiť.UseVisualStyleBackColor = true;
            // 
            // Stahovať
            // 
            this.Stahovať.Location = new System.Drawing.Point(105, 120);
            this.Stahovať.Name = "Stahovať";
            this.Stahovať.Size = new System.Drawing.Size(148, 23);
            this.Stahovať.TabIndex = 1;
            this.Stahovať.Text = "Stahovať";
            this.Stahovať.UseVisualStyleBackColor = true;
            // 
            // Ddmoznost
            // 
            this.Ddmoznost.FormattingEnabled = true;
            this.Ddmoznost.Items.AddRange(new object[] {
            "Sociálna poisťovňa",
            "Daňový úrad",
            "Zdravotná poisťovňa VŠZP",
            "Zdravotná poisťovňa Dôvera",
            "Zdravotná poisťovňa Union"});
            this.Ddmoznost.Location = new System.Drawing.Point(106, 31);
            this.Ddmoznost.Name = "Ddmoznost";
            this.Ddmoznost.Size = new System.Drawing.Size(275, 21);
            this.Ddmoznost.TabIndex = 2;
            // 
            // lblMoznost
            // 
            this.lblMoznost.AutoSize = true;
            this.lblMoznost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblMoznost.Location = new System.Drawing.Point(4, 39);
            this.lblMoznost.Name = "lblMoznost";
            this.lblMoznost.Size = new System.Drawing.Size(55, 13);
            this.lblMoznost.TabIndex = 4;
            this.lblMoznost.Text = "Dlžníci :";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            // 
            // KurzListok
            // 
            this.KurzListok.FormattingEnabled = true;
            this.KurzListok.Items.AddRange(new object[] {
            "Kurzový lístok NBS"});
            this.KurzListok.Location = new System.Drawing.Point(106, 69);
            this.KurzListok.Name = "KurzListok";
            this.KurzListok.Size = new System.Drawing.Size(275, 21);
            this.KurzListok.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(4, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Kurzový lístok :";
            // 
            // DlzniciForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 156);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.KurzListok);
            this.Controls.Add(this.lblMoznost);
            this.Controls.Add(this.Ddmoznost);
            this.Controls.Add(this.Stahovať);
            this.Controls.Add(this.Zrušiť);
            this.Name = "DlzniciForm";
            this.Text = "Master_Sync";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Zrušiť;
        private System.Windows.Forms.Button Stahovať;
        private System.Windows.Forms.ComboBox Ddmoznost;
        private System.Windows.Forms.Label lblMoznost;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ComboBox KurzListok;
        private System.Windows.Forms.Label label1;
    }
}

