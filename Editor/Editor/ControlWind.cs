using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Editor
{
    public partial class ControlWind : Form
    {
        public bool grid = false;
        public bool collisions = false;
        public bool exits = false;
        public bool Visual = true;
        public string SelectedWorld = "";
        public string SelectedTile = "";
        public string LastName = "";
        Dictionary<string, TileMap> tilemaps;
        string lastkeys = "";
        public ControlWind()
        {
            InitializeComponent();
            tilemaps = World.TileMaps;
            if (lastkeys != tilemaps.Keys.CombineKeys())
            {
                comboBox2.Items.Clear();
                comboBox1.Items.Clear();
                foreach (string key in tilemaps.Keys)
                {
                    comboBox2.Items.Add(key);
                    comboBox1.Items.Add(key);
                }
                lastkeys = tilemaps.Keys.CombineKeys();
            }
        }

        public void UpdateForm()
        {
            WorldsUpdate();
            UpdateThing();
            UpdateText();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) { checkBox1.Text = "On"; grid = true; }
            else { checkBox1.Text = "Off"; grid = false; }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Control_Load(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex != -1 )
            {
                if (World.TileMaps.ContainsKey(comboBox2.Items[comboBox2.SelectedIndex].ToString()))
                {
                    World.ActiveMap = comboBox2.Items[comboBox2.SelectedIndex].ToString();
                }
            }
        }
        private void WorldsUpdate()
        {
            if (lastkeys != World.TileMaps.Keys.CombineKeys())
            {
                comboBox2.Items.Clear();
                comboBox1.Items.Clear();
                foreach (string key in World.TileMaps.Keys)
                {
                    comboBox2.Items.Add(key);
                    comboBox1.Items.Add(key);
                }
                lastkeys = World.TileMaps.Keys.CombineKeys();
            }
            if (SelectedWorld != World.ActiveMap)
            {
                int index = 0;
                foreach (string key in World.TileMaps.Keys)
                {
                    if (key == World.ActiveMap) { break; }
                    index++;
                }
                comboBox2.SelectedIndex = index;
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) { checkBox2.Text = "On"; collisions = true; }
            else { checkBox2.Text = "Off"; collisions = false; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Resources.SaveMap();
            
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked) { checkBox3.Text = "On"; exits = true; }
            else { checkBox3.Text = "Off"; exits = false; }
        }

        private void UpdateThing()
        {
            if (SelectedWorld != World.ActiveMap)
            {
                SelectedWorld = World.ActiveMap;
                button1.Text = "Save " + '"'+SelectedWorld+'"';
            }
        }

        private void UpdateText()
        {
            if (LastName != textBox2.Text)
            {
                LastName=textBox2.Text;
                button2.Text = "Create " +'"'+ LastName+'"';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex != -1)
            {
                World.TileMaps.Add(textBox2.Text, new TileMap(new Dictionary<Microsoft.Xna.Framework.Point, Tile>(), new Dictionary<Microsoft.Xna.Framework.Point, Tile>(), new Dictionary<Microsoft.Xna.Framework.Point, string[]>(), new Dictionary<Microsoft.Xna.Framework.Point, Vector2>(), comboBox3.SelectedIndex, textBox2.Text));
                World.ActiveMap = textBox2.Text;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                if (!World.TileMaps[World.ActiveMap].exits.ContainsKey(new Microsoft.Xna.Framework.Point(int.Parse(textBox1.Text), int.Parse(textBox3.Text))) && !World.TileMaps[World.ActiveMap].exitpos.ContainsKey(new Microsoft.Xna.Framework.Point(int.Parse(textBox1.Text), int.Parse(textBox3.Text))))
                {
                    World.TileMaps[World.ActiveMap].exits.Add(new Microsoft.Xna.Framework.Point(int.Parse(textBox1.Text), int.Parse(textBox3.Text)), new string[2] { comboBox1.Items[comboBox1.SelectedIndex].ToString(), World.ActiveMap });
                    World.TileMaps[World.ActiveMap].exitpos.Add(new Microsoft.Xna.Framework.Point(int.Parse(textBox1.Text), int.Parse(textBox3.Text)), new Vector2(int.Parse(textBox5.Text), int.Parse(textBox4.Text)));
                }
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (Visual)
            {
                checkBox4.Text = "Off";
                Visual = false;
            }
            else
            {
                checkBox4.Text = "On";
                Visual = true;
            }
        }
    }
}
