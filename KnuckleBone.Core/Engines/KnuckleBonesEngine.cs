using KnuckleBones.Core.Models.Game;
using KnuckleBones.Core.Models.Game.Opponents;

namespace KnuckleBones.Core.Engines
{
    public delegate void GameEventHandler();

    public class KnuckleBonesEngine
    {
        public GameEventHandler? OnGameOver;

        public GameState State { get; }
        public bool GameOver { get; set; }

        private readonly Random _rnd = new Random();

        public KnuckleBonesEngine()
        {
            State = new GameState();
            State.FirstOpponent = new PlayerOpponent(Guid.NewGuid(), "player", "");
            State.FirstOpponentBoard = new BoardDefinition()
            {
                Columns = new List<ColumnDefinition>()
                {
                    new ColumnDefinition(){ Cells = new List<int>(){ 0, 0, 0 } },
                    new ColumnDefinition(){ Cells = new List<int>(){ 0, 0, 0 } },
                    new ColumnDefinition(){ Cells = new List<int>(){ 0, 0, 0 } },
                }
            };
            State.SecondOpponent = new RandomPositionOpponent(Guid.NewGuid(), "CPU", "");
            State.SecondOpponentBoard = new BoardDefinition()
            {
                Columns = new List<ColumnDefinition>()
                {
                    new ColumnDefinition(){ Cells = new List<int>(){ 0, 0, 0 } },
                    new ColumnDefinition(){ Cells = new List<int>(){ 0, 0, 0 } },
                    new ColumnDefinition(){ Cells = new List<int>(){ 0, 0, 0 } },
                }
            };
            State.CurrentDice = new DiceDefinition() { Sides = 6 };
            State.CurrentDice.Value = _rnd.Next(1, State.CurrentDice.Sides + 1);
            State.Turn = State.FirstOpponent.OpponentID;
        }

        public bool TakeTurn()
        {
            if (GameOver)
                return false;

            IOpponent opponent;
            BoardDefinition board;

            IOpponent opponent2;
            BoardDefinition board2;

            if (State.Turn == State.FirstOpponent.OpponentID)
            {
                opponent = State.FirstOpponent;
                board = State.FirstOpponentBoard;
                opponent2 = State.SecondOpponent;
                board2 = State.SecondOpponentBoard;
            }
            else if (State.Turn == State.SecondOpponent.OpponentID)
            {
                opponent = State.SecondOpponent;
                board = State.SecondOpponentBoard;
                opponent2 = State.FirstOpponent;
                board2 = State.FirstOpponentBoard;
            }
            else
                return false;

            if (State.CurrentDice == null)
                return false;
            if (State.CurrentDice.Value == 0)
                return false;
            var columnID = opponent.GetTargetColumn(board);
            if (columnID < 0 || columnID >= board.Columns.Count)
                return false;
            var column = board.Columns[columnID];
            if (column.Cells[column.Cells.Count - 1] != 0)
                return false;
            var lowest = column.Cells.IndexOf(0);
            column.Cells[lowest] = State.CurrentDice.Value;

            RemoveOpposites(board, board2);

            if (board.IsFull())
            {
                OnGameOver?.Invoke();
                GameOver = true;
                return true;
            }

            State.CurrentDice = new DiceDefinition() { Sides = 6 };
            State.CurrentDice.Value = _rnd.Next(1, State.CurrentDice.Sides + 1);

            if (State.Turn == State.FirstOpponent.OpponentID)
                State.Turn = State.SecondOpponent.OpponentID;
            else
                State.Turn = State.FirstOpponent.OpponentID;

            return true;
        }

        private void RemoveOpposites(BoardDefinition newBoard, BoardDefinition opponentBoard)
        {
            if (newBoard.Columns.Count != opponentBoard.Columns.Count)
                throw new Exception("Boards are not the same!");
            for (int i = 0; i < newBoard.Columns.Count; i++)
            {
                // Remove all equals in each column from the opponent board
                bool any = false;
                foreach (var value in newBoard.Columns[i].Cells)
                {
                    if (value == 0)
                        continue;

                    while (opponentBoard.Columns[i].Cells.Any(x => x == value))
                    {
                        any = true;
                        opponentBoard.Columns[i].Cells.AddRange(Enumerable.Repeat(0, opponentBoard.Columns[i].Cells.RemoveAll(x => x == value)));
                    }
                }
                // Collapse column
                if (any)
                {
                    var targetSize = opponentBoard.Columns[i].Cells.Count;
                    opponentBoard.Columns[i].Cells.RemoveAll(x => x == 0);
                    opponentBoard.Columns[i].Cells.AddRange(Enumerable.Repeat(0, targetSize - opponentBoard.Columns[i].Cells.Count));
                }
            }
        }
    }
}