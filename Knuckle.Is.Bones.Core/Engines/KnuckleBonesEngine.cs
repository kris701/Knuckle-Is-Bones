using Knuckle.Is.Bones.Core.Engines.Actions;
using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.Core.Models.Game.MoveModules;

namespace Knuckle.Is.Bones.Core.Engines
{
	public class KnuckleBonesEngine : IKnuckleBonesEngine
	{
		public GameEventHandler? OnGameOver { get; set; }
		public GameEventHandler? OnOpponentDiceRemoved { get; set; }
		public GameEventHandler? OnCombo { get; set; }
		public GameEventHandler? OnTurn { get; set; }
		public GameBoardModifiedEventHandler? OnBoardModified { get; set; }

		public GameState State { get; }

		public KnuckleBonesEngine(GameState state)
		{
			State = state;
		}

		public bool Execute(IEngineAction action)
		{
			if (State.GameOver)
				return false;

			switch (action)
			{
				case SetCPUMoveAction act:
					return SetCPUMove();
				case SetCPUBoardModificationAction act:
					return SetCPUBoardModification();
				case SetPlayerMoveAction act:
					return SetPlayerMove(act.TargetColumn);
				case TurnAction act:
					return TakeTurn();
				case CheckGameStateAction act:
					return CheckGameOverState();
				default:
					throw new Exception("Unknown action!");
			}
		}

		private bool CheckGameOverState()
		{
			var board1 = State.GetCurrentOpponentBoard();
			var board2 = State.GetNextOpponentBoard();
			RemoveOpposites(board1, board2);
			if (board1.IsFull() || board2.IsFull())
			{
				OnGameOver?.Invoke();
				State.GameOver = true;
				var opponent1Value = State.GetFirstOpponentBoardValue();
				var opponent2Value = State.GetSecondOpponentBoardValue();
				if (opponent1Value > opponent2Value)
					State.Winner = State.FirstOpponent.MoveModule.OpponentID;
				else
					State.Winner = State.SecondOpponent.MoveModule.OpponentID;
				GameSaveHelpers.Save(State);
				return true;
			}
			return false;
		}

		private bool TakeTurn()
		{
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
			if (opponent.MoveModule.TargetColumn == -1)
				return false;
			var columnID = opponent.MoveModule.TargetColumn;
			if (columnID < 0 || columnID >= board.Columns.Count)
				return false;
			var column = board.Columns[columnID];
			if (column.Cells[column.Cells.Count - 1] != 0)
				return false;
			var lowest = column.Cells.IndexOf(0);
			if (column.Cells.Contains(State.CurrentDice.Value))
				OnCombo?.Invoke();
			column.Cells[lowest] = State.CurrentDice.Value;

			State.TurnIndex++;
			OnTurn?.Invoke();

			RemoveOpposites(board, board2);

			opponent.MoveModule.Reset();
			if (opponent.MoveModule is IBoardModifier oppMod1)
				oppMod1.Reset();
			opponent2.MoveModule.Reset();
			if (opponent2.MoveModule is IBoardModifier oppMod2)
				oppMod2.Reset();

			if (CheckGameOverState())
				return true;

			State.CurrentDice.RollValue();

			if (State.Turn == State.FirstOpponent.MoveModule.OpponentID)
				State.Turn = State.SecondOpponent.MoveModule.OpponentID;
			else
				State.Turn = State.FirstOpponent.MoveModule.OpponentID;

			GameSaveHelpers.Save(State);
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

		private bool SetPlayerMove(int target)
		{
			var current = State.GetCurrentOpponent();
			if (State.Turn != current.MoveModule.OpponentID)
				return false;
			if (State.GameOver || CheckGameOverState())
				return false;
			if (current.MoveModule.TargetColumn != -1)
				return false;

			if (current.MoveModule is PlayerMoveModule player)
			{
				player.SetTargetColumn(target);
				return true;
			}
			return false;
		}

		private bool SetCPUMove()
		{
			var current = State.GetCurrentOpponent();
			if (State.Turn != current.MoveModule.OpponentID)
				return false;
			if (State.GameOver || CheckGameOverState())
				return false;
			if (current.MoveModule.TargetColumn != -1)
				return false;

			if (current.MoveModule is ICPUMove cpu)
			{
				var currentBoard = State.GetCurrentOpponentBoard();
				var otherBoard = State.GetNextOpponentBoard();
				cpu.SetTargetColumn(State.CurrentDice, currentBoard, otherBoard, State.TurnIndex);
				return true;
			}
			return false;
		}

		private bool SetCPUBoardModification()
		{
			var current = State.GetCurrentOpponent();
			if (State.Turn != current.MoveModule.OpponentID)
				return false;
			if (State.GameOver || CheckGameOverState())
				return false;

			if (current.MoveModule is IBoardModifier board)
			{
				var other = State.GetNextCurrentOpponent();
				var currentBoard = State.GetCurrentOpponentBoard();
				var otherBoard = State.GetNextOpponentBoard();

				var modifications = board.ModifyBoards(State.CurrentDice, currentBoard, otherBoard, State.TurnIndex);
				foreach (var modification in modifications)
				{
					switch (modification)
					{
						case ModifyerType.Mine:
							OnBoardModified?.Invoke(current.MoveModule.OpponentID);
							break;
						case ModifyerType.Opponent:
							OnBoardModified?.Invoke(other.MoveModule.OpponentID);
							break;
					}
				}
				if (modifications.Count > 0)
					return true;
			}
			return false;
		}
	}
}