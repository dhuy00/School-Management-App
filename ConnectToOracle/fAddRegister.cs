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
    public partial class fAddRegister : Form
    {
        Database database;
        int selected1 = -1;
        int selected2 = -1;
        Exception ex;
        public fAddRegister()
        {
            InitializeComponent();
            database = Database.getInstance();
        }

        private void AddButtonColumn(string colName, string colText)
        {
            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.Name = colName;
            btnColumn.HeaderText = "";
            btnColumn.Text = colText;
            btnColumn.UseColumnTextForButtonValue = true;
            btnColumn.Width = 250;
            gridOpenCourses.Columns.Add(btnColumn);

        }

        public void getAllStudent() 
        {
            DataTable data = database.GetStudentIdName();
            gridStudentInfo.DataSource = data;
        }

        public void getAllOpenCourse()
        {
            DataTable openCourses = database.GetAllAssignment();
            gridOpenCourses.DataSource = openCourses;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string maSV;
            string maHP;
            string HK;
            string year;
            string maCT;
            string maGV;
            if (selected1 >= 0 && selected2 >= 0)
            {
                DataGridViewRow row = gridOpenCourses.Rows[selected1];
                maGV = row.Cells["MAGV"].Value.ToString();
                maHP = row.Cells["MAHP"].Value.ToString();
                HK = row.Cells["HK"].Value.ToString();
                year = row.Cells["NAM"].Value.ToString();
                maCT = row.Cells["MACT"].Value.ToString();
                DataGridViewRow row1 = gridStudentInfo.Rows[selected2];
                maSV = row1.Cells["MASV"].Value.ToString();
                MessageBox.Show(maGV + maHP + HK + year + maCT);
                database.AddResgiter(maSV, maGV, maHP, HK, year, maCT, "0", "0", "0", "0");;

                if (ex == null)
                {
                    this.Dispose();
                }
            }
            
        }

        private void gridOpenCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gridOpenCourses.Columns["RegisterButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = gridOpenCourses.Rows[e.RowIndex];
                string teacherID = row.Cells["MAGV"].Value.ToString();
                string courseID = row.Cells["MAHP"].Value.ToString();
                string semester = row.Cells["HK"].Value.ToString();
                string year = row.Cells["NAM"].Value.ToString();
                string curriculumID = row.Cells["MACT"].Value.ToString();
                database.RegCourse(teacherID, courseID, semester, year, curriculumID);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fAddRegister_Load(object sender, EventArgs e)
        {
            getAllOpenCourse();
            getAllStudent();
        }

        private void gridStudentInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selected2 = e.RowIndex;
            }
        }

        private void gridOpenCourses_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selected1 = e.RowIndex;
            }
        }
    }
}
