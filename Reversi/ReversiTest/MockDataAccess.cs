using Bead.Reversi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bead.Reversi.Persistance;

namespace ReversiTest
{
    public class MockDataAccess : IReversiDataAccess
    {
        public Task SaveAsync(String path,ReversiTable table, TimeSpan whiteTime, TimeSpan blackTime)
        {
            return Task.CompletedTask;
        }
        public Task<(ReversiTable,TimeSpan,TimeSpan)> LoadAsync(String path)
        {
            var table = new ReversiTable(10);
            var whiteTime = TimeSpan.Zero;
            var blackTime = TimeSpan.Zero;
            table.SetValue(4,4,"w");
            table.SetValue(4,5,"b");
            table.SetValue(5,4,"b");
            table.SetValue(5,5,"w");
            return Task.FromResult((table,whiteTime,blackTime));
        }
    }
}
