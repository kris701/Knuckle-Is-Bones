using Knuckle.Is.Bones.Core.Models;
using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.Core.Models.Saves;
using System.Text.Json.Serialization;

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

		[JsonConstructor]
		public GameState(Guid winner, Guid turn, int turnIndex, OpponentDefinition firstOpponent, BoardDefinition firstOpponentBoard, OpponentDefinition secondOpponent, BoardDefinition secondOpponentBoard, DiceDefinition currentDice, UserSaveDefinition user)
		{
			Winner = winner;
			Turn = turn;
			TurnIndex = turnIndex;
			FirstOpponent = firstOpponent;
			FirstOpponentBoard = firstOpponentBoard;
			SecondOpponent = secondOpponent;
			SecondOpponentBoard = secondOpponentBoard;
			CurrentDice = currentDice;
			User = user;
		}

		public GameState(OpponentDefinition firstOpponent, BoardDefinition firstOpponentBoard, OpponentDefinition secondOpponent, BoardDefinition secondOpponentBoard, DiceDefinition currentDice, UserSaveDefinition user)
		{
			Winner = Guid.Empty;
			Turn = Guid.Empty;
			TurnIndex = 0;
			FirstOpponent = firstOpponent;
			FirstOpponentBoard = firstOpponentBoard;
			SecondOpponent = secondOpponent;
			SecondOpponentBoard = secondOpponentBoard;
			CurrentDice = currentDice;
			User = user;
		}

		public GameState Clone() => new GameState(Winner, Turn, TurnIndex, FirstOpponent, FirstOpponentBoard, SecondOpponent, SecondOpponentBoard, CurrentDice, User);
	}
}