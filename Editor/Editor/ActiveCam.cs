using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    class ActiveCam:Camera
    {
        public static Camera camera;
        public ActiveCam(Camera camera) : base(camera.position.X, camera.position.Y) { ActiveCam.camera = camera; }
    }
}
