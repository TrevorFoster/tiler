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
    class SideBar:Window
    {
        public List<Component> components = new List<Component>();
        public static int ActiveComponent = 0;
        Color tabOn = new Color(100, 100, 100, 100);
        Color tabOff = new Color(80, 80, 80 , 120);
        bool Hidden = false;
        public static int tabwidth = 25;
        string font = "Gui";

        public SideBar(Vector2 position, int Width, int Height, bool Hidden)
            : base(position, Width, Height, Hidden, true, "SideBar")
        {
            AddComponent(new TileBar(200, Game1.WindowHeight - 15, position));
            AddComponent(new Options(200, Game1.WindowHeight - 15, position));
            AddComponent(new SchematicHandler(200, Game1.WindowHeight - 15, position));
        }

        public void AddComponent(Component component)
        {
            components.Add(component);
        }

        public Component GetActiveComponent()
        {
            return components[ActiveComponent];
        }

        public override int Updates()
        {
            if (World.change)
            {
                foreach (Component c in components)
                {
                    c.ResetIndeces();
                }
            }
            
            float theight = components[ActiveComponent].height / components.Count;

            if (Listener.NewPressLeft())
            {
                float ofs = 0;
                if (!Hidden)
                {
                    ofs = components[ActiveComponent].width;
                }
                for (int i = 0; i < components.Count; i++)
                {
                    if (Listener.mrect.Intersects(new Rectangle((int)(ofs + position.X), (int)(i * theight + position.Y+moverect.Height), tabwidth, (int)theight)) && TileMover.heldtile==null && !TileMover.lastheld)
                    {
                        if (ActiveComponent == i && !windowmove)
                        {
                            if (!Hidden) { Hidden = true; }
                            else { Hidden = false; }
                        }
                        else if(!windowmove)
                        {
                            Hidden = false;
                            ActiveComponent = i;
                        }
                        break;
                    }

                }
            }

            if (!Hidden)
            {
                components[ActiveComponent].Update(position+new Vector2(0,moverect.Height));
                components[ActiveComponent].UpdateRectangle(position + new Vector2(0, moverect.Height));
                moverect.Width = components[ActiveComponent].width + tabwidth;
                Width = components[ActiveComponent].width ;
            }
            else
            {
                moverect.Width = tabwidth;
                Width = 0;
            }
            Height = components[ActiveComponent].height + moverect.Height ;
            return -1;
        }

        public void DrawBar()
        {
            float theight = components[ActiveComponent].height / components.Count;
            float ofs = 0;
            if (!Hidden)
            {
                ofs = components[ActiveComponent].width;
            }

            for (int i = 0; i < components.Count; i++)
            {
                if (i == ActiveComponent)
                {
                    Shapes.DrawRectangle(tabwidth, theight, new Vector2(ofs, i * theight + moverect.Height) + position, tabOn, 0);
                }
                else
                {
                    Shapes.DrawRectangle(tabwidth, theight, new Vector2(ofs, i * theight + moverect.Height) + position, tabOff, 0);
                }
                Shapes.DrawRectangle(tabwidth, 2, new Vector2(ofs, i * theight + theight - 2 + moverect.Height) + position, new Color(80, 80, 80, 150), 0);
                //Shapes.DrawRectangle(2, theight-theight+20, new Vector2(ofs, i * theight+theight-20) + position, new Color(80, 80, 80, 150), 0);
                int c = 0;
                float tot = components[i].Name.Measure(Resources.Fonts[font]).X;
                Color text;
                foreach (char s in components[i].Name)
                {
                    float charwidth = s.ToString().Measure(Resources.Fonts[font]).X;
                    float charheight = s.ToString().Measure(Resources.Fonts[font]).Y;
                    if (i == ActiveComponent)
                    {
                        text = Color.Black;
                    }
                    else
                    {
                        text = Color.White;
                    }
                    Game1.spriteBatch.DrawString(Resources.Fonts[font], s.ToString(), new Vector2(ofs + tabwidth / 2 - charwidth / 2, i * theight + c * (s.ToString().Measure(Resources.Fonts[font]).Y - 4) + theight / 2 - tot + moverect.Height) + position, text);
                    c++;
                }
            }
            if (!Hidden)
            {
                components[ActiveComponent].Draw(position+new Vector2(0,moverect.Height));
            }
            //Shapes.DrawRectangle(mrect, Color.Black, 2);
            //Shapes.DrawRectangle(mrect, tabOff, 0);
        }

        public override void Extra()
        {
            DrawBar();
        }
    }
}