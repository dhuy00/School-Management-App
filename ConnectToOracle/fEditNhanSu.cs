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
    public partial class fEditNhanSu : Form
    {
        Database database;
        Exception ex = null;
        string current_user;
        string HOTEN = "";
        string PHAI = "";
        string NGSINH = "";
        string PHUCAP = "";
        string DT = "";
        string VAITRO = "";
        string MADV = "";
        public fEditNhanSu(string _current_user)
        {
            InitializeComponent();
            database = Database.getInstance();
            this.current_user = _current_user;
            setUserInfo();
        }

        private void setUserInfo()
        {
            string roleName = VAITRO = database.getRoleByID(current_user, ref ex);
            DataRow userInfo = database.GetEmployeeByIDForEdit(current_user);
            if (userInfo != null)
            {
                textBox2.Text = current_user;
                textBox3.Text = HOTEN = userInfo["HOTEN"].ToString();
                textBox4.Text = PHAI = userInfo["PHAI"].ToString();
                textBox5.Text = NGSINH = userInfo["NGSINH"].ToString();
                textBox6.Text = PHUCAP = userInfo["PHUCAP"].ToString();
                textBox7.Text = DT = userInfo["DT"].ToString();
                if (roleName != null)
                {
                    comboBox1.Text = roleName;
                }
                textBox1.Text = MADV = userInfo["MADV"].ToString();
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin nhân viên");
                //MessageBox.Show(current_user);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (HOTEN == textBox3.Text)
            {
                HOTEN = "";
            }
            else
            {
                HOTEN = textBox3.Text;
            }

            if (PHAI == textBox4.Text)
            {
                PHAI = "";
            }
            else
            {
                PHAI = textBox4.Text;
            }

            if (NGSINH == textBox5.Text)
            {
                NGSINH = "";
            }
            else
            {
                NGSINH = textBox5.Text;
            }

            if (PHUCAP == textBox6.Text)
            {
                PHUCAP = "";
            }
            else
            {
                PHUCAP = textBox6.Text;
            }

            if (DT == textBox7.Text)
            {
                DT = "";
            }
            else
            {
                DT = textBox7.Text;
            }
            if (VAITRO == comboBox1.Text)
            {
                VAITRO = "";
            }
            else
            {
                VAITRO = comboBox1.Text;
            }

            if (MADV == textBox1.Text)
            {
                MADV = "";
            }
            else
            {
                MADV = textBox1.Text;
            }
            MessageBox.Show(current_user + HOTEN + PHAI + NGSINH + PHUCAP + DT + VAITRO + MADV);
            database.updateARowNhanSu(current_user, HOTEN, PHAI, NGSINH, PHUCAP, DT, VAITRO, MADV, ref ex);
            this.Dispose();
        }
    }
}
