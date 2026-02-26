namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public abstract class BaseMoveModule : IMoveModule
	{
		public Guid OpponentID { get; set; }
		public int TargetColumn { get; internal set; } = -1;

		public BaseMoveModule(Guid opponentID)
		{
			if (opponentID == Guid.Empty)
				OpponentID = Guid.NewGuid();
			else
				OpponentID = opponentID;
		}

		public abstract IMoveModule Clone();

		void IMoveModule.Reset() => TargetColumn = -1;
	}
}
