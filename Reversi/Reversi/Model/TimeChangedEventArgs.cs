using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bead.Reversi.Model
{
    public class TimeChangedEventArgs : EventArgs
    {
        private TimeSpan _player1Time;
        private TimeSpan _player2Time;

        public TimeSpan Player1Time { get { return _player1Time; } }
        public TimeSpan Player2Time { get { return _player2Time; } }

        public TimeChangedEventArgs(TimeSpan player1Time, TimeSpan player2Time)
        {
            _player1Time = player1Time;
            _player2Time = player2Time;
        }
    }
}
