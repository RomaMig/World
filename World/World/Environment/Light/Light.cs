using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace World
{
    class Light
    {
        private List<LightSource> sources;
        private Dictionary<Point, IBrightness> brightnesses;
        private MapShadows shadows;
        public volatile bool readyToUpdate;

        public Light()
        {
            this.sources = new List<LightSource>();
            this.brightnesses = new Dictionary<Point, IBrightness>();
        }

        public Light(LightSource source, params IBrightness[] brightnesses) : this()
        {
            AddSources(source);
            AddItems(brightnesses);
        }

        public void AddSources(params LightSource[] sources)
        {
            this.sources.AddRange(sources);
            this.sources.ForEach((LightSource source) =>
            {
                source.changed += Change;
            });
        }

        public void AddItems(params IBrightness[] brightnesses)
        {
            for (int i = 0; i < brightnesses.Length; i++)
            {
                this.brightnesses.Add(brightnesses[i].Location, brightnesses[i]);
            }
        }

        public void Clear()
        {
            sources.Clear();
            brightnesses.Clear();
        }

        public void FirstPass()
        {
            Upd();
        }

        public void Start()
        {
            if (sources != null)
                sources.ForEach(
                (LightSource source) =>
                {
                    if (source is DinamicLight)
                    {
                        DinamicLight dl = (DinamicLight)source;
                        dl.Start();
                    }
                });
        }

        public void Stop()
        {
            if (sources != null)
                sources.ForEach(
                    (LightSource source) =>
                    {
                        if (source is DinamicLight)
                        {
                            DinamicLight dl = (DinamicLight)source;
                            dl.Runnable = false;
                        }
                    });
        }

        public Task Update()
        {
            readyToUpdate = false;
            return Task.Run(() => Upd());
        }

        private void Change(object sender, EventArgs e)
        {
            readyToUpdate = true;
        }

        private void Upd()
        {
            foreach (IBrightness b in brightnesses.Values)
            {
                float tmp = 0;
                Color rflct = Color.FromArgb(0, 0, 0);
                Vector3 vtmp = new Vector3();
                sources.ForEach((LightSource s) =>
                {
                    float brtns = s.Brightness * Math.Abs(Math.Min(0, Vector3.Dot(
                        b.Normal,
                        s.Form == LightSource.FieldForm.UNIFORM ?
                            s.Direction :
                            Vector3.normalize(vtmp = new Vector3(s.Location, new Point3(b.Location.X, b.Location.Y, 0))))));
                        /// (s.Form == LightSource.FieldForm.UNIFORM ? 1 : vtmp.Lenght);
                        rflct = Color.FromArgb(
                        Math.Min(255, rflct.R + (int)(s.Color.R * brtns)),
                        Math.Min(255, rflct.G + (int)(s.Color.G * brtns)),
                        Math.Min(255, rflct.B + (int)(s.Color.B * brtns)));
                    tmp += brtns;
                });
                b.Brightness = Math.Min(1, tmp);
                b.ReflectColor = rflct;
                b.updateBrightness();
            }
        }
    }
}