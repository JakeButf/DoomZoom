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
    public partial class Main : Form
    {
        private protected string version = "1.1.1";
        private const uint CodeHandlerStart = 0x01133000;
        private const uint CodeHandlerEnd = 0x01134300;
        private const uint CodeHandlerEnabled = 0x10014CFC;
        public TCPGecko Gecko;
        private bool connected;
        private static uint lastpositionx;
        private static uint lastpositiony;
        private static uint lastpositionz;
        private static uint lastfacing;
        public Main()
        {
            InitializeComponent();
            wiiuip.Text = Settings.Default.LatestIP;
            mapbox.SelectedIndex = 0;
            inventoryBox.SelectedIndex = 0;
            bottleitem.SelectedIndex = 0;
            itemspoofbox.SelectedIndex = 0;
            boatcoordinatesList.SelectedIndex = 0;
            maxheartsBox.SelectedIndex = 0;
            maxmagicBox.SelectedIndex = 0;

            //this.BackColor = Color.FromArgb(255, 15, 16, 19);
            panel_gecko.Enabled = false;
            panel_link.Enabled = false;
            panel_spoof.Enabled = false;
            panel_inventory.Enabled = false;
            panel_itemmods.Enabled = false;
            panel_coordinates.Enabled = false;
            panel_stageload.Enabled = false;
            panel_memfile.Enabled = false;
            panel_watchesmaster.Enabled = false;
            panel_debug.Enabled = false;
            panel_credits.Enabled = false;
            //Assign Panel Location
            panel_gecko.Location = new Point(-1, 43);
            panel_link.Location = new Point(-1, 43);
            panel_spoof.Location = new Point(-1, 43);
            panel_inventory.Location = new Point(-1, 43);
            panel_itemmods.Location = new Point(-1, 43);
            panel_coordinates.Location = new Point(-1, 43);
            panel_stageload.Location = new Point(-1, 43);
            panel_memfile.Location = new Point(-1, 43);
            panel_watchesmaster.Location = new Point(-1, 43);
            panel_debug.Location = new Point(-1, 43);
            panel_credits.Location = new Point(-1, 43);

            panel_gecko.Show();
            button_codes.ForeColor = Color.Red;

            panel_link.Hide();
            panel_spoof.Hide();
            panel_inventory.Hide();
            panel_itemmods.Hide();
            panel_coordinates.Hide();
            panel_stageload.Hide();
            panel_memfile.Hide();
            panel_watchesmaster.Hide();
            panel_debug.Hide();
            panel_credits.Hide();
        }

        #region UI Controls
        private void HideAllPanels()
        {
            panel_gecko.Hide();
            panel_link.Hide();
            panel_spoof.Hide();
            panel_inventory.Hide();
            panel_itemmods.Hide();
            panel_coordinates.Hide();
            panel_stageload.Hide();
            panel_memfile.Hide();
            panel_watchesmaster.Hide();
            panel_debug.Hide();
            panel_credits.Hide();
        }
        private void ResetButtonColors()
        {
            button_codes.ForeColor = Color.White;
            button_link.ForeColor = Color.White;
            button_itemspoof.ForeColor = Color.White;
            button_inventory.ForeColor = Color.White;
            button_itemmods.ForeColor = Color.White;
            button_coordinates.ForeColor = Color.White;
            button_stageload.ForeColor = Color.White;
            button_memfiles.ForeColor = Color.White;
            button_watches.ForeColor = Color.White;
            button_debug.ForeColor = Color.White;
            button_credits.ForeColor = Color.White;
        }

        private void button_codes_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ResetButtonColors();
            panel_gecko.Show();
            button_codes.ForeColor = Color.Red;  
        }
        private void button_link_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ResetButtonColors();
            panel_link.Show();
            button_link.ForeColor = Color.Red;
        }
        private void button_itemspoof_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ResetButtonColors();
            panel_spoof.Show();
            button_itemspoof.ForeColor = Color.Red;
        }
        private void button_inventory_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ResetButtonColors();
            panel_inventory.Show();
            button_inventory.ForeColor = Color.Red;
        }
        private void button_itemmods_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ResetButtonColors();
            panel_itemmods.Show();
            button_itemmods.ForeColor = Color.Red;
        }
        private void button_coordinates_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ResetButtonColors();
            panel_coordinates.Show();
            button_coordinates.ForeColor = Color.Red;
        }
        private void button_stageload_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ResetButtonColors();
            panel_stageload.Show();
            button_stageload.ForeColor = Color.Red;
        }
        private void button_memfiles_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ResetButtonColors();
            panel_memfile.Show();
            button_memfiles.ForeColor = Color.Red;
            maptimer.Start();
        }
        private void button_watches_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ResetButtonColors();
            panel_watchesmaster.Show();
            button_watches.ForeColor = Color.Red;
        }
        private void button_debug_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ResetButtonColors();
            panel_debug.Show();
            button_debug.ForeColor = Color.Red;
        }
        private void button_credits_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            ResetButtonColors();
            panel_credits.Show();
            button_credits.ForeColor = Color.Red;
        }
        #endregion

        private void wiiuconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (wiiuconnect.Tag == null)
                {
                    Gecko = new TCPGecko(wiiuip.Text, 7331);

                    connected = Gecko.Connect();

                    if (connected)
                    {
                        label_connection.Text = "Connected!";
                        label_connection.ForeColor = System.Drawing.Color.Green;
                        wiiuconnect.Tag = "connect";
                        wiiuconnect.Text = "Disconnect";
                        cheatTab.Enabled = true;
                        inputtimer.Start();

                        panel_gecko.Enabled = true;
                        panel_link.Enabled = true;
                        panel_spoof.Enabled = true;
                        panel_inventory.Enabled = true;
                        panel_itemmods.Enabled = true;
                        panel_coordinates.Enabled = true;
                        panel_stageload.Enabled = true;
                        panel_memfile.Enabled = true;
                        panel_watchesmaster.Enabled = true;
                        panel_debug.Enabled = true;
                        panel_credits.Enabled = true;
                    }
                }
                else
                {
                    label_connection.Text = "Not Connected!";
                    label_connection.ForeColor = System.Drawing.Color.Red;
                    wiiuconnect.Tag = null;
                    Gecko.Disconnect();
                    wiiuconnect.Text = "Connect";
                    cheatTab.Enabled = false;
                    readcoordinates.Checked = false;
                    coordinatestimer.Stop();
                    maptimer.Stop();
                }
            }
            catch (ETCPGeckoException ex)
            {
                connected = false;
                MessageBox.Show(ex.Message);
            }
            catch (System.Net.Sockets.SocketException)
            {
                connected = false;
                MessageBox.Show("The IP is not valid.");
            }
        }
        #region General Methods
        private void cheatTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cheatTab.SelectedIndex)
            {
                case 8: //Memfiles Page
                    maptimer.Start();
                    watchestimer.Stop();
                    break;
                case 7: //Watches Page
                    UpdateWatchLabels();
                    break;
                default:
                    watchestimer.Stop();
                    break;
            }
        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.LatestIP = wiiuip.Text;
            Settings.Default.Save();
        }

        private void backtomenu_Click(object sender, EventArgs e)
        {
            Gecko.poke08(0x1098F293, 0x01);
        }
        #endregion

        #region Gecko Tab
        private void SetCheats(ICollection<Geckocodes.Cheat> cheats)
        {
            #region codehandler
            Geckocodes getcheat = new Geckocodes();
            Gecko.poke32(CodeHandlerEnabled, 0x00000000);
            var clear = CodeHandlerStart;
            while (clear <= CodeHandlerEnd)
            {
                this.Gecko.poke32(clear, 0x0);
                clear += 0x4;
            }
            #endregion
            var codes = new List<uint>();

            if (cheats.Contains(Geckocodes.Cheat.Health))
                foreach (var entry in getcheat.GetInfHealth())
                    codes.Add(entry);

            if (cheats.Contains(Geckocodes.Cheat.MoonJump))
                foreach (var entry in getcheat.GetMoonJump())
                    codes.Add(entry);

            if (cheats.Contains(Geckocodes.Cheat.Magic))
                foreach (var entry in getcheat.GetInfMagic())
                    codes.Add(entry);

            if (cheats.Contains(Geckocodes.Cheat.Rupee))
                foreach (var entry in getcheat.GetInfRupees())
                    codes.Add(entry);

            if (cheats.Contains(Geckocodes.Cheat.Arrow))
                foreach (var entry in getcheat.GetInfArrows())
                    codes.Add(entry);

            if (cheats.Contains(Geckocodes.Cheat.Bomb))
                foreach (var entry in getcheat.GetInfBombs())
                    codes.Add(entry);

            if (cheats.Contains(Geckocodes.Cheat.Air))
                foreach (var entry in getcheat.GetInfAir())
                    codes.Add(entry);

            if (cheats.Contains(Geckocodes.Cheat.ForestWater))
                foreach (var entry in getcheat.GetInfForestWater())
                    codes.Add(entry);

            if (cheats.Contains(Geckocodes.Cheat.InfItems))
                foreach (var entry in getcheat.GetInfItems())
                    codes.Add(entry);

            if (cheats.Contains(Geckocodes.Cheat.SuperSwim))
                foreach (var entry in getcheat.GetSuperswim())
                    codes.Add(entry);

            if (cheats.Contains(Geckocodes.Cheat.SuperCrouch))
                foreach (var entry in getcheat.GetSupercrouch())
                    codes.Add(entry);

            if (cheats.Contains(Geckocodes.Cheat.CompletedMap))
                foreach (var entry in getcheat.GetCompletedMap())
                    codes.Add(entry);

            if (cheats.Contains(Geckocodes.Cheat.AllCharts))
                foreach (var entry in getcheat.GetAllTreasureCharts())
                    codes.Add(entry);


            var address = CodeHandlerStart;

            foreach (var code in codes)
            {
                Gecko.poke32(address, code);
                address += 0x4;
            }

            Gecko.poke32(CodeHandlerEnabled, 0x00000001);
        }

        private void setoptions_Click(object sender, EventArgs e)
        {
            var selected = new List<Geckocodes.Cheat>();

            if (infhealth.Checked == true)
                selected.Add(Geckocodes.Cheat.Health);

            if (moonjump.Checked == true)
                selected.Add(Geckocodes.Cheat.MoonJump);

            if (infmagic.Checked == true)
                selected.Add(Geckocodes.Cheat.Magic);

            if (infrupees.Checked == true)
                selected.Add(Geckocodes.Cheat.Rupee);

            if (infarrows.Checked == true)
                selected.Add(Geckocodes.Cheat.Arrow);

            if (infbombs.Checked == true)
                selected.Add(Geckocodes.Cheat.Bomb);

            if (infair.Checked == true)
                selected.Add(Geckocodes.Cheat.Air);

            if (infforestwatertime.Checked == true)
                selected.Add(Geckocodes.Cheat.ForestWater);

            if (infitems.Checked == true)
                selected.Add(Geckocodes.Cheat.InfItems);

            if (superswim.Checked == true)
                selected.Add(Geckocodes.Cheat.SuperSwim);

            if (supercrouch.Checked == true)
                selected.Add(Geckocodes.Cheat.SuperCrouch);

            if (completedmap.Checked == true)
                selected.Add(Geckocodes.Cheat.CompletedMap);

            if (alltreasurecharts.Checked == true)
                selected.Add(Geckocodes.Cheat.AllCharts);

            if (dpadlowhealth.Checked == true)
                selected.Add(Geckocodes.Cheat.LowHealth);

            SetCheats(selected);
        }

        private void allfigurines_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You need atleast 1 figurine to access the rooms.\nContinue?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Gecko.poke32(0x15073D24, 0xFFFFFFFF);
                Gecko.poke32(0x15073D28, 0xFFFFFF06);
                Gecko.poke32(0x15073D30, 0xFFFFFFFF);
                Gecko.poke08(0x15073D40, 0xFF);
                Gecko.poke16(0x15073D54, 0xFFFF);
            }
            else if (dialogResult == DialogResult.No)
            {
                //do nothing
            }

        }
        #endregion
        
        #region Stage Loader Tab
        private async void loadmap_Click(object sender, EventArgs e)
        {
            var readmap = await Map.SetMap(mapbox.Text);
            Gecko.poke32(0x109763F0, 0x00000000); //Clearing Stage name to prevent Crash
            Gecko.poke32(0x109763F4, 0x00000000); //Clearing Stage name to prevent Crash
            Gecko.poke32(0x109763F0, readmap[0]);
            Gecko.poke32(0x109763F4, readmap[1]);
            Gecko.poke08(0x109763F9, Convert.ToByte(spawnid.Value)); //Spawn ID
            Gecko.poke08(0x109763FA, Convert.ToByte(roomid.Value)); //Room ID
            Gecko.poke08(0x109763FB, Convert.ToByte(layerid.Value)); //Layer ID
            Gecko.poke08(0x109763FC, 0x01); //Reload Stage
        }

        //Reload Stage
        private void button9_Click_1(object sender, EventArgs e)
        {
            Gecko.poke08(0x109763FC, 0x01);
        }
        #endregion

        #region Inventory Tab
        private async void additem_Click(object sender, EventArgs e)
        {
            var readitemaddress = await Inventory.GetItemAddress(inventoryBox.Text);
            var readitemvalue = await Inventory.GetItemValue(inventoryBox.Text);
            Gecko.poke08(readitemaddress[0], readitemvalue[0]);
            if (inventoryBox.Text == "Power Bracelets")
                Gecko.poke08(0x15073736, 0xFF);
        }

        private async void removeitem_Click(object sender, EventArgs e)
        {
            var readitemaddress = await Inventory.GetItemAddress(inventoryBox.Text);
            Gecko.poke08(readitemaddress[0], 0xFF);
            if (inventoryBox.Text == "Power Bracelets")
                Gecko.poke08(0x15073736, 0x00);
            if (inventoryBox.Text == "Hero's Charm")
                Gecko.poke08(0x15073738, 0x00);
        }

        private async void bottle1_Click(object sender, EventArgs e)
        {
            var readbottlevalue = await Inventory.GetItemValue(bottleitem.Text);
            Gecko.poke08(0x150736CA, readbottlevalue[0]);
        }

        private async void bottle2_Click(object sender, EventArgs e)
        {
            var readbottlevalue = await Inventory.GetItemValue(bottleitem.Text);
            Gecko.poke08(0x150736CB, readbottlevalue[0]);
        }

        private async void bottle3_Click(object sender, EventArgs e)
        {
            var readbottlevalue = await Inventory.GetItemValue(bottleitem.Text);
            Gecko.poke08(0x150736CC, readbottlevalue[0]);
        }

        private async void bottle4_Click(object sender, EventArgs e)
        {
            var readbottlevalue = await Inventory.GetItemValue(bottleitem.Text);
            Gecko.poke08(0x150736CD, readbottlevalue[0]);
        }

        private async void sloty_Click(object sender, EventArgs e)
        {
            var readitemvalue = await Inventory.GetItemValue(itemspoofbox.Text);
            Gecko.poke08(0x10976E6C, readitemvalue[0]);
        }

        private async void slotx_Click(object sender, EventArgs e)
        {
            var readitemvalue = await Inventory.GetItemValue(itemspoofbox.Text);
            Gecko.poke08(0x10976E6B, readitemvalue[0]);
        }

        private async void slotr_Click(object sender, EventArgs e)
        {
            var readitemvalue = await Inventory.GetItemValue(itemspoofbox.Text);
            Gecko.poke08(0x10976E6D, readitemvalue[0]);
        }
        #endregion

        #region Map Timer
        private async void maptimer_Tick(object sender, EventArgs e)
        {
            try
            {
                string mapname = "";
                string origin = Gecko.peek(0x109763F0).ToString("X");
                string hexValues = Regex.Replace(origin, ".{2}", "$0 ");
                string[] hexValuesSplit = hexValues.Split(' ');
                foreach (string hex in hexValuesSplit)
                {
                    if (string.IsNullOrWhiteSpace(hex)) continue;
                    int value = Convert.ToInt32(hex, 16);
                    mapname += Char.ConvertFromUtf32(value);
                }

                string mapname2 = "";
                string origin2 = Gecko.peek(0x109763F4).ToString("X");
                string hexValues2 = Regex.Replace(origin2, ".{2}", "$0 ");
                string[] hexValuesSplit2 = hexValues2.Split(' ');
                foreach (string hex in hexValuesSplit2)
                {
                    if (string.IsNullOrWhiteSpace(hex)) continue;
                    int value = Convert.ToInt32(hex, 16);
                    mapname2 += Char.ConvertFromUtf32(value);
                }

                uint roomid = Gecko.peek(0x109763F9) >> ((~0x109763F9 & 2) * 4) & 0xff;
                Console.WriteLine(roomid + "roomid");
                uint spawnid = Gecko.peek(0x109763F9) >> ((~0x109763F9 & 4) * 4) & 0xff;
                uint layerid = Gecko.peek(0x109763F9) >> ((~0x109763F9 & 0) * 4) & 0xff;
                label_memfile_stage.Text = mapname + mapname2;
                //MEMFILE INFO
                savedRoom = roomid;
                savedSpawn = spawnid;
                savedLayer = layerid;
                var readrealroom = await Map.GetRoomName(roomid.ToString());
            }
            catch
            {
                wiiuconnect.PerformClick();
                MessageBox.Show("Disconnected due to a connection loss");
            }
        }
        #endregion

        #region Item Mods Tab
        private void reloadstage_Click(object sender, EventArgs e)
        {
            Gecko.poke08(0x109763FC, 0x01);
        }

        private async void boatteleport_Click(object sender, EventArgs e)
        {
            var readboatcoordinate = await Coordinates.GetBoatCoordinates(boatcoordinatesList.Text);
            Gecko.poke32(0x48723EC4, readboatcoordinate[0]);
            Gecko.poke32(0x48723ECC, readboatcoordinate[1]);
        }

        private void bombRadius_CheckedChanged(object sender, EventArgs e)
        {
            if (bombRadius.Checked)
                Gecko.poke32(0x1050CA50, 0x46000000);
            else
                Gecko.poke32(0x1050CA50, 0x43480000);
        }

        private void bowRadius_CheckedChanged(object sender, EventArgs e)
        {
            if (bowRadius.Checked)
                Gecko.poke32(0x10509634, 0x45F00000);
            else
                Gecko.poke32(0x10509634, 0x40A00000);
        }

        private void hookshotSpeed_CheckedChanged(object sender, EventArgs e)
        {
            if (hookshotSpeed.Checked)
                Gecko.poke32(0x105138C0, 0x42000000);
            else
                Gecko.poke32(0x105138C0, 0x40E00000);
        }

        private void hookshotRange_CheckedChanged(object sender, EventArgs e)
        {
            if (hookshotRange.Checked)
                Gecko.poke32(0x1051392C, 0x45800000);
            else
                Gecko.poke32(0x1051392C, 0x41700000);
        }

        private void bowSpeed_CheckedChanged(object sender, EventArgs e)
        {
            if (bowSpeed.Checked)
                Gecko.poke32(0x1050962C, 0x46000000);
            else
                Gecko.poke32(0x1050962C, 0x43480000);
        }

        private void boomerangRange_CheckedChanged(object sender, EventArgs e)
        {
            if (boomerangRange.Checked)
            {
                Gecko.poke32(0x1050D1A4, 0x467A0000);
                Gecko.poke32(0x1050D1A8, 0x467A0000);
            }

            else
            {
                Gecko.poke32(0x1050D1A4, 0x459C4000);
                Gecko.poke32(0x1050D1A8, 0x451C4000);
            }
        }

        private void boomerangThrow_CheckedChanged(object sender, EventArgs e)
        {
            if (boomerangThrow.Checked)
                Gecko.poke32(0x1050D1D0, 0x43700000);
            else
                Gecko.poke32(0x1050D1D0, 0x42700000);
        }
        #endregion

        #region Link Mod Tab
        private void setcurrenthearts_Click(object sender, EventArgs e)
        {
            Gecko.poke08(0x15073683, Convert.ToByte(currentheartsBox.Value));
        }
        private async void sethearts_Click(object sender, EventArgs e)
        {
            var gethearts = await Linktweaks.GetMaxHearts(maxheartsBox.Text);
            Gecko.poke08(0x15073681, gethearts[0]);
        }

        private void linkmod_binddpaddown_CheckedChanged(object sender, EventArgs e)
        {
            dpaddown = linkmod_binddpaddown.Checked;
        }

        private async void setmagic_Click(object sender, EventArgs e)
        {
            var getmagic = await Linktweaks.GetMaxMagic(maxmagicBox.Text);
            Gecko.poke08(0x15073693, getmagic[0]);
        }

        private void setarrows_Click(object sender, EventArgs e)
        {
            Gecko.poke08(0x150736EF, Convert.ToByte(maxarrowsBox.Value));
        }

        private void setbombs_Click(object sender, EventArgs e)
        {
            Gecko.poke08(0x150736F0, Convert.ToByte(maxbombsBox.Value));
        }
        #endregion

        #region Coordinates Tab
        private void readcoordinates_CheckedChanged(object sender, EventArgs e)
        {
            if (readcoordinates.Checked)
                coordinatestimer.Start();
            else
                coordinatestimer.Stop();
        }

        private void coordinatestimer_Tick(object sender, EventArgs e)
        {
            try
            {
                uint speed = 0x00000000;
                speed = Gecko.peek(0x10989C76);
                byte[] linkxbytes = BitConverter.GetBytes(Gecko.peek(0x1096EF48));
                byte[] linkybytes = BitConverter.GetBytes(Gecko.peek(0x1096EF4C));
                byte[] linkzbytes = BitConverter.GetBytes(Gecko.peek(0x1096EF50));
                byte[] linkspeedbytes = BitConverter.GetBytes(Gecko.peek(speed + 0x00006938));
                byte[] boatxbytes = BitConverter.GetBytes(Gecko.peek(0x48723EC4));
                byte[] boatybytes = BitConverter.GetBytes(Gecko.peek(0x48723EC8));
                byte[] boatzbytes = BitConverter.GetBytes(Gecko.peek(0x48723ECC));
                float LinkX = BitConverter.ToSingle(linkxbytes, 0);
                float LinkY = BitConverter.ToSingle(linkybytes, 0);
                float LinkZ = BitConverter.ToSingle(linkzbytes, 0);
                float LinkSpeed = BitConverter.ToSingle(linkspeedbytes, 0);
                float BoatX = BitConverter.ToSingle(boatxbytes, 0);
                float BoatY = BitConverter.ToSingle(boatybytes, 0);
                float BoatZ = BitConverter.ToSingle(boatzbytes, 0);
                linkx.Text = LinkX.ToString();
                linky.Text = LinkY.ToString();
                linkz.Text = LinkZ.ToString();
                linkspeed.Text = LinkSpeed.ToString();
                boatx.Text = BoatX.ToString();
                boaty.Text = BoatY.ToString();
                boatz.Text = BoatZ.ToString();
            }
            catch
            {
                wiiuconnect.PerformClick();
                MessageBox.Show("Disconnected due to a connection loss");
            }
        }

        private void button_coordinates_teleport_Click(object sender, EventArgs e)
        {
            float LinkX = float.Parse(textBox_coordinates_x.Text.ToString());
            float LinkY = float.Parse(textBox_coordinates_y.Text.ToString());
            float LinkZ = float.Parse(textBox_coordinates_z.Text.ToString());

            byte[] linkxbytes = BitConverter.GetBytes(LinkX);
            byte[] linkybytes = BitConverter.GetBytes(LinkY);
            byte[] linkzbytes = BitConverter.GetBytes(LinkZ);

            Gecko.poke32(0x1096EF48, BitConverter.ToUInt32(linkxbytes, 0));
            Gecko.poke32(0x1096EF4C, BitConverter.ToUInt32(linkybytes, 0));
            Gecko.poke32(0x1096EF50, BitConverter.ToUInt32(linkzbytes, 0));
            Gecko.poke32(0x1096EF10, Convert.ToUInt32(textBox_coordinates_angle.Text));
        }

        private void button_coordinates_loadCurrentCoords_Click(object sender, EventArgs e)
        {
            byte[] linkxbytes = BitConverter.GetBytes(Gecko.peek(0x1096EF48));
            byte[] linkybytes = BitConverter.GetBytes(Gecko.peek(0x1096EF4C));
            byte[] linkzbytes = BitConverter.GetBytes(Gecko.peek(0x1096EF50));
            //byte[] linkanglebytes = BitConverter.GetBytes();

            float LinkX = BitConverter.ToSingle(linkxbytes, 0);
            float LinkY = BitConverter.ToSingle(linkybytes, 0);
            float LinkZ = BitConverter.ToSingle(linkzbytes, 0);
            //float LinkAngle = BitConverter.ToSingle(linkanglebytes, 0);

            textBox_coordinates_x.Text = LinkX.ToString();
            textBox_coordinates_y.Text = LinkY.ToString();
            textBox_coordinates_z.Text = LinkZ.ToString();
            textBox_coordinates_angle.Text = Convert.ToUInt32(Gecko.peek(0x1096EF10)).ToString();
        }

        private void linkteleport_Click(object sender, EventArgs e)
        {
            uint collision = 0x00000000;
            uint collisionold = 0x00000000;
            collision = Gecko.peek(0x1097648C);
            collisionold = Gecko.peek(collision + 0x00000834);
            Gecko.poke32(collision + 0x00000834, 0x00004004);
            Gecko.poke32(0x1096EF48, lastpositionx);
            Gecko.poke32(0x1096EF4C, lastpositiony);
            Gecko.poke32(0x1096EF50, lastpositionz);
            Gecko.poke32(0x1096EF10, lastfacing);
            Thread.Sleep(1000);
            Gecko.poke32(collision + 0x00000834, collisionold);
        }

        private async void mapbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            readrealmap.Text = await Map.GetRealName(mapbox.Text);
        }

        private void storeposition_Click(object sender, EventArgs e)
        {
            lastpositionx = Gecko.peek(0x1096EF48);
            lastpositiony = Gecko.peek(0x1096EF4C);
            lastpositionz = Gecko.peek(0x1096EF50);
            lastfacing = Gecko.peek(0x1096EF10);
        }
        #endregion

        #region Quick access menu (not visible in this fork)
        private void geckocodesMenu_Click(object sender, EventArgs e)
        {
            //cheatTab.SelectedIndex = 0;
        }

        private void linktweaksMenu_Click(object sender, EventArgs e)
        {
            //cheatTab.SelectedIndex = 1;
        }

        private void itemspoofMenu_Click(object sender, EventArgs e)
        {
            //cheatTab.SelectedIndex = 2;
        }

        private void inventoryMenu_Click(object sender, EventArgs e)
        {
            //cheatTab.SelectedIndex = 3;
        }

        private void bottleMenu_Click(object sender, EventArgs e)
        {
            //cheatTab.SelectedIndex = 4;
        }

        private void itemmodsMenu_Click(object sender, EventArgs e)
        {
            //cheatTab.SelectedIndex = 5;
        }

        private void coordinatesMenu_Click(object sender, EventArgs e)
        {
            //cheatTab.SelectedIndex = 6;
        }

        private void customteleportMenu_Click(object sender, EventArgs e)
        {
            //cheatTab.SelectedIndex = 7;
        }

        private void objectswapMenu_Click(object sender, EventArgs e)
        {
            //cheatTab.SelectedIndex = 8;
        }

        private void stageloaderMenu_Click(object sender, EventArgs e)
        {
            //cheatTab.SelectedIndex = 9;
        }
        #endregion

        #region Coordinates Legacy (remove at some point)
        private void copycoordinates_Click(object sender, EventArgs e)
        {
            Clipboard.SetText($"X={Gecko.peek(0x1096EF48)}\nY={Gecko.peek(0x1096EF4C)}\nZ={Gecko.peek(0x1096EF50)}\nFacing={Gecko.peek(0x1096EF10)}");
        }

        private void opencoordinatesfile_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Environment.CurrentDirectory + @"\Coordinates.ini");
            }
            catch
            {
                MessageBox.Show("Couldn't find 'Coordinates.ini'.\n\nMake sure the file is in the same folder as the Program.");
            }

        }
        #endregion

        #region Links
        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Dekirai/WindWakerHDTrainer");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/396014/");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/350100/");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/399819/");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/327808/");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/393668/");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/jacobbutfiloski/WindWakerHDTrainer");
        }
        private void githubItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Dekirai/WindWakerHDTrainer");
        }

        private void dekiraiItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/393668/");
        }

        private void cosmocortneyItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/327808/");
        }

        private void theword21Item_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/350100/");
        }

        private void gamepilItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/399819/");
        }

        private void pikaarcItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://gbatemp.net/members/396014/");
        }

        #endregion

        #region MemFile Tab
        private string savedMap = "";
        private uint savedRoom;
        private uint savedSpawn;
        private uint finalHealth;
        private uint finalMagic;
        private uint finalRupees;
        private uint finalArrows;
        private uint finalBombs;
        private bool memfileDpadControls;

        private string folderPath;

        private decimal finalRoom;
        private decimal finalSpawn;
        private static uint savedLayer = 0;
        private static uint coordx;
        private static uint coordy;
        private static uint coordz;
        private static uint coordfacing;
        private bool returnedMap = false;
        private bool searching = false;

        //SAVE MEMFILE
        private void button1_Click(object sender, EventArgs e)
        {
            //memfile_save_fbd.Title = "Save Memfile";
            savedMap = label_memfile_stage.Text;
            coordx = Gecko.peek(0x1096EF48);
            coordy = Gecko.peek(0x1096EF4C);
            coordz = Gecko.peek(0x1096EF50);
            coordfacing = Gecko.peek(0x1096EF10);
       
            maptimer.Stop();
            finalRoom = returnRoom();
            finalSpawn = returnSpawn();
            //COORDS
            
            finalHealth = returnHealth();
            finalMagic = Gecko.peek(0x15073694);
            finalRupees = Gecko.peek(0x15073684);
            finalArrows = Gecko.peek(0x150736E9);
            finalBombs = Gecko.peek(0x150736EA);      
        }
        private void button_memfile_export_Click(object sender, EventArgs e)
        {
            if (memfile_save_fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    folderPath = memfile_save_fbd.SelectedPath;
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(folderPath + @"\" + textBox_memfile_name.Text + ".memfile");
                    writer.WriteLine(savedMap);
                    writer.WriteLine(returnRoom());
                    writer.WriteLine(returnSpawn());
                    writer.WriteLine(savedLayer);

                    writer.WriteLine(coordx);
                    writer.WriteLine(coordy);
                    writer.WriteLine(coordz);
                    writer.WriteLine(coordfacing);

                    writer.WriteLine(returnHealth());
                    writer.WriteLine(finalMagic);
                    writer.WriteLine(finalRupees);
                    writer.WriteLine(finalArrows);
                    writer.WriteLine(finalBombs);

                    writer.Close();
                    writer.Dispose();

                }
                catch (Exception es)
                {
                    Console.WriteLine(es);
                }
            }
        }
        //MEMFILE LOAD
        List<String> validMaps = new List<String> { "A_mori", "A_nami", "A_R00", "A_umikz", "Abesso", "Abship", "Adanmae", "ADMumi", "Amos_T", "Asoko", "Atorizk", "Cave01", "Cave02", "Cave03", "Cave04", "Cave05", "Cave06", "Cave07", "Cave08", "Cave09", "Cave10", "Cave11", "Comori", "DmSpot0", "E3ROOP", "Ebesso", "Edaichi", "Ekaze", "ENDumi", "Fairy01", "Fairy02", "Fairy03", "Fairy04", "Fairy05", "Fairy06", "figureA", "figureB", "figureC", "figureD", "figureE", "figureF", "figureG", "GanonA", "GanonB", "GanonC", "GanonD", "GanonE", "GanonJ", "GanonK", "GanonL", "GanonM", "GanonN", "GTower", "H_test", "Hyroom", "Hyrule", "I_SubAN", "I_TestM", "I_TestR", "ITest61", "ITest62", "ITest63", "K_Test2", "K_Test3", "K_Test4", "K_Test5", "K_Test6", "K_Test8", "K_Test9", "K_Testa", "K_Testb", "K_Testc", "K_Testd", "K_Teste", "Kaisen", "KATA_HB", "KATA_RM", "kazan", "kaze", "kazeB", "kazeMB", "kenroom", "kinBOSS", "kindan", "kinMB", "LinkRM", "LinkUG", "M_Dai", "M_DaiB", "M_DaiMB", "M_Dra09", "M_DragB", "M_NewD2", "M2ganon", "M2tower", "ma2room", "ma3room", "majroom", "MajyuE", "MiniHyo", "MiniKaz", "Mjtower", "morocam", "Msmoke", "Mukao", "Name", "Nitiyou", "Obombh", "Obshop", "Ocean", "Ocmera", "Ocrogh", "Ojhous", "Ojhous2", "Omasao", "Omori", "Onobuta", "Opub", "Orichh", "Otkura", "Pdrgsh", "Pfigure", "Pjavdou", "Pnezumi", "PShip", "Pship2", "Pship3", "sea", "sea_E", "sea_T", "ShipD", "Siren", "SirenB", "SirenMB", "SubD42", "SubD43", "SubD44", "SubD45", "SubD51", "SubD71", "TEST", "TF_01", "TF_02", "TF_03", "TF_04", "TF_05", "TF_06", "TF_07", "tincle", "TyuTyu", "VrTest", "WarpD", "Xboss0", "Xboss1", "Xboss2", "Xboss3" };
        private async void button_memfile_load_Click(object sender, EventArgs e) //Inject button
        {
            if(memfile_open_ofd.ShowDialog() == DialogResult.OK)
            {
                String[] lines = File.ReadAllLines(memfile_open_ofd.FileName);
                savedMap = lines[0];
                finalRoom = Convert.ToDecimal(lines[1]);
                finalSpawn = Convert.ToDecimal(lines[2]);
                coordx = Convert.ToUInt32(lines[4]);
                coordy = Convert.ToUInt32(lines[5]);
                coordz = Convert.ToUInt32(lines[6]);
                coordfacing = Convert.ToUInt32(lines[7]);
                finalHealth = Convert.ToUInt32(lines[8]);
                finalMagic = Convert.ToUInt32(lines[9]);
                finalRupees = Convert.ToUInt32(lines[10]);
                finalArrows = Convert.ToUInt32(lines[11]);
                finalBombs = Convert.ToUInt32(lines[12]);
                textBox_memfile_name.Text = System.IO.Path.GetFileNameWithoutExtension(memfile_open_ofd.FileName);
            }      
        }

        private async void button_mem_load_Click(object sender, EventArgs e) //Load Button
        {
            loadMemfile();
        }

        private async void loadMemfile()
        {
            if (validateMap(savedMap))
            {
                LoadHealth();
                Gecko.poke32(0x15073694, finalMagic);
                Gecko.poke32(0x15073684, finalRupees);
                Gecko.poke32(0x150736E9, finalArrows);
                Gecko.poke32(0x150736EA, finalBombs);
                if(savedMap == "M_NewD2" || savedMap == "kindan" || savedMap == "Siren" || savedMap == "M_Dai" || savedMap == "kaze") //if in dungeon, load test room first to prevent teleporting to entrance
                {
                    Console.WriteLine("Saved in dungeon");
                    Gecko.poke32(0x109763F0, 0x00000000); //Clearing Stage name to prevent Crash
                    Gecko.poke32(0x109763F4, 0x00000000); //Clearing Stage name to prevent Crash
                    Gecko.poke32(0x109763F0, 0x4533524F);
                    Gecko.poke32(0x109763F4, 0x4F500000);
                    Console.WriteLine(finalRoom + "Final Room");
                    Gecko.poke08(0x109763F9, 0); //Spawn ID
                    Gecko.poke08(0x109763FA, 0); //Room ID
                    Gecko.poke08(0x109763FB, 0); //Layer ID
                    Gecko.poke08(0x109763FC, 0x01); //Reload Stage
                }
                var readmap = await Map.SetMap(savedMap);
                Gecko.poke32(0x109763F0, 0x00000000); //Clearing Stage name to prevent Crash
                Gecko.poke32(0x109763F4, 0x00000000); //Clearing Stage name to prevent Crash
                Gecko.poke32(0x109763F0, readmap[0]);
                Gecko.poke32(0x109763F4, readmap[1]);
                Console.WriteLine(finalRoom + "Final Room");
                Gecko.poke08(0x109763F9, Convert.ToByte(finalSpawn)); //Spawn ID
                Gecko.poke08(0x109763FA, Convert.ToByte(finalRoom)); //Room ID
                Gecko.poke08(0x109763FB, Convert.ToByte(0)); //Layer ID
                Gecko.poke08(0x109763FC, 0x01); //Reload Stage
                Thread.Sleep(3500);
                //TP TO COORDS
                LoadCoords();
            }
            else
            {
                MessageBox.Show("Invalid Map, map '" + savedMap + "' is invalid.");
            }
            maptimer.Start();
        }

        private void checkBox_memfile_controls_CheckedChanged(object sender, EventArgs e)
        {
            memfileDpadControls = checkBox_memfile_controls.Checked;
        }

        private bool validateMap(string MapName)
        {
            foreach(String s in validMaps)
            {
                if(MapName == s)
                {
                    return true;
                }
            }
            return false;
        }

        private void LoadCoords()
        {
            uint collision = 0x00000000;
            uint collisionold = 0x00000000;
            collision = Gecko.peek(0x1097648C);
            collisionold = Gecko.peek(collision + 0x00000834);
            Gecko.poke32(collision + 0x00000834, 0x00004004);
            Gecko.poke32(0x1096EF48, coordx);
            Gecko.poke32(0x1096EF4C, coordy);
            Gecko.poke32(0x1096EF50, coordz);
            Gecko.poke32(0x1096EF10, coordfacing);
            Thread.Sleep(1000);
            Gecko.poke32(collision + 0x00000834, collisionold);
        }

        private void LoadHealth()
        {
            Gecko.poke32(0x15073681, finalHealth);
        }
        private uint returnRoom()
        {
            return savedRoom;
        }

        private uint returnSpawn()
        {
            return savedSpawn;
        }

        private uint returnHealth()
        {
            return Gecko.peek(0x15073681);
        }
        private void memfilesPage_Enter(object sender, EventArgs e)
        {
            //maptimer.Start();
        }

        private void stageloaderPage_Leave(object sender, EventArgs e)
        {

        }
        #endregion

        #region Watches Tab
        List<uint> watchList = new List<uint> { 0x1096EF48, 0x1096EF4C, 0x1096EF50, 0x1096EF10, 0x10976554};
        List<String> watchLabels = new List<String> {"Link X", "Link Y", "Link Z", "Facing", "Get Item Value"};
        List<uint> watchValues = new List<uint> { };
        List<Label> labelList = new List<Label> { };
        List<Label> valueLabel = new List<Label> { };
        int labelX = 3;
        int labelYMod = 20;
        int watchX = 140;
        private void watchestimer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("hi");
            watchValues.Clear();
            for(int i = 0; i < valueLabel.Count; i++) //Add labels to panel
            {
                watchValues.Add(Gecko.peek(watchList[i]));
                Console.WriteLine(valueLabel[i].Text);
            }
            for (int i = 0; i < valueLabel.Count; i++)
            {
                valueLabel[i].Text = watchValues[i].ToString();
            }
        }
        

        private void button9_Click(object sender, EventArgs e)
        {
            AddWatch(textBox_watch_name.Text, Convert.ToUInt32(textBox_watch_address.Text, 16));

        }

        private void AddWatch(String name, uint address)
        {
            /*watchestimer.Stop();
            foreach (Control c in panel_watches.Controls)
            {
                panel_watches.Controls.Remove(c);
            }
            watchLabels.Add(name);
            watchList.Add(address);
            UpdateWatchLabels();
            watchestimer.Start();*/
        }

        private void UpdateWatchLabels()
        {
            /*watchestimer.Interval = 500;
            watchValues.Clear();
            labelList.Clear();
            //valueLabel.Clear();
            foreach(Control c in panel_watches.Controls)
            {
                panel_watches.Controls.Remove(c);
            }

            for (int i = 0; i < watchList.Count; i++) //Label Labels (the names i dont know how to phrase this better lol)
            {
                Console.WriteLine(watchLabels[i]);
                Label l = new Label(); //Names
                l.Location = new System.Drawing.Point(labelX, i * labelYMod);
                labelList.Add(l);
                labelList[i].Text = watchLabels[i];
                labelList[i].ForeColor = System.Drawing.Color.White;
                panel_watches.Controls.Add(labelList[i]);
                l = new Label(); //Values
                l.Location = new System.Drawing.Point(watchX, i * labelYMod);
                valueLabel.Add(l);
                valueLabel[i].ForeColor = System.Drawing.Color.White;
                panel_watches.Controls.Add(valueLabel[i]);
            }

            //valueLabel.Clear();
            for (int i = 0; i < watchValues.Count; i++)
            {
                Label l = new Label(); //Values
                l.Location = new System.Drawing.Point(watchX, i * labelYMod);
                valueLabel.Add(l);
                valueLabel[i].ForeColor = System.Drawing.Color.White;
                panel_watches.Controls.Add(valueLabel[i]);
            }

            foreach (Label s in valueLabel)
            {
                s.Text = "";
            }
            watchestimer.Start();*/
        }
        #endregion

        #region Debug Tab
        String lastError;
        private void button_debug_poker_Click(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_poker.SelectedIndex)
                {
                    case 0://8bit
                        Gecko.poke08(Convert.ToUInt32(textBox_debug_poke_memAdd.Text, 16), Convert.ToByte(textBox_debug_poke_value.Text, 16));
                        break;
                    case 1://16bit
                        Gecko.poke16(Convert.ToUInt32(textBox_debug_poke_memAdd.Text, 16), Convert.ToByte(textBox_debug_poke_value.Text, 16));
                        break;
                    case 2://32bit
                        Gecko.poke32(Convert.ToUInt32(textBox_debug_poke_memAdd.Text, 16), Convert.ToByte(textBox_debug_poke_value.Text, 16));
                        break;
                }
                label_poker.Text = "Value " + textBox_debug_poke_value.Text + " successfully written to address " + textBox_debug_poke_memAdd.Text;
                label_poker.ForeColor = System.Drawing.Color.Green;
            } catch(Exception ex)
            {
                label_poker.Text = "Poke failed. Click for more details.";
                label_poker.ForeColor = System.Drawing.Color.Red;
                lastError = ex.ToString();
            }
        }

        private void label_poker_Click(object sender, EventArgs e)
        {
            MessageBox.Show(lastError, "Error Report", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void button_debug_peek_Click(object sender, EventArgs e)
        {
            try
            {
                uint result;
                result = Gecko.peek(Convert.ToUInt32(textBox_debug_peek_memAdd.Text, 16));
                switch (comboBox_debug_peek.SelectedIndex)
                {
                    case 0:
                        result = Convert.ToUInt32(result);
                        break;
                    case 1:
                        result = Convert.ToUInt16(result);
                        break;
                    case 2:
                        result = Convert.ToUInt32(result);
                        break;
                    default:
                        result = 0;
                        break;
                }
                
                MessageBox.Show("0x" + Convert.ToString(result, 16), textBox_debug_peek_memAdd.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch(Exception ex)
            {
                MessageBox.Show(e.ToString(), "Error Report", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }


        #endregion

        #region Global Input Update
        bool dpaddown;
        UInt32 color = 0;
        GameInput gameInput = new GameInput();
        private async void inputtimer_Tick(object sender, EventArgs e)
        {
            //Watermark
            color += 10000;
            if(color == 2147483646)
            {
                color = 0;
            }
            Gecko.poke32(0x26917418, color);
            if(dpaddown)
            {
                if(gameInput.IsDpadDownDown(Gecko))
                    Gecko.poke08(0x15073683, Convert.ToByte(currentheartsBox.Value));
            }

            if(memfileDpadControls)
            {
                if (gameInput.IsDpadLeftDown(Gecko))
                    button1_Click(new object(), new EventArgs());
                else if (gameInput.IsDpadRightDown(Gecko))
                    loadMemfile();
            }
        }


        #endregion

        #region Dump
        private void currentheartsBox_ValueChanged(object sender, EventArgs e)
        {

        }

        private void infitems_CheckedChanged(object sender, EventArgs e)
        { }

        private void moonjump_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void memfilesPage_Leave(object sender, EventArgs e)
        { }

        private void Main_Load(object sender, EventArgs e)
        {

        }
        #endregion

        private void cheatTab_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font f;
            Brush backBrush;
            Brush foreBrush;

            if (e.Index == this.cheatTab.SelectedIndex)
            {
                f = new Font(e.Font, FontStyle.Regular);
                backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.FromArgb(255, 54, 57, 63), Color.FromArgb(255, 54, 57, 63), System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);
                foreBrush = Brushes.White;
            }
            else
            {
                f = e.Font;
                backBrush = new SolidBrush(e.BackColor);
                foreBrush = new SolidBrush(e.ForeColor);
            }

            string tabName = this.cheatTab.TabPages[e.Index].Text;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            e.Graphics.FillRectangle(backBrush, e.Bounds);
            Rectangle r = e.Bounds;
            r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
            e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

            sf.Dispose();
            if (e.Index == this.cheatTab.SelectedIndex)
            {
                f.Dispose();
                backBrush.Dispose();
            }
            else
            {
                backBrush.Dispose();
                foreBrush.Dispose();
            }

            Graphics g = e.Graphics;
            Pen p = new Pen(Color.FromArgb(255, 54, 57, 63), 4);
            g.DrawRectangle(p, this.cheatTab.Bounds);
        }

        
    }
} 