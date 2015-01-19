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
    static class Shapes
    {
        private static Texture2D blankTexture;

        public static Texture2D CreateSquare(int w, int h, Color color)
        {
            Texture2D rectangle = new Texture2D(Game1.graphicsdevice, w, h, false, SurfaceFormat.Color);
            Color[] colorData = new Color[w * h];
            for (int i = 0; i < w * h; i++) { colorData[i] = color; }
            rectangle.SetData<Color>(colorData);
            return rectangle;
        }

        public static void LoadContent()
        {
            blankTexture = new Texture2D(Game1.graphicsdevice, 1, 1, false, SurfaceFormat.Color);
            blankTexture.SetData(new[] { Color.White });
        }

        public static void LoadContent(Texture2D blankTexture)
        {
            Shapes.blankTexture = blankTexture;
        }

        public static void DrawLineSegment(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float lineWidth)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(blankTexture, point1, null, color, angle, Vector2.Zero, new Vector2(length, lineWidth), SpriteEffects.None, 0);
        }

        public static void DrawPolygon(List<Vector2> vertex, Color color, float lineWidth)
        {
            if (vertex.Count > 0)
            {
                for (int i = 0; i < vertex.Count - 1; i++)
                {
                    DrawLineSegment(Game1.spriteBatch, vertex[i], vertex[i + 1], color, lineWidth);
                }
                DrawLineSegment(Game1.spriteBatch, vertex[vertex.Count - 1], vertex[0], color, lineWidth);
            }
        }

        public static void DrawRectangle(float w, float h, Vector2 pos, Color color, float lineWidth)
        {
            List<Vector2> vertex = new List<Vector2>();
            if (lineWidth == 0)
            {
                Game1.spriteBatch.Draw(blankTexture, pos, null, color, 0, Vector2.Zero, new Vector2(w, h), SpriteEffects.None, 0);
            }
            else
            {
                vertex.Add(new Vector2(pos.X, pos.Y));
                vertex.Add(new Vector2(pos.X + w, pos.Y));
                vertex.Add(new Vector2(pos.X + w, pos.Y + h));
                vertex.Add(new Vector2(pos.X, pos.Y + h));
                DrawPolygon(vertex, color, lineWidth);
            }

        }
        public static void DrawRectangle(Rectangle s, Color color, int lineWidth)
        {
            List<Vector2> vertex = new List<Vector2>();
            if (lineWidth == 0)
            {
                Game1.spriteBatch.Draw(blankTexture, new Vector2(s.X,s.Y), null, color, 0, Vector2.Zero, new Vector2(s.Width, s.Height), SpriteEffects.None, 0);
            }
            else
            {
                vertex.Add(new Vector2(s.X, s.Y));
                vertex.Add(new Vector2(s.X + s.Width, s.Y));
                vertex.Add(new Vector2(s.X + s.Width, s.Y + s.Height));
                vertex.Add(new Vector2(s.X, s.Y + s.Height));
                DrawPolygon(vertex, color, lineWidth);
            }
        }

        public static void DrawCircle(Vector2 center, float radius, Color color, int lineWidth, int segments)
        {
            List<Vector2> vertex = new List<Vector2>();
            for (int i = 0; i < segments; i++) { vertex.Add(Vector2.Zero); }
            double increment = Math.PI * 2.0 / segments;
            double theta = 0.0;
            for (int i = 0; i < segments; i++)
            {
                vertex[i] = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                theta += increment;
            }
            DrawPolygon(vertex, color, lineWidth);
        }
    }
}
