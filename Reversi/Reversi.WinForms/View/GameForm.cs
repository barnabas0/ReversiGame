using Bead.Reversi.Model;
using Bead.Reversi.Persistance;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Windows.Forms;

namespace Bead.Reversi.View
{
    public partial class GameForm : Form
    {
        #region Fields

        private ReversiGameModel _model;
        private Button[,] _buttonGrid = null!;
        private TableLayoutPanel? _boardPanel = null!;
        private bool paused = false;

        #endregion

        #region Constructors
        public GameForm()
        {
            InitializeComponent();

            IReversiDataAccess _dataAccess = new ReversiFileDataAccess();

            _model = new ReversiGameModel(_dataAccess, new TimerInheritance());
            _model.BoardChanged += new EventHandler<BoardChangedEventArgs>(Game_BoardChanged);
            _model.CurrentPlayerChanged += new EventHandler<Player?>(Game_CurrentPlayerChanged);
            _model.TimeChanged += new EventHandler<TimeChangedEventArgs>(Game_TimeChanged);
            _model.GameOver += new EventHandler<GameEndedEventArgs>(Game_GameOver);


            InitializeBoard();

            RenderBoard();
            UpdateCurrentPlayerLabel();
            UpdateTimes(_model.GetWhiteTime(),_model.GetBlackTime());
            SetupMenus();

            _model.NewGame(_model.Table.Size);
            SetupTable();
        }

        
        #endregion

        #region Game event handlers

