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
    class Tile
    {
        public string id;
        public int type;
        public float X;
        public float Y;
        public Rectangle rect;

        public Tile(string id, int type, float X, float Y)
        {
            this.id = id;
            this.X = X;
            this.Y = Y;
            this.type = type;
            if (type == 0)
            {
                rect.X = (int)this.X * Resources.TTiles[id].Width;
                rect.Y = (int)this.Y * Resources.TTiles[id].Height;
                rect.Width = Resources.TTiles[id].Width;
                rect.Height = Resources.TTiles[id].Height;
            }
            else if (type == 1)
            {
                rect.X = (int)this.X * Resources.STiles[id].Width;
                rect.Y = (int)this.Y * Resources.STiles[id].Height;
                rect.Width = Resources.STiles[id].Width;
                rect.Height = Resources.STiles[id].Height;
            }
            else
            {
                rect.X = (int)this.X * Resources.CTiles[id].Width;
                rect.Y = (int)this.Y * Resources.CTiles[id].Height;
                rect.Width = Resources.CTiles[id].Width;
                rect.Height = Resources.CTiles[id].Height;
            }
            
        }
        public Tile(Tile tile)
        {
            this.id = tile.id;
            this.X = tile.X;
            this.Y = tile.Y;
            this.type = tile.type;
            if (tile.type == 0)
            {
                rect.X = (int)this.X * Resources.TTiles[tile.id].Width;
                rect.Y = (int)this.Y * Resources.TTiles[tile.id].Height;
                rect.Width = Resources.TTiles[tile.id].Width;
                rect.Height = Resources.TTiles[tile.id].Height;
            }
            else if (tile.type == 1)
            {
                rect.X = (int)this.X * Resources.STiles[tile.id].Width;
                rect.Y = (int)this.Y * Resources.STiles[tile.id].Height;
                rect.Width = Resources.STiles[tile.id].Width;
                rect.Height = Resources.STiles[tile.id].Height;
            }
            else
            {
                rect.X = (int)this.X * Resources.CTiles[tile.id].Width;
                rect.Y = (int)this.Y * Resources.CTiles[tile.id].Height;
                rect.Width = Resources.CTiles[tile.id].Width;
                rect.Height = Resources.CTiles[tile.id].Height;
            }

        }

        public void Draw()
        {
            if (type == 0)
            {
                Game1.spriteBatch.Draw(Resources.TTiles[id], new Vector2(X * Game1.gridcellwidth - ActiveCam.camera.position.X, Y * Game1.gridcellheight - ActiveCam.camera.position.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)Game1.gridcellwidth / (float)rect.Width, (float)Game1.gridcellheight / (float)rect.Height), SpriteEffects.None, 1);
            }
            else if (type == 1)
            {
                Game1.spriteBatch.Draw(Resources.STiles[id], new Vector2(X * Game1.gridcellwidth - ActiveCam.camera.position.X, Y * Game1.gridcellheight - ActiveCam.camera.position.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)Game1.gridcellwidth / (float)rect.Width, (float)Game1.gridcellheight / (float)rect.Height), SpriteEffects.None, 1);
            }
            else
            {
                Game1.spriteBatch.Draw(Resources.CTiles[id], new Vector2(X * Game1.gridcellwidth - ActiveCam.camera.position.X, Y * Game1.gridcellheight - ActiveCam.camera.position.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)Game1.gridcellwidth / (float)rect.Width, (float)Game1.gridcellheight / (float)rect.Height), SpriteEffects.None, 1);
            }
        }

        public void Draw(Vector2 position,Vector2 size,bool expand)
        {
            int o = 0;
            int ts = 0;
            if (expand)
            {
                o = 2;
                ts = o / 2;
            }
            if (type == 0)
            {
                Game1.spriteBatch.Draw(Resources.TTiles[id], new Vector2(X * (size.X + o) + position.X, Y * (size.Y + o) + position.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)(size.X + ts) / (float)rect.Width, (float)(size.Y + ts) / (float)rect.Height), SpriteEffects.None, 1);
            }
            else if (type == 1)
            {
                Game1.spriteBatch.Draw(Resources.STiles[id], new Vector2(X * (size.X + o) + position.X, Y * (size.Y + o) + position.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)(size.X + ts) / (float)rect.Width, (float)(size.Y + ts) / (float)rect.Height), SpriteEffects.None, 1);
            }
            else
            {
                Game1.spriteBatch.Draw(Resources.CTiles[id], new Vector2(X * (size.X + o) + position.X, Y * (size.Y + o) + position.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)(size.X + ts) / (float)rect.Width, (float)(size.Y + ts) / (float)rect.Height), SpriteEffects.None, 1);
            }
        }
    }
}
