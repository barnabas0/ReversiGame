using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bead.Reversi.Model
{
    public interface ITimer
    {
        bool Enabled { get; set; }

        double Interval { get; set; }

        event EventHandler? Elapsed;

        void Start();

        void Stop();
    }
}
