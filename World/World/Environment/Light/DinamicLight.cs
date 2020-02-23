using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace World
{
    class DinamicLight : LightSource, ITask
    {
        public bool Runnable { get; set; }
        public Task Task { get; set; }
        public Action Behavior { get; set; }

        public DinamicLight(Point3 location, Vector3 direction, Color color, float brightness, FieldForm form) : base(location, direction, color, brightness, form)
        {

        }

        public DinamicLight(Point3 location, Vector3 direction, Color color, float brightness, FieldForm form, Action behavior) : this(location, direction, color, brightness, form)
        {
            AddBehavior(behavior);
        }

        public DinamicLight(Point3 location, Vector3 direction, Color color, float brightness, FieldForm form, Action behavior, int timeSleep) : this(location, direction, color, brightness, form)
        {
            AddBehavior(behavior, timeSleep);
        }

        public void AddBehavior(Action behavior)
        {
            Behavior = behavior;

            Task = new Task(() =>
            {
                Behavior();
                Runnable = false;
            });
        }

        public void AddBehavior(Action behavior, int timeSleep)
        {
            Behavior = behavior;

            Task = new Task(() =>
            {
                while (Runnable)
                {
                    Behavior();
                    Thread.Sleep(timeSleep);
                }
            });
        }

        public void Start()
        {
            if (Task != null)
            {
                Runnable = true;
                Task.Start();
            }
        }
    }
}
