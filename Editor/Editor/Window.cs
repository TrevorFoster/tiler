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
    class Window
    {
        public Vector2 position;
        public int Width;
        public int Height;
        public int snaplength = 10;
        public bool Open = true;
        public Rectangle moverect;
        public bool windowmove = false;
        public Dictionary<string, Control> Controls;
        public int index = 0;
        public Rectangle close;
        public int ButtonWidth = 25;
        public int ButtonHeight = 15;
        public string ToolBarIcon;
        bool over = false;
        bool Closeable;
        bool last = false;

        public Window(Vector2 position, int Width, int Height,bool Open, bool Closeable, string ToolBarIcon)
        {
            Init();
            this.position = position;
            this.Width = Width;
            this.Height = Height;
            this.Open = Open;
            this.Closeable = Closeable;
            this.ToolBarIcon = ToolBarIcon;
            moverect.Width = Width;
            moverect.Height = 15;
            moverect.X = (int)position.X;
            moverect.Y = (int)position.Y;
            close.X = (int)position.X+Width-ButtonWidth;
            close.Y = (int)position.Y;
            close.Width = ButtonWidth;
            close.Height = ButtonHeight;
            
        }

        public void Init()
        {
            Controls = new Dictionary<string, Control>();
            moverect = new Rectangle();
            close = new Rectangle();
        }
        public int Update()
        {
            if (Listener.mrect.Intersects(close))
            {
                over = true;
                if (Listener.NewPressLeft() && Closeable)
                {
                    OnClose();
                    return index;
                }
            }
            else
            {
                over = false;
            }

            #region Edge Snapping
            if (Listener.IsLeftDown() && !last && (Listener.mrect.Intersects(moverect) || windowmove) && TileMover.heldtile == null )
            {
                if (!WindowManager.Any(index, new Vector2(Listener.mrect.X, Listener.mrect.Y)))
                {
                    position += Listener.lastpos[0] - Listener.lastpos[1];
                    windowmove = true;
                }
                
            }

            else if (!Listener.IsLeftDown())
            {
                windowmove = false;
            }

            if(Listener.NewPressLeft() && Listener.mrect.Intersects(new Rectangle((int)position.X,(int)position.Y,Width,Height)))
            {
                if (!WindowManager.Any(index, new Vector2(Listener.mrect.X, Listener.mrect.Y)))
                {
                    WindowManager.Focused = index;
                }
            }

            foreach (Control control in Controls.Values)
            {
                control.Update(position);
            }

            Vector2 movement = Listener.lastpos[0] - Listener.lastpos[1];
            if (movement.X < 0 && Listener.IsLeftDown())
            {
                if (position.X < snaplength)
                {
                    position.X = 0;
                }
            }
            else if ((movement.X > 0 && Listener.IsLeftDown()) || position.X > Game1.WindowWidth)
            {
                if (position.X > Game1.WindowWidth - snaplength - moverect.Width)
                {
                    position.X = Game1.WindowWidth - moverect.Width;
                }
            }
            if (movement.Y < 0 && Listener.IsLeftDown())
            {
                if (position.Y  < snaplength)
                {
                    position.Y = 0;
                }
            }
            else if ((movement.Y > 0 && Listener.IsLeftDown()) || position.Y > Game1.WindowHeight)
            {
                if (position.Y + Height > Game1.WindowHeight - snaplength)
                {
                    position.Y = Game1.WindowHeight - Height;
                }
            }
            #endregion

            
            int ind = Updates();
            if (ind != -1)
            {
                return ind;
            }

            moverect.X = (int)position.X;
            moverect.Y = (int)position.Y;
            close.X = (int)position.X + moverect.Width - ButtonWidth;
            close.Y = (int)position.Y;
            return -1;
        }

        public virtual void Draw()
        {
            Shapes.DrawRectangle(moverect, new Color(120, 120, 120, 110), 0);
            Shapes.DrawRectangle(Width, Height - moverect.Height, position + new Vector2(0, moverect.Height), new Color(100, 100, 100, 100), 0);
            if (Closeable) { Shapes.DrawRectangle(close, new Color(255, 0, 0, 100), 0); }
            if (Closeable && over)
            {
                Shapes.DrawRectangle(close, new Color(255, 0, 0, 100), 2);
            }
            //Shapes.DrawRectangle(close.Width-14, close.Height-8, new Vector2(close.X+7, close.Y+4), new Color(100,100,100,100), 0);

            List<Control> sorted = Sort(Controls.Values);

            for (int i = 0; i < sorted.Count; i++)
            {
                sorted[i].Draw(position);
            }
            Extra();
        }

        private List<Control> Sort(Dictionary<string, Control>.ValueCollection list)
        {
            List<Control> temp = new List<Control>();

            foreach (Control c in list)
            {
                if (c.GetType().Name != "DropDown" || !c.IsDropOpen())
                {
                    temp.Add(c);
                }
            }
            foreach (Control c in list)
            {
                if (c.GetType().Name == "DropDown" && c.IsDropOpen())
                {
                    temp.Add(c);
                }
            }
            return temp;
        }

        public void AddControl(Control control)
        {
            control.position += new Vector2(0,moverect.Height);
            Controls.Add(GiveKey(control), control);
        }


        public string GiveKey(Control control)
        {
            string type = control.GetType().Name;
            int oftype = 1;
            foreach (Control c in Controls.Values)
            {
                if (c.GetType().Name == type) { oftype += 1; }
            }
            return type + oftype.ToString();
        }

        public virtual int Updates()
        {
            return -1;
        }

        public virtual void Extra()
        {

        }

        public virtual void OnClose()
        {

        }
        public virtual void Initialize()
        {
        }
    }
}