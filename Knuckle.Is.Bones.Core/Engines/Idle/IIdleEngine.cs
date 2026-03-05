using Knuckle.Is.Bones.Core.Models.Saves;

namespace Knuckle.Is.Bones.Core.Engines.Idle
{
	public interface IIdleEngine
	{
		public void Update(UserSaveDefinition user);
	}
}
