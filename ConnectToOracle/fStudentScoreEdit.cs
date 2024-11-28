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
    public partial class fStudentScoreEdit : Form
    {
        Database database;
        string student_ID;
        string teacher_ID;
        string course_ID;
        string course_semester;
        string course_year;
        string curriculum_ID;
        string processScore;
        string labScore;
        string examEndScore;
        string finalScore;
        public fStudentScoreEdit(string studentID, string teacherID, string courseID, string semester, string year, string curriculumID)
        {
            InitializeComponent();
            database = Database.getInstance();
            this.student_ID = studentID; ;
            this.teacher_ID = teacherID ;
            this.course_ID = courseID ;
            this.course_semester = semester;
            this.course_year = year ;
            this.curriculum_ID = curriculumID;
        }


        public void setInfo()
        {
            lbStudentID.Text = student_ID;
            lbTeacherID.Text = teacher_ID;
            lbCourseID.Text = course_ID;
            lbSemester.Text = course_semester;
            lbYear.Text = course_year;
            lbCurriculumID.Text = curriculum_ID;

            DataRow info = database.GetOneRegisterRow(student_ID, teacher_ID, course_ID, course_semester, course_year, curriculum_ID);
            if(info != null)
            {
                txtBoxLabScore.Text = info["DIEMTH"].ToString();
                txtBoxProcessScore.Text = info["DIEMQT"].ToString();
                txtBoxExamEndScore.Text = info["DIEMCK"].ToString();
                txtBoxFinalScore.Text = info["DIEMTK"].ToString();
            }
            else
            {
            }
            
        }

        private void fStudentScoreEdit_Load(object sender, EventArgs e)
        {
            setInfo();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.processScore = txtBoxProcessScore.Text;
            this.labScore = txtBoxLabScore.Text;
            this.examEndScore = txtBoxExamEndScore.Text;
            this.finalScore = txtBoxFinalScore.Text;

            database.EditStudentScore(student_ID, teacher_ID, course_ID, course_semester, course_year, curriculum_ID, labScore, processScore, examEndScore, finalScore);

        }
    }
}
