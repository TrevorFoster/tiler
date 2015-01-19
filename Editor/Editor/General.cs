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
    static class General
    {
        public static List<string> Split(this string source, string character)
        {
            List<string> list = new List<string>();
            string last = "";
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i].ToString() == character)
                {
                    if (i + 1 < source.Length - 1)
                    {
                        if (source[i + 1].ToString() != character)
                        {
                            list.Add(last);
                            last = "";
                        }
                    }
                    else
                    {
                        list.Add(last);
                        last = "";
                    }
                }
                else { last += source[i]; }
            }
            if (last.Length != 0)
            {
                list.Add(last);
            }
            return list;
        }
        public static string Slice(this string source, int start, int end)
        {
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = end - start;
            return source.Substring(start, len);
        }

        public static string StripSpaces(this string source)
        {
            string s = "";
            foreach (char c in source)
            {
                if ((int)c != 32 && (int)c != 10 && (int)c != 9)
                {
                    s += c;
                }
            }
            return s;
        }
        public static string CombineKeys(this Dictionary<string,TileMap>.KeyCollection keys)
        {
            string s = "";
            foreach (string key in keys)
            {
                s += key;
            }
            return s;
        }
        public static string CombineKeys(this Dictionary<string, Schematic>.KeyCollection keys)
        {
            string s = "";
            foreach (string key in keys)
            {
                s += key;
            }
            return s;
        }

        public static string GetKey(this Dictionary<string, object> dic,int i)
        {
            int c = 0;
            foreach (string key in dic.Keys)
            {
                if (c == i)
                {
                    return key;
                }
                c++;
            }
            return "";
        }
        public static string GetKey(this Dictionary<string, Schematic> dic, int i)
        {
            int c = 0;
            foreach (string key in dic.Keys)
            {
                if (c == i)
                {
                    return key;
                }
                c++;
            }
            return "";
        }

        public static string CombineList(this List<string> source)
        {
            string s = "";
            foreach (string i in source)
            {
                s += i;
            }
            return s;
        }

        public static Vector2 Measure(this string source, SpriteFont font)
        {
            try
            {
                return font.MeasureString(source);
            }
            catch { return Vector2.Zero; }
        }

        public static bool IsDigit(this string s)
        {
            if (s == "") { return false; }
            foreach (Char i in s)
            {
                if (!IsNum(i)) { return false; }
            }
            return true;
        }

        public static bool IsNum(char i)
        {
            char[] nums = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            if (nums.Contains(i)) { return true; }
            else { return false; }
        }

    }
}
