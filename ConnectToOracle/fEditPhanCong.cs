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
    public partial class fEditPhanCong : Form
    {
        Database database;
        string teacher_ID = string.Empty;
        string course_ID = string.Empty;
        string semester = string.Empty;
        string year = string.Empty;
        string curriculum_ID = string.Empty;
        string emp_role = string.Empty;
        Exception ex;
        public fEditPhanCong(string teacherID, string courseID, string semester, string year, string curriculumID)
        {
            InitializeComponent();
            database = Database.getInstance();
            this.teacher_ID = teacherID;
            this.course_ID = courseID;
            this.semester = semester;
            this.year = year;
            this.curriculum_ID = curriculumID;
            emp_role = database.getCurrentRole();
        }

        public void setData()
        {
            if (teacher_ID != string.Empty && course_ID != string.Empty && semester != string.Empty && year != string.Empty && curriculum_ID != string.Empty)
            {
                DataRow data = database.GetAssignment(teacher_ID, course_ID, semester, year, curriculum_ID);
                if (data != null)
                {
                    txtBoxTeacherID.Text = data["MAGV"].ToString();
                    txtBoxCourseID.Text = data["MAHP"].ToString();
                    txtBoxCourseID.Enabled = false;
                    txtBoxSemester.Text = data["HK"].ToString();
                    txtBoxSemester.Enabled = false;
                    txtBoxYear.Text = data["NAM"].ToString();
                    txtBoxYear.Enabled = false;
                    txtBoxCurriculumID.Text = data["MACT"].ToString();
                    txtBoxCurriculumID.Enabled = false;

                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (this.emp_role != "GIAOVU"  && teacher_ID != string.Empty && course_ID != string.Empty && semester != string.Empty && year != string.Empty && curriculum_ID != string.Empty)
            {
                database.updateARowPhanCong(teacher_ID, course_ID, semester, year, curriculum_ID, txtBoxTeacherID.Text,ref ex);

            }
            else
            {
                database.EditAssignment(teacher_ID, course_ID, semester, year, curriculum_ID, txtBoxTeacherID.Text, txtBoxCourseID.Text, txtBoxSemester.Text, txtBoxYear.Text, txtBoxCurriculumID.Text);
            }
        }

        private void fEditPhanCong_Load(object sender, EventArgs e)
        {
            setData();
        }
    }
}
