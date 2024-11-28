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
    public partial class fAddEditOpenCourse : Form
    {
        Database database;
        string course_ID = string.Empty;
        string semester = string.Empty;
        string year = string.Empty;
        string curriculum_ID = string.Empty;
        public fAddEditOpenCourse(string courseID, string semester, string year, string curriculumID)
        {
            InitializeComponent();
            database = Database.getInstance();
            this.course_ID = courseID;
            this.semester = semester;
            this.year = year;
            this.curriculum_ID = curriculumID;
        }

        public void setOpenCourseInfo()
        {
            if (course_ID != string.Empty && semester != string.Empty && year != string.Empty && curriculum_ID != string.Empty )
            {
                DataRow openCourseInfo = database.GetOpenCourseByID(course_ID, semester, year, curriculum_ID);
                if (openCourseInfo != null)
                {
                    txtBoxCourseID.Text = openCourseInfo["MAHP"].ToString();
                    txtBoxSemester.Text = openCourseInfo["HK"].ToString();
                    txtBoxYear.Text = openCourseInfo["NAM"].ToString();
                    txtBoxCurriculumID.Text = openCourseInfo["MACT"].ToString();
                }
            }
        }

        private void fAddEditOpenCourse_Load(object sender, EventArgs e)
        {
            setOpenCourseInfo();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (course_ID != string.Empty && semester != string.Empty && year != string.Empty && curriculum_ID != string.Empty)
            {
                database.EditOpenCourseInfo(course_ID, semester, year, curriculum_ID, txtBoxCourseID.Text, txtBoxSemester.Text, txtBoxYear.Text, txtBoxCurriculumID.Text);

            }
            else
            {
                database.AddOpenCourse(txtBoxCourseID.Text, txtBoxSemester.Text, txtBoxYear.Text, txtBoxCurriculumID.Text);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
