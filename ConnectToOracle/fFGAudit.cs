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
    public partial class fFGAudit : Form
    {
        Database database;
        Exception ex = null;
        public fFGAudit()
        {
            InitializeComponent();
            database = Database.getInstance();
            getList();
        }

        private void getList()
        {
            List<List<String>> list = database.getFGAuditList(ref ex);
            if (ex != null)
            {
                MessageBox.Show(ex.Message);
                ex = null;
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = list[i][0];
                item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = list[i][1] });
                item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = list[i][2] });
                item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = list[i][3] });
                item.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Italic);
                listUsers.Items.Add(item);

            }
        }
    }
}
