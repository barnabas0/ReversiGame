using Bead.Reversi.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bead.Reversi.Model
{
    public class BoardChangedEventArgs : EventArgs
    {
        private ReversiTable _board;

        public BoardChangedEventArgs(ReversiTable board) => _board = board;
    }
}
