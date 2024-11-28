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
    public partial class fCourse : Form
    {
        Database database;
        string emp_role;
        public fCourse()
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
            gridCourses.Columns.Add(buttonColumn);
        }

        public void getAllCourses()
        {
            DataTable courses = database.GetAllCourse();
            gridCourses.DataSource = courses;

            gridCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (gridCourses.Columns["TENHP"] != null)
            {
                gridCourses.Columns["TENHP"].Width = 300; 
            }
            if (emp_role == "GIAOVU")
            {
                AddEditButtonColumn("EditButton", "Cập nhật");
            }
            else
            {
                btnAddCourse.Visible = false;
            }

        }

        private void fCourse_Load(object sender, EventArgs e)
        {
            getAllCourses();
        }

        private void gridCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gridCourses.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = gridCourses.Rows[e.RowIndex];
                string courseID = row.Cells["MAHP"].Value.ToString();


                fAddEditCourse f = new fAddEditCourse(courseID);
                this.Hide();
                f.ShowDialog();
                this.Show();

                fCourse form = new fCourse();
                this.Dispose();
                form.ShowDialog();
            }
        }

        private void btnAddCourse_Click(object sender, EventArgs e)
        {
            fAddEditCourse f = new fAddEditCourse(string.Empty);
            this.Hide();
            f.ShowDialog();
            this.Show();

            fCourse form = new fCourse();
            this.Dispose();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
