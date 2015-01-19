using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Editor
{
    class OptionBox
    {
        string[] options;
        int[] keys;
        public bool on;
        int texth;
        string font;
        Rectangle bounds;
        int choice = 0;

        public OptionBox(string[] Options, string Font, int[] keys)
        {
            on = false;
            options = Options;
            this.keys = keys;
            font = Font;
            texth = (int)options[0].Measure(Resources.Fonts[font]).Y;
            List<int> strlen = new List<int>();
            foreach (string option in options)
            {
                strlen.Add((int)option.Measure(Resources.Fonts[font]).X);
            }
            strlen.Sort();
            bounds = new Rectangle(200, 200, strlen[strlen.Count-1]+20, texth*options.Length+10);
        }

        public int Update()
        {
            if (on)
            {
                if (Listener.NewPush(Keys.W))
                {
                    choice--;
                    if (choice < 0)
                    {
                        choice = options.Length - 1;
                    }
                }
                else if (Listener.NewPush(Keys.S))
                {
                    choice++;
                    if (choice >= options.Length)
                    {
                        choice = 0;
                    }
                }
                if (Listener.NewPush(Keys.E))
                {
                    on = false;
                    return keys[choice];
                }
            }
            return -1;
        }

        public void Draw()
        {
            if (on)
            {
                Shapes.DrawRectangle(bounds, Color.Blue, 0);
                for (int line = 0; line < options.Length; line++)
                {
                    if (line == choice)
                    {
                        Game1.spriteBatch.DrawString(Resources.Fonts[font], options[line], new Vector2(bounds.X + 10, (texth + 3) * line + bounds.Y + 5), Color.Red);
                    }
                    else
                    {
                        Game1.spriteBatch.DrawString(Resources.Fonts[font], options[line], new Vector2(bounds.X + 10, (texth + 3) * line + bounds.Y + 5), Color.White);
                    }
                }
            }
        }
        public void Toggle()
        {
            on = !on;
        }
    }
}
