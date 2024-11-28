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

    public partial class fStandardAudit : Form
    {
        Database database;
        Exception ex = null;
        public fStandardAudit()
        {
            InitializeComponent();
            database = Database.getInstance();
            getList();
        }

        private void getList()
        {
            /*List<List<String>> list = database.getStandardAuditList(ref ex);
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
                item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = list[i][4] });
                item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = list[i][5] });
                item.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Italic);
                listUsers.Items.Add(item);

            }*/
            DataTable data = database.GetStandardAudit();
            gridStandardAudit.DataSource = data;
        }

        private void fStandardAudit_Load(object sender, EventArgs e)
        {

        }
    }


}

