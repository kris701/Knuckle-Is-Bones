using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.Core.Models.Game.MoveModules;
using Knuckle.Is.Bones.Core.Models.Saves;

namespace Knuckle.Is.Bones.Core.Engines
{
	public delegate void GameEventHandler();

	public class KnuckleBonesEngine
	{
		public GameEventHandler? OnGameOver;
		public GameEventHandler? OnOpponentDiceRemoved;
		public GameEventHandler? OnCombo;
		public GameEventHandler? OnTurn;

		public GameState State { get; }
		public bool GameOver { get; set; }

		public KnuckleBonesEngine(GameState state)
		{
			State = state;
		}

		public OpponentDefinition GetCurrentOpponent()
		{
			if (State.Turn == State.FirstOpponent.MoveModule.OpponentID)
				return State.FirstOpponent;
			return State.SecondOpponent;
		}

		public BoardDefinition GetCurrentOpponentBoard()
		{
			if (State.Turn == State.FirstOpponent.MoveModule.OpponentID)
				return State.FirstOpponentBoard;
			return State.SecondOpponentBoard;
		}

		public OpponentDefinition GetNextOpponent()
		{
			if (State.Turn != State.FirstOpponent.MoveModule.OpponentID)
				return State.FirstOpponent;
			return State.SecondOpponent;
		}

		public BoardDefinition GetNextOpponentBoard()
		{
			if (State.Turn != State.FirstOpponent.MoveModule.OpponentID)
				return State.FirstOpponentBoard;
			return State.SecondOpponentBoard;
		}

		public bool TakeTurn()
		{
			if (GameOver)
				return false;

			OnTurn?.Invoke();

			OpponentDefinition opponent;
			BoardDefinition board;

			OpponentDefinition opponent2;
			BoardDefinition board2;

			if (State.Turn == State.FirstOpponent.MoveModule.OpponentID)
			{
				opponent = State.FirstOpponent;
				board = State.FirstOpponentBoard;
				opponent2 = State.SecondOpponent;
				board2 = State.SecondOpponentBoard;
			}
			else if (State.Turn == State.SecondOpponent.MoveModule.OpponentID)
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
			var columnID = opponent.MoveModule.GetTargetColumn();
			if (columnID < 0 || columnID >= board.Columns.Count)
				return false;
			var column = board.Columns[columnID];
			if (column.Cells[column.Cells.Count - 1] != 0)
				return false;
			var lowest = column.Cells.IndexOf(0);
			if (column.Cells.Contains(State.CurrentDice.Value))
				OnCombo?.Invoke();
			column.Cells[lowest] = State.CurrentDice.Value;

			RemoveOpposites(board, board2);

			if (board.IsFull())
			{
				OnGameOver?.Invoke();
				GameOver = true;
				var opponent1Value = State.FirstOpponentBoard.GetValue();
				var opponent2Value = State.SecondOpponentBoard.GetValue();
				if (opponent1Value > opponent2Value)
					State.Winner = State.FirstOpponent.MoveModule.OpponentID;
				else
					State.Winner = State.SecondOpponent.MoveModule.OpponentID;
				State.Save();
				return true;
			}

			State.CurrentDice.RollValue();

			if (State.Turn == State.FirstOpponent.MoveModule.OpponentID)
				State.Turn = State.SecondOpponent.MoveModule.OpponentID;
			else
				State.Turn = State.FirstOpponent.MoveModule.OpponentID;

			State.Save();
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
					OnOpponentDiceRemoved?.Invoke();

					var targetSize = opponentBoard.Columns[i].Cells.Count;
					opponentBoard.Columns[i].Cells.RemoveAll(x => x == 0);
					opponentBoard.Columns[i].Cells.AddRange(Enumerable.Repeat(0, targetSize - opponentBoard.Columns[i].Cells.Count));
				}
			}
		}

		public GameResult GetGameResult()
		{
			if (!GameOver)
				throw new Exception("Game is not over yet!");

			var playerWon = false;
			var hadPlayer = false;
			var pointsGained = 0;
			var completedItems = new HashSet<Guid>();
			var winnerName = "";
			if ((State.FirstOpponent.MoveModule is PlayerMoveModule) && (State.SecondOpponent.MoveModule is not PlayerMoveModule) && State.Winner == State.FirstOpponent.MoveModule.OpponentID)
			{
				playerWon = true;
				pointsGained = (int)(State.FirstOpponentBoard.GetValue() * State.SecondOpponent.Difficulty);
				completedItems.Add(State.SecondOpponent.ID);
				completedItems.Add(State.FirstOpponentBoard.ID);
				completedItems.Add(State.CurrentDice.ID);
			}
			if ((State.SecondOpponent.MoveModule is PlayerMoveModule) && (State.FirstOpponent.MoveModule is not PlayerMoveModule) && State.Winner == State.SecondOpponent.MoveModule.OpponentID)
			{
				playerWon = true;
				pointsGained = (int)(State.SecondOpponentBoard.GetValue() * State.FirstOpponent.Difficulty);
				completedItems.Add(State.FirstOpponent.ID);
				completedItems.Add(State.FirstOpponentBoard.ID);
				completedItems.Add(State.CurrentDice.ID);
			}
			if ((State.FirstOpponent.MoveModule is PlayerMoveModule) && (State.SecondOpponent.MoveModule is not PlayerMoveModule))
				hadPlayer = true;
			if ((State.SecondOpponent.MoveModule is PlayerMoveModule) && (State.FirstOpponent.MoveModule is not PlayerMoveModule))
				hadPlayer = true;

			if (State.Winner == State.FirstOpponent.MoveModule.OpponentID)
				winnerName = $"{State.FirstOpponent.Name}";
			else
				winnerName = $"{State.SecondOpponent.Name}";

			var result = new GameResult()
			{ 
				PlayerWon = playerWon,
				HadPlayer = hadPlayer,
				PointsGained = pointsGained,
				CompletedItems = completedItems,
				WinnerName = winnerName
			};
			return result;
		}
	}
}