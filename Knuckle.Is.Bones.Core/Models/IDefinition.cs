using FormMatter.Builders;

namespace Knuckle.Is.Bones.Core.Models
{
	public interface IDefinition : IIdentifiable
	{
		public string Name { get; set; }
		public string Description { get; set; }
	}
}