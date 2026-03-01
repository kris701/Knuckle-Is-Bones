using Knuckle.Is.Bones.Core.Engines.Game.Actions;

namespace Knuckle.Is.Bones.Core.Engines.Game
{
	public delegate void GameEventHandler();
	public delegate void GameBoardModifiedEventHandler(Guid opponentId);

	public interface IGameEngine
	{
		public GameEventHandler? OnGameOver { get; set; }
		public GameEventHandler? OnOpponentDiceRemoved { get; set; }
		public GameEventHandler? OnCombo { get; set; }
		public GameEventHandler? OnTurn { get; set; }
		public GameBoardModifiedEventHandler? OnBoardModified { get; set; }

		public GameState State { get; }

		public bool Execute(IEngineAction action);
	}
}
