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
    public partial class fTeacherAssignment : Form
    {
        Database database;
        string emp_role;
        public fTeacherAssignment()
        {
            InitializeComponent();
            database = Database.getInstance();
            this.emp_role = database.getCurrentRole();
        }

        private void AddEditButtonColumn(string colName, string colText)
        {
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.Name = colName;
            buttonColumn.HeaderText = "";
            buttonColumn.Text = colText;
            buttonColumn.UseColumnTextForButtonValue = false;
            gridTeacherAssignment.Columns.Add(buttonColumn);
        }

        bool AreRowsEqual(DataRow row1, DataRow row2)
        {
            if (row1 == null || row2 == null)
            {
                return false;
            }

            return row1.ItemArray.SequenceEqual(row2.ItemArray);
        }

        private void UpdateButtonText(int rowIndex, string text)
        {
            gridTeacherAssignment.Rows[rowIndex].Cells["EditButton"].Value = text;
        }

        private void getTeacherAssignment()
        {
            DataTable assignments = new DataTable();
            if (emp_role == "GIAOVU")
            {
                assignments = database.GetAllAssignment();
                gridTeacherAssignment.DataSource = assignments;

            }
            else if (emp_role == "GIANGVIEN")
            {
                assignments = database.GetTeacherAssignment();
                gridTeacherAssignment.DataSource = assignments;
                
            }
            else if (emp_role == "TRUONGDONVI")
            {
                assignments = database.GetTDVAssignment();
                gridTeacherAssignment.DataSource = assignments;
            }
            else if (emp_role == "TRUONGKHOA")
            {
                assignments = database.GetTKAssignment();
                gridTeacherAssignment.DataSource = assignments;
            }


        }


        private void fTeacherAssignment_Load(object sender, EventArgs e)
        {
            getTeacherAssignment();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
