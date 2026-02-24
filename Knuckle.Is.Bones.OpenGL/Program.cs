using Knuckle.Is.Bones.OpenGL.Views.SplashScreenView;
using System;

namespace Knuckle.Is.Bones.OpenGL
{
	public static class Program
	{
		[STAThread]
		private static void Main()
		{
			using (var mainWindow = new KnuckleBoneWindow((g) => new SplashScreen(g)))
				mainWindow.Run();
		}
	}
}