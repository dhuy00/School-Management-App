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
    public partial class fStudentRegister : Form
    {
        Database database;
        public fStudentRegister()
        {
            InitializeComponent();
            database = Database.getInstance();
        }

        private void AddButtonColumn(string colName, string colText)
        {
            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.Name = colName;
            btnColumn.HeaderText = "";
            btnColumn.Text = colText;
            btnColumn.UseColumnTextForButtonValue = true;
            btnColumn.Width = 250;
            gridStudentRegister.Columns.Add(btnColumn);

        }

        public void getStuRegister()
        {
            DataTable registers = database.GetStuRegister();
            gridStudentRegister.DataSource = registers;
            AddButtonColumn("DeleteButton", "Xóa");
        }


        private void fStudentRegister_Load(object sender, EventArgs e)
        {
            getStuRegister();
        }

        private void gridStudentRegister_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gridStudentRegister.Columns["DeleteButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = gridStudentRegister.Rows[e.RowIndex];
                string teacherID = row.Cells["MAGV"].Value.ToString();
                string courseID = row.Cells["MAHP"].Value.ToString();
                string semester = row.Cells["HK"].Value.ToString();
                string year = row.Cells["NAM"].Value.ToString();
                string curriculumID = row.Cells["MACT"].Value.ToString();
                database.DeleteRegCourse(teacherID, courseID, semester, year, curriculumID);

                fStudentRegister form = new fStudentRegister();
                this.Dispose();
                form.ShowDialog();
            }  
        }
    }
}
