using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Environment;
using World.Interfaces;

namespace World.EngineSystem
{
    class Engine : ITask
    {
        private List<IChangeable> preTask;
        private List<IChangeable> parallel;
        private List<IChangeable> postTask;
        public bool Runnable { get; set; }
        public Task Task { get; set; }
        public Stopwatch Watch { get; set; }

        public Engine()
        {
            Watch = new Stopwatch();
            preTask = new List<IChangeable>();
            parallel = new List<IChangeable>();
            postTask = new List<IChangeable>();
        }

        public void Start()
        {
            Runnable = true;
            preTask.ForEach((IChangeable c) =>
            {
                IWork w;
                if ((w = c as IWork) != null)
                {
                    w.Start();
                }
            });
            parallel.ForEach((IChangeable c) =>
            {
                IWork w;
                if ((w = c as IWork) != null)
                {
                    w.Start();
                }
            });
            postTask.ForEach((IChangeable c) =>
            {
                IWork w;
                if ((w = c as IWork) != null)
                {
                    w.Start();
                }
            });
        }

        public void Stop()
        {
            Runnable = false;
            preTask.ForEach((IChangeable c) =>
            {
                IWork w;
                if ((w = c as IWork) != null)
                {
                    w.Stop();
                }
            });
            parallel.ForEach((IChangeable c) =>
            {
                IWork w;
                if ((w = c as IWork) != null)
                {
                    w.Stop();
                }
            });
            postTask.ForEach((IChangeable c) =>
            {
                IWork w;
                if ((w = c as IWork) != null)
                {
                    w.Stop();
                }
            });
        }

        public void Work(Action preAct, Action postAct, Action<Exception> error)
        {
            Task = Task.Run(
                () =>
                {
                    preAct?.Invoke();
                    while (Runnable)
                    {
                        Watch.Restart();
                        try
                        {
                            Parallel.ForEach(preTask, (IChangeable c) => { c.Update()?.Wait(); });
                            Parallel.ForEach(parallel, (IChangeable c) => { c.Update()?.Wait(); });
                            Parallel.ForEach(postTask, (IChangeable c) => { c.Update()?.Wait(); });
                        }
                        catch (Exception e)
                        {
                            error(e);
                        }
                        finally
                        {
                            postAct?.Invoke();
                        }
                    }
                });
        }

        public void AddPre(params IChangeable[] c)
        {
            preTask.AddRange(c);
        }
        public void AddParallel(params IChangeable[] c)
        {
            parallel.AddRange(c);
        }
        public void AddPost(params IChangeable[] c)
        {
            postTask.AddRange(c);
        }
        public void Clear()
        {
            preTask.Clear();
            parallel.Clear();
            postTask.Clear();
        }
    }
}
