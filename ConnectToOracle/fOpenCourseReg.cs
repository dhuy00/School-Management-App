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
    public partial class fOpenCourseReg : Form
    {
        Database database;
        public fOpenCourseReg()
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

        public void getAllOpenCourse()
        {
            DataTable openCourses = database.GetAllOpenCourseWithTeacher();
            gridOpenCourses.DataSource = openCourses;
            AddButtonColumn("RegisterButton", "Đăng ký");
        }

        private void fOpenCourse_Load(object sender, EventArgs e)
        {
            getAllOpenCourse();          
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
    }
}
