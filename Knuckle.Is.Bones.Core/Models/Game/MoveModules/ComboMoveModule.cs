using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public class ComboMoveModule : IMoveModule, ICPUMove
	{
		public Guid OpponentID { get; set; } = Guid.NewGuid();

		private readonly Random _rnd = new Random();
		private int _targetColumn = 0;

		[JsonConstructor]
		public ComboMoveModule(Guid opponentID)
		{
			OpponentID = opponentID;
		}

		void ICPUMove.SetTargetColumn(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex)
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
				_targetColumn = orderedQueue.Dequeue();
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
			_targetColumn = target;
		}

		public int GetTargetColumn() => _targetColumn;

		public IMoveModule Clone() => new ComboMoveModule(OpponentID);
	}
}