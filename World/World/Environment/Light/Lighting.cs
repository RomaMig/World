using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using World.Environment.Map;
using World.Utilite;
using World.Interfaces;

namespace World.Environment.Light
{
    class Lighting : IChangeable, IWork
    {
        private List<LightSource> sources;
        private Dictionary<Point, IBrightness> brightnesses;
        private MapShadows shadows;
        private volatile bool readyToUpdate;

        public Lighting()
        {
            this.sources = new List<LightSource>();
            this.brightnesses = new Dictionary<Point, IBrightness>();
        }

        public Lighting(LightSource source, params IBrightness[] brightnesses) : this()
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
                    DinamicLight dl;
                    if ((dl = source as DinamicLight) != null)
                    {
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
                        DinamicLight dl;
                        if ((dl = source as DinamicLight) != null)
                        {
                            dl.Runnable = false;
                        }
                    });
        }

        public Task Update()
        {
            if (readyToUpdate)
            {
                readyToUpdate = false;
                return Task.Run(() => Upd());
            }
            return null;
        }

        private void Change(object sender, EventArgs e)
        {
            readyToUpdate = true;
        }

        private void Upd()
        {
            /*
            Parallel.ForEach(brightnesses.Values, (IBrightness b) =>
            {
            });
            */

            float tmp;
            float R, G, B;
            Vector3 vtmp = new Vector3();
            foreach (IBrightness b in brightnesses.Values)
            {
                tmp = R = G = B = 0;
                sources.ForEach((LightSource s) =>
                {
                    float brtns = s.Brightness * Math.Abs(Math.Min(0, Vector3.Dot(
                        b.Normal,
                        s.Form == LightSource.FieldForm.UNIFORM ?
                            s.Direction :
                            new Vector3(s.Location, new Point3(b.Location.X, b.Location.Y, 0)).normalize())));
                    //Vector3.normalize(vtmp = new Vector3(s.Location, new Point3(b.Location.X, b.Location.Y, 0))))));
                    /// (s.Form == LightSource.FieldForm.UNIFORM ? 1 : vtmp.Lenght);

                    R = Math.Min(255, R + s.Color.R * brtns);
                    G = Math.Min(255, G + s.Color.G * brtns);
                    B = Math.Min(255, B + s.Color.B * brtns);
                    tmp += brtns;
                });
                b.Brightness = Math.Min(1, tmp);
                b.updateBrightness(R, G, B);
            }
        }
    }
}