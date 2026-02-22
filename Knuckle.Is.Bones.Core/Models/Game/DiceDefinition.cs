using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game
{
	public class DiceDefinition : IUnlockable, IGenericClonable<DiceDefinition>
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Sides { get; set; }
		public int Value { get; set; } = 0;
		public int RequiredPoints { get; set; }

		private readonly Random _rnd = new Random();

		[JsonConstructor]
		public DiceDefinition(Guid iD, string name, string description, int sides, int value, int requiredPoints)
		{
			ID = iD;
			Name = name;
			Description = description;
			Sides = sides;
			Value = value;
			RequiredPoints = requiredPoints;
		}

		public DiceDefinition(DiceDefinition other)
		{
			ID = other.ID;
			Name = other.Name;
			Description = other.Description;
			Sides = other.Sides;
			Value = other.Value;
			RequiredPoints = other.RequiredPoints;
		}

		public DiceDefinition Clone() => new DiceDefinition(this);

		public void RollValue()
		{
			Value = _rnd.Next(1, Sides + 1);
		}
	}
}