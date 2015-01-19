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
    static class Listener
    {
        public static KeyboardState keystate;
        public static KeyboardState laststate;
        public static MouseState ms;
        public static MouseState lastms;
        public static Rectangle mrect = new Rectangle();
        public static int lastscroll = 0;
        public static List<Vector2> lastpos = new List<Vector2>() { new Vector2(0, 0), new Vector2(0, 0) };
        public static Keys[] keys = new Keys[36] { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I,
            Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W,
            Keys.X, Keys.Y, Keys.Z,Keys.D0,Keys.D1,Keys.D2,Keys.D3,Keys.D4,Keys.D5,Keys.D6,Keys.D7,Keys.D8,Keys.D9};
        public static string[] Upper = new string[36] { "A", "B", "C", "D", "E", "F", "G", "H", "I",
            "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z","0","1","2","3","4","5","6","7","8","9"};
        public static string[] Lower = new string[36] { "a", "b", "c", "d", "e", "f", "g", "h", "i",
            "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z","0","1","2","3","4","5","6","7","8","9"};
        public static bool[] Down = new bool[36] { false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
            false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false};

        public static void Update() 
        {
            laststate = keystate;
            keystate = Keyboard.GetState();
            lastms = ms;
            ms = Mouse.GetState();
            lastpos.RemoveAt(1);
            lastpos.Insert(0, new Vector2(ms.X, ms.Y));
            mrect.X = (int)ms.X;
            mrect.Y = (int)ms.Y;
            mrect.Width = 1;
            mrect.Height = 1;
            int c = 0;
            foreach (Keys key in keys)
            {
                if (NewPush(key))
                {
                    Down[c] = true;
                }
                else
                {
                    Down[c] = false;
                }
                c++;
            }
        }
        public static bool KeyDown(Keys key)
        {
            return keystate.IsKeyDown(key);
        }
        public static bool NewPush(Keys key)
        {
            return laststate.IsKeyUp(key) && keystate.IsKeyDown(key);
        }
        public static bool IsLeftDown()
        {
            return ms.LeftButton == ButtonState.Pressed;
        }
        public static bool NewPressLeft()
        {
            return ms.LeftButton == ButtonState.Pressed && !(lastms.LeftButton == ButtonState.Pressed);
        }
        public static bool IsRightDown()
        {
            if (ms.RightButton == ButtonState.Pressed)
            {
                return true;
            }
            else { return false; }
        }
        public static int ScrollDirection()
        {
            if (ms.ScrollWheelValue>lastscroll)
            {
                lastscroll = ms.ScrollWheelValue;
                return 1;
            }
            else if (ms.ScrollWheelValue < lastscroll)
            {
                lastscroll = ms.ScrollWheelValue;
                return 2;
            }

            else { lastscroll = ms.ScrollWheelValue; return 0; }
        }

        public static List<string> Pressed()
        {
            List<string> p = new List<string>();
            for (int i = 0; i < Lower.Length; i++)
            {
                if (Down[i])
                {
                    if (KeyDown(Keys.LeftShift) || KeyDown(Keys.RightShift))
                    {
                        p.Add(Upper[i]);
                    }
                    else
                    {
                        p.Add(Lower[i]);
                    }
                }
            }
            return p;
        }
    }
}
