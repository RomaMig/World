using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using World.Environment;
using World.Utilite;

namespace World.Environment.Light
{
    class LightSource
    {
        private Point3 location;
        private Vector3 direction;
        private Color color;
        private float brightness;
        public Point3 Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                if (changed != null)
                    changed(this, null);
            }
        }
        public Vector3 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
                if (changed != null)
                    changed(this, null);
            }
        }
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                if (changed != null)
                    changed(this, null);
            }
        }
        public float Brightness
        {
            get
            {
                return brightness;
            }
            set
            {
                brightness = value;
                if (changed != null)
                    changed(this, null);
            }
        }
        public FieldForm Form { get; }

        public event EventHandler changed;

        public enum FieldForm
        {
            UNIFORM,
            NON_UNIFORM
        }

        public LightSource(Point3 location, Vector3 direction, Color color, float brightness, FieldForm form)
        {
            this.location = location;
            this.direction = direction;
            this.color = color;
            this.brightness = brightness;
            Form = form;
            changed = null;
        }
    }
}
