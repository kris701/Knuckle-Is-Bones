using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Engines.Game
{
	public class ResultPointsBreakdown
	{
		public double Multiplier { get; set; } = 1;
		public int Count { get; set; } = 1;

		public int Value { get; set; } = 0;

		public string Type { get; set; }

		public ResultPointsBreakdown(double multiplier, int count, string type)
		{
			Multiplier = multiplier;
			Count = count;
			Type = type;
		}

		public ResultPointsBreakdown(int value, string type)
		{
			Value = value;
			Type = type;
		}
	}
}
