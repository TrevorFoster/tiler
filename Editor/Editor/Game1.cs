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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static ControlWind control;
        World world;
        public static int gridcellwidth = 16;
        public static int gridcellheight = 16;
        public static int origgridcellwidth = 16;
        public static int origgridcellheight = 16;

        public static GraphicsDevice graphicsdevice;
        public static ContentManager content;
        public static int WindowWidth = 1200;
        public static int WindowHeight = 900;
        TileMover tilemover;
        Bin bin;
        bool savedlast = false;

        Dictionary<string, TileMap> TileMaps = new Dictionary<string, TileMap>();
        public static bool Collision = false;
        public static bool Visual = true;
        public static bool Grid = false;
        public static bool Exits = false;
        //int frames = 0;
        //double time = 0;

        List<Window> Windows = new List<Window>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = false;
        }


        protected override void Initialize()
        {
            ActiveCam.camera = new ActiveCam(new Camera(0, 0));
            tilemover = new TileMover();
            bin = new Bin(new Vector2(WindowWidth - 50, WindowHeight - 50), 50, 50);
            this.Window.AllowUserResizing = true;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            content = Content;
            graphicsdevice = GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Shapes.LoadContent();
            Resources.LoadContent();
            world = new World();
            WindowManager.NewWindow(new SideBar( new Vector2(0, 0), 0, 0, true));
            //WindowManager.NewWindow(new DialogBoxWindow("Editor", new Vector2(20, 20), WindowWidth - 40, WindowHeight - 40));
            WindowManager.NewWindow(new Toolbar(new Vector2(0, WindowHeight - 100), WindowWidth, 100));
        }


        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            WindowWidth = this.Window.ClientBounds.Width;
            WindowHeight = this.Window.ClientBounds.Height;
            //if (time < 1000)
            //{
            //    time += gameTime.ElapsedGameTime.TotalMilliseconds;
            //    frames += 1;
            //}
            //else
            //{

            //    frames = 0;
            //    time = 0;
            //}
            Listener.Update();
            WindowManager.Update();
            world.Update();
            bin.Update();
            tilemover.Update();
            if (Listener.KeyDown(Keys.LeftControl) && Listener.KeyDown(Keys.S) && !savedlast){Resources.SaveMap();savedlast=true;}
            else if (!Listener.KeyDown(Keys.LeftControl) && !Listener.KeyDown(Keys.S) && savedlast) { savedlast = false; }
            ActiveCam.camera.Update();
            
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            world.Draw();
            if (Grid) { DrawGrid(); }
            bin.Draw();
            tilemover.Draw();
            WindowManager.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }
        
        public void DrawGrid()
        {

            for (int y = (int)ActiveCam.camera.position.Y / gridcellheight - 1; y < (int)(ActiveCam.camera.position.Y + WindowHeight) / gridcellheight + 1; y++)
            {
                for (int x = (int)ActiveCam.camera.position.X / gridcellwidth - 1; x < (int)(ActiveCam.camera.position.X + WindowWidth) / gridcellwidth + 1; x++)
                {
                    Shapes.DrawRectangle(gridcellwidth, gridcellheight, new Vector2(x * gridcellwidth, y * gridcellheight) - ActiveCam.camera.position, new Color(250, 255, 255, 50), 1f);
                }
            }

        }
    }
}
