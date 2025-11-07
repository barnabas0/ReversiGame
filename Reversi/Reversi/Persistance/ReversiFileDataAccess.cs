using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Bead.Reversi.Model;

namespace Bead.Reversi.Persistance
{
    public class ReversiFileDataAccess : IReversiDataAccess
    {
        public async Task<(ReversiTable, TimeSpan, TimeSpan)> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] nums = line.Split(' ');
                    Int32 tableSize = Int32.Parse(nums[0]);
                    TimeSpan whiteTime = TimeSpan.Parse(nums[1]);
                    TimeSpan blackTime = TimeSpan.Parse(nums[2]);
                    ReversiTable table = new ReversiTable(tableSize);

                    for (Int32 i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        String[] letters = line.Split(' ');

                        for (Int32 j = 0; j < tableSize; j++)
                        {
                            table.SetValue(i, j, letters[j]);
                        }
                    }

                    for (Int32 i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        String[] locks = line.Split(' ');

                        for (Int32 j = 0; j < tableSize; j++)
                        {
                            if (locks[j] == "1")
                                table.SetLock(i, j);
                        }
                    }
                    return (table,whiteTime,blackTime);
                }
            }
            catch
            {
                throw new ReversiDataException();
            }
        }

        public async Task SaveAsync(String path, ReversiTable table, TimeSpan whiteTime, TimeSpan blackTime)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(table.Size + " " + whiteTime + " " + blackTime);
                    await writer.WriteLineAsync(" ");
                    for (Int32 i = 0; i < table.Size; i++)
                    {
                        for (Int32 j = 0; j < table.Size; j++)
                        {
                            await writer.WriteAsync(table[i, j] + " ");
                        }
                        await writer.WriteLineAsync();
                    }
                    for (Int32 i = 0; i < table.Size; i++)
                    {
                        for (Int32 j = 0; j < table.Size; j++)
                        {
                            await writer.WriteAsync((table.IsLocked(i,j) ? "1" : "0") + " ");
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new ReversiDataException();
            }
        }
    }
}
