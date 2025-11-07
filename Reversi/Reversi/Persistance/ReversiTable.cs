using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bead.Reversi.Persistance
{
    public class ReversiTable
    {
        #region Fields

        private String[,] _fieldValues;
        private Boolean[,] _fieldLocks;
        private Int32 _tableSize;
        

        #endregion

        #region Properties

        public Boolean IsFilled
        {
            get
            {
                foreach (String value in _fieldValues)
                {
                    if (value == "e")
                        return false;
                }
                return true;
            }
        }
        public Int32 Size
        {
            get
            {
                return _tableSize;
            }
            set
            {
                _tableSize = value;
            }
        }
        public String this[Int32 x, Int32 y]
        {
            get { return GetValue(x, y);}
        }
        
        #endregion

        #region Constructors

        public ReversiTable() : this(10) { }

        public ReversiTable(Int32 tableSize)
        {
            if (tableSize != 10 && tableSize != 20 && tableSize != 30)
                throw new ArgumentOutOfRangeException(nameof(tableSize), "The table size must be 10, 20 or 30.");

            _fieldValues = new String[tableSize, tableSize];
            _fieldLocks = new Boolean[tableSize, tableSize];
            _tableSize = tableSize;

            for (Int32 i = 0; i < tableSize; i++)
            {
                for (Int32 j = 0; j < tableSize; j++)
                {
                    _fieldValues[i, j] = "e";
                    if (i == tableSize / 2 - 1 && j == tableSize / 2 - 1)
                    {
                        _fieldLocks[i, j] = true;
                        _fieldValues[i, j] = "w";
                    }
                        
                    if (i == tableSize / 2 && j == tableSize / 2 - 1)
                    {
                        _fieldLocks[i, j] = true;
                        _fieldValues[i, j] = "w";
                    }
                    if (i == tableSize / 2 - 1 && j == tableSize / 2)
                    {
                        _fieldLocks[i, j] = true;
                        _fieldValues[i, j] = "b";
                    }
                    if (i == tableSize / 2 && j == tableSize / 2)
                    {
                        _fieldLocks[i, j] = true;
                        _fieldValues[i, j] = "b";
                    }
                }
            }
        }

        #endregion

        #region Public methods

        public Boolean IsEmpty(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _fieldValues[x, y] == "e";
        }
        public Boolean IsLocked(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _fieldLocks[x, y];
        }
        public String GetValue(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _fieldValues[x, y];
        }
        public void SetValue(Int32  x, Int32 y, String value)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");
            
            

            _fieldValues[x, y] = value;
        }
        public void SetLock(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            _fieldLocks[x, y] = true;
        }

        #endregion
    }
}
