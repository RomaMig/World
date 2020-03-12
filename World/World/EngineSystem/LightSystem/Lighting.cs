using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using World.Environment.Map;
using World.Utilite;
using World.Interfaces;

namespace World.Environment.Light
{
    class Lighting : IChangeable, IWork
    {
        private List<LightSource> sources;
        private List<IBrightness> brightnesses;
        private MapShadows shadows;
        private volatile bool readyToUpdate;

        public Lighting()
        {
            this.sources = new List<LightSource>();
            this.brightnesses = new List<IBrightness>();
        }

        public Lighting(LightSource source, params IBrightness[] brightnesses) : this()
        {
            AddSources(source);
            Add(brightnesses);
        }

        public void AddSources(params LightSource[] sources)
        {
            this.sources.AddRange(sources);
            this.sources.ForEach((LightSource source) =>
            {
                source.changed += Change;
            });
        }

        public void Add(params IBrightness[] brightnesses)
        {
            for (int i = 0; i < brightnesses.Length; i++)
            {
                this.brightnesses.Add(brightnesses[i]);
            }
        }

        public void Remove(params IBrightness[] brightnesses)
        {
            for (int i = 0; i < brightnesses.Length; i++)
            {
                this.brightnesses.Remove(brightnesses[i]);
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
            Parallel.ForEach(brightnesses, (IBrightness b) =>
            {
            });
            */

            Vector3 vtmp = new Vector3();
            Parallel.ForEach(brightnesses, (IBrightness b) =>
            {
                if (b.Infreez)
                {
                    float tmp;
                    float R, G, B;
                    tmp = R = G = B = 0;
                    sources.ForEach((LightSource s) =>
                    {
                        Vector3 lsv;
                        if (s.Form == LightSource.FieldForm.UNIFORM)
                        {
                            lsv = s.Direction;
                        }
                        else
                        {
                            lsv = new Vector3(
                                s.Location,
                                new Point3(b.Location.X, b.Location.Y, 0)).normalize();
                        }
                        float brtns = Math.Abs(Math.Min(0, Vector3.Dot(
                                b.Normal,
                                lsv)));
                        brtns *= s.Brightness;
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
            });
        }
    }
}