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
    class AddExits:Window
    {

        string lastkeys = ""; 
        public AddExits( Vector2 position, int Width, int Height, bool Hidden)
            : base(position, Width, Height, Hidden, true, "ExitAddition")
        {
            AddControl(new DropDown(new Vector2(125, 150), 80, 20, new List<string>(),false));
            AddControl(new Label(new Vector2(10, 40), "X:"));
            AddControl(new TextBox(new Vector2(25, 40), 60, 20, ""));
            AddControl(new Label(new Vector2(10, 160), "Y:"));
            AddControl(new TextBox(new Vector2(25, 160), 60, 20, ""));
            AddControl(new Label(new Vector2(240, 40), "X:"));
            AddControl(new TextBox(new Vector2(265, 40), 60, 20, ""));
            AddControl(new Label(new Vector2(240, 160), "Y:"));
            AddControl(new TextBox(new Vector2(265, 160), 60, 20, ""));
            AddControl(new Button(new Vector2(125, 90), 100, 20, "Add Exit"));
        }

        public override int Updates()
        {
            UpdateWorlds();
            if (Controls["Button1"].Trigger && Controls["TextBox1"].Text.IsDigit() && Controls["TextBox2"].Text.IsDigit() &&
                Controls["TextBox3"].Text.IsDigit() && Controls["TextBox4"].Text.IsDigit() && Controls["DropDown1"].SelectedIndex!=-1)
            {
                if (!World.TileMaps[World.ActiveMap].exits.ContainsKey(new Point(int.Parse(Controls["TextBox1"].Text), int.Parse(Controls["TextBox2"].Text))) && !World.TileMaps[World.ActiveMap].exitpos.ContainsKey(new Point(int.Parse(Controls["TextBox1"].Text), int.Parse(Controls["TextBox2"].Text))))
                {
                    World.TileMaps[World.ActiveMap].exits.Add(new Point(int.Parse(Controls["TextBox1"].Text), int.Parse(Controls["TextBox2"].Text)), new string[2] { Controls["DropDown1"].Items[Controls["DropDown1"].SelectedIndex].ToString(), World.ActiveMap });
                    World.TileMaps[World.ActiveMap].exitpos.Add(new Point(int.Parse(Controls["TextBox1"].Text), int.Parse(Controls["TextBox2"].Text)), new Vector2(int.Parse(Controls["TextBox3"].Text), int.Parse(Controls["TextBox4"].Text)));
                }
            }
            else if (Controls["Button1"].Trigger)
            {
                WindowManager.NewWindow(new Alert(new Vector2(position.X+Width/2, position.Y+Height/2), 200, 100, "Invalid"));
            }
            return -1;
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
