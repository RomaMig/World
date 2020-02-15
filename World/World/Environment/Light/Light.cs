using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    class Light
    {
        private List<LightSource> sources;
        private List<IBrightness> brightnesses;

        public Light(LightSource source, params IBrightness[] brightnesses)
        {
            this.sources = new List<LightSource>();
            this.brightnesses = new List<IBrightness>();
            sources.Add(source);
            source.changed += Update;
            AddItems(brightnesses);
        }

        public void AddSources(params LightSource[] sources)
        {
            this.sources.AddRange(sources);
            this.sources.ForEach((LightSource source) =>
            {
                source.changed += Update;
            });
        }

        public void AddItems(params IBrightness[] brightnesses)
        {
            this.brightnesses.AddRange(brightnesses);
        } 

        public void Start()
        {
            Update(this, null);
        }

        private void Update(object sender, EventArgs e)
        {
            brightnesses.ForEach((IBrightness b) =>
                {
                    float tmp = 0;
                    sources.ForEach((LightSource s) =>
                        {
                            tmp += s.Brightness * Math.Abs(Math.Min(0, Vector3.Dot(
                                b.Normal,
                                s.Form == LightSource.FieldForm.UNIFORM ?
                                    s.Direction :
                                    (new Vector3(new Point3(b.Location.X, b.Location.Y, 0), s.Location)).normalize())));
                        });
                    b.Brightness = Math.Min(1, tmp);
                    b.updateBrightness();
                });
        }
    }
}