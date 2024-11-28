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
    public partial class fAddEditAssignment : Form
    {
        Database database;
        string emp_role;
        Exception ex;
        public fAddEditAssignment()
        {
            InitializeComponent();
            database = Database.getInstance();
            emp_role = database.getCurrentRole();
        }

        private void AddButtonColumn(string colName, string colText)
        {
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.Name = colName;
            buttonColumn.HeaderText = "";
            buttonColumn.Text = colText;
            buttonColumn.UseColumnTextForButtonValue = true;
            gridAssignment.Columns.Add(buttonColumn);
        }

        public void setData()
        {
            DataTable assignments = new DataTable();
            if(emp_role == "GIAOVU")
            {
                assignments = database.GetFacultyAssignment();
                gridAssignment.DataSource = assignments;
                AddButtonColumn("EditButton", "Chỉnh sửa");
                btnAddAssignment.Visible = false;
            }
            else if(emp_role == "TRUONGDONVI")
            {
                assignments = database.getTDVPhanCongForEdit();
                gridAssignment.DataSource = assignments;
                AddButtonColumn("EditButton", "Chỉnh sửa");
                AddButtonColumn("DeleteButton", "Xóa");
            }
            else if(emp_role == "TRUONGKHOA")
            {
                assignments = database.getTKPhanCongForEdit();
                gridAssignment.DataSource = assignments;
                AddButtonColumn("EditButton", "Chỉnh sửa");
                AddButtonColumn("DeleteButton", "Xóa");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fAddEditAssignment_Load(object sender, EventArgs e)
        {
            setData();
        }

        private void gridAssignment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gridAssignment.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = gridAssignment.Rows[e.RowIndex];
                string maGV = row.Cells["MAGV"].Value.ToString();
                string maHP = row.Cells["MAHP"].Value.ToString();
                string HK = row.Cells["HK"].Value.ToString();
                string year = row.Cells["NAM"].Value.ToString();
                string maCT = row.Cells["MACT"].Value.ToString();
                fEditPhanCong f = new fEditPhanCong(maGV, maHP, HK, year, maCT);
                this.Hide();
                f.ShowDialog();

                fAddEditAssignment form = new fAddEditAssignment();
                this.Dispose();
                form.ShowDialog();
            }
            else if (emp_role != "GIAOVU" && e.ColumnIndex == gridAssignment.Columns["DeleteButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = gridAssignment.Rows[e.RowIndex];
                string maGV = row.Cells["MAGV"].Value.ToString();
                string maHP = row.Cells["MAHP"].Value.ToString();
                string HK = row.Cells["HK"].Value.ToString();
                string year = row.Cells["NAM"].Value.ToString();
                string maCT = row.Cells["MACT"].Value.ToString();
                database.deleteARowPhanCong(maGV, maHP, HK, year, maCT, ref ex);
                if (ex != null)
                {
                    MessageBox.Show("Hien da co giang vien dang ky!!!");
                }
                fAddEditAssignment form = new fAddEditAssignment();
                this.Dispose();
                form.ShowDialog();
            }
        }

        private void btnAddAssignment_Click(object sender, EventArgs e)
        {
            fAddPhanCong f = new fAddPhanCong();
            this.Hide();
            f.ShowDialog();

            fAddEditAssignment form = new fAddEditAssignment();
            this.Dispose();
            form.ShowDialog();
        }
    }
}
