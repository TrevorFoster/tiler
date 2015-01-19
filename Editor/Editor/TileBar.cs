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
    class TileBar:Component
    {
        public bool active = false;
        int scrollbarwidth = 10;
        float scrollbary = 0;
        float scrollbarx = 0;
        float scrollspeed = 2;
        int buffer = 10;
        int topbuffer = 20;
        int tilesperline =4;
        public static Rectangle Bounds = new Rectangle();
        Vector2 Tpos;
        List<Point> selection = new List<Point>();
        List<Tile> moving = new List<Tile>();
        public static int tab = 0;
        int ntabs = 2;
        int tabheight = 20;
        int tabwidth = 0;
        string tab1 = "Visual(1)";
        string tab2 = "Masks(2)";
        public float rotate = 0;
        public int rotind = -1;
        string font = "font";

        float bufferleft = 0;
        public static bool move = true;

        public TileBar(int width, int height, Vector2 position)
            : base("TileBar", width, height,position)
        {
            tabwidth = width / ntabs;
        }

        public override void Update(Vector2 position)
        {
            height = Game1.WindowHeight-50;
            scrollbarx = position.X + width - scrollbarwidth;
            Bounds.X = (int)position.X;
            Bounds.Y = (int)position.Y;
            Bounds.Width = width;
            Bounds.Height = height;

            bufferleft = tabwidth-(32*tilesperline+tilesperline*buffer)/2-buffer/2;
            if (Listener.KeyDown(Keys.LeftControl) || Listener.KeyDown(Keys.RightControl))
            {
                move = false;
            }
            else { move = true; }

            if (Listener.KeyDown(Keys.D1)) { tab = 0; }
            if (Listener.KeyDown(Keys.D2)) { tab = 1; }

            if (Listener.KeyDown(Keys.Down)) { scrollbary -= scrollspeed; }
            if (Listener.KeyDown(Keys.Up)) { scrollbary += scrollspeed; }

            if (Listener.IsLeftDown() && new Rectangle(Listener.ms.X, Listener.ms.Y, 1, 1).Intersects(new Rectangle((int)position.X, (int)position.Y, tabwidth, tabheight)))
            {
                tab = 0;
            }
            if (Listener.IsLeftDown() && new Rectangle(Listener.ms.X, Listener.ms.Y, 1, 1).Intersects(new Rectangle((int)position.X+tabwidth, (int)position.Y, tabwidth, tabheight)))
            {
                tab = 1;
            }

            if (TileMover.heldtile==null)
            {
                Tpos.X = TileMover.ppos.X ;
                Tpos.Y = TileMover.ppos.Y ;
            }
            rotind = -1;
            if ( TileMover.heldtile==null && !TileMover.moving)
            {
                if (World.TileMaps.ContainsKey(World.ActiveMap))
                {
                    Rectangle mrect = Listener.mrect;
                    
                    if (tab == 0)
                    {
                        if (World.TileMaps[World.ActiveMap].type == 0)
                        {
                            List<string> keys = new List<string>();
                            foreach (string key in Resources.TTiles.Keys)
                            {
                                keys.Add(key);
                            }
                            int drawn = 0;
                            float ypos = position.Y + buffer + topbuffer;
                            while (drawn < Resources.TTiles.Count)
                            {
                                float xpos = position.X + buffer+bufferleft;
                                for (int x = 0; x < tilesperline; x++)
                                {
                                    if (drawn < keys.Count)
                                    {
                                        if (mrect.Intersects(new Rectangle((int)xpos, (int)(ypos - scrollbary), 32, 32)))
                                        {
                                            if (Listener.IsLeftDown())
                                            {
                                                TileMover.heldtile = new Tile(keys[drawn], 0, Listener.ms.X, Listener.ms.Y);
                                                TileMover.lastheld = true;
                                            }
                                            else
                                            {
                                                rotind = drawn;
                                                break;
                                            }
                                        }
                                        xpos += buffer + 32;
                                        drawn++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                ypos += buffer + 32;
                            }
                        }
                        else
                        {
                            List<string> keys = new List<string>();
                            foreach (string key in Resources.STiles.Keys)
                            {
                                keys.Add(key);
                            }
                            int drawn = 0;
                            float ypos = position.Y + buffer + topbuffer;
                            while (drawn < Resources.STiles.Count)
                            {
                                float xpos = position.X + buffer+bufferleft;
                                for (int x = 0; x < tilesperline; x++)
                                {
                                    if (drawn < keys.Count)
                                    {
                                        if (mrect.Intersects(new Rectangle((int)xpos, (int)(ypos - scrollbary), 32, 32)))
                                        {
                                            if (Listener.IsLeftDown())
                                            {
                                                TileMover.heldtile = new Tile(keys[drawn], 1, Listener.ms.X, Listener.ms.Y);
                                                TileMover.lastheld = true;
                                            }
                                            else
                                            {
                                                rotind = drawn;
                                                break;
                                            }
                                        }
                                        xpos += buffer + 32;
                                        drawn++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                ypos += buffer + 32;
                            }
                        }

                    }
                    else if (tab == 1)
                    {
                        List<string> keys = new List<string>();
                        foreach (string key in Resources.CTiles.Keys)
                        {
                            keys.Add(key);
                        }
                        int drawn = 0;
                        float ypos = position.Y + buffer + topbuffer;
                        while (drawn < Resources.CTiles.Count)
                        {
                            float xpos = position.X + buffer + bufferleft;
                            for (int x = 0; x < tilesperline; x++)
                            {
                                if (drawn < keys.Count)
                                {
                                    if (mrect.Intersects(new Rectangle((int)xpos, (int)(ypos - scrollbary), 32, 32)))
                                    {
                                        if (Listener.IsLeftDown())
                                        {
                                            TileMover.heldtile = new Tile(keys[drawn], 2, Listener.ms.X, Listener.ms.Y);
                                            TileMover.lastheld = true;
                                        }
                                        else
                                        {
                                            rotind = drawn;
                                        }
                                    }
                                    xpos += buffer + 32;
                                    drawn++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            ypos += buffer + 32;
                        }
                    }
                }
                if (rotind != -1)
                {
                    rotate = 0;
                }
            }
        }

        public override void Draw(Vector2 ofs)
        {
            //Shapes.DrawRectangle(width, height - topbuffer, new Vector2(0, topbuffer)+ofs, new Color(100, 100, 100, 100), 0);
            if (World.TileMaps.ContainsKey(World.ActiveMap))
            {
                if (tab == 0)
                {
                    if (World.TileMaps[World.ActiveMap].type == 0)
                    {
                        List<string> keys = new List<string>();
                        foreach (string key in Resources.TTiles.Keys)
                        {
                            keys.Add(key);
                        }
                        int drawn = 0;
                        float ypos = buffer + topbuffer;
                        while (drawn < Resources.TTiles.Count)
                        {
                            float xpos = bufferleft + buffer;
                            for (int x = 0; x < tilesperline; x++)
                            {
                                if (drawn < keys.Count)
                                {
                                    if (((ypos - scrollbary)) + 32 < height)
                                    {
                                        if (drawn == rotind)
                                        {
                                            Game1.spriteBatch.Draw(Resources.TTiles[keys[drawn]], new Vector2(xpos, (ypos - scrollbary)) + ofs + new Vector2(16, 16), null, Color.White, rotate, new Vector2(8, 8), new Vector2(48 / Resources.TTiles[keys[drawn]].Width, 48 / Resources.TTiles[keys[drawn]].Height), SpriteEffects.None, 1);
                                            //Shapes.DrawRectangle(32, 32, new Vector2(xpos, (ypos - scrollbary)) + ofs, Color.White, 1);
                                        }
                                        else
                                        {
                                            Game1.spriteBatch.Draw(Resources.TTiles[keys[drawn]], new Vector2(xpos, (ypos - scrollbary)) + ofs, null, Color.White, 0, new Vector2(0, 0), new Vector2(32 / Resources.TTiles[keys[drawn]].Width, 32 / Resources.TTiles[keys[drawn]].Height), SpriteEffects.None, 1);
                                            Shapes.DrawRectangle(32, 32, new Vector2(xpos, (ypos - scrollbary)) + ofs, Color.White, 1);
                                        }
                                    }
                                    xpos += buffer + 32;
                                    drawn++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            ypos += buffer + 32;
                        }
                    }
                    else
                    {
                        List<string> keys = new List<string>();
                        foreach (string key in Resources.STiles.Keys)
                        {
                            keys.Add(key);
                        }
                        int drawn = 0;
                        float ypos = buffer + topbuffer;
                        while (drawn < Resources.STiles.Count)
                        {
                            float xpos = bufferleft + buffer;
                            for (int x = 0; x < tilesperline; x++)
                            {
                                if (drawn < keys.Count)
                                {
                                    if (((ypos - scrollbary)) + 32 <  height)
                                    {
                                        if (drawn == rotind)
                                        {
                                            Game1.spriteBatch.Draw(Resources.STiles[keys[drawn]], new Vector2(xpos, (ypos - scrollbary)) + ofs + new Vector2(16, 16), null, Color.White, rotate, new Vector2(8, 8), new Vector2(48 / Resources.STiles[keys[drawn]].Width, 48 / Resources.STiles[keys[drawn]].Height), SpriteEffects.None, 1);
                                            //Shapes.DrawRectangle(32, 32, new Vector2(xpos, (ypos - scrollbary)) + ofs, Color.White, 1);
                                        }
                                        else
                                        {

                                            Game1.spriteBatch.Draw(Resources.STiles[keys[drawn]], new Vector2(xpos, (ypos - scrollbary)) + ofs, null, Color.White, 0, new Vector2(0, 0), new Vector2(32 / Resources.STiles[keys[drawn]].Width, 32 / Resources.STiles[keys[drawn]].Height), SpriteEffects.None, 1);
                                            Shapes.DrawRectangle(32, 32, new Vector2(xpos, (ypos - scrollbary)) + ofs, Color.White, 1);
                                        }
                                    }
                                    xpos += buffer + 32;
                                    drawn++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            ypos += buffer + 32;
                        }
                    }
                }
                else if (tab == 1)
                {
                    List<string> keys = new List<string>();
                    foreach (string key in Resources.CTiles.Keys)
                    {
                        keys.Add(key);
                    }
                    int drawn = 0;
                    float ypos =  buffer + topbuffer;
                    while (drawn < Resources.CTiles.Count)
                    {
                        float xpos =  bufferleft + buffer;
                        for (int x = 0; x < tilesperline; x++)
                        {
                            if (drawn < keys.Count)
                            {
                                if (((ypos - scrollbary)) + 32 <  height)
                                {
                                    if (drawn == rotind)
                                    {
                                        Game1.spriteBatch.Draw(Resources.CTiles[keys[drawn]], new Vector2(xpos, (ypos - scrollbary)) + ofs + new Vector2(16, 16), null, Color.White, rotate, new Vector2(8, 8), new Vector2(48 / Resources.CTiles[keys[drawn]].Width, 48 / Resources.CTiles[keys[drawn]].Height), SpriteEffects.None, 1);
                                        //Shapes.DrawRectangle(32, 32, new Vector2(xpos, (ypos - scrollbary)) + ofs, Color.White, 1);
                                    }
                                    else
                                    {

                                        Game1.spriteBatch.Draw(Resources.CTiles[keys[drawn]], new Vector2(xpos, (ypos - scrollbary)) + ofs, null, Color.White, 0, new Vector2(0, 0), new Vector2(32 / Resources.CTiles[keys[drawn]].Width, 32 / Resources.CTiles[keys[drawn]].Height), SpriteEffects.None, 1);
                                        Shapes.DrawRectangle(32, 32, new Vector2(xpos, (ypos - scrollbary)) + ofs, Color.White, 1);
                                    }
                                }
                                xpos += buffer + 32;
                                drawn++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        ypos += buffer + 32;
                    }

                }
            }
            if (tab == 0)
            {
                Shapes.DrawRectangle(tabwidth, tabheight, new Vector2(0, 0)+ofs, tabOn, 0);
                Game1.spriteBatch.DrawString(Resources.Fonts[font], tab1, new Vector2(tabwidth / 2 - tab1.Measure(Resources.Fonts[font]).X / 2, 0) + ofs, Color.Black);
                Shapes.DrawRectangle(tabwidth, tabheight, new Vector2(tabwidth, 0)+ofs, tabOff, 0);
                Game1.spriteBatch.DrawString(Resources.Fonts[font], tab2, new Vector2(tabwidth + tabwidth / 2 - tab2.Measure(Resources.Fonts[font]).X / 2, 0)+ofs, Color.Black);
            }
            if (tab == 1)
            {
                Shapes.DrawRectangle(tabwidth, tabheight, new Vector2(0, 0)+ofs, tabOff, 0);
                Game1.spriteBatch.DrawString(Resources.Fonts[font], tab1, new Vector2( tabwidth / 2 - tab1.Measure(Resources.Fonts[font]).X / 2, 0)+ofs, Color.Black);
                Shapes.DrawRectangle(tabwidth, tabheight, new Vector2(tabwidth, 0)+ofs, tabOn, 0);
                Game1.spriteBatch.DrawString(Resources.Fonts[font], tab2, new Vector2(tabwidth + tabwidth / 2 - tab2.Measure(Resources.Fonts[font]).X / 2, 0)+ofs, Color.Black);
            }
            
            
            if (Game1.Grid)
            {
                Shapes.DrawRectangle(Game1.gridcellwidth, Game1.gridcellheight, new Vector2(TileMover.ppos.X * Game1.gridcellwidth - ActiveCam.camera.position.X, TileMover.ppos.Y * Game1.gridcellheight - ActiveCam.camera.position.Y), Color.Black, 2);
            }
        }
    }
}