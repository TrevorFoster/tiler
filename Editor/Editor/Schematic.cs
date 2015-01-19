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
    class Schematic
    {
        public int type = 0;
        public string filename = "";
        public Dictionary<Point, Tile> tilemap;
        public Dictionary<Point, Tile> collision;
        public int Width = 0;
        public int Height = 0;

        public Schematic(Dictionary<Point, Tile> tilemap, Dictionary<Point, Tile> collision, int type, string filename)
        {
            this.tilemap = tilemap;
            this.collision = collision;
            this.type = type;
            this.filename = filename;
            foreach (Tile t in tilemap.Values)
            {
                if (t.X > Width)
                {
                    Width = (int)t.X;
                }
                if (t.Y > Height)
                {
                    Height =(int) t.Y;
                }
            }
            foreach (Tile t in collision.Values)
            {
                if (t.X > Width)
                {
                    Width = (int)t.X;
                }
                if (t.Y > Height)
                {
                    Height = (int)t.Y;
                }
            }
            Width += 1;
            Height += 1;
        }

        public void Draw(Vector2 position,Vector2 size,bool expand)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Point point = new Point(x,y);
                    if (tilemap.ContainsKey(point))
                    {
                        tilemap[point].Draw(position, new Vector2(size.X, size.Y),expand);
                    }
                    if (collision.ContainsKey(point))
                    {
                        collision[point].Draw(position, new Vector2(size.X, size.Y), expand);
                    }
                }
            }
        }
    }
}
