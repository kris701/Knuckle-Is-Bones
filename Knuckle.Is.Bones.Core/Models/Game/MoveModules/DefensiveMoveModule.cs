using Knuckle.Is.Bones.Core.Helpers;
using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public class DefensiveMoveModule : BaseMoveModule, ICPUMove
	{
		internal readonly Random _rnd = new Random();

		[JsonConstructor]
		public DefensiveMoveModule(Guid opponentID) : base(opponentID)
		{
		}

		void ICPUMove.SetTargetColumn(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex)
		{
			var orderedQueue = new PriorityQueue<int, int>();
			var index = 0;
			foreach (var column in opponentBoard.Columns)
			{
				if (column.Cells.Any(x => x == diceValue.Value) && !myBoard.Columns[index].IsFull())
					orderedQueue.Enqueue(index, column.Cells.Count - column.Cells.Count(x => x == diceValue.Value));
				index++;
			}
			if (orderedQueue.Count > 0)
			{
				TargetColumn = orderedQueue.Dequeue();
				return;
			}

			var target = MoveHelpers.GetRandomFreeColumn(myBoard);
			if (target is int targetAct)
				TargetColumn = targetAct;
		}

		public override IMoveModule Clone() => new DefensiveMoveModule(OpponentID);
	}
}