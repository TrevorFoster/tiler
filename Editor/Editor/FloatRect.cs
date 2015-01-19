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
    class FloatRect
    {
        public float Width;
        public float Height;
        public Vector2 position;

        public FloatRect(float Width,float Height,Vector2 position)
        {
            this.Width = Width;
            this.Height = Height;
            this.position = position;
        }

        public bool Intersects(FloatRect rect)
        {
            float c = position.X-rect.position.X;
            if (c >= 0 && c < rect.Width)
            {
                float g = position.Y - rect.position.Y;
                if (g >= 0 && g < rect.Height)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
