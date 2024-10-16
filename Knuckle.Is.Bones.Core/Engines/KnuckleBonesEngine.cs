using KnuckleBones.Core.Models.Game;
using KnuckleBones.Core.Resources;

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
            State.FirstOpponent = ResourceManager.Opponents.GetResource(new Guid("d6032478-b6ec-483e-8750-5976830d66b2")).Clone();
            State.FirstOpponentBoard = ResourceManager.Boards.GetResource(new Guid("907bddf8-cbe1-49f4-a1f8-92ad5266f116")).Clone();
            State.SecondOpponent = ResourceManager.Opponents.GetResource(new Guid("42244cf9-6ad3-4729-8376-a0d323440a18")).Clone();
            State.SecondOpponentBoard = ResourceManager.Boards.GetResource(new Guid("907bddf8-cbe1-49f4-a1f8-92ad5266f116")).Clone();
            State.CurrentDice = ResourceManager.Dice.GetResource(new Guid("fb539a3a-9989-4623-88d1-bf216320f717")).Clone();
            State.CurrentDice.Value = _rnd.Next(1, State.CurrentDice.Sides + 1);
            State.Turn = State.FirstOpponent.Module.OpponentID;
        }

        public OpponentDefinition GetCurrentOpponent()
        {
            if (State.Turn == State.FirstOpponent.Module.OpponentID)
                return State.FirstOpponent;
            else if (State.Turn == State.SecondOpponent.Module.OpponentID)
                return State.SecondOpponent;
            return null;
        }

        public BoardDefinition GetCurrentOpponentBoard()
        {
            if (State.Turn == State.FirstOpponent.Module.OpponentID)
                return State.FirstOpponentBoard;
            else if (State.Turn == State.SecondOpponent.Module.OpponentID)
                return State.SecondOpponentBoard;
            return null;
        }

        public bool TakeTurn()
        {
            if (GameOver)
                return false;

            OpponentDefinition opponent;
            BoardDefinition board;

            OpponentDefinition opponent2;
            BoardDefinition board2;

            if (State.Turn == State.FirstOpponent.Module.OpponentID)
            {
                opponent = State.FirstOpponent;
                board = State.FirstOpponentBoard;
                opponent2 = State.SecondOpponent;
                board2 = State.SecondOpponentBoard;
            }
            else if (State.Turn == State.SecondOpponent.Module.OpponentID)
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
            var columnID = opponent.Module.GetTargetColumn();
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
                var opponent1Value = State.FirstOpponentBoard.GetValue();
                var opponent2Value = State.SecondOpponentBoard.GetValue();
                if (opponent1Value > opponent2Value)
                    State.Winner = State.FirstOpponent.Module.OpponentID;
                else
                    State.Winner = State.SecondOpponent.Module.OpponentID;
                return true;
            }

            State.CurrentDice.Value = _rnd.Next(1, State.CurrentDice.Sides + 1);

            if (State.Turn == State.FirstOpponent.Module.OpponentID)
                State.Turn = State.SecondOpponent.Module.OpponentID;
            else
                State.Turn = State.FirstOpponent.Module.OpponentID;

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