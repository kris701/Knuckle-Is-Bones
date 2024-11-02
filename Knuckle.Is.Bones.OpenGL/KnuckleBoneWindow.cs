using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.OpenGL.ResourcePacks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.IO;
using ToolsSharp;

namespace Knuckle.Is.Bones.OpenGL
{
    public class KnuckleBoneWindow : BaseWindow
    {
        private static readonly string _contentDir = "Content";
        private static readonly string _modsDir = "Mods";

        public ResourcePackController ResourcePackController { get; private set; }
        public UserSaveDefinition User {  get; private set; }

        private readonly Func<KnuckleBoneWindow, IView> _screenToLoad;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public KnuckleBoneWindow(Func<KnuckleBoneWindow, IView> screen) : base("Knuckle Is Bones")
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Content.RootDirectory = _contentDir;
            _screenToLoad = screen;
            IsMouseVisible = true;

            if (File.Exists("user.json"))
                User = JsonSerializerHelpers.DeserializeOrDefault<UserSaveDefinition>(File.ReadAllText("user.json"), () => new UserSaveDefinition());
            else
                User = new UserSaveDefinition();

            Device.PreferredBackBufferWidth = 1068;
            Device.PreferredBackBufferHeight = 600;
            Device.ApplyChanges();
            UpdateScale();
        }

        protected override void Initialize()
        {
            base.Initialize();

            ResourcePackController = new ResourcePackController(this);

            BasicTextures.Initialize(Device.GraphicsDevice);
            MediaPlayer.IsRepeating = true;
            SoundEffect.Initialize();
            LoadMods();
            ResourcePackController.LoadResourcePack(new Guid("4f686e3a-9bd8-41cd-854c-17cca5fce01b"));

            CurrentScreen = _screenToLoad(this);
            CurrentScreen.Initialize();
        }

        private void LoadMods()
        {
            if (!Directory.Exists(_modsDir))
                Directory.CreateDirectory(_modsDir);
            ResourcePackController.LoadMods(_modsDir);
        }

        protected override void Update(GameTime tst)
        {
            if (IsActive)
                Console.WriteLine("aaa");
            base.Update(tst);
        }
    }
}