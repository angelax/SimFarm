using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimFarm
{
    public partial class GameForm : Form
    {
        //GameForm constructor
        public GameForm()
        {
            InitializeComponent();
        }

        //runs when user clicks for the 10 by 10 grid
        private void btnDanielForm_Click(object sender, EventArgs e)
        {
            //stores DanielForm
            //DanielForm danielForm = new DanielForm();
            //opens DanielForm
            //DanielForm.Show();
            //disable both buttons on the form
            btnAngelaForm.Enabled = false;
            btnDanielForm.Enabled = false;
        }

        //runs when user clicks for the 20 by 5 grid
        private void btnAngelaForm_Click(object sender, EventArgs e)
        {
            //stores AngelaForm
            AngelaForm angelaForm = new AngelaForm();
            //opens AngelaForm
            angelaForm.Show();
            //disable both buttons on the form
            btnAngelaForm.Enabled = false;
            btnDanielForm.Enabled = false;
        }
    }
}
