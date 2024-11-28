using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace ConnectToOracle
{
    public partial class fLogin : Form
    {
        Database database;
        Exception ex = null;
        int selectedApplication;
        List<string> role = new List<string>();
        public fLogin()
        {
            InitializeComponent();
            database = Database.getInstance();
            selectedApplication = -1;
        }

        private void btnLogInEnter(object sender, EventArgs e)
        {
            if (database != null)
            {
                if (selectedApplication == 0)
                {
                    role = database.logInToDataBse(txtUsername.Text, txtPassword.Text, ref ex, "SYSDBA");
                }
                else if (selectedApplication == 1)
                {
                    role = database.logInToDataBse(txtUsername.Text, txtPassword.Text, ref ex, "OTHERS");
                }
                else
                {
                    MessageBox.Show("Hãy chọn một ứng dụng");
                    return;
                }
;
            }
            if (ex != null)
            {
                MessageBox.Show(ex.Message);
                ex = null;
                database.logOut();
                database = Database.getInstance();
                txtUsername.Text = "";
                txtPassword.Text = "";
                role = new List<String>();
                return;
            }
            // fMain f = new fMain();
            if (role.Contains("SYSDBA"))
            {
                AllUserForm f = new AllUserForm(txtUsername.Text);
                this.Hide();
                f.ShowDialog();
            }
            else if (role.Contains("NHANVIENCOBAN") || role.Contains("GIANGVIEN") || role.Contains("GIAOVU") || role.Contains("TRUONGDONVI") || role.Contains("TRUONGKHOA"))
            {
                fTeacher f = new fTeacher(txtUsername.Text);
                this.Hide();
                f.ShowDialog();
            }
            else if (role.Contains("SINHVIEN"))
            {
                fStudent f = new fStudent(txtUsername.Text);
                this.Hide();
                f.ShowDialog();
            }
            else
            {
                MessageBox.Show("Hãy chọn một ứng dụng");
            }


            database.logOut();
            database = Database.getInstance();
            txtUsername.Text = "";
            txtPassword.Text = "";
            role = new List<String>();
            this.Show();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Exit the program?", "Notify", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
                
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedApplication = comboBox1.SelectedIndex;

        }
    }


}
