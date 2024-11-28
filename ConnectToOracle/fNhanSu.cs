using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectToOracle
{
    public partial class fNhanSu : Form
    {
        Database database;
        Exception ex = null;
        public fNhanSu()
        {
            InitializeComponent();
            database = Database.getInstance();
            getTeacherRegister();
            AddEditButtonColumn();
            AddDeleteButtonColumn();
        }

        private void AddEditButtonColumn()
        {
            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            editButtonColumn.Name = "EditButton";
            editButtonColumn.HeaderText = "";
            editButtonColumn.Text = "Chỉnh sửa";
            editButtonColumn.UseColumnTextForButtonValue = true;
            gridNhanSu.Columns.Add(editButtonColumn);
            gridNhanSu.Columns["EditButton"].Width = 150;
        }
        private void AddDeleteButtonColumn()
        {
            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            editButtonColumn.Name = "DeleteButton";
            editButtonColumn.HeaderText = "";
            editButtonColumn.Text = "Xóa";
            editButtonColumn.UseColumnTextForButtonValue = true;
            gridNhanSu.Columns.Add(editButtonColumn);
            gridNhanSu.Columns["EditButton"].Width = 150;
        }

        public void getTeacherRegister()
        {
            DataTable registers = database.getAllNhanSuForEdit();
            gridNhanSu.DataSource = registers;
        }

        private void gridNhanSu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gridNhanSu.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = gridNhanSu.Rows[e.RowIndex];
                string maGV = row.Cells["MANV"].Value.ToString();
                fEditNhanSu f = new fEditNhanSu(maGV);
                this.Hide();
                f.ShowDialog();

                fNhanSu form = new fNhanSu();
                this.Dispose();
                form.ShowDialog();
            }
            else if (e.ColumnIndex == gridNhanSu.Columns["DeleteButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = gridNhanSu.Rows[e.RowIndex];
                string maNV = row.Cells["MANV"].Value.ToString();
                database.deleteARowNhanSu(maNV);
                if (ex != null)
                {
                    MessageBox.Show(ex.Message);
                }
                fNhanSu form = new fNhanSu();
                this.Dispose();
                form.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fAddNhanSu f = new fAddNhanSu();
            this.Hide();
            f.ShowDialog();
            fNhanSu form = new fNhanSu();
            this.Dispose();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

