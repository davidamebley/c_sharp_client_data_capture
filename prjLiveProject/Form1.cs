using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjLiveProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new frmPurchases().Show();
        }

        private void btnNewPurchase_Click(object sender, EventArgs e)
        {
            frmNewPurchase NewPurchaseForm = new frmNewPurchase();
            NewPurchaseForm.Show();
        }
    }
}
