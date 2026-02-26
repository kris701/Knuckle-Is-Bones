using Knuckle.Is.Bones.Core.Helpers;
using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public class ComboCheaterMoveModule : ComboMoveModule, IBoardModifier
	{
		public bool HasModified { get; private set; }
		private int _nextTargetTurn = 5;

		[JsonConstructor]
		public ComboCheaterMoveModule(Guid opponentID) : base(opponentID)
		{
		}

		List<ModifyerType> IBoardModifier.ModifyBoards(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex)
		{
			if (HasModified)
				return new List<ModifyerType>();
			if (turnIndex >= _nextTargetTurn)
			{
				if (opponentBoard.IsEmpty())
					return new List<ModifyerType>();
				if (diceValue.Options.Count <= 1)
					return new List<ModifyerType>();

				_nextTargetTurn = _rnd.Next(turnIndex + 4, turnIndex + 7);

				var checkOrder = MoveHelpers.GetRandomColumnOrder(myBoard);
				foreach (var col in checkOrder)
				{
					if (myBoard.Columns[col].IsFull())
						continue;
					foreach (var dice in diceValue.Options)
					{
						var count = myBoard.Columns[col].Cells.Count(x => x == dice);
						if (count >= 1)
						{
							for (int row = 0; row < myBoard.Columns[col].Cells.Count; row++)
							{
								if (myBoard.Columns[col].Cells[row] == 0)
								{
									myBoard.Columns[col].Cells[row] = dice;
									break;
								}
							}
							return new List<ModifyerType>() { ModifyerType.Mine };
						}
					}
				}
				HasModified = true;
				return new List<ModifyerType>();
			}
			return new List<ModifyerType>();
		}
		void IBoardModifier.Reset() => HasModified = false;

		public override IMoveModule Clone() => new ComboCheaterMoveModule(OpponentID);
	}
}