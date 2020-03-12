using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.EngineSystem.OptimizeSystem;

namespace World.CameraSystem
{
    interface IPaintable : IFreezeable
    {
        event EventHandler<IPaintable> repaint;

        void Paint(Bitmap bitmap);
    }
}
