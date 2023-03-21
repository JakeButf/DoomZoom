using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using WindWakerHD_Rewrite.Properties;
using MadMilkman.Ini;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Drawing;

namespace WindWakerHD_Rewrite
{
    public partial class WatchForm : Form
    {
        public WatchForm()
        {
            InitializeComponent();
        }

        private List<UInt32> addresses = new List<UInt32>() { };
        private List<String> names = new List<String> { };
        TCPGecko Gecko;


        //On Watch Creation
        private void button_createWatch_Click(object sender, EventArgs e)
        {
            string name = textBox_watchName.Text;
            UInt32 address = Convert.ToUInt32(textBox_watchAddress.Text, 16);

            addresses.Add(address);
            names.Add(name);

            Label l = new Label();
            l.Text = name + ": " + Gecko.peek(address);
            l.ForeColor = Color.White;
            flowLayoutPanel1.Controls.Add(l);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateWatches();
        }

        private void UpdateWatches()
        {
            for(int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                Label l = (Label)flowLayoutPanel1.Controls[i];
                l.Text = names[i] + ": " + Gecko.peek(addresses[i]);
            }
        }

        public void SetGecko(TCPGecko gecko)
        {
            Gecko = gecko;
        }
    }
}
