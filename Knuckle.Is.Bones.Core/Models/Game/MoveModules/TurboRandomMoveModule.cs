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

		List<ModifyerType> IBoardModifier.ModifyBoards(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex)
		{
			if (turnIndex >= _nextTargetTurn)
			{
				if (opponentBoard.IsEmpty())
					return new List<ModifyerType>();
				if (diceValue.Options.Count <= 1)
					return new List<ModifyerType>();

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
				return new List<ModifyerType>() { ModifyerType.Opponent };
			}
			return new List<ModifyerType>();
		}

		public override IMoveModule Clone() => new TurboRandomMoveModule(OpponentID);
	}
}