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
    class DialogBoxWindow:Window
    {
        string font = "font";
        int Buffer = 4;
        bool editing = false;
        public float BubbleHeight = 60;
        int lineheld = -1;
        int NInd = -1;
        Vector2 nheld = Vector2.Zero;
        string CurrentTree = "";

        public DialogBoxWindow(Vector2 position,int Width,int Height,string CurrentTree)
            : base(position, Width, Height, true, true, "DialogueBoxEditor")
        {
            this.CurrentTree = CurrentTree;
            foreach (DialogueTree d in Resources.DiagTrees.Values)
            {
                d.Initialize(Vector2.Zero);
            }

            AddControl(new Button(new Vector2(0, 0), 80, 20, "Edit"));
            AddControl(new Button(new Vector2(0, 30), 80, 20, "Save"));
            AddControl(new DropDown(new Vector2(100, 0), 80, 20, Resources.DiagTrees.Keys.ToList(), false));
        }


        public override int Updates()
        {
            foreach (int i in Resources.DiagTrees[CurrentTree].tree.Keys)
            {
                Vector2 mainnode = Resources.DiagTrees[CurrentTree].tree[i].node.position + position;
                int total = Resources.DiagTrees[CurrentTree].tree[i].options.Count;
                float H = (float)BubbleHeight / (float)total;
                if (Listener.IsLeftDown() && ((Listener.mrect.Intersects(new Rectangle((int)mainnode.X, (int)mainnode.Y, 200, 80))) || Resources.DiagTrees[CurrentTree].tree[i].node.moving))
                {
                    Resources.DiagTrees[CurrentTree].tree[i].node.position += Listener.lastpos[0] - Listener.lastpos[1];
                    Resources.DiagTrees[CurrentTree].tree[i].node.moving = true;
                }
                if (!Listener.IsLeftDown())
                {
                    Resources.DiagTrees[CurrentTree].tree[i].node.moving = false;
                }
                
                for (int b = 0; b < Resources.DiagTrees[CurrentTree].tree[i].options.Count; b++)
                {
                    Vector2 nodeposition = Resources.DiagTrees[CurrentTree].tree[i].options[b].position + position;
                    if (Listener.IsLeftDown() && ((Listener.mrect.Intersects(new Rectangle((int)nodeposition.X, (int)nodeposition.Y, 200, (int)BubbleHeight))) || Resources.DiagTrees[CurrentTree].tree[i].options[b].moving))
                    {
                        Resources.DiagTrees[CurrentTree].tree[i].options[b].position += Listener.lastpos[0] - Listener.lastpos[1];
                        Resources.DiagTrees[CurrentTree].tree[i].options[b].moving = true;
                    }
                    if (!Listener.IsLeftDown())
                    {
                        Resources.DiagTrees[CurrentTree].tree[i].options[b].moving = false;
                    }

                    if (editing)
                    {
                        if (b * H + H > BubbleHeight)
                        {
                            H = b * H + H - BubbleHeight;
                        }
                        if (Listener.NewPressLeft() && new FloatRect(1, 1, new Vector2(Listener.mrect.X, Listener.mrect.Y)).Intersects(new FloatRect(10, H, mainnode + new Vector2(200, (float)Math.Ceiling((float)(b * H))))))
                        {
                            if (NInd == -1)
                            {
                                lineheld = b;
                                NInd = i;
                                nheld = mainnode + new Vector2(200 + 5, (float)Math.Ceiling((float)(b * H)) + H / 2);
                            }
                            else
                            {
                                Resources.DiagTrees[CurrentTree].Limbo.Add(new OptionNode(Resources.DiagTrees[CurrentTree].tree[i].options[b]));
                                Resources.DiagTrees[CurrentTree].tree[i].options[b] = new OptionNode(Resources.DiagTrees[CurrentTree].tree[NInd].options[lineheld]);
                                Resources.DiagTrees[CurrentTree].tree[NInd].options.RemoveAt(lineheld);
                                lineheld = -1;
                                NInd = -1;
                            }
                        }
                        
                    }
                }
            }
            foreach(OptionNode n in Resources.DiagTrees[CurrentTree].Limbo)
            {
                Vector2 nodeposition = n.position + position;
                if (Listener.IsLeftDown() && ((Listener.mrect.Intersects(new Rectangle((int)nodeposition.X, (int)nodeposition.Y, 200, (int)BubbleHeight))) || n.moving))
                {
                    n.position += Listener.lastpos[0] - Listener.lastpos[1];
                    n.moving = true;
                }
                if (!Listener.IsLeftDown())
                {
                    n.moving = false;
                }
            }
            if (Controls["Button1"].Trigger)
            {
                if (editing) { editing = false; }
                else { editing = true; }
            }
            if (Controls["Button2"].Trigger)
            {
                Resources.SaveDiagTrees("DiagTrees");
            }
            if(Controls["DropDown1"].Trigger)
            {
                if(Controls["DropDown1"].SelectedIndex!=-1)
                {
                    CurrentTree = Controls["DropDown1"].Items[Controls["DropDown1"].SelectedIndex];
                }
            }
            return -1;
        }



        public override void Extra()
        {
            foreach (int i in Resources.DiagTrees[CurrentTree].tree.Keys)
            {
                Vector2 mainnode = Resources.DiagTrees[CurrentTree].tree[i].node.position + position + new Vector2(100, BubbleHeight/2);
                int total = Resources.DiagTrees[CurrentTree].tree[i].options.Count;
                float H = (float)BubbleHeight / (float)total;
                
                for (int b = 0; b < total; b++)
                {
                    
                    //Game1.spriteBatch.DrawLineSegment(mainnode, Resources.DiagTrees[CurrentTree].tree[i].options[b].position + position + new Vector2(100, BubbleHeight / 2), Color.Red, 3);
                    if (Resources.DiagTrees[CurrentTree].tree[i].options[b].Target != -1)
                    {
                        Game1.spriteBatch.DrawLineSegment(Resources.DiagTrees[CurrentTree].tree[i].options[b].position + position + new Vector2(100, BubbleHeight / 2), Resources.DiagTrees[CurrentTree].tree[Resources.DiagTrees[CurrentTree].tree[i].options[b].Target].node.position + position + new Vector2(100, BubbleHeight / 2), Color.Black, 3);
                    }
                    Shapes.DrawRectangle(10, 10, mainnode - new Vector2(5, 5), Color.Orange, 0);
                    Shapes.DrawRectangle(10, 10, Resources.DiagTrees[CurrentTree].tree[i].options[b].position + position + new Vector2(100, BubbleHeight / 2) - new Vector2(5, 5), Color.Blue, 0);
                    if (editing)
                    {
                        if (b * H + H > BubbleHeight)
                        {
                            H = b * H + H - BubbleHeight;
                        }
                        if (new FloatRect(1, 1, new Vector2(Listener.mrect.X, Listener.mrect.Y)).Intersects(new FloatRect(10, H, mainnode + new Vector2(100, -BubbleHeight/2 + (float)Math.Ceiling((float)(b * H))))))
                        {
                            Shapes.DrawRectangle(10, H, mainnode + new Vector2(100, -BubbleHeight/2 + (float)Math.Ceiling((float)(b * H))), new Color(255, 100, 100, 100), 0);
                        }
                        else
                        {
                            Shapes.DrawRectangle(10, H, mainnode + new Vector2(100, -BubbleHeight / 2 + (float)Math.Ceiling((float)(b * H))), new Color(100, 100, 100, 100), 0);
                        }
                        //Game1.spriteBatch.DrawLineSegment(Resources.DiagTrees[CurrentTree].tree[i].options[b].position + position + new Vector2(100, BubbleHeight / 2), Resources.DiagTrees[CurrentTree].tree[Resources.DiagTrees[CurrentTree].tree[i].options[b].Target].node.position + position + new Vector2(100, BubbleHeight / 2), Color.Black, 3);
                        Shapes.DrawRectangle(10, H, mainnode + new Vector2(100, -BubbleHeight / 2 + (float)Math.Ceiling((float)(b * H))), new Color(20, 20, 20, 100), 1);
                        if (b == lineheld && i == NInd)
                        {
                            //Game1.spriteBatch.DrawLineSegment(new Vector2(Listener.mrect.X, Listener.mrect.Y), nheld, Color.Red, 3);
                            Game1.spriteBatch.DrawLineSegment(new Vector2(Listener.mrect.X, Listener.mrect.Y), Resources.DiagTrees[CurrentTree].tree[i].options[b].position + position + new Vector2(100, BubbleHeight / 2), Color.Red, 3);
                        }
                        else
                        {
                            Game1.spriteBatch.DrawLineSegment(mainnode + new Vector2(100 + 5, -BubbleHeight / 2 + (float)Math.Ceiling((float)(b * H)) + H / 2), Resources.DiagTrees[CurrentTree].tree[i].options[b].position + position + new Vector2(100, BubbleHeight / 2), Color.Red, 3);
                        }
                    }
                    
                    DrawText(Resources.DiagTrees[CurrentTree].tree[i].options[b].message, Resources.DiagTrees[CurrentTree].tree[i].options[b].position+position, new Vector2(200, BubbleHeight));
                }
                
                if (Resources.DiagTrees[CurrentTree].tree[i].options.Count == 0)
                {
                    Shapes.DrawRectangle(10, 10, mainnode - new Vector2(5, 5), Color.Red, 0);
                }
                DrawText("Node: " + Resources.DiagTrees[CurrentTree].tree[i].node.thiskey + " " + Resources.DiagTrees[CurrentTree].tree[i].node.message, Resources.DiagTrees[CurrentTree].tree[i].node.position + position, new Vector2(200, BubbleHeight));
                
            }
            foreach(OptionNode n in Resources.DiagTrees[CurrentTree].Limbo)
            {
                DrawText(n.message, n.position+position, new Vector2(200, BubbleHeight));
                Shapes.DrawLineSegment(Game1.spriteBatch, Resources.DiagTrees[CurrentTree].tree[n.Target].node.position + position + new Vector2(100, BubbleHeight / 2), n.position + position + new Vector2(100, BubbleHeight / 2), Color.Black, 3);
            }
        }

        public void DrawText(string source, Vector2 pos, Vector2 size)
        {
            Shapes.DrawRectangle(size.X, size.Y, pos, new Color(100, 100, 100, 120), 0);
            Shapes.DrawRectangle(size.X, size.Y, pos, new Color(200, 200, 200, 120), 2);
            float spacewidth = " ".Measure(Resources.Fonts[font]).X;
            List<string> message = source.Split(" ");
            float y = 0;
            float x = Buffer;
            foreach (string word in message)
            {
                float length = word.Measure(Resources.Fonts[font]).X;
                float height = word.Measure(Resources.Fonts[font]).Y;
                if (pos.X + x + length < pos.X + size.X - Buffer && pos.Y + y + height  < pos.Y + size.Y)
                {
                    Game1.spriteBatch.DrawString(Resources.Fonts[font], word, new Vector2(pos.X + x, pos.Y + y), Color.Black);
                    x += length + spacewidth;
                }
                else if (pos.Y + y + height < pos.Y + size.Y)
                {
                    y += height;
                    x = Buffer;
                    Game1.spriteBatch.DrawString(Resources.Fonts[font], word, new Vector2(pos.X + x, pos.Y + y), Color.Black);
                    x += length + spacewidth;
                }
                else
                {
                    break;
                }
            }
            if (editing)
            {
                Game1.spriteBatch.DrawString(Resources.Fonts[font], "Editing", new Vector2(position.X + Width - "Editing".Measure(Resources.Fonts[font]).X, position.Y + moverect.Height), Color.Black);
            }
            else
            {
                Game1.spriteBatch.DrawString(Resources.Fonts[font], "Not Editing", new Vector2(position.X + Width - "Not Editing".Measure(Resources.Fonts[font]).X, position.Y + moverect.Height), Color.Black);
            }
        }
    }
}
