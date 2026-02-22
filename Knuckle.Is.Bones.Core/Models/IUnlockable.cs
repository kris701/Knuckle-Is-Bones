namespace Knuckle.Is.Bones.Core.Models
{
	public interface IUnlockable : IDefinition
	{
		public int RequiredPoints { get; set; }
	}
}