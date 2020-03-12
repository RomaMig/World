using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World.Interfaces
{
    interface ITask : IWork
    {
        bool Runnable { get; set; }
        Task Task { get; set; }
    }
}
