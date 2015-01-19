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
    class DropDown : Control
    {
        public int LastIndex = -1;
        public bool DropOpen = false;
        int Width = 0;
        int Height = 0;
        int ButtonWidth = 20;
        bool Last = false;
        string font = "font2";
        int arrowselect = -1;

        public DropDown(Vector2 position, int Width, int Height, List<string> Items,bool Reset)
            : base(position, "")
        {
            this.Items = Items;
            this.Width = Width;
            this.Height = Height;
            reset = Reset;
        }

        public override void Update(Vector2 ofs)
        {
            if (!Last && Listener.IsLeftDown() && DropOpen)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Listener.mrect.Intersects(new Rectangle((int)(position.X + ofs.X), (int)(position.Y + ofs.Y + Height * (i + 1)), Width + ButtonWidth, Height)))
                    {
                        SelectedIndex = i;
                        break;
                    }
                }
            }
            if (Listener.mrect.Intersects(new Rectangle((int)(position.X + ofs.X + Width), (int)(position.Y + ofs.Y), ButtonWidth, Height)) &&  Listener.NewPressLeft())
            {
                if (DropOpen) { DropOpen = false; arrowselect = -1; }
                else { DropOpen = true; }
            }
            else if (!Listener.mrect.Intersects(new Rectangle((int)(position.X + ofs.X + Width), (int)(position.Y + ofs.Y), ButtonWidth, Height)) && Listener.NewPressLeft())
            {
                DropOpen = false;
                arrowselect = -1;
            }
            if (Listener.NewPush(Keys.Down) && DropOpen)
            {
                if (arrowselect < Items.Count - 1)
                {
                    arrowselect += 1;
                }
                else
                {
                    arrowselect = -1;
                }
            }
            if (Listener.NewPush(Keys.Up) && DropOpen)
            {
                if (arrowselect > -1)
                {
                    arrowselect -= 1;
                }
                else
                {
                    arrowselect = Items.Count-1;
                }
            }
            if ( DropOpen&&arrowselect != -1 && Listener.NewPush(Keys.Enter))
            {
                SelectedIndex = arrowselect;
                DropOpen = false;
            }
            if (LastIndex != SelectedIndex)
            {
                Trigger = true;
                LastIndex = SelectedIndex;
            }
            else
            {
                Trigger = false;
            }
        }

        public override void Draw(Vector2 ofs)
        {
            Shapes.DrawRectangle(Width + ButtonWidth, Height, position + ofs, new Color(255,255,255,100), 0);
            Shapes.DrawRectangle(ButtonWidth, Height, position + ofs + new Vector2(Width, 0), Color.Gray, 0);
            Shapes.DrawRectangle(Width + ButtonWidth, Height, position + ofs, Color.Black, 1);
            Shapes.DrawRectangle(ButtonWidth, Height, position + ofs + new Vector2(Width, 0), Color.Black, 1);
            Game1.spriteBatch.Draw(Resources.Textures["DropDown"], position + ofs + new Vector2(Width + ButtonWidth / 2 - Resources.Textures["DropDown"].Width / 2, Height / 2 - Resources.Textures["DropDown"].Height / 2), Color.White);
            if (SelectedIndex != -1)
            {
                Game1.spriteBatch.DrawString(Resources.Fonts[font], Items[SelectedIndex], position + ofs, Color.Black);
            }
            if (DropOpen && Items.Count>0)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Shapes.DrawRectangle(Width + ButtonWidth, Height, position + ofs + new Vector2(0, Height * (i + 1)), Color.White, 0);
                    if (i == arrowselect)
                    {
                        Shapes.DrawRectangle(Width + ButtonWidth, Height, position + ofs + new Vector2(0, Height * (i + 1)), new Color(30, 144, 255, 255), 0);
                        Game1.spriteBatch.DrawString(Resources.Fonts[font], Items[i], position + ofs + new Vector2(0, Height * (i + 1)), Color.White);
                    }
                    
                    else
                    {
                        Game1.spriteBatch.DrawString(Resources.Fonts[font], Items[i], position + ofs + new Vector2(0, Height * (i + 1)), Color.Black);
                    }
                    if ((Listener.mrect.Intersects(new Rectangle((int)(position.X + ofs.X), (int)(position.Y + ofs.Y + Height * (i + 1)), Width + ButtonWidth, Height))))
                    {
                        arrowselect = i;
                    }
                    
                    
                }
                Shapes.DrawRectangle(Width + ButtonWidth, Height * Items.Count, position + ofs + new Vector2(0, Height), Color.Black, 1);
            }
        }
        public override bool IsDropOpen()
        {
            return DropOpen;
        }

    }
}
