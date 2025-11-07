using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bead.Reversi.Model
{
    public class GameEndedEventArgs : EventArgs
    {
        private Int32 _player1Score;
        private Int32 _player2Score;
        private String _result;
        public Int32 Player1Score { get { return _player1Score; } }
        public Int32 Player2Score { get { return _player2Score; } }
        public String Result { get { return _result; } }

        public GameEndedEventArgs(Int32 player1Score, Int32 player2Score, String result)
        {
            _player1Score = player1Score;
            _player2Score = player2Score;
            _result = result;
        }
    }
}
