using KnuckleBones.OpenGL.Models;
using KnuckleBones.OpenGL.ResourcePacks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Audio;
using MonoGame.OpenGL.Formatter.BackgroundWorkers;
using MonoGame.OpenGL.Formatter.Fonts;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Textures;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace KnuckleBones.OpenGL
{
    public class KnuckleBoneWindow : Game, IWindow
    {
        private static readonly string _contentDir = "Content";
        private static readonly string _modsDir = "Mods";

        public float XScale { get; private set; } = 1;
        public float YScale { get; private set; } = 1;

        public GraphicsDeviceManager Device { get; }
        public IView CurrentScreen { get; set; }
        public List<IBackgroundWorker> BackroundWorkers { get; set; } = new List<IBackgroundWorker>();

        public AudioController Audio { get; private set; }
        public TextureController Textures { get; private set; }
        public FontController Fonts { get; private set; }
        public ResourcePackController ResourcePackController { get; private set; }

        public SettingsDefinition Settings { get; private set; }

        private readonly Func<KnuckleBoneWindow, IView> _screenToLoad;
        private SpriteBatch? _spriteBatch;
        private Matrix _scaleMatrix;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public KnuckleBoneWindow(Func<KnuckleBoneWindow, IView> screen) : base()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _scaleMatrix = Matrix.CreateScale(XScale, YScale, 1.0f);
            Device = new GraphicsDeviceManager(this);
            Content.RootDirectory = _contentDir;
            _screenToLoad = screen;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var thisVersion = Assembly.GetEntryAssembly()?.GetName().Version!;
            var thisVersionStr = $"v{thisVersion.Major}.{thisVersion.Minor}.{thisVersion.Build}";
            Window.Title = $"KnuckleBone {thisVersionStr}";

            Audio = new AudioController(Content);
            Textures = new TextureController(Content);
            Fonts = new FontController(Content);
            ResourcePackController = new ResourcePackController(this);

            if (File.Exists("settings.json"))
                Settings = JsonSerializer.Deserialize<SettingsDefinition>(File.ReadAllText("settings.json"));
            else
                Settings = new SettingsDefinition();

            BasicTextures.Initialize(GraphicsDevice);
            MediaPlayer.IsRepeating = true;
            SoundEffect.Initialize();
            BackroundWorkers = new List<IBackgroundWorker>();
            LoadMods();
            ApplySettings();

            foreach (var worker in BackroundWorkers)
                worker.Initialize();

            CurrentScreen = _screenToLoad(this);
            CurrentScreen.Initialize();
        }

        private void LoadMods()
        {
            if (!Directory.Exists(_modsDir))
                Directory.CreateDirectory(_modsDir);
            ResourcePackController.LoadMods(_modsDir);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
            foreach (var worker in BackroundWorkers)
                worker.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch!.Begin(transformMatrix: _scaleMatrix);
            CurrentScreen.Draw(gameTime, _spriteBatch);
            foreach (var worker in BackroundWorkers)
                worker.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void ApplySettings()
        {
            Device.PreferredBackBufferHeight = Settings.ScreenHeight;
            Device.PreferredBackBufferWidth = Settings.ScreenWidth;
            Device.SynchronizeWithVerticalRetrace = Settings.IsVsync;
            Device.HardwareModeSwitch = false;
            Device.IsFullScreen = Settings.IsFullscreen;
            if (Settings.IsFullscreen)
            {
                Device.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                Device.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            }
            ResourcePackController.LoadResourcePack(Settings.ResourcePack);
            MediaPlayer.Volume = Settings.MusicVolume;
            SoundEffect.MasterVolume = Settings.EffectsVolume;
            Device.ApplyChanges();
            XScale = Device.PreferredBackBufferWidth / (float)IWindow.BaseScreenSize.X;
            YScale = Device.PreferredBackBufferHeight / (float)IWindow.BaseScreenSize.Y;
            _scaleMatrix = Matrix.CreateScale(XScale, YScale, 1.0f);
        }
    }
}