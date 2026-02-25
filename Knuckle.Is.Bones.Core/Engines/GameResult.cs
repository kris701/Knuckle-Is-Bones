namespace Knuckle.Is.Bones.Core.Engines
{
	public class GameResult
	{
		public bool PlayerWon { get; set; }
		public bool HadPlayer { get; set; }
		public int PointsGained { get; set; }
		public string WinnerName { get; set; }
		public HashSet<Guid> CompletedItems { get; set; }

		public GameResult(bool playerWon, bool hadPlayer, int pointsGained, string winnerName, HashSet<Guid> completedItems)
		{
			PlayerWon = playerWon;
			HadPlayer = hadPlayer;
			PointsGained = pointsGained;
			WinnerName = winnerName;
			CompletedItems = completedItems;
		}
	}
}
