using Bead.Reversi.Model;
using Bead.Reversi.Persistance;
using Microsoft.Extensions.DependencyModel;

namespace ReversiTest
{
    [TestClass]
    public sealed class ReversiGameModelTest
    {
        [TestClass]
        public class ReversiModelTests
        {
            private ReversiGameModel _model = null!;
            private MockDataAccess _dataAccess = null!;

            [TestInitialize]
            public void Initialize()
            {
                _dataAccess = new MockDataAccess();
                _model = new ReversiGameModel(_dataAccess, new TimerInheritance());
            }

            [TestMethod]
            public void NewGame_InitialBoard_CorrectSetup()
            {
                _model.NewGame(10);

                Assert.AreEqual("b", _model.GetValue(4,5));
                Assert.AreEqual("w", _model.GetValue(5,4));
                Assert.AreEqual("w", _model.GetValue(4,4));
                Assert.AreEqual("b", _model.GetValue(5,5));

                Assert.AreEqual(Player.White, _model.CurrentPlayer);
            }

            [TestMethod]
            public void Step_PlayerFlipsCorrectly()
            {
                _model.NewGame(10);

                bool valid = _model.TryMakeMove(3, 6);

                Assert.IsTrue(valid, "Move should be valid");
                Assert.AreEqual("w", _model.GetValue(4,5), "Piece should have flipped");
                Assert.AreEqual(Player.Black, _model.CurrentPlayer, "Turn should switch after move");
            }

            [TestMethod]
            public void Timer_TracksTimeCorrectly()
            {
                _model.NewGame(10);

                Thread.Sleep(1000);
                _model.TryMakeMove(3, 6);
                Thread.Sleep(1000);
                _model.PauseGame();

                Assert.IsTrue(_model.GetWhiteTime().Seconds > 0);
                Assert.IsTrue(_model.GetBlackTime().Seconds > 0);
            }

            [TestMethod]
            public async Task LoadAsync_ValidFile_LoadsCorrectly()
            {
                string path = Path.GetTempFileName();
                await File.WriteAllLinesAsync(path, new[]
                {
                    "10 00:00:15 00:00:30",
                    "e e e e e e e e e e ",
                    "e e e e e e e e e e ",
                    "e e e e e e e e e e ",
                    "e e e w e e e e e e ",
                    "e e e w w b e e e e ",
                    "e e e w w w e e e e ",
                    "e e e e w b w e e e ",
                    "e e e e e e b e e e ",
                    "e e e e e e e e e e ",
                    "e e e e e e e e e e ",
                    "0 0 0 0 0 0 0 0 0 0 ",
                    "0 0 0 0 0 0 0 0 0 0 ",
                    "0 0 0 0 0 0 0 0 0 0 ",
                    "0 0 0 1 0 0 0 0 0 0 ",
                    "0 0 0 1 1 1 0 0 0 0 ",
                    "0 0 0 1 1 1 0 0 0 0 ",
                    "0 0 0 0 1 1 1 0 0 0 ",
                    "0 0 0 0 0 0 1 0 0 0 ",
                    "0 0 0 0 0 0 0 0 0 0 ",
                    "0 0 0 0 0 0 0 0 0 0 "
                });

                var dataAccess = new ReversiFileDataAccess();
                var (table,whiteTime,blackTime) = await dataAccess.LoadAsync(path);

                Assert.AreEqual(10, table.Size);
                Assert.AreEqual(new TimeSpan(00,00,15), whiteTime);
                Assert.AreEqual(new TimeSpan(00, 00, 30), blackTime);
            }

            [TestMethod]
            public void Event_BoardChanged_FiresCorrectly()
            {
                bool eventRaised = false;
                _model.BoardChanged += (s, e) => eventRaised = true;

                _model.NewGame(10);
                _model.TryMakeMove(2, 3);

                Assert.IsTrue(eventRaised, "BoardChanged should fire on move");
            }
        }
        
    }
}
