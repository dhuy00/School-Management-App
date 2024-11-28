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
    public partial class fChangeAdd : Form
    {
        Database database;
        public fChangeAdd()
        {
            InitializeComponent();
            database = Database.getInstance();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            database.updateAddress(txtAddress.Text);
            this.Dispose();
        }
    }
}
