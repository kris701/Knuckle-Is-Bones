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
		public bool GameOver { get; internal set; }
		public Guid Winner { get; internal set; }
		public Guid Turn { get; internal set; }
		public int TurnIndex { get; internal set; }
		public OpponentDefinition FirstOpponent { get; internal set; }
		public BoardDefinition FirstOpponentBoard { get; internal set; }
		public OpponentDefinition SecondOpponent { get; internal set; }
		public BoardDefinition SecondOpponentBoard { get; internal set; }

		public DiceDefinition CurrentDice { get; internal set; }

		public UserSaveDefinition User { get; internal set; }

		private readonly Dictionary<int, double> _playerDiceValueMap;
		private readonly Dictionary<int, double> _playerDiceComboMap;
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
			_playerDiceComboMap = BuildPlayerDiceComboMultiplierMap();
		}

		public GameState(OpponentDefinition firstOpponent, BoardDefinition firstOpponentBoard, OpponentDefinition secondOpponent, BoardDefinition secondOpponentBoard, DiceDefinition currentDice, UserSaveDefinition user)
		{
			GameOver = false;
			Winner = Guid.Empty;
			TurnIndex = 0;
			FirstOpponent = firstOpponent;
			FirstOpponentBoard = firstOpponentBoard;
			SecondOpponent = secondOpponent;
			SecondOpponentBoard = secondOpponentBoard;
			CurrentDice = currentDice;
			User = user;

			Turn = SetRandomStartingPlayer();

			CurrentDice.RollValue();

			_blankDiceValueMap = BuildBlankDiceValueMultiplierMap();
			_playerDiceValueMap = BuildPlayerDiceValueMultiplierMap();
			_playerDiceComboMap = BuildPlayerDiceComboMultiplierMap();
		}

		public Guid SetRandomStartingPlayer()
		{
			var rnd = new Random();
			var targetOpponent = rnd.Next(0, 2);
			if (targetOpponent == 1)
				Turn = FirstOpponent.MoveModule.OpponentID;
			else
				Turn = SecondOpponent.MoveModule.OpponentID;
			return Turn;
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
			foreach (var purchaseId in User.PurchasedShopItems.Keys.Where(x => allShopItems.Contains(x)))
			{
				var item = ResourceManager.Shop.GetResource(purchaseId);
				foreach (var effect in item.Effects)
					if (effect is DiceMultiplierEffect eff && result.ContainsKey(eff.Number))
						for (int i = 0; i < User.PurchasedShopItems[purchaseId]; i++)
							result[eff.Number] *= eff.Multiplier;
			}

			return result;
		}

		private Dictionary<int, double> BuildPlayerDiceComboMultiplierMap()
		{
			var result = BuildBlankDiceValueMultiplierMap();

			var allShopItems = ResourceManager.Shop.GetResources();
			foreach (var purchaseId in User.PurchasedShopItems.Keys.Where(x => allShopItems.Contains(x)))
			{
				var item = ResourceManager.Shop.GetResource(purchaseId);
				foreach (var effect in item.Effects)
					if (effect is DiceComboMultiplierEffect eff && result.ContainsKey(eff.Number))
						for (int i = 0; i < User.PurchasedShopItems[purchaseId]; i++)
							result[eff.Number] *= eff.Multiplier;
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
				return FirstOpponentBoard.GetValue(_playerDiceValueMap, _playerDiceComboMap);
			else
				return FirstOpponentBoard.GetValue(_blankDiceValueMap, _blankDiceValueMap);
		}
		public int GetSecondOpponentBoardValue()
		{
			if (SecondOpponent.MoveModule is PlayerMoveModule)
				return SecondOpponentBoard.GetValue(_playerDiceValueMap, _playerDiceComboMap);
			else
				return SecondOpponentBoard.GetValue(_blankDiceValueMap, _blankDiceValueMap);
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
				pointsGained = GetPointsGained(GetFirstOpponentBoardValue(), SecondOpponent.Difficulty, FirstOpponentBoard.ID);
				completedItems.Add(SecondOpponent.ID);
				completedItems.Add(FirstOpponentBoard.ID);
				completedItems.Add(CurrentDice.ID);
			}
			if ((SecondOpponent.MoveModule is PlayerMoveModule) && (FirstOpponent.MoveModule is not PlayerMoveModule) && Winner == SecondOpponent.MoveModule.OpponentID)
			{
				playerWon = true;
				pointsGained = GetPointsGained(GetSecondOpponentBoardValue(), FirstOpponent.Difficulty, FirstOpponentBoard.ID);
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

		private int GetPointsGained(int boardValue, double opponentDifficulty, Guid boardId)
		{
			var value = (int)(boardValue * opponentDifficulty);

			var allShopItems = ResourceManager.Shop.GetResources();
			foreach (var purchaseId in User.PurchasedShopItems.Keys.Where(x => allShopItems.Contains(x)))
			{
				var item = ResourceManager.Shop.GetResource(purchaseId);
				foreach (var effect in item.Effects)
				{
					if (effect is PointsMultiplierEffect eff)
						for (int i = 0; i < User.PurchasedShopItems[purchaseId]; i++)
							value = (int)(value * eff.Multiplier);
					else if (effect is PointsBoardMultiplierEffect eff2 && eff2.BoardID == boardId)
						value = (int)(value * eff2.Multiplier);
				}
			}

			return value;
		}
	}
}