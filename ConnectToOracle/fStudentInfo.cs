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
    public partial class fStudentInfo : Form
    {
        Database database;
        string emp_role;
        public fStudentInfo()
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
            gridStudentList.Columns.Add(buttonColumn);
        }


        public void getAllStudent()
        {
            DataTable students = database.GetAllStudent();
            if (students != null )
            {
                gridStudentList.DataSource = students;
                gridStudentList.Columns["MASV"].Width = 60;
                gridStudentList.Columns["PHAI"].Width = 50;
                gridStudentList.Columns["MACT"].Width = 50;
                gridStudentList.Columns["SOTCTL"].Width = 50;
                gridStudentList.Columns["DTBTL"].Width = 50;


                if (emp_role == "GIAOVU")
                {
                    AddEditButtonColumn("EditButton", "Cập nhật");
                }
            }
            if(emp_role != "GIAOVU")
            {
                button1.Visible = false;
            }
        }

        private void fStudentInfo_Load(object sender, EventArgs e)
        {
            getAllStudent();
        }

        private void gridStudentList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gridStudentList.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = gridStudentList.Rows[e.RowIndex];
                string studentID = row.Cells["MASV"].Value.ToString();


                fAddEditStudent f = new fAddEditStudent(studentID);
                this.Hide();
                f.ShowDialog();
                this.Show();

                fStudentInfo form = new fStudentInfo();
                this.Dispose();
                form.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fAddEditStudent f = new fAddEditStudent(string.Empty);
            this.Hide();
            f.ShowDialog();
            this.Show();

            fStudentInfo form = new fStudentInfo();
            this.Dispose();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
