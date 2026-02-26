using Knuckle.Is.Bones.Core.Helpers;
using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public class RandomMoveModule : BaseMoveModule, ICPUMove
	{
		internal readonly Random _rnd = new Random();

		[JsonConstructor]
		public RandomMoveModule(Guid opponentID) : base(opponentID)
		{
		}

		void ICPUMove.SetTargetColumn(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex)
		{
			var target = MoveHelpers.GetRandomFreeColumn(myBoard);
			if (target is int targetAct)
				TargetColumn = targetAct;
		}

		public override IMoveModule Clone() => new RandomMoveModule(OpponentID);
	}
}