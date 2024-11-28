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
    public partial class fDepartment : Form
    {
        Database database;
        string emp_role;
        public fDepartment()
        {
            InitializeComponent();
            database = Database.getInstance();
            this.emp_role = database.getCurrentRole();
        }

        private void AddEditButtonColumn(string colName, string colText)
        {
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.Name = colName;
            buttonColumn.HeaderText = "";
            buttonColumn.Text = colText;
            buttonColumn.UseColumnTextForButtonValue = true;
            gridDepartments.Columns.Add(buttonColumn);
        }

        public void getALlDepartment()
        {
            DataTable departments = database.GetAllDepartments();
            gridDepartments.DataSource = departments;

            if (emp_role == "GIAOVU")
            {
                AddEditButtonColumn("EditButton", "Cập nhật");
            }
            else
            {
                button1.Visible = false;
            }
        }

        private void fDepartment_Load(object sender, EventArgs e)
        {
            getALlDepartment();
        }

        private void gridDepartments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gridDepartments.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = gridDepartments.Rows[e.RowIndex];
                string departmentID = row.Cells["MADV"].Value.ToString();


                fAddEditDepartment f = new fAddEditDepartment(departmentID);
                this.Hide();
                f.ShowDialog();
                this.Show();

                fDepartment form = new fDepartment();
                this.Dispose();
                form.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fAddEditDepartment f = new fAddEditDepartment(string.Empty);
            this.Hide();
            f.ShowDialog();
            this.Show();

            fDepartment form = new fDepartment();
            this.Dispose();
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
