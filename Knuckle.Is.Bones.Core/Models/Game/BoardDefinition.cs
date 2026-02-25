using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game
{
	public class BoardDefinition : IPurchasable, IGenericClonable<BoardDefinition>
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<ColumnDefinition> Columns { get; set; }
		public bool IsPurchasable { get; set; }

		[JsonConstructor]
		public BoardDefinition(Guid iD, string name, string description, List<ColumnDefinition> columns, bool isPurchasable)
		{
			ID = iD;
			Name = name;
			Description = description;
			Columns = columns;
			IsPurchasable = isPurchasable;

		}

		public BoardDefinition(BoardDefinition other)
		{
			ID = other.ID;
			Name = other.Name;
			Description = other.Description;
			Columns = new List<ColumnDefinition>(other.Columns.Count);
			foreach (var col in other.Columns)
				Columns.Add(new ColumnDefinition(col));
			IsPurchasable = other.IsPurchasable;
		}

		internal int GetValue(Dictionary<int, double> multipliers)
		{
			int value = 0;
			foreach (var column in Columns)
				value += column.GetValue(multipliers);
			return value;
		}

		internal bool IsFull()
		{
			foreach (var column in Columns)
				if (!column.IsFull())
					return false;
			return true;
		}

		internal bool IsEmpty()
		{
			foreach (var column in Columns)
				if (!column.IsEmpty())
					return false;
			return true;
		}

		public BoardDefinition Clone() => new BoardDefinition(this);
	}
}