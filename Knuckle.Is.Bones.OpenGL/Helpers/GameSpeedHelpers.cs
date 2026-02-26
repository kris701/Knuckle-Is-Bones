using Knuckle.Is.Bones.OpenGL.Models;

namespace Knuckle.Is.Bones.OpenGL.Helpers
{
	public static class GameSpeedHelpers
	{
		public static GameSpeeds GetGameSpeed(GameSpeedTypes target)
		{
			switch (target)
			{
				case GameSpeedTypes.Speedy:
					return new GameSpeeds()
					{
						RollTimer = 5,
						RollWaitTimer = 5,
						ModifyWaitTimer = 5,
						SelectWaitTimer = 5,
						PointsGainedTimer = 5
					};
				case GameSpeedTypes.Fast:
					return new GameSpeeds()
					{
						RollTimer = 50,
						RollWaitTimer = 50,
						ModifyWaitTimer = 100,
						SelectWaitTimer = 100,
						PointsGainedTimer = 10
					};
				case GameSpeedTypes.Normal:
					return new GameSpeeds()
					{
						RollTimer = 150,
						RollWaitTimer = 500,
						ModifyWaitTimer = 1000,
						SelectWaitTimer = 1000,
						PointsGainedTimer = 100
					};
				case GameSpeedTypes.Slow:
					return new GameSpeeds()
					{
						RollTimer = 300,
						RollWaitTimer = 1000,
						ModifyWaitTimer = 2000,
						SelectWaitTimer = 2000,
						PointsGainedTimer = 200
					};
				case GameSpeedTypes.Turtle:
					return new GameSpeeds()
					{
						RollTimer = 1000,
						RollWaitTimer = 1500,
						ModifyWaitTimer = 3000,
						SelectWaitTimer = 3000,
						PointsGainedTimer = 1000
					};
				default:
					throw new System.Exception("Unknown game speed!");
			}
		}
	}
}
