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
    public partial class fAddEditStudent : Form
    {
        Database database;
        string student_ID = string.Empty;
        public fAddEditStudent(string studentID)
        {
            InitializeComponent();
            database = Database.getInstance();
            this.student_ID = studentID;
        }

        public void setStudentInfo()
        {
            if (student_ID != string.Empty)
            {
                DataRow studentInfo = database.GetStudentByID(student_ID);
                if (studentInfo != null)
                {
                    txtBoxStudentID.Text = studentInfo["MASV"].ToString();
                    txtBoxName.Text = studentInfo["HOTEN"].ToString();
                    txtBoxGender.Text = studentInfo["PHAI"].ToString();

                    // Chuyển đổi NGSINH từ DataRow sang DateTime và đặt vào DateTimePicker
                    DateTime birthDate;
                    if (DateTime.TryParse(studentInfo["NGSINH"].ToString(), out birthDate))
                    {
                        dateTimePickerBirthday.Value = birthDate;
                    }

                    txtBoxAddress.Text = studentInfo["DCHI"].ToString();
                    txtBoxPhone.Text = studentInfo["DT"].ToString();
                    txtBoxCurriculumID.Text = studentInfo["MACT"].ToString();
                    txtBoxMajor.Text = studentInfo["MANGANH"].ToString();
                    txtBoxCredits.Text = studentInfo["SOTCTL"].ToString();
                    txtBoxGPA.Text = studentInfo["DTBTL"].ToString();
                }
            }
        }

        private void fAddEditStudent_Load(object sender, EventArgs e)
        {
            setStudentInfo();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if(student_ID != string.Empty)
            {
                string formattedBirthday = dateTimePickerBirthday.Value.ToString("yyyy-MM-dd");
                database.EditStudentInfo(student_ID, txtBoxStudentID.Text, txtBoxName.Text, txtBoxGender.Text, formattedBirthday, txtBoxAddress.Text, txtBoxPhone.Text, txtBoxCurriculumID.Text, txtBoxMajor.Text, txtBoxCredits.Text, txtBoxGPA.Text);
            }
            else
            {
                string formattedBirthday = dateTimePickerBirthday.Value.ToString("yyyy-MM-dd");
                database.AddStudent(txtBoxStudentID.Text, txtBoxName.Text, txtBoxGender.Text, formattedBirthday, txtBoxAddress.Text, txtBoxPhone.Text, txtBoxCurriculumID.Text, txtBoxMajor.Text, txtBoxCredits.Text, txtBoxGPA.Text);
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
