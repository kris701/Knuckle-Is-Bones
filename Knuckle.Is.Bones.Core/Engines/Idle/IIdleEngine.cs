using Knuckle.Is.Bones.Core.Models.Saves;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Engines.Idle
{
	public interface IIdleEngine
	{
		public void Update(UserSaveDefinition user);
	}
}
