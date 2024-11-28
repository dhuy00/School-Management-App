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
    public partial class fEditAssignment : Form
    {
        string teacher_ID;
        string course_ID;
        string semester;
        string year;
        string curriculum_ID;
        Database database;
        public fEditAssignment(string teacherID, string courseID, string semester, string year, string curriculumID)
        {
            InitializeComponent();
            database = Database.getInstance();
            this.teacher_ID = teacherID;
            this.course_ID = courseID;
            this.semester = semester;
            this.year = year;
            this.curriculum_ID = curriculumID;
        }

        public void setAssignMent()
        {
            DataRow assignmentInfo = database.GetAssignment(teacher_ID, course_ID, semester, year, curriculum_ID);
            if (assignmentInfo != null)
            {
                txtBoxTeacherID.Text = assignmentInfo["MAGV"].ToString();
                txtBoxCourseID.Text = assignmentInfo["MAHP"].ToString();
                txtBoxSemester.Text = assignmentInfo["HK"].ToString();
                txtBoxYear.Text = assignmentInfo["NAM"].ToString();
                txtBoxCurriculumID.Text = assignmentInfo["MACT"].ToString();
            }
        }

        private void fEditAssignment_Load(object sender, EventArgs e)
        {
            setAssignMent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            database.EditAssignment(teacher_ID, course_ID, semester, year, curriculum_ID, txtBoxTeacherID.Text, txtBoxCourseID.Text, txtBoxSemester.Text, txtBoxYear.Text, txtBoxCurriculumID.Text);
        }
    }
}
