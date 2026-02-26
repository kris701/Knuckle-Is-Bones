using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public class RandomMoveModule : BaseMoveModule, ICPUMove, IInternalCPUMove
	{
		internal readonly Random _rnd = new Random();

		[JsonConstructor]
		public RandomMoveModule(Guid opponentID) : base(opponentID)
		{
		}

		void IInternalCPUMove.SetTargetColumn(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex)
		{
			int target = -1;
			bool valid = false;
			while (!valid)
			{
				target = _rnd.Next(0, myBoard.Columns.Count);
				if (!myBoard.Columns[target].IsFull())
					valid = true;
			}

			TargetColumn = target;
		}

		public override IMoveModule Clone() => new RandomMoveModule(OpponentID);
	}
}