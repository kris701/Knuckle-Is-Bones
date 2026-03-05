namespace Knuckle.Is.Bones.Core.Models.Saves
{
	public class LastGameSetupModel
	{
		public enum LastGameSetupType
		{
			None,
			PvE,
			PvP,
			EvE
		}

		public Guid BoardID { get; set; }
		public Guid DiceID { get; set; }
		public Guid FirstOpponentID { get; set; }
		public Guid SecondOpponentID { get; set; }
	}
}
