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
    class World
    {
        public static string ActiveMap = "";
        public string Last = "";
        public static bool change = false;
        public static Dictionary<string, TileMap> TileMaps;
        public static Dictionary<string, Schematic> Schematics;

        public World()
        {
            TileMaps = Resources.LoadTileMaps("Levels");
            Schematics = Resources.LoadSchematics("Schematics");
        }

        public void Update()
        {
            if (Last != ActiveMap)
            {
                change = true;
                Last = ActiveMap;
            }
            else
            {
                change = false;
            }
        }

        public void Draw()
        {
            if (TileMaps.ContainsKey(ActiveMap))
            {
                TileMaps[ActiveMap].Draw();
            }

        }
        public static TileMap Active()
        {
            if (TileMaps.ContainsKey(ActiveMap))
            {
                return TileMaps[ActiveMap];
            }
            return null;
        }
    }
}
