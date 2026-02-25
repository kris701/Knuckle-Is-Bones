using Knuckle.Is.Bones.Core.Models;
using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.Core.Models.Saves;

namespace Knuckle.Is.Bones.Core.Engines
{
	public class GameState : IGenericClonable<GameState>
	{
		public Guid Winner { get; set; }
		public Guid Turn { get; set; }
		public int TurnIndex { get; set; }
		public OpponentDefinition FirstOpponent { get; set; }
		public BoardDefinition FirstOpponentBoard { get; set; }
		public OpponentDefinition SecondOpponent { get; set; }
		public BoardDefinition SecondOpponentBoard { get; set; }

		public DiceDefinition CurrentDice { get; set; }

		public UserSaveDefinition User { get; set; }

		public GameState Clone() => new GameState()
		{
			Winner = Winner,
			Turn = Turn,
			TurnIndex = TurnIndex,
			FirstOpponent = FirstOpponent.Clone(),
			FirstOpponentBoard = FirstOpponentBoard.Clone(),
			SecondOpponent = SecondOpponent.Clone(),
			SecondOpponentBoard = SecondOpponentBoard.Clone(),
			CurrentDice = CurrentDice.Clone(),
			User = User.Clone()
		};
	}
}