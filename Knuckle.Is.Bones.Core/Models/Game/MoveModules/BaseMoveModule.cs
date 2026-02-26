using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public abstract class BaseMoveModule : IMoveModule
	{
		public Guid OpponentID { get; set; } = Guid.NewGuid();
		public int TargetColumn { get; internal set; } = -1;

		public BaseMoveModule(Guid opponentID)
		{
			OpponentID = opponentID;
		}

		public abstract IMoveModule Clone();

		public void ClearTarget() => TargetColumn = -1;
	}
}
