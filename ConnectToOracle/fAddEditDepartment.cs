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
    public partial class fAddEditDepartment : Form
    {
        string departmentID = string.Empty;
        Database database;
        public fAddEditDepartment(string departmentID)
        {
            InitializeComponent();
            this.departmentID = departmentID;
            database = Database.getInstance();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void setDepartmentInfo()
        {
            if (departmentID != string.Empty)
            {
                DataRow departmentInfo = database.GetDepartmentByID(departmentID);
                if (departmentInfo != null)
                {
                    txtxBoxDepartmentID.Text = departmentInfo["MADV"].ToString();
                    txtBoxDepartmentName.Text = departmentInfo["TENDV"].ToString();
                    txtBoxDepartmentHead.Text = departmentInfo["TRGDV"].ToString();
                }
            }
        }

        private void fAddEditDepartment_Load(object sender, EventArgs e)
        {
            setDepartmentInfo();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if(departmentID != string.Empty)
            {
                database.EditDepartmentInfo(departmentID, txtxBoxDepartmentID.Text, txtBoxDepartmentName.Text, txtBoxDepartmentHead.Text);
            }
            else
            {
                database.AddDepartment(txtxBoxDepartmentID.Text, txtBoxDepartmentName.Text, txtBoxDepartmentHead.Text);
            }
        }
    }
}
