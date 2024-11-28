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
    public partial class fAddNhanSu : Form
    {
        Database database;
        Exception ex = null;
        string current_user = "";
        string HOTEN = "";
        string PHAI = "";
        string NGSINH = "";
        string PHUCAP = "";
        string DT = "";
        string VAITRO = "";
        string MADV = "";

        public fAddNhanSu()
        {
            InitializeComponent();
            database = Database.getInstance();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            current_user = database.getIDHighest();
            getNextID();
            MessageBox.Show(current_user);
            fillDonVi();
            if(comboBox3.Items.Count > 0)
            {
                comboBox3.SelectedIndex = 0;
            }
            VAITRO = comboBox1.Text;
            PHAI = comboBox2.Text;
            MADV = comboBox3.Text;
        }

        private void fillDonVi()
        {
            List<string> list = database.getAllDonVi(ref ex);
            if (ex != null) {
                MessageBox.Show(ex.Message);
                ex = null;
                return;
            }
            for (int i = 0; i < list.Count; i++) 
            { 
                comboBox3.Items.Add(list[i]);
            }

        }
        private void getNextID()
        {
            string ID;
            int number = 0;
            ID = current_user.Substring(2);
            Int32.TryParse(ID, out number);
            number++;
            if (number < 10)
            {
                current_user = "NV" + "000" + number.ToString();
            }
            else if (number < 100)
            {
                current_user = "NV" + "00" + number.ToString();
            }
            else if (number < 1000)
            {
                current_user = "NV" + "0" + number.ToString();
            }
            else if (number >= 1000)
            {
                current_user += "NV" + number.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HOTEN = textBox3.Text;
            PHAI = comboBox2.Text;
            NGSINH = textBox5.Text;
            PHUCAP = textBox6.Text;
            DT = textBox7.Text;
            MADV = comboBox3.Text;
            VAITRO = comboBox1.Text;
            if (current_user != "" && HOTEN != "" &&
                (PHAI == "Nam" || PHAI == "Nữ") && PHUCAP != "" && NGSINH != "" && DT != "" && VAITRO != "" && MADV != "")
            {
                MessageBox.Show(current_user + HOTEN + PHAI + NGSINH + PHUCAP + DT + VAITRO + MADV);
                database.addARowNhanSu(current_user, HOTEN, PHAI, NGSINH, PHUCAP, DT, VAITRO, MADV, ref ex);
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập lại!!!!!");
            }
        }
    }
}
