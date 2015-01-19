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
    class Options:Component
    {
        string lastkeys = "";

        public Options(int width,int height,Vector2 position)
            :base("Options",width,height,position)
        {
            #region Toggle Modifiers
            AddControl(new Label(new Vector2(5, 10), "Show:"));
            AddControl(new CheckBox(new Vector2(50, 20), "Grid", false));
            AddControl(new CheckBox(new Vector2(50, 60),"Collision Tiles",false));
            AddControl(new CheckBox(new Vector2(50, 100), "Visual Tiles", true));
            AddControl(new CheckBox(new Vector2(50, 140), "Exits", false));
            #endregion
            #region Save Button
            AddControl(new Button(new Vector2(50, 200), 120, 20, "Save"));
            #endregion
            #region World Selector
            AddControl(new Label(new Vector2(5, 300), "World:"));
            AddControl(new DropDown(new Vector2(50, 300), 120, 20, new List<string>(),false));
            #endregion
            #region Zoom Selection
            AddControl(new Label(new Vector2(5, 265), "Zoom:"));
            AddControl(new DropDown(new Vector2(50, 263), 120, 20, new List<string>(){"%50","%75","%100","%150","%200","%300"},false));
            #endregion
            #region World create
            AddControl(new Label(new Vector2(5, 387), "Name:"));
            AddControl(new TextBox(new Vector2(50, 387), 140, 20, ""));
            AddControl(new Label(new Vector2(5, 420), "Type:"));
            AddControl(new DropDown(new Vector2(50, 420), 120, 20, new List<string>(){"Top Down","Side Scroll"},false));
            AddControl(new Button(new Vector2(50,480), 120, 20, "Create"));
            #endregion
            #region Exit Addition Button
            AddControl(new Button(new Vector2(50,550),120, 20, "Exit Panel"));
            #endregion
            #region Dialogue Tree Editor
            AddControl(new Button(new Vector2(50, 580), 120, 20, "Tree Editor"));
            #endregion
        }

        

        public override void Update(Vector2 ofs)
        {
            UpdateWorlds();
            height = Game1.WindowHeight - 50;
            foreach (Control control in Controls.Values)
            {
                control.Update(ofs);
            }
            if (Controls["CheckBox1"].Trigger)
            {
                if (Controls["CheckBox1"].IsChecked())
                {
                    Game1.Grid = true;
                }
                else { Game1.Grid = false; }
            }
            if (Controls["CheckBox2"].Trigger)
            {
                if (Controls["CheckBox2"].IsChecked())
                {
                    Game1.Collision = true;
                }
                else { Game1.Collision = false; }
            }
            if (Controls["CheckBox3"].Trigger)
            {
                if (Controls["CheckBox3"].IsChecked())
                {
                    Game1.Visual = true;
                }
                else { Game1.Visual = false; }
            }
            if (Controls["CheckBox4"].Trigger)
            {
                if (Controls["CheckBox4"].IsChecked())
                {
                    Game1.Exits = true;
                }
                else { Game1.Exits = false; }
            }
            if (Controls["Button1"].Trigger)
            {
                Resources.SaveMap();
            }
            if (Controls["Button3"].Trigger)
            {
                if (!WindowManager.Exists("AddExits"))
                {
                    WindowManager.NewWindow(new AddExits( new Vector2(Game1.WindowWidth / 2 - 175, Game1.WindowHeight / 2 - 125), 350, 250, true));
                }
            }
            if(Controls["Button4"].Trigger)
            {
                if(!WindowManager.Exists("DialogueBoxWindow"))
                {
                    WindowManager.NewWindow(new DialogBoxWindow(new Vector2(20, 20), Game1.WindowWidth - 40, Game1.WindowHeight - 40,"test"));
                }
            }
            if (Controls["DropDown1"].Trigger)
            {
                if (Controls["DropDown1"].SelectedIndex != -1)
                {
                    if (World.TileMaps.ContainsKey(Controls["DropDown1"].Items[Controls["DropDown1"].SelectedIndex]))
                    {
                        World.ActiveMap = Controls["DropDown1"].Items[Controls["DropDown1"].SelectedIndex];
                        TileMover.held.Clear();
                        TileMover.moving = false;
                        TileMover.selection.Clear();
                    }
                }
            }
            if (Controls["DropDown2"].Trigger)
            {
                switch (Controls["DropDown2"].SelectedIndex)
                {
                    case 0:
                        Game1.gridcellwidth = (int)(Game1.origgridcellwidth * .5f);
                        Game1.gridcellheight = (int)(Game1.origgridcellheight * .5f);
                        break;
                    case 1:
                        Game1.gridcellwidth = (int)(Game1.origgridcellwidth * .75f);
                        Game1.gridcellheight = (int)(Game1.origgridcellheight * .75f);
                        break;
                    case 2:
                        Game1.gridcellwidth = (int)(Game1.origgridcellwidth);
                        Game1.gridcellheight = (int)(Game1.origgridcellheight);
                        break;
                    case 3:
                        Game1.gridcellwidth = (int)(Game1.origgridcellwidth * 1.50f);
                        Game1.gridcellheight = (int)(Game1.origgridcellheight * 1.50f);
                        break;
                    case 4:
                        Game1.gridcellwidth = (int)(Game1.origgridcellwidth * 2f);
                        Game1.gridcellheight = (int)(Game1.origgridcellheight * 2f);
                        break;
                    case 5:
                        Game1.gridcellwidth = (int)(Game1.origgridcellwidth * 3f);
                        Game1.gridcellheight = (int)(Game1.origgridcellheight * 3f);
                        break;
                }
            }
            if (Controls["Button2"].Trigger)
            {
                if (Controls["DropDown3"].SelectedIndex != -1 && Controls["TextBox1"].Text!="")
                {
                    World.TileMaps.Add(Controls["TextBox1"].Text, new TileMap(new Dictionary<Microsoft.Xna.Framework.Point, Tile>(), new Dictionary<Microsoft.Xna.Framework.Point, Tile>(), new Dictionary<Microsoft.Xna.Framework.Point, string[]>(), new Dictionary<Microsoft.Xna.Framework.Point, Vector2>(), Controls["DropDown3"].SelectedIndex, Controls["TextBox1"].Text));
                    World.ActiveMap = Controls["TextBox1"].Text;
                }
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
        }

        private void UpdateWorlds()
        {
            if (lastkeys != World.TileMaps.Keys.CombineKeys())
            {
                Controls["DropDown1"].Items.Clear();

                foreach (string key in World.TileMaps.Keys)
                {
                    Controls["DropDown1"].Items.Add(key);
                }
                lastkeys = World.TileMaps.Keys.CombineKeys();
            }
        }
    }
}
