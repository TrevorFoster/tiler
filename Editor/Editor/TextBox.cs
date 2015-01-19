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
    class TextBox:Control
    {
        bool Editing = false;
        public int Width = 0;
        public int Height = 0;
        int CursorIndex = 0;
        int frameSince = 0;
        int framesDrawn = 0;
        int DrawLength = 40;
        string font = "font2";

        public TextBox(Vector2 position, int Width,int Height,string Text)
            : base(position, Text)
        {
            this.Width = Width;
            this.Height = Height;
        }

        public override void Update(Vector2 ofs)
        {
            if (Listener.mrect.Intersects(new Rectangle((int)(position.X + ofs.X),(int)( position.Y + ofs.Y), Width, Height)) && Listener.NewPressLeft())
            {
                Editing = true;
            }
            else if (!Listener.mrect.Intersects(new Rectangle((int)(position.X + ofs.X), (int)(position.Y + ofs.Y), Width, Height)) && Listener.NewPressLeft())
            {
                Editing = false;
            }

            if (Editing)
            {
                if (Text.Measure(Resources.Fonts[font]).X + "D".Measure(Resources.Fonts[font]).X < Width)
                {
                    List<string> letters = Listener.Pressed();
                    foreach (string s in letters)
                    {
                        Text = Text.Slice(0, CursorIndex) + s + Text.Slice(CursorIndex, Text.Length);
                        CursorIndex += 1;
                    }
                }
                else if(Listener.NewPush(Keys.Back) && CursorIndex>0)
                {
                    if (Text.Length > 0)
                    {
                        Text = Text.Slice(0, CursorIndex-1)+Text.Slice(CursorIndex,Text.Length);
                        CursorIndex -= 1;
                    }
                }
                if (Listener.NewPush(Keys.Left))
                {
                    if (CursorIndex > 0)
                    {
                        CursorIndex -= 1;
                    }
                }
                if (Listener.NewPush(Keys.Right))
                {
                    if (CursorIndex < Text.Length)
                    {
                        CursorIndex += 1;
                    }
                }
                if (frameSince < DrawLength)
                {
                    frameSince += 1;
                }

                if (Listener.NewPush(Keys.Space))
                {
                    Text = Text.Slice(0, CursorIndex) + " " + Text.Slice(CursorIndex, Text.Length);
                    CursorIndex += 1;
                }
            }
        }

        public override void Draw(Vector2 ofs)
        {
            Shapes.DrawRectangle(Width, Height, position + ofs, Color.White, 0);
            Shapes.DrawRectangle(Width, Height, position + ofs, Color.Black, 1);
            if (Editing)
            {
                float buffer = 2;
                for (int i = 0; i < CursorIndex; i++)
                {
                    buffer += Text[i].ToString().Measure(Resources.Fonts[font]).X;
                }
                Shapes.DrawRectangle(Width, Height, position + ofs, new Color(100, 100, 100, 200), 2);
                Game1.spriteBatch.DrawString(Resources.Fonts[font], Text, position + ofs + new Vector2(2, 0), Color.Black);
                if (frameSince == DrawLength && framesDrawn < DrawLength)
                {
                    Game1.spriteBatch.Draw(Resources.Textures["Cursor"], position + ofs + new Vector2(buffer, Height / 2 - Resources.Textures["Cursor"].Height/2), Color.White);
                    framesDrawn += 1;
                }
                if (framesDrawn == DrawLength)
                {
                    framesDrawn = 0;
                    frameSince = 0;
                }
            }
            else
            {
                Game1.spriteBatch.DrawString(Resources.Fonts[font], Text, position + ofs + new Vector2(2, 0), Color.Black);
            }
            
        }

    }
}
