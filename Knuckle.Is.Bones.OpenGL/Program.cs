using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using System;

namespace Knuckle.Is.Bones.OpenGL
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using (var mainWindow = new KnuckleBoneWindow((g) => new MainMenu(g)))
                mainWindow.Run();
        }
    }
}