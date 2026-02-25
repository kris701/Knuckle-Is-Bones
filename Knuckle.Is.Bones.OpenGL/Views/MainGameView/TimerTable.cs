using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.MainGameView
{
	public static class TimerTable
	{
#if DEBUG
		public static int RollTimer = 5;
		public static int RollWaitTimer = 5;
		public static int ModifyWaitTimer = 5;
		public static int SelectWaitTimer = 5;
		public static int PointsGainedTimer = 5;
#else
		public static int RollTimer = 150;
		public static int RollWaitTimer = 500;
		public static int ModifyWaitTimer = 1000;
		public static int SelectWaitTimer = 1000;
		public static int PointsGainedTimer = 100;
#endif
	}
}