        private void Game_BoardChanged(object? sender, BoardChangedEventArgs e)
        {
            if (InvokeRequired) { BeginInvoke(new Action(() => RenderBoard())); }
            else RenderBoard();
        }
        private void Game_TimeChanged(object? sender, TimeChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateTimes(e.Player1Time, e.Player2Time)));
            }
            else
                UpdateTimes(e.Player1Time, e.Player2Time);
        }
        private void UpdateTimes(TimeSpan w, TimeSpan b)
        {
            _toolLabelPlayer1Time.Text = FormatTime(w);
            _toolLabelPlayer2Time.Text = FormatTime(b);
        }
        private string FormatTime(TimeSpan time) => $"{(int)time.TotalHours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";

        private void Game_CurrentPlayerChanged(object? sender, Player? p)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateCurrentPlayerLabel));
            }
            else
            {
                UpdateCurrentPlayerLabel();
            }
            
        }

        private void Game_GameOver(object? sender, GameEndedEventArgs e)
        {
            if (InvokeRequired)
            { BeginInvoke(new Action(() => ShowGameEnd(e))); }
            else
                ShowGameEnd(e);
        }

        #endregion

        #region Grid event handlers

        private void ButtonGrid_MouseClick(Object? sender, MouseEventArgs e)
        {
            if (paused) return;
            if (sender is Button button && button.Tag is ValueTuple<int,int> coords)
            {
                var (x, y) = coords;
                var valid = _model.GetValidMoves().Any(m => m.row == x && m.col == y);
                if (!valid) return;
                _model.TryMakeMove(x, y);
                _buttonGrid[x, y].Enabled = false;
            }
        }

        #endregion

        #region Menu event handlers

        private void MenuFileNewGame_Click(Object sender, EventArgs e)
        {
            _model.BoardChanged -= Game_BoardChanged;
            _menuFileSave.Enabled = true;
            Controls.Clear();
            InitializeComponent();
            _boardPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = _model.Table.Size,
                RowCount = _model.Table.Size,
                BackColor = Color.LightGray,
                Margin = new Padding(12)
            };
            for (int i = 0; i < _model.Table.Size; i++)
            {
                _boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / _model.Table.Size));
                _boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / _model.Table.Size));
            }
            _model.NewGame(_model.Table.Size);
            InitializeBoard();
            RenderBoard();
            _model.BoardChanged += Game_BoardChanged;
            _model.ResumeGame();
            SetupTable();
            SetupMenus();
        }

        private async void MenuFileLoadGame_Click(Object sender, EventArgs e)
        {
            _model.PauseGame();
            Controls.Clear();
            InitializeComponent();

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.LoadGameAsync(_openFileDialog.FileName);
                    _menuFileSave.Enabled = true;
                }
                catch (ReversiDataException)
                {
                    MessageBox.Show("Failed to load game!" + Environment.NewLine +
                                    "Invalid path or file format.", "Error!", MessageBoxButtons.OK,MessageBoxIcon.Error);

                    _model.NewGame(_model.Table.Size);
                    _model.ResumeGame();
                    _menuFileSave.Enabled = true;
                }
                InitializeBoard();
                RenderBoard();
                UpdateCurrentPlayerLabel();
                UpdateTimes(_model.GetWhiteTime(), _model.GetBlackTime());
                _model.ResumeGame();

                SetupTable();
            }
        }

        private async void MenuFileSaveGame_Click(Object sender, EventArgs e)
        {
            _model.PauseGame();

            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.SaveGameAsync(_saveFileDialog.FileName);
                    _model.ResumeGame();
                }
                catch (ReversiDataException)
                {
                    MessageBox.Show("Failed to save game!" + Environment.NewLine +
                                    "Invalid path or library cannot be written.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _model.NewGame(_model.Table.Size);
                    _model.ResumeGame();
                    _menuFileSave.Enabled = true;
                }

            }
        }

        private void MenuFileExit_Click(Object sender, EventArgs e)
        {
            _model.PauseGame();

            if (MessageBox.Show("Are you sure you want to quit?", "Reversi", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
            else
                _model.ResumeGame();
        }

        private void MenuGameSmall_Click(Object sender, EventArgs e)
        {
            _model.Table.Size = 10;
            SetupMenus();
        }
        
        private void MenuGameMedium_Click(Object sender, EventArgs e)
        {
            _model.Table.Size = 20;
            SetupMenus();
        }

        private void MenuGameLarge_Click(Object sender, EventArgs e)
        {
            _model.Table.Size = 30;
            SetupMenus();
        }
        #endregion

        #region Private methods
        private void InitializeBoard()
        {
            _boardPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = _model.Table.Size,
                RowCount = _model.Table.Size,
                BackColor = Color.LightGray,
                Margin = new Padding(12)
            };
            for (int i = 0; i < _model.Table.Size; i++)
            {
                _boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / _model.Table.Size));
                _boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / _model.Table.Size));
            }

            if (_buttonGrid != null)
            {
                foreach (var btn in _buttonGrid)
                    this.Controls.Remove(btn);
            }

            _buttonGrid = new Button[_model.Table.Size, _model.Table.Size];

            for (int i = 0; i < _model.Table.Size; i++)
                for (int j = 0; j < _model.Table.Size; j++)
                {
                    var b = new Button
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(1),
                        Tag = (i, j),
                        BackColor = Color.Green,
                        FlatStyle = FlatStyle.Flat
                    };
                    b.FlatAppearance.BorderSize = 1;
                    b.MouseClick += ButtonGrid_MouseClick;
                    _buttonGrid[i, j] = b;
                    _boardPanel.Controls.Add(b);
                }
        }

        private void RenderBoard()
        {
            for (int i = 0; i < _model.Table.Size; i++)
            for (int j = 0; j < _model.Table.Size; j++)
                {
                    var b = _buttonGrid[i, j];
                    b.Paint -= Circle_Paint_Black;
                    b.Paint -= Circle_Paint_White;
                    b.BackColor = Color.Green;
                    b.ForeColor = Color.White;
                    var cs = _model.GetValue(i,j);
                    if (cs == "w")
                        b.Paint += Circle_Paint_White;
                    else if (cs == "b")
                        b.Paint += Circle_Paint_Black;
                    else
                        b.BackColor = Color.Green;

                    b.Invalidate();
                }
            Controls.Add(_boardPanel);

            foreach (var btn in _buttonGrid) btn.Text = "";
            var valids = _model.GetValidMoves().ToList();
            foreach (var mv in valids)
            {
                _buttonGrid[mv.row, mv.col].Text = "*";
                _buttonGrid[mv.row, mv.col].ForeColor = Color.Yellow;
                _buttonGrid[mv.row,mv.col].Font = new Font(FontFamily.GenericSansSerif,12,FontStyle.Bold);
            }
        }
        private void Circle_Paint_White(Object? sender, PaintEventArgs e)
        {
            if (sender is not Button btn) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int padding = (int)(Math.Min(btn.Width, btn.Height) * 0.1);
            int diameter = Math.Min(btn.Width, btn.Height) - 2 * padding;
            int x = (btn.Width - diameter) / 2;
            int y = (btn.Height - diameter) / 2;
            using (Brush b = new SolidBrush(Color.White))
            {
                g.FillEllipse(b, x, y, diameter, diameter);
            }

        }
        private void Circle_Paint_Black(Object? sender, PaintEventArgs e)
        {
            if (sender is not Button btn) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int padding = (int)(Math.Min(btn.Width, btn.Height) * 0.1);
            int diameter = Math.Min(btn.Width, btn.Height) - 2 * padding;
            int x = (btn.Width - diameter) / 2;
            int y = (btn.Height - diameter) / 2;
            using (Brush b = new SolidBrush(Color.Black))
            {
                g.FillEllipse(b, x, y, diameter, diameter);
            }

        }
        private void UpdateCurrentPlayerLabel()
        {
            _toolLabelCurrentPlayer.Text = _model.CurrentPlayer == Player.White ? "Current player: White" : "Current player: Black";
        }
        
        private void SetupTable()
        {
            for (Int32 i = 0; i < _buttonGrid.GetLength(0); i++)
            {
                for (Int32 j = 0; j < _buttonGrid.GetLength(1); j++)
                {
                    if (!_model.IsLocked(i, j))
                    {
                        _buttonGrid[i, j].Enabled = true;
                    }
                    else
                    {
                        _buttonGrid[i,j].Enabled = false;
                    }
                }
            }
        }
        private void SetupMenus()
        {
            _menuGameSmall.Checked = _model.Table.Size == 10;
            _menuGameMedium.Checked = _model.Table.Size == 20;
            _menuGameLarge.Checked = _model.Table.Size == 30;
        }
        private void ShowGameEnd(GameEndedEventArgs e)
        {
            paused = true;
            MessageBox.Show($"{e.Result}\nWhite: {e.Player1Score} Black: {e.Player2Score}", "Game over!",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        private void GameForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
