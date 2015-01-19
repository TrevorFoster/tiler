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
    class Bin
    {
        Vector2 pos;
        int width;
        int height;
        //int insidew = 20;
        //int insideh = 20;
        public static Rectangle Bounds = new Rectangle();

        public Bin(Vector2 pos, int width, int height)
        {
            this.pos = pos;
            this.width = width;
            this.height = height;
        }

        public void Update()
        {
            pos.X = Game1.WindowWidth - width;
            pos.Y = Game1.WindowHeight - height;
            Bounds.X = (int)pos.X;
            Bounds.Y = (int)pos.Y;
            Bounds.Width = width;
            Bounds.Height = height;
            if (TileMover.heldtile != null && Listener.mrect.Intersects(Bounds) && TileMover.lastheld && !Listener.IsLeftDown())
            {
                TileMover.heldtile = null;
                TileMover.lastheld = false;
            }
            else if (TileMover.heldexit != null && Listener.mrect.Intersects(Bounds) && TileMover.lastheld && !Listener.IsLeftDown())
            {
                TileMover.heldexit = null;
                TileMover.lastheld = false;
            }
            else if (TileMover.held.Count > 0 && Listener.mrect.Intersects(Bounds) && TileMover.lastheld && !Listener.IsLeftDown())
            {
                TileMover.lastheld = false;
                TileMover.heldtile = null;
                TileMover.moving = false;
                TileMover.held.Clear();
                TileMover.selection.Clear();
            }
        }

        public void Draw()
        {
            Shapes.DrawRectangle(width, height, pos, new Color(100, 100, 100, 100), 0);
            Game1.spriteBatch.Draw(Resources.Textures["Trash"], new Vector2(pos.X + width / 2 - Resources.Textures["Trash"].Width / 2, pos.Y + height / 2 - Resources.Textures["Trash"].Height / 2), Color.White);
            //Shapes.DrawRectangle(insidew, insideh, new Vector2(pos.X + width / 2 - insidew/2, pos.Y + height / 2 - insideh/2), new Color(0, 0, 0, 200), 0);
            //Game1.spriteBatch.DrawLineSegment(new Vector2(pos.X + width / 2 - insidew / 2, pos.Y + height / 2 - insideh / 2), new Vector2(pos.X + width / 2 + insidew / 2, pos.Y + height / 2 + insideh / 2), Color.Red, 1);
            //Game1.spriteBatch.DrawLineSegment(new Vector2(pos.X + width / 2 + insidew / 2, pos.Y + height / 2 - insideh / 2), new Vector2(pos.X + width / 2 - insidew / 2, pos.Y + height / 2 + insideh / 2), Color.Red, 1);
            //Shapes.DrawRectangle(insidew, insideh, new Vector2(pos.X + width / 2 - insidew / 2, pos.Y + height / 2 - insideh / 2), new Color(0, 0, 0, 200), 0);
        }
    }
}
