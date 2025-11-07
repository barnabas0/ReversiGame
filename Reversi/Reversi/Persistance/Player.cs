using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bead.Reversi.Persistance
{
    public enum Player
    {
        White = 0,
        Black = 1
    }

    public static class PlayerExtensions
    {
        public static String ToCellState(this Player player)
            => player == Player.White ? "w" : "b";
        public static Player Opponent(this Player player)
            => player == Player.White ? Player.Black : Player.White;
    }
}
