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
    public partial class fTeacherRegister : Form
    {
        Database database;
        string emp_role;
        public fTeacherRegister()
        {
            InitializeComponent();
            database = Database.getInstance();
            this.emp_role = database.getCurrentRole();
        }

        private void AddButtonColumn(string colName, string colText)
        {
            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.Name = colName;
            btnColumn.HeaderText = "";
            btnColumn.Text = colText;
            btnColumn.UseColumnTextForButtonValue = true;
            btnColumn.Width = 250;
            gridTeacherRegister.Columns.Add(btnColumn);
             
        }


        public void getTeacherRegister()
        {


            if (emp_role == "GIANGVIEN" || emp_role == "TRUONGDONVI")
            {
                DataTable registers = database.GetTeacherRegister();
                gridTeacherRegister.DataSource = registers;
                AddButtonColumn("ScoreEdit", "Chỉnh sửa điểm");
              //  gridTeacherRegister.Columns["ScoreEdit"].Width = 150;
                btnAddRegister.Visible = false;
            }
            else if (emp_role == "GIAOVU")
            {
                DataTable registers = database.GetRegister();
                gridTeacherRegister.DataSource = registers;
                AddButtonColumn("Delete", "Xóa");
            }
            else if(emp_role == "TRUONGKHOA")
            {
                DataTable registers = database.GetRegister();
                gridTeacherRegister.DataSource = registers;
                btnAddRegister.Visible = false;
            }
            
        }

        private void fTeacherRegister_Load(object sender, EventArgs e)
        {
            getTeacherRegister();

        }
         
        private void gridTeacherRegister_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(emp_role == "GIANGVIEN" && e.ColumnIndex == gridTeacherRegister.Columns["ScoreEdit"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = gridTeacherRegister.Rows[e.RowIndex];
                string studentID = row.Cells["MASV"].Value.ToString();
                string teacherID = row.Cells["MAGV"].Value.ToString();
                string courseID = row.Cells["MAHP"].Value.ToString();
                string semester = row.Cells["HK"].Value.ToString();
                string year = row.Cells["NAM"].Value.ToString();
                string curriculumID = row.Cells["MACT"].Value.ToString();


                fStudentScoreEdit f = new fStudentScoreEdit(studentID, teacherID, courseID, semester, year, curriculumID);
                this.Hide();
                f.ShowDialog();
                this.Show();

                fTeacherRegister form = new fTeacherRegister();
                this.Dispose();
                form.ShowDialog();
            }

            if (emp_role == "GIAOVU" && e.ColumnIndex == gridTeacherRegister.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = gridTeacherRegister.Rows[e.RowIndex];
                string studentID = row.Cells["MASV"].Value.ToString();
                string teacherID = row.Cells["MAGV"].Value.ToString();
                string courseID = row.Cells["MAHP"].Value.ToString();
                string semester = row.Cells["HK"].Value.ToString();
                string year = row.Cells["NAM"].Value.ToString();
                string curriculumID = row.Cells["MACT"].Value.ToString();

                database.DeleteResgiter(studentID, teacherID, courseID, semester, year, curriculumID);

                fTeacherRegister form = new fTeacherRegister();
                this.Dispose();
                form.ShowDialog();
            }
        }

        private void btnAddRegister_Click(object sender, EventArgs e)
        {
            fAddRegister f = new fAddRegister();
            this.Hide();
            f.ShowDialog();
            this.Show();

            fTeacherRegister form = new fTeacherRegister();
            this.Dispose();
            form.ShowDialog();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
