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
    public partial class fAddEditCourse : Form
    {
        Database database;
        string course_ID = string.Empty;
        public fAddEditCourse(string courseID)
        {
            InitializeComponent();
            this.course_ID = courseID;
            database = Database.getInstance();
        }

        public void setCourseInfo()
        {
            if (course_ID != string.Empty)
            {
                DataRow courseInfo = database.GetCourseByID(course_ID);
                if (courseInfo != null)
                {
                    txtBoxCourseID.Text = courseInfo["MAHP"].ToString();
                    txtBoxCourseName.Text = courseInfo["TENHP"].ToString();
                    txtBoxCredits.Text = courseInfo["SOTC"].ToString();
                    txtBoxTheory.Text = courseInfo["STLT"].ToString();
                    txtBoxLab.Text = courseInfo["STTH"].ToString();
                    txtBoxMaxStudent.Text = courseInfo["SOSVTD"].ToString();
                    txtDepartment.Text = courseInfo["MADV"].ToString();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if(course_ID != string.Empty)
            {
                database.EditCourseInfo(course_ID, txtBoxCourseID.Text, txtBoxCourseName.Text, txtBoxCredits.Text, txtBoxTheory.Text, txtBoxLab.Text, txtBoxMaxStudent.Text, txtDepartment.Text);

            }
            else
            {
                database.AddCourse(txtBoxCourseID.Text, txtBoxCourseName.Text, txtBoxCredits.Text, txtBoxTheory.Text, txtBoxLab.Text, txtBoxMaxStudent.Text, txtDepartment.Text);

            }
        }

        private void fAddEditCourse_Load(object sender, EventArgs e)
        {
            setCourseInfo();
        }
    }
}
