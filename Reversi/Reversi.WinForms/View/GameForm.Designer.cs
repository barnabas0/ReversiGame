using System;
using System.Windows.Forms;
using System.Drawing;

namespace Bead.Reversi.View
{
    partial class GameForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _menuStrip = new MenuStrip();
            _menuFile = new ToolStripMenuItem();
            _menuFileNewGame = new ToolStripMenuItem();
            _menuFileLoad = new ToolStripMenuItem();
            _menuFileSave = new ToolStripMenuItem();
            _menuFileExit = new ToolStripMenuItem();
            _menuSettings = new ToolStripMenuItem();
            _menuGameSmall = new ToolStripMenuItem();
            _menuGameMedium = new ToolStripMenuItem();
            _menuGameLarge = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            _toolLabel1 = new ToolStripStatusLabel();
            _toolLabelPlayer1Time = new ToolStripStatusLabel();
            _toolLabel2 = new ToolStripStatusLabel();
            _toolLabelPlayer2Time = new ToolStripStatusLabel();
            _toolLabelCurrentPlayer = new ToolStripStatusLabel();
            _openFileDialog = new OpenFileDialog();
            _saveFileDialog = new SaveFileDialog();
            _menuStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // _menuStrip
            // 
            _menuStrip.BackColor = Color.Gray;
            _menuStrip.ImageScalingSize = new Size(20, 20);
            _menuStrip.Items.AddRange(new ToolStripItem[] { _menuFile, _menuSettings });
            _menuStrip.LayoutStyle = ToolStripLayoutStyle.Flow;
            _menuStrip.Location = new Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Size = new Size(682, 28);
            _menuStrip.TabIndex = 0;
            _menuStrip.Text = "menuStrip1";
            // 
            // _menuFile
            // 
            _menuFile.DropDownItems.AddRange(new ToolStripItem[] { _menuFileNewGame, _menuFileLoad, _menuFileSave, _menuFileExit });
            _menuFile.Name = "_menuFile";
            _menuFile.Size = new Size(46, 24);
            _menuFile.Text = "File";
            // 
            // _menuFileNewGame
            // 
            _menuFileNewGame.Name = "_menuFileNewGame";
            _menuFileNewGame.Size = new Size(164, 26);
            _menuFileNewGame.Text = "New game";
            _menuFileNewGame.Click += MenuFileNewGame_Click;
            // 
            // _menuFileLoad
            // 
            _menuFileLoad.Name = "_menuFileLoad";
            _menuFileLoad.Size = new Size(164, 26);
            _menuFileLoad.Text = "Load";
            _menuFileLoad.Click += MenuFileLoadGame_Click;
            // 
            // _menuFileSave
            // 
            _menuFileSave.Name = "_menuFileSave";
            _menuFileSave.Size = new Size(164, 26);
            _menuFileSave.Text = "Save";
            _menuFileSave.Click += MenuFileSaveGame_Click;
            // 
            // _menuFileExit
            // 
            _menuFileExit.Name = "_menuFileExit";
            _menuFileExit.Size = new Size(164, 26);
            _menuFileExit.Text = "Exit";
            _menuFileExit.Click += MenuFileExit_Click;
            // 
            // _menuSettings
            // 
            _menuSettings.DropDownItems.AddRange(new ToolStripItem[] { _menuGameSmall, _menuGameMedium, _menuGameLarge });
            _menuSettings.Name = "_menuSettings";
            _menuSettings.Size = new Size(76, 24);
            _menuSettings.Text = "Settings";
            // 
            // _menuGameSmall
            // 
            _menuGameSmall.Name = "_menuGameSmall";
            _menuGameSmall.Size = new Size(147, 26);
            _menuGameSmall.Text = "Small";
            _menuGameSmall.Click += MenuGameSmall_Click;
            // 
            // _menuGameMedium
            // 
            _menuGameMedium.Name = "_menuGameMedium";
            _menuGameMedium.Size = new Size(147, 26);
            _menuGameMedium.Text = "Medium";
            _menuGameMedium.Click += MenuGameMedium_Click;
            // 
            // _menuGameLarge
            // 
            _menuGameLarge.Name = "_menuGameLarge";
            _menuGameLarge.Size = new Size(147, 26);
            _menuGameLarge.Text = "Large";
            _menuGameLarge.Click += MenuGameLarge_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = Color.Gray;
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { _toolLabel1, _toolLabelPlayer1Time, _toolLabel2, _toolLabelPlayer2Time, _toolLabelCurrentPlayer });
            statusStrip1.LayoutStyle = ToolStripLayoutStyle.Flow;
            statusStrip1.Location = new Point(0, 627);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(682, 26);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // _toolLabel1
            // 
            _toolLabel1.Name = "_toolLabel1";
            _toolLabel1.Size = new Size(85, 20);
            _toolLabel1.Text = "White time:";
            // 
            // _toolLabelPlayer1Time
            // 
            _toolLabelPlayer1Time.Name = "_toolLabelPlayer1Time";
            _toolLabelPlayer1Time.Size = new Size(55, 20);
            _toolLabelPlayer1Time.Text = "0:00:00";
            // 
            // _toolLabel2
            // 
            _toolLabel2.Name = "_toolLabel2";
            _toolLabel2.Size = new Size(81, 20);
            _toolLabel2.Text = "Black time:";
            // 
            // _toolLabelPlayer2Time
            // 
            _toolLabelPlayer2Time.Name = "_toolLabelPlayer2Time";
            _toolLabelPlayer2Time.Size = new Size(55, 20);
            _toolLabelPlayer2Time.Text = "0:00:00";
            // 
            // _toolLabelCurrentPlayer
            // 
            _toolLabelCurrentPlayer.Name = "_toolLabelCurrentPlayer";
            _toolLabelCurrentPlayer.Size = new Size(0, 0);
            // 
            // _openFileDialog
            // 
            _openFileDialog.Filter = "Reversi table (*.stl)|*.stl";
            _openFileDialog.Title = "Loading Reversi game";
            // 
            // _saveFileDialog
            // 
            _saveFileDialog.Filter = "Reversi table (*.stl)|*.stl";
            _saveFileDialog.Title = "Saving Reversi game";
            // 
            // GameForm
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            AutoScroll = true;
            AutoSize = true;
            BackColor = Color.LightGray;
            ClientSize = new Size(682, 653);
            Controls.Add(statusStrip1);
            Controls.Add(_menuStrip);
            ForeColor = Color.Black;
            MainMenuStrip = _menuStrip;
            Name = "GameForm";
            Text = "Reversi";
            Load += GameForm_Load;
            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void _menuFileSave_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }



        #endregion

        private MenuStrip _menuStrip;
        private ToolStripMenuItem _menuFile;
        private ToolStripMenuItem _menuFileNewGame;
        private ToolStripMenuItem _menuFileLoad;
        private ToolStripMenuItem _menuFileSave;
        private ToolStripMenuItem _menuFileExit;
        private ToolStripMenuItem _menuSettings;
        private ToolStripMenuItem _menuGameSmall;
        private ToolStripMenuItem _menuGameMedium;
        private ToolStripMenuItem _menuGameLarge;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel _toolLabel1;
        private ToolStripStatusLabel _toolLabelPlayer1Time;
        private ToolStripStatusLabel _toolLabel2;
        private ToolStripStatusLabel _toolLabelPlayer2Time;
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;
        private ToolStripStatusLabel _toolLabelCurrentPlayer;
    }
}
