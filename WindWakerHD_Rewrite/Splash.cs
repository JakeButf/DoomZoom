using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindWakerHD_Rewrite
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
            OpenMainForm();
        }

        private async void OpenMainForm()
        {
            
            Main f = new Main();
            await Task.Delay(5000);
            this.Hide();
            f.Show();
            
        }

        private void Splash_Load(object sender, EventArgs e)
        {

        }
    }
}
