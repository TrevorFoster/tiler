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
    class TileMover
    {
        public static Tile heldtile;
        public static Point ppos;
        public static bool lastheld = false;
        public static List<Point> selection = new List<Point>();
        public static Exit heldexit;
        public static List<Tile> held = new List<Tile>();
        public static List<Point> lastpos = new List<Point>() { new Point(0, 0), new Point(0, 0) };
        public static bool moving = false;
        string font = "font2";
        int startx = 0;
        int starty = 0;
        int endx = 0;
        int endy = 0;

        public TileMover()
        {

        }

        public void Update()
        {
            for (int y = (int)ActiveCam.camera.position.Y / Game1.gridcellheight - 1; y < (int)(ActiveCam.camera.position.Y + Game1.WindowHeight) / Game1.gridcellheight + 1; y++)
            {
                for (int x = (int)ActiveCam.camera.position.X / Game1.gridcellwidth - 1; x < (int)(ActiveCam.camera.position.X + Game1.WindowWidth) / Game1.gridcellwidth + 1; x++)
                {
                    if (new Rectangle((int)(Listener.ms.X + ActiveCam.camera.position.X), (int)(Listener.ms.Y + ActiveCam.camera.position.Y), 1, 1).Intersects(new Rectangle(x * Game1.gridcellwidth, y * Game1.gridcellheight, Game1.gridcellwidth, Game1.gridcellheight)))
                    {
                        ppos = new Point(x, y);
                        break;
                    }
                }
            }
            lastpos.RemoveAt(1);
            lastpos.Insert(0, new Point(ppos.X, ppos.Y));

            #region Selection Box Code
            if (Listener.KeyDown(Keys.LeftControl) && Listener.KeyDown(Keys.D) && selection.Count > 0)
            {
                if (held.Count > 0)
                {
                    foreach (Tile t in held)
                    {
                        t.X += startx;
                        t.Y += starty;
                    }
                    Paste(held);
                }
                selection = new List<Point>();
                moving = false;
                lastheld = false;
                held = new List<Tile>();
            }
            if (selection.Count > 1)
            {
                if (Listener.KeyDown(Keys.Up))
                {
                    selection[0] = new Point(selection[0].X, selection[0].Y - 1);
                    selection[1] = new Point(selection[1].X, selection[1].Y - 1);
                }
                if (Listener.KeyDown(Keys.Down))
                {
                    selection[0] = new Point(selection[0].X, selection[0].Y + 1);
                    selection[1] = new Point(selection[1].X, selection[1].Y + 1);
                }
                if (Listener.KeyDown(Keys.Left))
                {
                    selection[0] = new Point(selection[0].X-1, selection[0].Y);
                    selection[1] = new Point(selection[1].X-1, selection[1].Y );
                }
                if (Listener.KeyDown(Keys.Right))
                {
                    selection[0] = new Point(selection[0].X+1, selection[0].Y );
                    selection[1] = new Point(selection[1].X+1, selection[1].Y );
                }
                starty = Math.Min(selection[0].Y, selection[1].Y);
                endy = Math.Max(selection[0].Y, selection[1].Y) + 1;
                startx = Math.Min(selection[0].X, selection[1].X);
                endx = Math.Max(selection[0].X, selection[1].X) + 1;
            }
            if (!WindowManager.Moving && (Listener.KeyDown(Keys.LeftShift) || Listener.KeyDown(Keys.RightShift)) && Listener.IsLeftDown() && heldtile == null && heldexit == null && !moving)
            {
                if (!selection.Contains(new Point(TileMover.ppos.X, TileMover.ppos.Y)))
                {
                    if (selection.Count == 0)
                    {
                        selection.Add(new Point(ppos.X, ppos.Y));
                    }
                    else if (selection.Count == 1)
                    {
                        selection.Add(new Point(ppos.X, ppos.Y));
                    }
                    else if (selection.Count > 1)
                    {
                        selection[1]=(new Point(ppos.X, ppos.Y));
                    }
                }
            }
            else if (!Listener.IsLeftDown() && heldtile == null && heldexit == null)
            {
                if (selection.Count != 0 && Listener.KeyDown(Keys.Delete) && World.TileMaps.ContainsKey(World.ActiveMap))
                {
                    if (selection.Count == 2)
                    {
                        for (int y = starty; y < endy; y++)
                        {
                            for (int x = startx; x < endx; x++)
                            {
                                if (TileBar.tab == 0)
                                {
                                    if (World.TileMaps[World.ActiveMap].tilemap.ContainsKey(new Point(x, y)))
                                    {
                                        World.TileMaps[World.ActiveMap].tilemap.Remove(new Point(x, y));
                                    }
                                }
                                else if (TileBar.tab == 1)
                                {
                                    if (World.TileMaps[World.ActiveMap].collision.ContainsKey(new Point(x, y)))
                                    {
                                        World.TileMaps[World.ActiveMap].collision.Remove(new Point(x, y));
                                    }
                                }
                            }
                        }
                        selection = new List<Point>();
                        if (moving)
                        {
                            moving = false;
                            held = new List<Tile>();
                        }
                    }
                    else if (selection.Count == 1)
                    {
                        if (TileBar.tab == 0)
                        {
                            if (World.TileMaps[World.ActiveMap].tilemap.ContainsKey(new Point(selection[0].X, selection[0].Y)))
                            {
                                World.TileMaps[World.ActiveMap].tilemap.Remove(new Point(selection[0].X, selection[0].Y));

                            }
                        }
                        else if (TileBar.tab == 1)
                        {
                            if (World.TileMaps[World.ActiveMap].collision.ContainsKey(new Point(selection[0].X, selection[0].Y)))
                            {
                                World.TileMaps[World.ActiveMap].collision.Remove(new Point(selection[0].X, selection[0].Y));
                            }
                        }
                        selection.Clear();
                    }
                }
            }
            else if ( !WindowManager.Moving && World.TileMaps.ContainsKey(World.ActiveMap) && Listener.IsLeftDown() && (new Rectangle((int)(ppos.X)*Game1.gridcellwidth, (int)(ppos.Y)*Game1.gridcellheight, Game1.gridcellwidth, Game1.gridcellheight).Intersects(new Rectangle(startx * Game1.gridcellwidth, starty*Game1.gridcellwidth, (endx - startx) * Game1.gridcellwidth, (endy - starty) * Game1.gridcellheight)) || lastheld) && selection.Count > 0)
            {
                
                if (selection.Count > 1)
                {
                    lastheld = true;
                    Point s0 = selection[0];
                    Point s1 = selection[1];
                    selection[0] = new Point(s0.X + (lastpos[0].X - lastpos[1].X), s0.Y + (lastpos[0].Y - lastpos[1].Y));
                    selection[1] = new Point(s1.X + (lastpos[0].X - lastpos[1].X), s1.Y + (lastpos[0].Y - lastpos[1].Y));
                }
                else
                {
                    lastheld = true;
                    Point s0 = selection[0];
                    selection[0] = new Point(s0.X + (lastpos[0].X - lastpos[1].X), s0.Y + (lastpos[0].Y - lastpos[1].Y));
                    selection.Add(selection[0]);
                    starty = Math.Min(selection[0].Y, selection[1].Y);
                    endy = Math.Max(selection[0].Y, selection[1].Y) + 1;
                    startx = Math.Min(selection[0].X, selection[1].X);
                    endx = Math.Max(selection[0].X, selection[1].X) + 1;
                }
                if (!moving)
                {
                    for (int y = starty; y < endy; y++)
                    {
                        for (int x = startx; x < endx; x++)
                        {
                            if (World.TileMaps[World.ActiveMap].tilemap.ContainsKey(new Point(x, y)))
                            {
                                moving = true;
                                Tile adding = World.TileMaps[World.ActiveMap].tilemap[new Point(x, y)];
                                adding.X = x - startx;
                                adding.Y = y - starty;
                                held.Add(new Tile(adding));
                                World.TileMaps[World.ActiveMap].tilemap.Remove(new Point(x, y));
                            }

                            if (World.TileMaps[World.ActiveMap].collision.ContainsKey(new Point(x, y)))
                            {
                                moving = true;
                                Tile adding = World.TileMaps[World.ActiveMap].collision[new Point(x, y)];
                                adding.X = x - startx;
                                adding.Y = y - starty;
                                held.Add(adding);
                                World.TileMaps[World.ActiveMap].collision.Remove(new Point(x, y));
                            }

                        }
                    }
                }
            }
            if (!Listener.IsLeftDown() && lastheld)
            {
                lastheld = false;
            }

            #endregion
            if (held.Count > 0 && Listener.KeyDown(Keys.C))
            {
                List<Tile> a =new List<Tile>();
                foreach (Tile t in held)
                {
                    a.Add(new Tile(t));
                }
                foreach (Tile t in a)
                {
                    t.X += startx;
                    t.Y += starty;
                }
                Paste(a);
            }
            else if (lastheld && Listener.IsLeftDown() && (heldtile != null || heldexit!=null) && Listener.KeyDown(Keys.C))
            {
                if (heldexit == null)
                {
                    heldtile.X = ppos.X;
                    heldtile.Y = ppos.Y;
                    Paste(heldtile);
                }
                else
                {
                    PasteExit(heldexit);
                }
            }
            else if (!lastheld && !Listener.IsLeftDown() && (heldtile != null || heldexit != null) && !Listener.KeyDown(Keys.LeftShift) && !Listener.KeyDown(Keys.RightShift))
            {
                if (heldexit == null)
                {
                    heldtile.X = ppos.X;
                    heldtile.Y = ppos.Y;
                    Paste(heldtile);
                    heldtile = null;
                }
                else
                {
                    PasteExit(heldexit);
                    heldexit = null;
                }
                lastheld = false;
            }
            else if (!moving && !WindowManager.Moving && held.Count==0 && Listener.IsLeftDown() && !lastheld && TileMover.heldtile == null && heldexit == null && !Listener.KeyDown(Keys.LeftShift) && !Listener.KeyDown(Keys.RightShift))
            {
                if (World.TileMaps.ContainsKey(World.ActiveMap))
                {
                    if (TileBar.tab == 0 && World.TileMaps[World.ActiveMap].tilemap.ContainsKey(ppos))
                    {
                        heldtile = new Tile(World.TileMaps[World.ActiveMap].tilemap[ppos]);
                        World.TileMaps[World.ActiveMap].tilemap.Remove(ppos);
                        lastheld = true;
                    }
                    else if (TileBar.tab == 1 && World.TileMaps[World.ActiveMap].collision.ContainsKey(ppos))
                    {
                        heldtile = new Tile(World.TileMaps[World.ActiveMap].collision[ppos]);
                        World.TileMaps[World.ActiveMap].collision.Remove(ppos);
                        lastheld = true;
                    }
                    else if (World.TileMaps[World.ActiveMap].exits.ContainsKey(ppos))
                    {
                        heldexit = new Exit(World.TileMaps[World.ActiveMap].exits[ppos], World.TileMaps[World.ActiveMap].exitpos[ppos]);
                        World.TileMaps[World.ActiveMap].exits.Remove(ppos);
                        World.TileMaps[World.ActiveMap].exitpos.Remove(ppos);
                        lastheld = true;
                    }
                }
            }
            int scrolldirection = Listener.ScrollDirection();
            if (scrolldirection == 1)
            {
                Game1.gridcellwidth += 2;
                Game1.gridcellheight += 2;
            }
            else if (scrolldirection == 2 && Game1.gridcellwidth - 2 > 0 && Game1.gridcellheight - 2 > 0)
            {
                Game1.gridcellwidth -= 2;
                Game1.gridcellheight -= 2;
            }
        }

        private void Paste(List<Tile> tiles)
        {
            foreach (Tile t in tiles)
            {
                Point p = new Point((int)t.X, (int)t.Y);
                if (t.type != 2)
                {
                    if (World.TileMaps[World.ActiveMap].tilemap.ContainsKey(p))
                    {
                        World.TileMaps[World.ActiveMap].tilemap.Remove(p);
                    }
                    World.TileMaps[World.ActiveMap].tilemap.Add(p, new Tile(t));
                }
                else
                {
                    if (World.TileMaps[World.ActiveMap].collision.ContainsKey(p))
                    {
                        World.TileMaps[World.ActiveMap].collision.Remove(p);
                    }
                    World.TileMaps[World.ActiveMap].collision.Add(p, new Tile(t));
                }

            }
        }
        private void PasteExit(Exit e)
        {
            
            if (World.TileMaps[World.ActiveMap].exits.ContainsKey(ppos))
            {
                World.TileMaps[World.ActiveMap].exits.Remove(ppos);
                World.TileMaps[World.ActiveMap].exits.Add(ppos, e.exit);
                World.TileMaps[World.ActiveMap].exitpos.Remove(ppos);
                World.TileMaps[World.ActiveMap].exitpos.Add(ppos, e.exitpos);
            }
            else
            {
                World.TileMaps[World.ActiveMap].exits.Add(ppos, e.exit);
                World.TileMaps[World.ActiveMap].exitpos.Add(ppos, e.exitpos);
            }
        }
        private void Paste(Tile t)
        {
            Point p = new Point((int)t.X, (int)t.Y);

            if (t.type != 2)
            {
                if (World.TileMaps[World.ActiveMap].tilemap.ContainsKey(p))
                {
                    World.TileMaps[World.ActiveMap].tilemap.Remove(p);
                }
                World.TileMaps[World.ActiveMap].tilemap.Add(p, new Tile(t));
            }
            else
            {
                if (World.TileMaps[World.ActiveMap].collision.ContainsKey(p))
                {
                    World.TileMaps[World.ActiveMap].collision.Remove(p);
                }
                World.TileMaps[World.ActiveMap].collision.Add(p, new Tile(t));
            }
        }

        public void Draw()
        {
            if (heldtile != null)
            {
                if (heldtile.type == 0)
                {
                    Game1.spriteBatch.Draw(Resources.TTiles[heldtile.id], new Vector2(ppos.X * Game1.gridcellwidth - ActiveCam.camera.position.X, ppos.Y * Game1.gridcellheight - ActiveCam.camera.position.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)Game1.gridcellwidth / (float)heldtile.rect.Width, (float)Game1.gridcellheight / (float)heldtile.rect.Height), SpriteEffects.None, 1);
                }
                else if (heldtile.type == 1)
                {
                    Game1.spriteBatch.Draw(Resources.STiles[heldtile.id], new Vector2(ppos.X * Game1.gridcellwidth - ActiveCam.camera.position.X, ppos.Y * Game1.gridcellheight - ActiveCam.camera.position.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)Game1.gridcellwidth / (float)heldtile.rect.Width, (float)Game1.gridcellheight / (float)heldtile.rect.Height), SpriteEffects.None, 1);
                }
                else if (heldtile.type == 2)
                {
                    Game1.spriteBatch.Draw(Resources.CTiles[heldtile.id], new Vector2(ppos.X * Game1.gridcellwidth - ActiveCam.camera.position.X, ppos.Y * Game1.gridcellheight - ActiveCam.camera.position.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)Game1.gridcellwidth / (float)heldtile.rect.Width, (float)Game1.gridcellheight / (float)heldtile.rect.Height), SpriteEffects.None, 1);
                }
            }
            else if (heldexit != null)
            {
                Shapes.DrawRectangle(Game1.gridcellwidth, Game1.gridcellheight, new Vector2(ppos.X * Game1.gridcellwidth - ActiveCam.camera.position.X, ppos.Y * Game1.gridcellheight - ActiveCam.camera.position.Y), new Color(100, 100, 100, 80), 0);
                string destination = "Exit to " + heldexit.exit[0];
                Game1.spriteBatch.DrawString(Resources.Fonts[font], destination, new Vector2(ppos.X * Game1.gridcellwidth - ActiveCam.camera.position.X - destination.Measure(Resources.Fonts[font]).X / 2 + Game1.gridcellwidth / 2, ppos.Y * Game1.gridcellheight - ActiveCam.camera.position.Y - destination.Measure(Resources.Fonts[font]).Y / 2 + Game1.gridcellheight / 2), Color.Black);
            }
            if (Listener.KeyDown(Keys.P))
            {
                Game1.spriteBatch.DrawString(Resources.Fonts[font], "(" + ppos.X.ToString() + "," + ppos.Y.ToString() + ")", new Vector2(ppos.X * Game1.gridcellwidth, ppos.Y * Game1.gridcellheight) - ActiveCam.camera.position, Color.White);
            }
            if (held.Count > 0)
            {
                if (Game1.spriteBatch != null)
                {
                    foreach (Tile t in held)
                    {
                        if (t.type == 0)
                        {
                            Game1.spriteBatch.Draw(Resources.TTiles[t.id], new Vector2((t.X + startx) * Game1.gridcellwidth - ActiveCam.camera.position.X, (t.Y + starty) * Game1.gridcellheight - ActiveCam.camera.position.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)Game1.gridcellwidth / (float)t.rect.Width, (float)Game1.gridcellheight / (float)t.rect.Height), SpriteEffects.None, 1);
                        }
                        if (t.type == 1)
                        {
                            Game1.spriteBatch.Draw(Resources.STiles[t.id], new Vector2((t.X + startx) * Game1.gridcellwidth - ActiveCam.camera.position.X, (t.Y + starty) * Game1.gridcellheight - ActiveCam.camera.position.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)Game1.gridcellwidth / (float)t.rect.Width, (float)Game1.gridcellheight / (float)t.rect.Height), SpriteEffects.None, 1);
                        }
                        if (t.type == 2)
                        {
                            Game1.spriteBatch.Draw(Resources.CTiles[t.id], new Vector2((t.X + startx) * Game1.gridcellwidth - ActiveCam.camera.position.X, (t.Y + starty) * Game1.gridcellheight - ActiveCam.camera.position.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)Game1.gridcellwidth / (float)t.rect.Width, (float)Game1.gridcellheight / (float)t.rect.Height), SpriteEffects.None, 1);
                        }

                    }
                }
            }
            if (selection.Count > 0)
            {
                if (selection.Count == 2)
                {
                    Shapes.DrawRectangle((endx - startx) * Game1.gridcellwidth, (endy - starty) * Game1.gridcellheight, new Vector2(startx * Game1.gridcellwidth - ActiveCam.camera.position.X, starty * Game1.gridcellheight - ActiveCam.camera.position.Y), new Color(255, 0, 0, 80), 0);
                    Shapes.DrawRectangle((endx - startx) * Game1.gridcellwidth, (endy - starty) * Game1.gridcellheight, new Vector2(startx * Game1.gridcellwidth - ActiveCam.camera.position.X, starty * Game1.gridcellheight - ActiveCam.camera.position.Y), new Color(255, 0, 0, 200), 4);
                }
                else if (selection.Count == 1)
                {
                    Shapes.DrawRectangle(Game1.gridcellwidth, Game1.gridcellheight, new Vector2(selection[0].X * Game1.gridcellwidth - ActiveCam.camera.position.X, selection[0].Y * Game1.gridcellheight - ActiveCam.camera.position.Y), new Color(255, 0, 0, 100), 0);
                }
            }
            
            Game1.spriteBatch.DrawString(Resources.Fonts["font"], Listener.ms.X.ToString() + "," + Listener.ms.Y.ToString(), new Vector2(Listener.ms.X+10, Listener.ms.Y+5), Color.White);
        }
    }
}