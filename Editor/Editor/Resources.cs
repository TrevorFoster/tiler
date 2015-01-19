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
using System.IO;

namespace Editor
{
    class Resources
    {
        public static Dictionary<string, Texture2D> TTiles;
        public static Dictionary<string, Texture2D> STiles;
        public static Dictionary<string, Texture2D> CTiles;
        public static Dictionary<string, Texture2D> Textures;
        public static Dictionary<string, SpriteFont> Fonts;
        public static Dictionary<string, DialogueTree> DiagTrees;
        public static Dictionary<string, Texture2D> ToolbarIcons;

        public static void LoadContent()
        {
            Fonts = LoadFonts();
            TTiles = Resources.LoadTextures("TTiles");
            STiles = Resources.LoadTextures("STiles");
            CTiles = Resources.LoadTextures("CTiles");
            Textures = Resources.LoadTextures("PlayerSheets");
            DiagTrees = Resources.LoadDiagTrees("NPCDiagTree");
            ToolbarIcons = Resources.LoadTextures("Gui/Toolbar Icons");
        }

        #region LoadFonts
        public static Dictionary<string,SpriteFont> LoadFonts()
        {
            Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
            fonts["font"] = Game1.content.Load<SpriteFont>("Font");
            fonts["font2"] = Game1.content.Load<SpriteFont>("Font2");
            fonts["Gui"] = Game1.content.Load<SpriteFont>("Gui");
            return fonts;
        }
        #endregion
        #region Load Textures
        public static Dictionary<string, Texture2D> LoadTextures(string contentFolder)
        {
            string before = Game1.content.RootDirectory;
            DirectoryInfo dir = new DirectoryInfo(before + "/" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            Dictionary<String, Texture2D> result = new Dictionary<String, Texture2D>();
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);

                try
                {
                    using (System.IO.Stream titleStream = TitleContainer.OpenStream(Game1.content.RootDirectory + "\\" + contentFolder + "\\" + file))
                    {
                        result[key] = Texture2D.FromStream(Game1.graphicsdevice, titleStream);
                    }
                }
                catch
                {
                    //throw new System.IO.FileLoadException("Cannot load '" + Game1.content.RootDirectory + "\\" + contentFolder + "\\" + key + "' file!");
                }
            }
            return result;
        }
        #endregion
        #region Load Tile Maps
        public static Dictionary<string, TileMap> LoadTileMaps(string folder)
        {
            Dictionary<string, TileMap> maps = new Dictionary<string, TileMap>();

            if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DaGame\\" + folder))
            {
                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DaGame\\" + folder);
            }
            string[] filePaths = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DaGame\\" + folder);
            foreach (string path in filePaths)
            {
                string line = "";
                System.IO.StreamReader file = new System.IO.StreamReader(path);

                int type = int.Parse(file.ReadLine());
                Dictionary<Point, string[]> exits = new Dictionary<Point, string[]>();
                Dictionary<Point, Vector2> exitpos = new Dictionary<Point, Vector2>();
                int Nexits = int.Parse(file.ReadLine());
                for (int e = 0; e < Nexits; e++)
                {
                    string[] exit = file.ReadLine().Split(',');

                    exits[new Point(int.Parse(exit[0]), int.Parse(exit[1]))] = new string[2] { exit[2], Path.GetFileNameWithoutExtension(path) };
                    exitpos[new Point(int.Parse(exit[0]), int.Parse(exit[1]))] = new Vector2(int.Parse(exit[3]), int.Parse(exit[4]));
                }
                int height = int.Parse(file.ReadLine());
                Dictionary<Point, Tile> visual = new Dictionary<Point, Tile>();
                for (int y = 0; y < height; y++)
                {
                    line = file.ReadLine();
                    List<string> s = line.Split(",");
                    int x = 0;
                    int cursor = 0;
                    while(cursor<s.Count)
                    {
                        string tile = s[cursor];
                        if (tile.IsDigit())
                        {
                            if (tile != "0")
                            {
                                visual.Add(new Point(x, y), new Tile(tile, type, x, y));
                            }
                            x++;
                        }
                        else
                        {
                            List<string> data = tile.Split("x");
                            for (int i = 0; i < int.Parse(data[1]); i++)
                            {
                                if (data[0] != "0")
                                {
                                    visual.Add(new Point(x, y), new Tile(data[0], type, x, y));
                                }
                                x++;
                            }
                        }
                        cursor += 1;
                    }
                }

                Dictionary<Point, Tile> collision = new Dictionary<Point, Tile>();
                for (int y = 0; y < height; y++)
                {
                    line = file.ReadLine();
                    List<string> s = line.Split(",");
                    int cursor = 0;
                    int x = 0;
                    while(cursor<s.Count)
                    {
                        string tile = s[cursor];
                        if (tile.IsDigit())
                        {
                            if (tile != "0")
                            {
                                collision[new Point(x, y)] = new Tile(tile, 2, x, y);
                            }
                            x++;
                        }
                        else
                        {
                            List<string> data = tile.Split("x");
                            for (int i = 0; i < int.Parse(data[1]); i++)
                            {
                                if (data[0] != "0")
                                {
                                    collision[new Point(x, y)] = new Tile(data[0], 2, x, y);
                                }
                                x++;
                            }
                        }
                        cursor += 1;
                    }
                }
                string[] spath = path.Split('\\');
                string name = spath[spath.Length - 1];
                maps[name.Split('.')[0]] = (new TileMap(visual, collision, exits, exitpos, type, name.Split('.')[0]));
                file.Close();
            }
            return maps;
        }
        #endregion
        #region Load Schematics
        public static Dictionary<string, Schematic> LoadSchematics(string folder)
        {
            Dictionary<string, Schematic> schems = new Dictionary<string, Schematic>();

            if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DaGame\\" + folder))
            {
                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DaGame\\" + folder);
            }
            string[] filePaths = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DaGame\\" + folder);
            foreach (string path in filePaths)
            {
                string line = "";
                System.IO.StreamReader file = new System.IO.StreamReader(path);

                int type = int.Parse(file.ReadLine());
                int height = int.Parse(file.ReadLine());
                Dictionary<Point, Tile> visual = new Dictionary<Point, Tile>();
                for (int y = 0; y < height; y++)
                {
                    line = file.ReadLine();
                    List<string> s = line.Split(",");
                    int x = 0;
                    int cursor = 0;
                    while (cursor < s.Count)
                    {
                        string tile = s[cursor];
                        if (tile.IsDigit())
                        {
                            if (tile != "0")
                            {
                                visual.Add(new Point(x, y), new Tile(tile, type, x, y));
                            }
                            x++;
                        }
                        else
                        {
                            List<string> data = tile.Split("x");
                            for (int i = 0; i < int.Parse(data[1]); i++)
                            {
                                if (data[0] != "0")
                                {
                                    visual.Add(new Point(x, y), new Tile(data[0], type, x, y));
                                }
                                x++;
                            }
                        }
                        cursor += 1;
                    }
                }

                Dictionary<Point, Tile> collision = new Dictionary<Point, Tile>();
                for (int y = 0; y < height; y++)
                {
                    line = file.ReadLine();
                    List<string> s = line.Split(",");
                    int cursor = 0;
                    int x = 0;
                    while (cursor < s.Count)
                    {
                        string tile = s[cursor];
                        if (tile.IsDigit())
                        {
                            if (tile != "0")
                            {
                                collision[new Point(x, y)] = new Tile(tile, 2, x, y);
                            }
                            x++;
                        }
                        else
                        {
                            List<string> data = tile.Split("x");
                            for (int i = 0; i < int.Parse(data[1]); i++)
                            {
                                if (data[0] != "0")
                                {
                                    collision[new Point(x, y)] = new Tile(data[0], 2, x, y);
                                }
                                x++;
                            }
                        }
                        cursor += 1;
                    }
                }
                string[] spath = path.Split('\\');
                string name = spath[spath.Length - 1];
                schems[name.Split('.')[0]] = (new Schematic(visual, collision, type, name.Split('.')[0]));
                file.Close();
            }
            return schems;
        }
        #endregion
        #region Load Dialogue Trees
        public static Dictionary<string, DialogueTree> LoadDiagTrees(string folder)
        {
            Dictionary<string, DialogueTree> trees = new Dictionary<string, DialogueTree>();
            string[] filePaths = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DaGame\\DiagTrees" );

            foreach (string path in filePaths)
            {
                
                System.IO.StreamReader file = new System.IO.StreamReader(path);
                Dictionary<int, TreeNode> tree = new Dictionary<int, TreeNode>();
                string line = "";
                while ((line = file.ReadLine()) != null)
                {
                    string[] split = line.Split('|');
                    string message = split[1];
                    int node = int.Parse(split[0]);
                    int directions = int.Parse(split[2]);
                    if (directions > 0)
                    {
                        List<string> values = new List<string>();
                        List<int> targets = new List<int>();
                        for (int n = 0; n < directions; n++)
                        {
                            string[] split1 = file.ReadLine().Split('|');                            
                            int target = int.Parse(split1[0].Remove(0, 1));
                            values.Add(split1[1]);
                            targets.Add(target);
                        }
                        tree.Add(node,new TreeNode(message,targets,values, "font", node.ToString()));
                    }
                    else
                    {
                        tree.Add(node,new TreeNode(message, "font",node.ToString()));
                    }
                    string[] spath = path.Split('\\');
                    trees[spath[spath.Length - 1].Split('.')[0]] = new DialogueTree(tree);
                }
                file.Close();
            }
            return trees;
        }
        #endregion
        #region Save Map
        public static void SaveMap()
        {
            if (World.TileMaps.ContainsKey(World.ActiveMap))
            {
                string total = "";
                total += World.TileMaps[World.ActiveMap].type + "\n";
                Point key = Point.Zero;
                foreach (Microsoft.Xna.Framework.Point point in World.TileMaps[World.ActiveMap].tilemap.Keys)
                {
                    key = point;
                    break;
                }
                Tile s = new Tile("0", World.TileMaps[World.ActiveMap].type, 0, 0);
                if (World.TileMaps[World.ActiveMap].tilemap.ContainsKey(key))
                {
                    s = World.TileMaps[World.ActiveMap].tilemap[key];
                }
                int minx = (int)s.X;
                int miny = (int)s.Y;
                int maxx = (int)s.X;
                int maxy = (int)s.X;
                foreach (Point point in World.TileMaps[World.ActiveMap].tilemap.Keys)
                {
                    if (point.X < minx) { minx = point.X; }
                    if (point.X > maxx) { maxx = point.X; }
                    if (point.Y < miny) { miny = point.Y; }
                    if (point.Y > maxy) { maxy = point.Y; }
                }
                foreach (Point point in World.TileMaps[World.ActiveMap].collision.Keys)
                {
                    if (point.X < minx) { minx = point.X; }
                    if (point.X > maxx) { maxx = point.X; }
                    if (point.Y < miny) { miny = point.Y; }
                    if (point.Y > maxy) { maxy = point.Y; }
                }
                total += World.TileMaps[World.ActiveMap].exits.Count + "\n";
                foreach (Point p in World.TileMaps[World.ActiveMap].exits.Keys)
                {
                    total += p.X + "," + p.Y + "," + World.TileMaps[World.ActiveMap].exits[p][0] + "," + World.TileMaps[World.ActiveMap].exitpos[p].X + "," + World.TileMaps[World.ActiveMap].exitpos[p].Y + "\n";
                }
                int j = maxy - miny + 1;
                total += (j).ToString() + "\n";


                string start = "";
                int repeat;
                string line = "";
                string next;
                for (int y = miny; y <= maxy; y++)
                {
                    start = "";
                    line = "";
                    int x = minx;
                    while (x <= maxx)
                    {
                        if (World.TileMaps[World.ActiveMap].tilemap.ContainsKey(new Point(x, y)))
                        {
                            start = World.TileMaps[World.ActiveMap].tilemap[new Point(x, y)].id;
                        }
                        else
                        {
                            start = "0";
                        }
                        x++;
                        repeat = 1;
                        if (World.TileMaps[World.ActiveMap].tilemap.ContainsKey(new Point(x, y)))
                        {
                            next = World.TileMaps[World.ActiveMap].tilemap[new Point(x, y)].id;
                        }
                        else
                        {
                            next = "0";
                        }
                        while (next == start && x <= maxx)
                        {
                            repeat++;
                            x++;
                            if (World.TileMaps[World.ActiveMap].tilemap.ContainsKey(new Point(x, y)))
                            {
                                next = World.TileMaps[World.ActiveMap].tilemap[new Point(x, y)].id;
                            }
                            else
                            {
                                next = "0";
                            }
                        }
                        if (repeat == 1)
                        {
                            line += start;
                        }
                        else
                        {
                            line += start;
                            line += "x";
                            line += repeat;
                        }
                        line += ",";
                    }
                    total += line;
                    total += "\n";
                }
                for (int y = miny; y <= maxy; y++)
                {
                    start = "";
                    line = "";
                    int x = minx;
                    while (x <= maxx)
                    {
                        if (World.TileMaps[World.ActiveMap].collision.ContainsKey(new Point(x, y)))
                        {
                            start = World.TileMaps[World.ActiveMap].collision[new Point(x, y)].id;
                        }
                        else
                        {
                            start = "0";
                        }
                        x++;
                        repeat = 1;
                        if (World.TileMaps[World.ActiveMap].collision.ContainsKey(new Point(x, y)))
                        {
                            next = World.TileMaps[World.ActiveMap].collision[new Point(x, y)].id;
                        }
                        else
                        {
                            next = "0";
                        }
                        while (next == start && x <= maxx)
                        {
                            repeat++;
                            x++;
                            if (World.TileMaps[World.ActiveMap].collision.ContainsKey(new Point(x, y)))
                            {
                                next = World.TileMaps[World.ActiveMap].collision[new Point(x, y)].id;
                            }
                            else
                            {
                                next = "0";
                            }
                        }
                        if (repeat == 1)
                        {
                            line += start;
                        }
                        else
                        {
                            line += start;
                            line += "x";
                            line += repeat;
                        }
                        line += ",";
                    }
                    total += line;
                    total += "\n";
                }
                System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DaGame\\Levels\\" + World.TileMaps[World.ActiveMap].filename + ".lvl");
                file.WriteLine(total);
                file.Close();
                Console.WriteLine("Saved..." + World.ActiveMap + ".lvl");
            }
        }
#endregion
        #region Save Schematics
        public static void SaveSchematics()
        {
            foreach(Schematic schem in World.Schematics.Values)
            {
                string total = "";
                total += schem.type + "\n";
                Point key = Point.Zero;
                foreach (Microsoft.Xna.Framework.Point point in schem.tilemap.Keys)
                {
                    key = point;
                    break;
                }
                Tile s = new Tile("0", schem.type, 0, 0);
                if (schem.tilemap.ContainsKey(key))
                {
                    s = schem.tilemap[key];
                }
                int minx = (int)s.X;
                int miny = (int)s.Y;
                int maxx = (int)s.X;
                int maxy = (int)s.X;
                foreach (Point point in schem.tilemap.Keys)
                {
                    if (point.X < minx) { minx = point.X; }
                    if (point.X > maxx) { maxx = point.X; }
                    if (point.Y < miny) { miny = point.Y; }
                    if (point.Y > maxy) { maxy = point.Y; }
                }
                foreach (Point point in schem.collision.Keys)
                {
                    if (point.X < minx) { minx = point.X; }
                    if (point.X > maxx) { maxx = point.X; }
                    if (point.Y < miny) { miny = point.Y; }
                    if (point.Y > maxy) { maxy = point.Y; }
                }
                int j = maxy - miny + 1;
                total += (j).ToString() + "\n";

                string start = "";
                int repeat;
                string line = "";
                string next;
                for (int y = miny; y <= maxy; y++)
                {
                    start = "";
                    line = "";
                    int x = minx;
                    while (x <= maxx)
                    {
                        if (schem.tilemap.ContainsKey(new Point(x, y)))
                        {
                            start = schem.tilemap[new Point(x, y)].id;
                        }
                        else
                        {
                            start = "0";
                        }
                        x++;
                        repeat = 1;
                        if (schem.tilemap.ContainsKey(new Point(x, y)))
                        {
                            next = schem.tilemap[new Point(x, y)].id;
                        }
                        else
                        {
                            next = "0";
                        }
                        while (next == start && x <= maxx)
                        {
                            repeat++;
                            x++;
                            if (schem.tilemap.ContainsKey(new Point(x, y)))
                            {
                                next = schem.tilemap[new Point(x, y)].id;
                            }
                            else
                            {
                                next = "0";
                            }
                        }
                        if (repeat == 1)
                        {
                            line += start;
                        }
                        else
                        {
                            line += start;
                            line += "x";
                            line += repeat;
                        }
                        line += ",";
                    }
                    total += line;
                    total += "\n";
                }
                for (int y = miny; y <= maxy; y++)
                {
                    start = "";
                    line = "";
                    int x = minx;
                    while (x <= maxx)
                    {
                        if (schem.collision.ContainsKey(new Point(x, y)))
                        {
                            start = schem.collision[new Point(x, y)].id;
                        }
                        else
                        {
                            start = "0";
                        }
                        x++;
                        repeat = 1;
                        if (schem.collision.ContainsKey(new Point(x, y)))
                        {
                            next = schem.collision[new Point(x, y)].id;
                        }
                        else
                        {
                            next = "0";
                        }
                        while (next == start && x <= maxx)
                        {
                            repeat++;
                            x++;
                            if (schem.collision.ContainsKey(new Point(x, y)))
                            {
                                next = schem.collision[new Point(x, y)].id;
                            }
                            else
                            {
                                next = "0";
                            }
                        }
                        if (repeat == 1)
                        {
                            line += start;
                        }
                        else
                        {
                            line += start;
                            line += "x";
                            line += repeat;
                        }
                        line += ",";
                    }
                    total += line;
                    total += "\n";
                }
                System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DaGame\\Schematics\\" + schem.filename + ".schm");
                file.WriteLine(total);
                file.Close();
                Console.WriteLine("Saved..." + schem.filename + ".schm");
            }
        }
        #endregion
        #region Save Dialogue Trees
        public static void SaveDiagTrees(string folder)
        {
            foreach(string filename in DiagTrees.Keys)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DaGame\\DiagTrees\\" + filename + ".dt");
                foreach(int node in DiagTrees[filename].tree.Keys)
                {
                    file.WriteLine(node.ToString() + "|" + DiagTrees[filename].tree[node].Message + "|" + DiagTrees[filename].tree[node].options.Count.ToString());
                    foreach(OptionNode op in DiagTrees[filename].tree[node].options)
                    {
                        file.WriteLine(">" + op.Target.ToString() + "|" + op.message);
                    }
                }
                
                file.Close();
            }
        }
        #endregion
    }
}