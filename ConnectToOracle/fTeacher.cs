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
    public partial class fTeacher : Form
    {
        string current_user;
        Database database;
        string emp_role;
        Exception ex = null;
        bool isShowNotifications = false;
        public fTeacher(string username)
        {
            InitializeComponent();
            database = Database.getInstance();
            this.current_user = username.ToUpper();
            labelUsername.Text = "User: " + username;
            this.emp_role = database.getCurrentRole();
        }

        public void setUserInfo()
        {
            DataRow userInfo = database.GetEmployeeByID(current_user);
            if (userInfo != null)
            {
                labelEmpID.Text = userInfo["MANV"].ToString();
                labelEmpName.Text = userInfo["HOTEN"].ToString();
                labelEmpGender.Text = userInfo["PHAI"].ToString();
                labelEmpBirthday.Text = userInfo["NGSINH"].ToString();
                labelEmpAllowance.Text = userInfo["PHUCAP"].ToString();
                labelEmpPhone.Text = userInfo["DT"].ToString();
                labelEmpRole.Text = userInfo["VAITRO"].ToString();
                labelEmpDepartment.Text = userInfo["MADV"].ToString();
                if (emp_role == "NHANVIENCOBAN")
                {
                    btnAssignmentInfo.Visible = false;
                    btnAssignmentAddDelEdit.Visible = false;
                    btnRegisterInfo.Visible = false;
                    editNhanSutBtn.Visible = false;
                }
                else if (emp_role == "GIANGVIEN")
                {
                    btnAssignmentAddDelEdit.Visible = false;
                    editNhanSutBtn.Visible = false;
                }
                else if(emp_role == "TRUONGDONVI" || emp_role == "GIAOVU")
                {
                    editNhanSutBtn.Visible = false;
                }
            }
        }

        private void btnPhoneEdit_Click(object sender, EventArgs e)
        {
            fPhoneEdit f = new fPhoneEdit(current_user);
            this.Hide();
            f.ShowDialog();
            this.Show();

            fTeacher form = new fTeacher(current_user);
            this.Dispose();
            form.ShowDialog();
        }

        private void fTeacher_Load(object sender, EventArgs e)
        {
            MessageBox.Show(this.emp_role);
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
            fTeacherRegister f = new fTeacherRegister();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
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

        private void button3_Click(object sender, EventArgs e)
        {
            fAddEditAssignment f = new fAddEditAssignment();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void editNhanSutBtn_Click(object sender, EventArgs e)
        {
            fNhanSu f = new fNhanSu();
            this.Hide(); f.ShowDialog();
            this.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
