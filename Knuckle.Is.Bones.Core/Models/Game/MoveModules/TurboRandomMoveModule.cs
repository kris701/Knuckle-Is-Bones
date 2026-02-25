using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public class TurboRandomMoveModule : RandomMoveModule, IBoardModifier
	{
		private int _nextTargetTurn = 3;

		[JsonConstructor]
		public TurboRandomMoveModule(Guid opponentID) : base(opponentID)
		{
		}

		bool IBoardModifier.ModifyBoards(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex)
		{
			if (turnIndex >= _nextTargetTurn)
			{
				if (opponentBoard.IsEmpty())
					return false;
				if (diceValue.Sides <= 1)
					return false;

				_nextTargetTurn = _rnd.Next(turnIndex + 1, turnIndex + 7);
				int targetCol = -1;
				int targetRow = -1;
				bool valid = false;
				while (!valid)
				{
					targetCol = _rnd.Next(0, opponentBoard.Columns.Count);
					targetRow = _rnd.Next(0, opponentBoard.Columns[targetCol].Cells.Count);
					if (opponentBoard.Columns[targetCol].Cells[targetRow] != 0)
						valid = true;
				}

				var targetValue = diceValue.RollValueIndependent();
				while(targetValue == opponentBoard.Columns[targetCol].Cells[targetRow])
					targetValue = diceValue.RollValueIndependent();
				opponentBoard.Columns[targetCol].Cells[targetRow] = targetValue;
				return true;
			}
			return false;
		}

		public override IMoveModule Clone() => new TurboRandomMoveModule(OpponentID);
	}
}