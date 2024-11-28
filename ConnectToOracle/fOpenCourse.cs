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
    public partial class fOpenCourse : Form
    {
        Database database;
        string emp_role;
        public fOpenCourse()
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
            gridOpenCourses.Columns.Add(buttonColumn);
        }

        public void getAllOpenCourse()
        {
            DataTable openCourses = database.GetAllOpenCourse();
            gridOpenCourses.DataSource = openCourses;

            if (emp_role == "GIAOVU")
            {
                AddEditButtonColumn("EditButton", "Cập nhật");
            }
            else
            {
                btnAddOpenCourse.Visible = false;
            }
        }

        private void fOpenCourse_Load(object sender, EventArgs e)
        {
            getAllOpenCourse();
        }

        private void gridOpenCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gridOpenCourses.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = gridOpenCourses.Rows[e.RowIndex];
                string courseID = row.Cells["MAHP"].Value.ToString();
                string semester = row.Cells["HK"].Value.ToString();
                string year = row.Cells["NAM"].Value.ToString();
                string curriculum = row.Cells["MACT"].Value.ToString();


                fAddEditOpenCourse f = new fAddEditOpenCourse(courseID, semester, year, curriculum);
                this.Hide();
                f.ShowDialog();
                this.Show();

                fOpenCourse form = new fOpenCourse();
                this.Dispose();
                form.ShowDialog();
            }
        }

        private void btnAddOpenCourse_Click(object sender, EventArgs e)
        {
            fAddEditOpenCourse f = new fAddEditOpenCourse(string.Empty, string.Empty, string.Empty, string.Empty);
            this.Hide();
            f.ShowDialog();
            this.Show();

            fOpenCourse form = new fOpenCourse();
            this.Dispose();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
