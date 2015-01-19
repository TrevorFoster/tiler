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
    class Component
    {
        public string Name = "";
        public int width;
        public int height;
        public Color tabOn = new Color(100, 100, 100, 100);
        public Color tabOff = new Color(160, 160, 160, 180);
        public Dictionary<string, Control> Controls = new Dictionary<string, Control>();
        public Rectangle rect = new Rectangle();

        public Component(string name,int width,int height, Vector2 position)
        {
            this.Name = name;
            this.width = width;
            this.height = height;
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
            rect.Width = width + SideBar.tabwidth;
            rect.Height = height;
        }

        public void AddControl(Control control)
        {
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
        public List<Control> Sort(Dictionary<string, Control>.ValueCollection list)
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
        public virtual void Update(Vector2 position)
        {

        }

        public void ResetIndeces()
        {
            foreach (Control c in Controls.Values)
            {
                if (c.GetType().Name == "DropDown" && c.reset)
                {
                    c.SelectedIndex = -1;
                }
            }
        }

        public void UpdateRectangle(Vector2 position)
        {
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
            rect.Width = width+SideBar.tabwidth;
            rect.Height = height;
        }

        public virtual void Draw(Vector2 ofs)
        {

        }
    }
}
