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
    public partial class fStudent : Form
    {
        string current_user;
        Database database;
        Exception ex = null;
        bool isShowNotifications = false;
        public fStudent(string username)
        {
            InitializeComponent();
            database = Database.getInstance();
            this.current_user = username.ToUpper();
            labelUsername.Text = "User: " + username;
        }

        public void setUserInfo()
        {
            DataRow userInfo = database.GetStudentByID(current_user);
            if (userInfo != null)
            {
                labelStuID.Text = userInfo["MASV"].ToString();
                labelStuName.Text = userInfo["HOTEN"].ToString();
                labelStuGender.Text = userInfo["PHAI"].ToString();
                labelStuBirthday.Text = userInfo["NGSINH"].ToString();
                labelStuAddr.Text = userInfo["DCHI"].ToString();
                labelStuPhone.Text = userInfo["DT"].ToString();
                labelStuProgram.Text = userInfo["MACT"].ToString();
                labelStuDepartment.Text = userInfo["MANGANH"].ToString();
                labelStuCredit.Text = userInfo["SOTCTL"].ToString();
                labelStuGrade.Text = userInfo["DTBTL"].ToString();
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin nhân viên");
                //MessageBox.Show(current_user);
            }
        }

        private void btnPhoneEdit_Click(object sender, EventArgs e)
        {
            fChangePhoneNumber f = new fChangePhoneNumber();
            this.Hide();
            f.ShowDialog();
            setUserInfo();
            this.Show();
        }

        private void fTeacher_Load(object sender, EventArgs e)
        {
            setUserInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fStudentInfo f = new fStudentInfo();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnDepartmentInfo_Click(object sender, EventArgs e)
        {
            fDepartment f = new fDepartment();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnOpenCourseInfo_Click(object sender, EventArgs e)
        {
            fOpenCourse f = new fOpenCourse();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnCourseInfo_Click(object sender, EventArgs e)
        {
            fCourse f = new fCourse();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnAssignmentInfo_Click(object sender, EventArgs e)
        {
            fTeacherAssignment f = new fTeacherAssignment();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnRegisterInfo_Click(object sender, EventArgs e)
        {
            fStudentRegister f = new fStudentRegister();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            fOpenCourseReg f = new fOpenCourseReg();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnAddressEdit_Click(object sender, EventArgs e)
        {
            fChangeAdd f = new fChangeAdd();
            this.Hide();
            f.ShowDialog();
            setUserInfo();
            this.Show();
        }

        private void notificationLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!isShowNotifications)
            {
                notificationsView.Visible = true;
                isShowNotifications = true;
                List<string> temp = database.getAllNotifications(ref ex);
                for (int i = 0; i < temp.Count; i++)
                {
                    notificationsView.Nodes.Add(temp[i].ToString());
                }
            }
            else
            {
                notificationsView.Nodes.Clear();
                notificationsView.Visible = false;
                isShowNotifications = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
