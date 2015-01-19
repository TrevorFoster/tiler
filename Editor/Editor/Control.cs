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
    class Control
    {
        public Vector2 position;
        public bool Trigger = false;
        public string Text = "";
        public Color tabOn = new Color(100, 100, 100, 100);
        public Color tabOff = new Color(160, 160, 160, 180);
        public List<string> Items = new List<string>();
        public int SelectedIndex = -1;
        public bool reset = false;

        public Control(Vector2 position,string Text)
        {
            this.position = position;
            this.Text = Text;
        }

        public virtual void Update(Vector2 ofs)
        {

        }
        public virtual void Draw(Vector2 ofs)
        {

        }

        public virtual bool IsChecked()
        {
            return false;
        }
        public virtual bool IsDropOpen()
        {
            return false;
        }
    }
}
