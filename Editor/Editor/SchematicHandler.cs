using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Editor
{
    class SchematicHandler : Component
    {
        string lastkeys = "";
        bool expand = false;
        Vector2 drawpos = new Vector2(0, 0);
        Vector2 size = new Vector2(0, 0);
        int areaheight = 300;
        List<string> relevantkeys = new List<string>();
        public SchematicHandler(int width, int height, Vector2 position)
            : base("Schematics", width, height, position)
        {
            AddControl(new DropDown(new Vector2(50, 310), 120, 20, new List<string>(),true));
            AddControl(new TextBox(new Vector2(60, 370), 120, 20, ""));
            AddControl(new Label(new Vector2(10, 370), "Name:"));
            AddControl(new Button(new Vector2(60, 420), 80, 20, "Create"));
            AddControl(new Button(new Vector2(10, 520), 180, 20, "Save Schematics"));
        }
        public override void Update(Vector2 ofs)
        {
            height = Game1.WindowHeight - 50;
            foreach (Control control in Controls.Values)
            {
                control.Update(ofs);
            }
            if (Controls["Button1"].Trigger && Controls["TextBox1"].Text != "" && TileMover.held.Count > 0)
            {
                Dictionary<Point, Tile> tilemap = new Dictionary<Point, Tile>();
                Dictionary<Point, Tile> collision = new Dictionary<Point, Tile>();
                int c = 0;
                foreach (Tile t in TileMover.held)
                {
                    if (t.type == 0 || t.type == 1)
                    {
                        tilemap.Add(new Point((int)t.X, (int)t.Y), new Tile(t));
                    }
                    else
                    {
                        collision.Add(new Point((int)t.X, (int)t.Y), new Tile(t));
                    }
                    c++;
                }
                World.Schematics.Add(Controls["TextBox1"].Text, new Schematic(tilemap, collision, World.TileMaps[World.ActiveMap].type, Controls["TextBox1"].Text));
            }
            if (Controls["Button2"].Trigger)
            {
                Resources.SaveSchematics();
            }
            if (Controls["DropDown1"].SelectedIndex != -1)
            {
                Schematic s = World.Schematics[Controls["DropDown1"].Items[Controls["DropDown1"].SelectedIndex]];
                if (TileMover.held.Count == 0 && Listener.mrect.Intersects(new Rectangle((int)(drawpos.X + ofs.X), (int)(drawpos.Y + ofs.Y), (int)(s.Width * size.X), (int)(s.Height * size.Y))) && Listener.NewPressLeft())
                {
                    TileMover.selection = new List<Point>();
                    TileMover.selection.Add(new Point(TileMover.ppos.X, TileMover.ppos.Y));
                    TileMover.selection.Add(new Point(TileMover.ppos.X + s.Width - 1, TileMover.ppos.Y + s.Height - 1));

                    foreach (Tile t in s.tilemap.Values)
                    {
                        TileMover.held.Add(new Tile(t));
                    }
                    foreach (Tile t in s.collision.Values)
                    {
                        TileMover.held.Add(new Tile(t));
                    }
                    TileMover.moving = true;
                }
                else if (Listener.mrect.Intersects(new Rectangle((int)(drawpos.X + ofs.X), (int)(drawpos.Y + ofs.Y), (int)(s.Width * size.X), (int)(s.Height * size.Y))))
                {
                    expand = true;
                }
                else
                {
                    expand = false;
                }
            }
            if (World.Active() != null)
            {
                UpdateSchems();
            }
        }
        public override void Draw(Vector2 ofs)
        {
            //Shapes.DrawRectangle(width, height, ofs, tabOn, 0);
            List<Control> sorted = Sort(Controls.Values);

            for (int i = 0; i < sorted.Count; i++)
            {
                sorted[i].Draw(ofs);
            }
            if (Controls["DropDown1"].SelectedIndex != -1)
            {
                Schematic s = World.Schematics[Controls["DropDown1"].Items[Controls["DropDown1"].SelectedIndex]];
                //size = new Vector2(width / s.Width, areaheight / s.Height);
                size = new Vector2(8, 8);
                if (expand)
                {
                    drawpos = new Vector2(width / 2 - (s.Width * (size.X + 2)) / 2, areaheight / 2 - (s.Height * (size.Y + 2)) / 2);
                }
                else
                {
                    drawpos = new Vector2(width / 2 - (s.Width * size.X) / 2, areaheight / 2 - (s.Height * size.Y) / 2);
                }
                s.Draw(drawpos + ofs, new Vector2(size.X, size.Y), expand);

            }
        }

        private void UpdateSchems()
        {
            string t = Relevant();
            if (lastkeys != t)
            {
                Controls["DropDown1"].Items = new List<string>();
                foreach (Schematic schem in World.Schematics.Values)
                {
                    if (schem.type == World.Active().type)
                    {
                        Controls["DropDown1"].Items.Add(schem.filename);
                    }
                }
                lastkeys = t;
            }
        }

        private string Relevant()
        {
            string s = "";
            foreach (Schematic schm in World.Schematics.Values)
            {
                if (schm.type == World.Active().type)
                {
                    s += schm.filename;
                }
            }
            return s;
        }
    }
}
