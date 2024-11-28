namespace ConnectToOracle
{
    partial class fFGAudit
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
            this.listUsers = new System.Windows.Forms.ListView();
            this.usernameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableNameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.actionCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listUsers
            // 
            this.listUsers.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.listUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.usernameCol,
            this.tableNameCol,
            this.actionCol,
            this.timeCol});
            this.listUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.listUsers.ForeColor = System.Drawing.SystemColors.InfoText;
            this.listUsers.FullRowSelect = true;
            this.listUsers.GridLines = true;
            this.listUsers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listUsers.HideSelection = false;
            this.listUsers.Location = new System.Drawing.Point(0, 0);
            this.listUsers.MultiSelect = false;
            this.listUsers.Name = "listUsers";
            this.listUsers.Size = new System.Drawing.Size(789, 450);
            this.listUsers.TabIndex = 3;
            this.listUsers.UseCompatibleStateImageBehavior = false;
            this.listUsers.View = System.Windows.Forms.View.Details;
            // 
            // usernameCol
            // 
            this.usernameCol.Text = "UserName";
            this.usernameCol.Width = 182;
            // 
            // tableNameCol
            // 
            this.tableNameCol.Text = "TableName";
            this.tableNameCol.Width = 145;
            // 
            // actionCol
            // 
            this.actionCol.Text = "Action";
            this.actionCol.Width = 91;
            // 
            // timeCol
            // 
            this.timeCol.Text = "Time";
            this.timeCol.Width = 301;
            // 
            // fFGAudit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 450);
            this.Controls.Add(this.listUsers);
            this.Name = "fFGAudit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fFGAudit";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listUsers;
        private System.Windows.Forms.ColumnHeader usernameCol;
        private System.Windows.Forms.ColumnHeader tableNameCol;
        private System.Windows.Forms.ColumnHeader actionCol;
        private System.Windows.Forms.ColumnHeader timeCol;
    }
}