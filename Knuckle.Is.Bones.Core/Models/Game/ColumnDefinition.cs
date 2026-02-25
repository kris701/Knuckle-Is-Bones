using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game
{
	public class ColumnDefinition : IGenericClonable<ColumnDefinition>
	{
		public List<int> Cells { get; set; }

		[JsonConstructor]
		public ColumnDefinition(List<int> cells)
		{
			Cells = cells;
		}

		public ColumnDefinition(ColumnDefinition other)
		{
			Cells = new List<int>(other.Cells.Count);
			foreach (var cell in other.Cells)
				Cells.Add(cell);
		}

		internal int GetValue(Dictionary<int, double> multipliers)
		{
			int value = 0;
			for (int i = 0; i < Cells.Count; i++)
				if (Cells[i] != 0)
					value += (int)((Cells[i] * Cells.Count(x => x == Cells[i])) * multipliers[Cells[i]]);
			return value;
		}

		internal bool IsFull()
		{
			foreach (var cell in Cells)
				if (cell == 0)
					return false;
			return true;
		}

		internal bool IsEmpty()
		{
			foreach (var cell in Cells)
				if (cell != 0)
					return false;
			return true;
		}

		internal int EmptyCount()
		{
			var count = 0;
			foreach (var cell in Cells)
				if (cell != 0)
					count++;
			return count;
		}

		public ColumnDefinition Clone() => new ColumnDefinition(this);
	}
}