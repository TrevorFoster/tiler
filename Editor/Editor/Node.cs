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
    
    class Node
    {
        public Vector2 position = Vector2.Zero;
        public string message;
        public bool moving = false;
        public string thiskey;

        public Node(string message, string nodekey)
        {
            this.message = message;
            thiskey = nodekey;
        }
    }
}
