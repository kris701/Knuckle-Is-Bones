using Knuckle.Is.Bones.Core.Models.Game.MoveModules;
using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game
{
	public class OpponentDefinition : IPurchasable, IGenericClonable<OpponentDefinition>
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public IMoveModule MoveModule { get; set; }
		public double Difficulty { get; set; }
		public bool IsPurchasable { get; set; }

		[JsonConstructor]
		public OpponentDefinition(Guid iD, string name, string description, IMoveModule moveModule, double difficulty, bool isPurchasable)
		{
			ID = iD;
			Name = name;
			Description = description;
			MoveModule = moveModule;
			Difficulty = difficulty;
			IsPurchasable = isPurchasable;
		}

		public OpponentDefinition(OpponentDefinition other)
		{
			ID = other.ID;
			Name = other.Name;
			Description = other.Description;
			if (other.MoveModule.Clone() is IMoveModule module)
				MoveModule = module;
			else
				throw new ArgumentException("Could not clone module!");
			Difficulty = other.Difficulty;
			IsPurchasable = other.IsPurchasable;
		}

		public OpponentDefinition Clone() => new OpponentDefinition(this);
	}
}