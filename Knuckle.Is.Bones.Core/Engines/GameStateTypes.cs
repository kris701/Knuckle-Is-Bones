using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Engines
{
	public enum GameStateTypes
	{
		None,
		AwaitingPlayer,
		GameOver,
		AwaitingCPUBoardModification,
		AwaitingCPUMove
	}
}
