using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.OpponentModules
{
	public class ComboOpponentModule : IOpponentModule
	{
		public Guid OpponentID { get; set; } = Guid.NewGuid();

		private readonly Random _rnd = new Random();
		private int _targetColumn = 0;

		[JsonConstructor]
		public ComboOpponentModule(Guid opponentID)
		{
			OpponentID = opponentID;
		}

		public void SetTargetColumn(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard)
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

		public IOpponentModule Clone() => new ComboOpponentModule(OpponentID);
	}
}