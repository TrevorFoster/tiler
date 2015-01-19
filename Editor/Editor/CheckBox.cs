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
    class CheckBox:Control
    {
        public bool Checked = false;
        public bool LastState = false;
        public bool Changed = false;
        public int Width = 18;
        public int Height = 18;
        string font = "font";

        public CheckBox(Vector2 pos,string Text,bool check): base(pos,Text)
        {
            Checked = check;
        }

        public override void Update(Vector2 ofs)
        {
            if(Listener.mrect.Intersects(new Rectangle((int)(position.X+ofs.X),(int)(position.Y+ofs.Y),Width,Height)) && Listener.NewPressLeft())
            {
                if (Checked) { Checked = false; }
                else { Checked = true; }
            }

            if (LastState != Checked)
            {
                LastState = Checked;
                Trigger = true;
            }
            else { Trigger = false; }
        }
        public override void Draw(Vector2 ofs)
        {
            Shapes.DrawRectangle(Width, Height, ofs + position, new Color(0, 0, 0, 150), 0);
            if (Checked)
            {
                Shapes.DrawRectangle(Width - 10, Height - 10, position + ofs + new Vector2(5, 5), Color.Gray, 0);
            }
            Shapes.DrawRectangle(Width, Height, ofs + position, new Color(255, 255, 255, 100), 2);
            Game1.spriteBatch.DrawString(Resources.Fonts[font], Text, position + ofs + new Vector2(Width + 5, 0), Color.Black);
        }

        public override bool IsChecked()
        {
            return Checked;
        }
    }
}
