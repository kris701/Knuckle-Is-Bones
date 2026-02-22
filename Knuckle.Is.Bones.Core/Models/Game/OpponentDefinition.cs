using Knuckle.Is.Bones.Core.Models.Game.OpponentModules;
using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game
{
	public class OpponentDefinition : IUnlockable, IGenericClonable<OpponentDefinition>
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public IOpponentModule Module { get; set; }
		public double Difficulty { get; set; }
		public int RequiredPoints { get; set; }

		[JsonConstructor]
		public OpponentDefinition(Guid iD, string name, string description, IOpponentModule module, double difficulty, int requiredPoints)
		{
			ID = iD;
			Name = name;
			Description = description;
			Module = module;
			Difficulty = difficulty;
			RequiredPoints = requiredPoints;
		}

		public OpponentDefinition(OpponentDefinition other)
		{
			ID = other.ID;
			Name = other.Name;
			Description = other.Description;
			if (other.Module.Clone() is IOpponentModule module)
				Module = module;
			else
				throw new ArgumentException("Could not clone module!");
			Difficulty = other.Difficulty;
			RequiredPoints = other.RequiredPoints;
		}

		public OpponentDefinition Clone() => new OpponentDefinition(this);
	}
}