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
    class Toolbar:Window
    {
        List<Window> WindowTypes = new List<Window>();
        int intersectDown = -1;
        int intersect = -1;
        int pad = 10;
        public Toolbar(Vector2 position, int Width, int Height)
            : base(position, Width, Height, true, false,null)
        {
            WindowTypes.Add(new SideBar(new Vector2(0,0),0,0,true));
            WindowTypes.Add(new AddExits(new Vector2(Game1.WindowWidth / 2 - 175, Game1.WindowHeight / 2 - 125),350,250,true));
            WindowTypes.Add(new DialogBoxWindow(new Vector2(20, 20), Game1.WindowWidth - 40, Game1.WindowHeight - 40,"test"));
        }

        public override int Updates()
        {
            float xbuffer = pad;
            int i = 0;
            int great = 0;
            foreach (Window w in WindowTypes)
            {
                if (Resources.ToolbarIcons[w.ToolBarIcon].Height > great)
                {
                    great = Resources.ToolbarIcons[w.ToolBarIcon].Height;
                }
                if (Listener.mrect.Intersects(new Rectangle((int)(position.X + xbuffer), (int)(position.Y + 15), Resources.ToolbarIcons[w.ToolBarIcon].Width, Resources.ToolbarIcons[w.ToolBarIcon].Height)))
                {
                    intersect = i;
                    if (Listener.NewPressLeft())
                    {
                        intersectDown = i;
                    }
                }
                if(intersect == i && !Listener.mrect.Intersects(new Rectangle((int)(position.X + xbuffer), (int)(position.Y + 15), Resources.ToolbarIcons[w.ToolBarIcon].Width, Resources.ToolbarIcons[w.ToolBarIcon].Height)))
                {
                    intersect = -1;
                }

                if (i == intersectDown && !Listener.IsLeftDown())
                {
                    if (Listener.mrect.Intersects(new Rectangle((int)(position.X + xbuffer), (int)(position.Y + 15), Resources.ToolbarIcons[w.ToolBarIcon].Width, Resources.ToolbarIcons[w.ToolBarIcon].Height)))
                    {
                        if (!WindowManager.Exists(w.GetType().Name))
                        {
                            WindowManager.NewWindow(w);
                        }
                    }

                    intersectDown = -1;
                }
                xbuffer += Resources.ToolbarIcons[w.ToolBarIcon].Width + pad;
                i++;
            }
            Width = Game1.WindowWidth;
            moverect.Width = 0;
            Height = great+pad*2;
            position.Y = Game1.WindowHeight - Height;

            return -1;
        }

        public override void Draw()
        {
            Shapes.DrawRectangle(Width, Height, position, new Color(100, 100, 100, 100), 0);
            Shapes.DrawRectangle(Width, pad, position, new Color(100, 100, 100, 100), 0);
            float xbuffer = pad;
            int i = 0;
            foreach(Window w in WindowTypes)
            {
                if (i == intersectDown)
                {
                    Game1.spriteBatch.Draw(Resources.ToolbarIcons[w.ToolBarIcon], new Vector2(position.X + xbuffer + 4, position.Y + pad + 4), Color.White);
                }
                else
                {
                    Game1.spriteBatch.Draw(Resources.ToolbarIcons[w.ToolBarIcon], new Vector2(position.X + xbuffer, position.Y + pad), Color.White);
                }
                if(i == intersect)
                {
                    Shapes.DrawRectangle(Resources.ToolbarIcons[w.ToolBarIcon].Width + pad, Height - pad, new Vector2(position.X + xbuffer - pad / 2, position.Y + pad), Color.Black, 2);

                    Shapes.DrawRectangle((Resources.ToolbarIcons[w.ToolBarIcon].Width + pad), Height - pad, new Vector2(position.X + xbuffer - pad / 2, position.Y + pad), new Color(100, 100, 100, 100), 0);
                }
                else
                {
                    Shapes.DrawRectangle(Resources.ToolbarIcons[w.ToolBarIcon].Width + pad, Height - pad, new Vector2(position.X + xbuffer - pad / 2, position.Y + pad), Color.Black, 2);
                }
                
                xbuffer += Resources.ToolbarIcons[w.ToolBarIcon].Width + pad;
                i++;
            }
            
        }
    }
}
