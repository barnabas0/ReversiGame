using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Bead.Reversi.Model
{
    public class TimerInheritance : Timer, ITimer
    {
        private readonly Dictionary<EventHandler, ElapsedEventHandler> _delegateMapper = new();

        public new event EventHandler? Elapsed
        {
            add
            {
                if (value != null)
                {
                    var handler = new ElapsedEventHandler(value.Invoke);
                    _delegateMapper.Add(value, handler);
                    base.Elapsed += handler;
                }
            }
            remove
            {
                if (value != null && _delegateMapper.TryGetValue(value, out var handler))
                {
                    _delegateMapper.Remove(value);
                    base.Elapsed -= handler;
                }
            }
        }
    }
}
