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
    public partial class fAddPhanCong : Form
    {
        Database database;
        int selected1 = -1;
        int selected2 = -1;
        Exception ex = null;
        string roleName;
        public fAddPhanCong()
        {
            InitializeComponent();
            database = Database.getInstance();
            roleName = database.getCurrentRole();
            DataTable registers1 = null;
            if (roleName == "TRUONGDONVI")
            {
                registers1 = database.getTDVAllKHMO();
            }
            else if (roleName == "TRUONGKHOA")
            {
                registers1 = database.getTKAllKHMO();
            }

            DataTable registers2 = database.getAllNhanSu();
            gridOpenCourse.DataSource = registers1;
            gridTeacher.DataSource = registers2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string maHP;
            string HK;
            string year;
            string maCT;
            string maGV;
            if (selected1 >= 0 && selected2 >= 0)
            {
                DataGridViewRow row = gridOpenCourse.Rows[selected1];
                maHP = row.Cells["MAHP"].Value.ToString();
                HK = row.Cells["HK"].Value.ToString();
                year = row.Cells["NAM"].Value.ToString();
                maCT = row.Cells["MACT"].Value.ToString();
                DataGridViewRow row1 = gridTeacher.Rows[selected2];
                maGV = row1.Cells["MANV"].Value.ToString();
                MessageBox.Show(maGV + maHP + HK + year + maCT);
                database.addARowPhanCong(maGV, maHP, HK, year, maCT, ref ex);

                if (ex == null)
                {
                    this.Dispose();
                }
            }
        }

        private void gridTeacher_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selected2 = e.RowIndex;
            }
        }

        private void gridOpenCourse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selected1 = e.RowIndex;
            }
        }
    }
}
