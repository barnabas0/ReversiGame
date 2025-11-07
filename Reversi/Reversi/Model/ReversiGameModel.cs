using Bead.Reversi.Persistance;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Bead.Reversi.Model
{
    public class ReversiGameModel
    {
        #region Fields

        private IReversiDataAccess _dataAccess;
        private Player _currentPlayer;
        private ReversiTable _table;
        
        private readonly Stopwatch _whiteStopwatch = new();
        private readonly Stopwatch _blackStopwatch = new();
        private TimeSpan _whiteTime = TimeSpan.Zero;
        private TimeSpan _blackTime = TimeSpan.Zero;

        private readonly ITimer _timer;


       
        public record Move(Int32 row, Int32 col);
        private readonly (int dx, int dy)[] directions = new (int, int)[]
        {
            (-1,-1),(-1,0),(-1,1),
            (0,-1),(0,1),
            (1,-1),(1,0),(1,1)
        };
        private int consecutivePasses = 0;

        #endregion

        #region Properties

        public ReversiTable Table => _table;
        public String this[Int32 x, Int32 y] => _table[x, y];
        
        public Boolean IsGameOver
        {
            get { return _table.IsFilled; }
        }
        public Player CurrentPlayer { get  { return _currentPlayer; } set { _currentPlayer = value; } }

        #endregion

        #region Events

        public event EventHandler<BoardChangedEventArgs>? BoardChanged;

        public event EventHandler<Player?>? CurrentPlayerChanged;

        public event EventHandler<TimeChangedEventArgs>? TimeChanged;

        public event EventHandler<GameEndedEventArgs>? GameOver;

        #endregion

        #region Constructor

        public ReversiGameModel(IReversiDataAccess dataAccess, ITimer timer)
        {
            _dataAccess = dataAccess;
            _table = new ReversiTable();
            _timer = timer;
            _timer.Interval = 250;
            _timer.Elapsed += (s, e) => TimeChanged?.Invoke(this, new TimeChangedEventArgs(GetWhiteTime(), GetBlackTime()));
            _timer.Start();
        }
                #endregion

        #region Public table accessors

        public String GetValue(Int32 x, Int32 y) => _table.GetValue(x, y);
        public Boolean IsEmpty(Int32 x, Int32 y) => _table.IsEmpty(x, y);
        public Boolean IsLocked(Int32 x, Int32 y)  => _table.IsLocked(x, y);
        
        #endregion

        #region Public game methods

        public void NewGame(int size)
        {
            _table = new ReversiTable(size);
            
            CurrentPlayer = Player.White;
            _blackStopwatch.Reset();
            _whiteStopwatch.Reset();
            _whiteTime = TimeSpan.Zero;
            _blackTime = TimeSpan.Zero;
            consecutivePasses = 0;

            
            StartCurrentPlayerTimer();
            BoardChanged?.Invoke(this, new BoardChangedEventArgs(CloneBoard()));
            CurrentPlayerChanged?.Invoke(this, CurrentPlayer);
            TimeChanged?.Invoke(this, new TimeChangedEventArgs(GetWhiteTime(),GetBlackTime()));
        }
        private ReversiTable CloneBoard()
        {
            var c = new ReversiTable(_table.Size);
            c = _table;
            return c;
        }
        private void StartCurrentPlayerTimer()
        {
            if (CurrentPlayer == Player.White)
            {
                _blackStopwatch.Stop();
                _whiteStopwatch.Start();
            }
            else
            {
                _whiteStopwatch.Stop();
                _blackStopwatch.Start();
            }
        }
        private void StopCurrentPlayerTimer()
        {
            if (CurrentPlayer == Player.White)
            {
                _whiteStopwatch.Stop();
                _whiteTime += _whiteStopwatch.Elapsed;
                _whiteStopwatch.Reset();
            }
            else
            {
                _blackStopwatch.Stop();
                _blackTime += _blackStopwatch.Elapsed;
                _blackStopwatch.Reset();
            }
        }
        public TimeSpan GetWhiteTime() => _whiteTime + _whiteStopwatch.Elapsed;
        public TimeSpan GetBlackTime() => _blackTime + _blackStopwatch.Elapsed;
        public void PauseGame()
        {
            StopCurrentPlayerTimer();
            _timer.Stop();
            TimeChanged?.Invoke(this, new TimeChangedEventArgs(GetWhiteTime(), GetBlackTime()));
        }
        public void ResumeGame()
        {
            if (!IsGameOver)
                StartCurrentPlayerTimer();
                _timer.Start();
        }

        private bool IsValidMove(Int32 x, Int32 y)
        {
            var my = CurrentPlayer.ToCellState();
            var opp = CurrentPlayer.Opponent().ToCellState();
            
            foreach (var (dx,dy) in directions)
            {
                int k = x + dx;
                int l = y + dy;
                bool hasOpp = false;
                while (InBounds(k,l) && GetValue(k,l) == opp)
                {
                    hasOpp = true;
                    k += dx;
                    l += dy;
                }
                if (hasOpp && InBounds(k, l) && GetValue(k, l) == my) return true;
            }
            
            return false;
        }
        private bool InBounds(Int32 x, Int32 y) => x >= 0 && y >= 0 && x < _table.Size && y < _table.Size;
        public IEnumerable<Move> GetValidMoves()
        {
            for (int i = 0; i < _table.Size; i++)
            for (int j = 0; j < _table.Size; j++)
            {
                if (GetValue(i, j) != "e") continue;
                if (IsValidMove(i, j)) yield return new Move(i, j);
            }
        }
        public bool TryMakeMove(int x, int y)
        {
            if (!InBounds(x, y)) return false;
            if (GetValue(x, y) != "e") return false;
            if (!IsValidMove(x, y)) return false;

            var flips = new List<(int i, int j)>();
            var curr = CurrentPlayer.ToCellState();
            var opp = CurrentPlayer.Opponent().ToCellState();

            foreach (var (dx,dy) in directions)
            {
                var stack = new List<(int i, int j)>();
                int k = x + dx;
                int l = y + dy;
                while (InBounds(k, l) && GetValue(k,l) == opp)
                {
                    stack.Add((k,l));
                    k += dx;
                    l += dy;
                }
                if (stack.Count > 0 && InBounds(k,l) && GetValue(k,l) == curr)
                    flips.AddRange(stack);
            }

            _table.SetValue(x, y, curr);
            _table.SetLock(x, y);
            foreach (var (i, j) in flips) _table.SetValue(i,j,curr);

            StopCurrentPlayerTimer();

            consecutivePasses = 0;

            CurrentPlayer = CurrentPlayer.Opponent();
            StartCurrentPlayerTimer();

            BoardChanged?.Invoke(this, new BoardChangedEventArgs(CloneBoard()));
            CurrentPlayerChanged?.Invoke(this, CurrentPlayer);
            TimeChanged?.Invoke(this, new TimeChangedEventArgs(GetWhiteTime(), GetBlackTime()));

            CheckEndCondition();
            return true;
        }
        private void Pass()
        {
            StopCurrentPlayerTimer();
            consecutivePasses++;
            CurrentPlayer = CurrentPlayer.Opponent();
            StartCurrentPlayerTimer();
            CurrentPlayerChanged?.Invoke(this, CurrentPlayer);
            TimeChanged?.Invoke(this, new TimeChangedEventArgs(GetWhiteTime(), GetBlackTime()));
            BoardChanged?.Invoke(this, new BoardChangedEventArgs(CloneBoard()));
            CheckEndCondition();
        }
        private void CheckEndCondition()
        {
            bool hasEmpty = _table.IsFilled;
            int player1Score = 0;
            int player2Score = 0;
            for (int i = 0; i < _table.Size; i++)
            for (int j = 0; j < _table.Size; j++)
                {
                    if (GetValue(i,j) == "e") hasEmpty = true;
                    else if (GetValue(i, j) == "w") player1Score++;
                    else if (GetValue(i, j) == "b") player2Score++;
                }

            if (!hasEmpty || consecutivePasses >= 2)
            {
                StopCurrentPlayerTimer();
                string result;
                if (player1Score > player2Score) result = "Player1 (White) won.";
                else if (player1Score < player2Score) result = "Player2 (Black) won.";
                else result = "Draw";
                GameOver?.Invoke(this, new GameEndedEventArgs(player1Score, player2Score, result));
                _timer.Stop();
            }
            else
            {
                var nextValid = GetValidMoves().Any();
                if (!nextValid)
                    Pass();
            }
        }

        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
            {
                throw new InvalidOperationException("No data access is provided.");
            }

            (_table, _whiteTime, _blackTime) = await _dataAccess.LoadAsync(path);
        }

        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
            {
                throw new InvalidOperationException("No data access is provided.");
            }

            await _dataAccess.SaveAsync(path, _table, GetWhiteTime(), GetBlackTime());
        }

        #endregion
    }
}
