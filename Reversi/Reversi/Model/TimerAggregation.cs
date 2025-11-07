using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Bead.Reversi.Model
{
    public class TimerAggregation : ITimer, IDisposable
    {
        private readonly Timer _timer;

        public bool Enabled
        {
            get => _timer.Enabled;
            set => _timer.Enabled = value;
        }

        public double Interval
        {
            get => _timer.Interval;
            set => _timer.Interval = value;
        }

        public event EventHandler? Elapsed;

        public TimerAggregation()
        {
            _timer = new Timer();
            _timer.Elapsed += (s, e) =>
            {
                Elapsed?.Invoke(this, e);
            };
        }

        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
