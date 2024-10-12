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
            State.FirstOpponentBoard = new BoardDefinition()
            {
                Columns = new List<ColumnDefinition>()
                {
                    new ColumnDefinition(){ Cells = new List<int>(3) },
                    new ColumnDefinition(){ Cells = new List<int>(3) },
                    new ColumnDefinition(){ Cells = new List<int>(3) },
                }
            };
            State.SecondOpponentBoard = new BoardDefinition()
            {
                Columns = new List<ColumnDefinition>()
                {
                    new ColumnDefinition(){ Cells = new List<int>(3) },
                    new ColumnDefinition(){ Cells = new List<int>(3) },
                    new ColumnDefinition(){ Cells = new List<int>(3) },
                }
            };
            State.CurrentDice = new DiceDefinition() { Sides = 6 };
        }

        public bool TakeTurn()
        {
            if (GameOver)
                return false;

            IOpponent opponent;
            BoardDefinition board;

            if (State.Turn == State.FirstOpponent.OpponentID)
            {
                opponent = State.FirstOpponent;
                board = State.FirstOpponentBoard;
            }
            else if (State.Turn == State.SecondOpponent.OpponentID)
            {
                opponent = State.SecondOpponent;
                board = State.SecondOpponentBoard;
            }
            else
                return false;

            if (State.Turn != opponent.OpponentID)
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

            if (board.IsFull())
            {
                OnGameOver?.Invoke();
                GameOver = true;
                return true;
            }

            State.CurrentDice = new DiceDefinition();
            State.CurrentDice.Value = _rnd.Next(1, State.CurrentDice.Sides + 1);

            return true;
        }
    }
}