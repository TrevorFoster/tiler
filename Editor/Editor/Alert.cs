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
    class Alert:Window
    {
        public Alert(Vector2 position, int Width, int Height, string Message)
            : base(position, Width, Height, true, true, null)
        {
            AddControl(new Label(new Vector2(Width / 2 - Message.Measure(Resources.Fonts["font"]).X / 2, Height / 2 - Message.Measure(Resources.Fonts["font"]).Y / 2 - Height / 3), Message));
            AddControl(new Button(new Vector2(Width / 2 - 30, Height / 2 - Message.Measure(Resources.Fonts["font"]).Y / 2 - Height / 3 + 40), 60, 20, "Ok"));
        }

        public override int Updates()
        {
            if (Controls["Button1"].Trigger)
            {
                return index;
            }
            return -1;
        }
    }
}
