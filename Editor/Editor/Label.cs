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
    class Label:Control
    {
        string font = "font";
        public Label(Vector2 position,string Text):base(position,Text)
        {
        }

        public override void Draw(Vector2 ofs)
        {
            Game1.spriteBatch.DrawString(Resources.Fonts[font], Text, position + ofs, Color.Black);
        }
    }
}
