using Knuckle.Is.Bones.Core.Models.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Helpers
{
	internal static class MoveHelpers
	{
		public static List<int> GetRandomColumnOrder(BoardDefinition board)
		{
			var checkOrder = new List<int>();
			for (int i = 0; i < board.Columns.Count; i++)
				checkOrder.Add(i);
			checkOrder = checkOrder.Shuffle().ToList();
			return checkOrder;
		}

		public static int? GetRandomFreeColumn(BoardDefinition board)
		{
			var checkOrder = GetRandomColumnOrder(board);

			foreach (var check in checkOrder)
			{
				if (!board.Columns[check].IsFull())
					return check;
			}
			return null;
		}
	}
}
