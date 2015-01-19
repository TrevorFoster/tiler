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
    class Camera
    {
        public Vector2 position;
        public float ScrollSpeed = 2;

        public Camera(float X, float Y)
        {
            position.X = X;
            position.Y = Y;
            
        }

        public void Update()
        {
            ScrollSpeed = 3 ;
            if (TileBar.move)
            {
                if (Listener.KeyDown(Keys.D))
                {
                    position.X += ScrollSpeed;
                }
                if (Listener.KeyDown(Keys.A))
                {
                    position.X -= ScrollSpeed;
                }
                if (Listener.KeyDown(Keys.S))
                {
                    position.Y += ScrollSpeed;
                }
                if (Listener.KeyDown(Keys.W))
                {
                    position.Y -= ScrollSpeed;
                }
            }
        }
    }
}
