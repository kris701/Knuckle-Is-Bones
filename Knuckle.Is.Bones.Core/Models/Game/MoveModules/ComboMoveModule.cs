using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public class ComboMoveModule : BaseMoveModule, ICPUMove, IInternalCPUMove
	{
		internal readonly Random _rnd = new Random();

		[JsonConstructor]
		public ComboMoveModule(Guid opponentID) : base(opponentID)
		{
		}

		void IInternalCPUMove.SetTargetColumn(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex)
		{
			var orderedQueue = new PriorityQueue<int, int>();
			var index = 0;
			foreach (var column in myBoard.Columns)
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

		public override IMoveModule Clone() => new ComboMoveModule(OpponentID);
	}
}