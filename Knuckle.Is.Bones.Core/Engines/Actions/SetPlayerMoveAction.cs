using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Engines.Actions
{
	public class SetPlayerMoveAction : IEngineAction
	{
		public int TargetColumn { get; set; }

		public SetPlayerMoveAction(int targetColumn)
		{
			TargetColumn = targetColumn;
		}
	}
}
