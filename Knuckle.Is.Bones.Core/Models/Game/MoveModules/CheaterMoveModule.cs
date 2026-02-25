using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public class CheaterMoveModule : DefensiveMoveModule, IBoardModifier
	{
		private int _nextTargetTurn = 5;

		[JsonConstructor]
		public CheaterMoveModule(Guid opponentID) : base(opponentID)
		{
		}

		List<ModifyerType> IBoardModifier.ModifyBoards(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex)
		{
			if (turnIndex >= _nextTargetTurn)
			{
				if (opponentBoard.IsEmpty())
					return new List<ModifyerType>();
				if (diceValue.Sides <= 1)
					return new List<ModifyerType>();

				_nextTargetTurn = _rnd.Next(turnIndex + 4, turnIndex + 7);

				var checkOrder = new List<int>();
				for (int col = 0; col < myBoard.Columns.Count; col++)
					if (!myBoard.Columns[col].IsFull())
						checkOrder.Add(col);
				if (checkOrder.Count == 0)
					return new List<ModifyerType>();
				checkOrder = checkOrder.Shuffle().ToList();
				
				foreach(var col in checkOrder)
				{
					for(int dice = 1; dice <= diceValue.Sides; dice++)
					{
						var count = myBoard.Columns[col].Cells.Count(x => x == dice);
						if (count >= 1)
						{
							for(int row = 0; row < myBoard.Columns[col].Cells.Count; row++)
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
				return new List<ModifyerType>();
			}
			return new List<ModifyerType>();
		}

		public override IMoveModule Clone() => new CheaterMoveModule(OpponentID);
	}
}