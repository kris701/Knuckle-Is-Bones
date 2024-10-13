using KnuckleBones.OpenGL.Views.MainGameView;
using System;

namespace KnuckleBones.OpenGL
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using (var mainWindow = new KnuckleBoneWindow((g) => new MainGame(g)))
                mainWindow.Run();
        }
    }
}