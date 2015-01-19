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
    class Button:Control
    {
        int Width = 0;
        int Height = 0;
        bool Last = false;
        string font = "font";

        public Button(Vector2 position,int Width, int Height, string text)
            : base(position, text)
        {
            this.Width = Width;
            this.Height = Height;
        }

        public override void Update(Vector2 ofs)
        {
            Trigger = false;
            if (Listener.mrect.Intersects(new Rectangle((int)(position.X + ofs.X), (int)(position.Y + ofs.Y), Width, Height)) && !Last && Listener.IsLeftDown())
            {
                Last = true;
            }
            else if(Listener.mrect.Intersects(new Rectangle((int)(position.X + ofs.X), (int)(position.Y + ofs.Y), Width, Height)) && Last && !Listener.IsLeftDown())
            {
                Last = false;
                Trigger = true;
            }
            else if (Last && !Listener.IsLeftDown())
            {
                Last = false;
            }
        }

        public override void Draw(Vector2 ofs)
        {
           
            Shapes.DrawRectangle(Width, Height, position + ofs, Color.Gray, 0);
            if (Last)
            {
                Shapes.DrawRectangle(Width, Height, position + ofs, Color.White, 2);
            }
            else
            {
                Shapes.DrawRectangle(Width, Height, position + ofs, Color.Black, 2);
            }
            
            Game1.spriteBatch.DrawString(Resources.Fonts[font], Text, position + ofs+new Vector2(Width/2-Text.Measure(Resources.Fonts[font]).X/2,Height/2-Text.Measure(Resources.Fonts[font]).Y/2), Color.Black);
        }
    }
}
