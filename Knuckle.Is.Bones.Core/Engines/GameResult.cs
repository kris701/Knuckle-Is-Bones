namespace Knuckle.Is.Bones.Core.Engines
{
	public class GameResult
	{
		public bool PlayerWon { get; set; }
		public bool HadPlayer { get; set; }
		public int PointsGained { get; set; }
		public string WinnerName { get; set; }
		public HashSet<Guid> CompletedItems { get; set; }
	}
}
