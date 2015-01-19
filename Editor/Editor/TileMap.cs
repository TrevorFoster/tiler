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
using System.IO;

namespace Editor
{
    class TileMap
    {

        public int type = 0;
        public string filename = "";
        public Dictionary<Point, Tile> tilemap;
        public Dictionary<Point, string[]> exits;
        public Dictionary<Point, Vector2> exitpos;
        public Dictionary<Point, Tile> collision;
        string font = "font2";


        public TileMap(Dictionary<Point, Tile> tilemap, Dictionary<Point, Tile> collision, Dictionary<Point, string[]> exits, Dictionary<Point, Vector2> exitpos,int type,string filename)
        {
            this.tilemap = tilemap;
            this.exits = exits;
            this.exitpos = exitpos;
            this.collision = collision;
            this.type = type;
            this.filename = filename;
        }

        public void Draw()
        {

            for (int x = (int)ActiveCam.camera.position.X / Game1.gridcellheight-1; x < ActiveCam.camera.position.X / Game1.gridcellheight + (int)(Game1.WindowWidth / Game1.gridcellheight)+1; x++)
            {
                for (int y = (int)ActiveCam.camera.position.Y / Game1.gridcellwidth-1; y < ActiveCam.camera.position.Y / Game1.gridcellwidth + (int)(Game1.WindowHeight / Game1.gridcellwidth) + 1; y++)
                {
                    if (tilemap.ContainsKey(new Point(x, y)) && Game1.Visual)
                    {
                        tilemap[new Point(x, y)].Draw();
                    }
                    if (collision.ContainsKey(new Point(x, y)) && Game1.Collision)
                    {
                        collision[new Point(x, y)].Draw();
                    }
                    if (exits.ContainsKey(new Point(x, y)) && Game1.Exits)
                    {
                        if (exits[new Point(x, y)][1] == World.ActiveMap)
                        {
                            Shapes.DrawRectangle(Game1.gridcellwidth, Game1.gridcellheight, new Vector2(x * Game1.gridcellwidth - ActiveCam.camera.position.X, y * Game1.gridcellheight - ActiveCam.camera.position.Y), new Color(100, 100, 100, 80), 0);
                            string destination = "Exit to " + exits[new Point(x, y)][0];
                            Game1.spriteBatch.DrawString(Resources.Fonts[font], destination, new Vector2(x * Game1.gridcellwidth - ActiveCam.camera.position.X - destination.Measure(Resources.Fonts[font]).X / 2 + Game1.gridcellwidth / 2, y * Game1.gridcellheight - ActiveCam.camera.position.Y - destination.Measure(Resources.Fonts[font]).Y / 2 + Game1.gridcellheight / 2), Color.Black);
                        }
                    }
                }
            }
        }

    }
}
