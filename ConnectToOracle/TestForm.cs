using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectToOracle
{
    public partial class TestForm : Form
    {
        Database db;
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            string query = "SELECT * FROM SINHVIEN";
            dtgvAccount.DataSource = db.ExecuteQuery(query, "sys", "123456");
        }
    }
}
