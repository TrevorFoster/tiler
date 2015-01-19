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
    class OptionNode
    {
        public Vector2 position = Vector2.Zero;
        public string message;
        public int Target;
        public bool moving = false;

        public OptionNode(string message,int Target)
        {
            this.message = message;
            this.Target = Target;
        }

        public OptionNode(OptionNode node)
        {
            this.position = node.position;
            this.message = node.message;
            this.Target = node.Target;
        }

    }
}
