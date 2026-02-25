using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game
{
	public class DiceDefinition : IPurchasable, IGenericClonable<DiceDefinition>
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<int> Options { get; set; }
		public int Value { get; set; } = 0;
		public bool IsPurchasable { get; set; }

		private readonly Random _rnd = new Random();

		[JsonConstructor]
		public DiceDefinition(Guid iD, string name, string description, List<int> options, int value, bool isPurchasable)
		{
			ID = iD;
			Name = name;
			Description = description;
			Options = options;
			Value = value;
			IsPurchasable = isPurchasable;

		}

		public DiceDefinition(DiceDefinition other)
		{
			ID = other.ID;
			Name = other.Name;
			Description = other.Description;
			Options = other.Options;
			Value = other.Value;
			IsPurchasable = other.IsPurchasable;
		}

		public DiceDefinition Clone() => new DiceDefinition(this);

		public void RollValue()
		{
			Value = Options[_rnd.Next(0, Options.Count)];
		}

		public int RollValueIndependent()
		{
			return Options[_rnd.Next(0, Options.Count)];
		}
	}
}