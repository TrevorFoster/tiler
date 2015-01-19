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
    static class WindowManager
    {
        public static List<Window> Windows = new List<Window>();
        public static int Focused;
        public static bool Moving = false;

        public static int NewWindow(Window window)
        {
            window.index = Windows.Count;
            Windows.Add(window);
            Windows[Windows.Count - 1].Initialize();
            return Windows.Count - 1;
        }

        private static List<Window> FocusSort(List<Window> windows)
        {
            List<Window> temp = new List<Window>();
            for (int i = 0; i < windows.Count; i++)
            {
                if (i != Focused)
                {
                    temp.Add(windows[i]);
                }
            }
            temp.Add(windows[Focused]);
            return temp;
        }

        public static void DisposeWindow(int window)
        {
            Windows.RemoveAt(window);
        }

        public static void DisposeWindow(Window window)
        {
            Windows.Remove(window);
        }

        public static bool Exists(string window)
        {
            foreach (Window wind in Windows)
            {
                if (wind.GetType().Name == window)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Any(int index, Vector2 position)
        {
            for (int i = 0; i < Windows.Count;i++)
            {
                if(i!=index)
                {
                    if(new Rectangle((int)position.X,(int)position.Y,1,1).Intersects(new Rectangle((int)Windows[i].position.X,(int)Windows[i].position.Y,Windows[i].Width,Windows[i].Height)))
                    {
                        return true;
                    }
                }
            }
                return false;
        }

        public static void Update()
        {
            Moving = false;
            for (int i = 0; i < Windows.Count; i++)
            {
                if (Windows[i].windowmove)
                {
                    Moving = true;
                }
            }
            for (int i = 0;i<Windows.Count;i++)
            {

                if (Windows[i].Open)
                {
                    int ind = Windows[i].Update();
                    if (ind != -1)
                    {
                        for (int b = ind; b < Windows.Count ; b++)
                        {
                            Windows[b].index -= 1;
                        }
                        Windows.RemoveAt(ind);
                        
                        Focused = -1;
                        break;
                    }
                }
                
            }
            if (Focused != -1)
            {
                List<Window> a = FocusSort(Windows);
            }
        }

        public static Window ToolBar()
        {
            foreach(Window w in Windows)
            {
                if (w.GetType().Name == "Toolbar")
                {
                    return w;
                }
            }
            return null;
        }

        public static void Draw()
        {
            List<Window> a = new List<Window>();

            if (Focused != -1)
            {
                a = FocusSort(Windows);
            }
            else
            {
                a = Windows.ToList();
            }
            for(int i = 0;i<a.Count;i++)
            {
                if (a[i].Open)
                {
                    a[i].Draw();
                }
            }
        }
    }
}
