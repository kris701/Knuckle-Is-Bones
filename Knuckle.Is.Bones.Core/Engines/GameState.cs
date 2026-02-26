using Knuckle.Is.Bones.Core.Models;
using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.Core.Models.Game.MoveModules;
using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects;
using Knuckle.Is.Bones.Core.Resources;
using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Engines
{
	public class GameState : IGenericClonable<GameState>
	{
		public bool GameOver { get; set; }
		public Guid Winner { get; set; }
		public Guid Turn { get; set; }
		public int TurnIndex { get; set; }
		public OpponentDefinition FirstOpponent { get; set; }
		public BoardDefinition FirstOpponentBoard { get; set; }
		public OpponentDefinition SecondOpponent { get; set; }
		public BoardDefinition SecondOpponentBoard { get; set; }

		public DiceDefinition CurrentDice { get; set; }

		public UserSaveDefinition User { get; set; }

		private readonly Dictionary<int, double> _playerDiceValueMap;
		private readonly Dictionary<int, double> _blankDiceValueMap;

		[JsonConstructor]
		public GameState(bool gameOver, Guid winner, Guid turn, int turnIndex, OpponentDefinition firstOpponent, BoardDefinition firstOpponentBoard, OpponentDefinition secondOpponent, BoardDefinition secondOpponentBoard, DiceDefinition currentDice, UserSaveDefinition user)
		{
			GameOver = gameOver;
			Winner = winner;
			Turn = turn;
			TurnIndex = turnIndex;
			FirstOpponent = firstOpponent;
			FirstOpponentBoard = firstOpponentBoard;
			SecondOpponent = secondOpponent;
			SecondOpponentBoard = secondOpponentBoard;
			CurrentDice = currentDice;
			User = user;

			_blankDiceValueMap = BuildBlankDiceValueMultiplierMap();
			_playerDiceValueMap = BuildPlayerDiceValueMultiplierMap();
		}

		public GameState(OpponentDefinition firstOpponent, BoardDefinition firstOpponentBoard, OpponentDefinition secondOpponent, BoardDefinition secondOpponentBoard, DiceDefinition currentDice, UserSaveDefinition user)
		{
			GameOver = false;
			Winner = Guid.Empty;
			Turn = Guid.Empty;
			TurnIndex = 0;
			FirstOpponent = firstOpponent;
			FirstOpponentBoard = firstOpponentBoard;
			SecondOpponent = secondOpponent;
			SecondOpponentBoard = secondOpponentBoard;
			CurrentDice = currentDice;
			User = user;

			_blankDiceValueMap = BuildBlankDiceValueMultiplierMap();
			_playerDiceValueMap = BuildPlayerDiceValueMultiplierMap();
		}

		private Dictionary<int, double> BuildBlankDiceValueMultiplierMap()
		{
			var result = new Dictionary<int, double>();
			foreach (var value in CurrentDice.Options)
				result.Add(value, 1);

			return result;
		}

		private Dictionary<int, double> BuildPlayerDiceValueMultiplierMap()
		{
			var result = BuildBlankDiceValueMultiplierMap();

			var allShopItems = ResourceManager.Shop.GetResources();
			foreach (var purchaseId in User.PurchasedShopItems.Where(x => allShopItems.Contains(x)))
			{
				var item = ResourceManager.Shop.GetResource(purchaseId);
				foreach (var effect in item.Effects)
					if (effect is DiceMultiplierEffect eff && result.ContainsKey(eff.Number))
						result[eff.Number] = eff.Multiplier;
			}

			return result;
		}

		public GameState Clone() => new GameState(GameOver, Winner, Turn, TurnIndex, FirstOpponent.Clone(), FirstOpponentBoard.Clone(), SecondOpponent.Clone(), SecondOpponentBoard.Clone(), CurrentDice.Clone(), User.Clone());

		public OpponentDefinition GetCurrentOpponent()
		{
			if (Turn == FirstOpponent.MoveModule.OpponentID)
				return FirstOpponent;
			return SecondOpponent;
		}

		public OpponentDefinition GetNextCurrentOpponent()
		{
			if (Turn != FirstOpponent.MoveModule.OpponentID)
				return FirstOpponent;
			return SecondOpponent;
		}

		public BoardDefinition GetCurrentOpponentBoard()
		{
			if (Turn == FirstOpponent.MoveModule.OpponentID)
				return FirstOpponentBoard;
			return SecondOpponentBoard;
		}

		public BoardDefinition GetNextOpponentBoard()
		{
			if (Turn != FirstOpponent.MoveModule.OpponentID)
				return FirstOpponentBoard;
			return SecondOpponentBoard;
		}

		public int GetFirstOpponentBoardValue()
		{
			if (FirstOpponent.MoveModule is PlayerMoveModule)
				return FirstOpponentBoard.GetValue(_playerDiceValueMap);
			else
				return FirstOpponentBoard.GetValue(_blankDiceValueMap);
		}
		public int GetSecondOpponentBoardValue()
		{
			if (SecondOpponent.MoveModule is PlayerMoveModule)
				return SecondOpponentBoard.GetValue(_playerDiceValueMap);
			else
				return SecondOpponentBoard.GetValue(_blankDiceValueMap);
		}

		public GameResult GetGameResult()
		{
			if (!GameOver)
				throw new Exception("Game is not over yet!");

			var playerWon = false;
			var hadPlayer = false;
			var pointsGained = 0;
			var completedItems = new HashSet<Guid>();
			var winnerName = "";
			if ((FirstOpponent.MoveModule is PlayerMoveModule) && (SecondOpponent.MoveModule is not PlayerMoveModule) && Winner == FirstOpponent.MoveModule.OpponentID)
			{
				playerWon = true;
				pointsGained = GetPointsGained(GetFirstOpponentBoardValue(), SecondOpponent.Difficulty);
				completedItems.Add(SecondOpponent.ID);
				completedItems.Add(FirstOpponentBoard.ID);
				completedItems.Add(CurrentDice.ID);
			}
			if ((SecondOpponent.MoveModule is PlayerMoveModule) && (FirstOpponent.MoveModule is not PlayerMoveModule) && Winner == SecondOpponent.MoveModule.OpponentID)
			{
				playerWon = true;
				pointsGained = GetPointsGained(GetSecondOpponentBoardValue(), FirstOpponent.Difficulty);
				completedItems.Add(FirstOpponent.ID);
				completedItems.Add(FirstOpponentBoard.ID);
				completedItems.Add(CurrentDice.ID);
			}
			if ((FirstOpponent.MoveModule is PlayerMoveModule) && (SecondOpponent.MoveModule is not PlayerMoveModule))
				hadPlayer = true;
			if ((SecondOpponent.MoveModule is PlayerMoveModule) && (FirstOpponent.MoveModule is not PlayerMoveModule))
				hadPlayer = true;

			if (Winner == FirstOpponent.MoveModule.OpponentID)
				winnerName = $"{FirstOpponent.Name}";
			else
				winnerName = $"{SecondOpponent.Name}";

			return new GameResult(playerWon, hadPlayer, pointsGained, winnerName, completedItems);
		}

		private int GetPointsGained(int boardValue, double opponentDifficulty)
		{
			var value = (int)(boardValue * opponentDifficulty);

			var allShopItems = ResourceManager.Shop.GetResources();
			foreach (var purchaseId in User.PurchasedShopItems.Where(x => allShopItems.Contains(x)))
			{
				var item = ResourceManager.Shop.GetResource(purchaseId);
				foreach (var effect in item.Effects)
					if (effect is PointsMultiplierEffect eff)
						value = (int)(value * eff.Multiplier);
			}

			return value;
		}
	}
}