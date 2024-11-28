namespace ConnectToOracle
{
    partial class fStandardAudit
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
            this.gridStandardAudit = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridStandardAudit)).BeginInit();
            this.SuspendLayout();
            // 
            // gridStandardAudit
            // 
            this.gridStandardAudit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStandardAudit.Location = new System.Drawing.Point(12, 12);
            this.gridStandardAudit.Name = "gridStandardAudit";
            this.gridStandardAudit.RowHeadersWidth = 51;
            this.gridStandardAudit.RowTemplate.Height = 24;
            this.gridStandardAudit.Size = new System.Drawing.Size(896, 426);
            this.gridStandardAudit.TabIndex = 0;
            // 
            // fStandardAudit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 450);
            this.Controls.Add(this.gridStandardAudit);
            this.Name = "fStandardAudit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fStandardAudit";
            this.Load += new System.EventHandler(this.fStandardAudit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridStandardAudit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridStandardAudit;
    }
}