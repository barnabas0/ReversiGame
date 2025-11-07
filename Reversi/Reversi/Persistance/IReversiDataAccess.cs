using Bead.Reversi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bead.Reversi.Persistance
{
    public interface IReversiDataAccess
    {
        Task<(ReversiTable, TimeSpan, TimeSpan)> LoadAsync(String path);

        Task SaveAsync(String path, ReversiTable table, TimeSpan whiteTime, TimeSpan blackTime);
    }
}
