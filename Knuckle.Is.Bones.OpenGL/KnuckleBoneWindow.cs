using FormMatter.OpenGL;
using FormMatter.OpenGL.Helpers;
using FormMatter.OpenGL.Views;
using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Models;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Steamworks;
using System;
using System.IO;

namespace Knuckle.Is.Bones.OpenGL
{
	public class KnuckleBoneWindow : BaseWindow
	{
		private static readonly string _contentDir = "Content";
		private static readonly string _modsDir = "Mods";

		public UserSaveDefinition User { get; private set; }
		public SettingsDefinition Settings { get; set; }

		private readonly Func<KnuckleBoneWindow, IView> _screenToLoad;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		public KnuckleBoneWindow(Func<KnuckleBoneWindow, IView> screen) : base("Knuckle Is Bones")
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		{
			Content.RootDirectory = _contentDir;
			_screenToLoad = screen;
			IsMouseVisible = true;

			var user = UserSaveHelpers.LoadSaveFile();
			if (user != null)
				User = user;
			else
				User = new UserSaveDefinition();
			var settings = SettingsSaveHelpers.LoadSaveFile();
			if (settings != null)
				Settings = settings;
			else
				Settings = new SettingsDefinition();

			ApplySettings();
		}

		protected override void Initialize()
		{
			base.Initialize();

			BasicTextures.Initialize(Device.GraphicsDevice);
			MediaPlayer.IsRepeating = true;
			SoundEffect.Initialize();
			ResourcePacks.OnLoadMod += (f) => ResourceManager.ReloadResources();
			LoadMods();
			ResourcePacks.LoadResourcePack(Settings.ResourcePackID);

			SteamHelpers.IsInitialized = SteamAPI.Init();

			AchievementHelper.UpdateAchievements(User);

			CurrentScreen = _screenToLoad(this);
			CurrentScreen.Initialize();
		}

		private void LoadMods()
		{
			if (!Directory.Exists(_modsDir))
				Directory.CreateDirectory(_modsDir);
			ResourcePacks.LoadMods(_modsDir);
		}

		public void ApplySettings()
		{
			Device.PreferredBackBufferWidth = Settings.ResolutionX;
			Device.PreferredBackBufferHeight = Settings.ResolutionY;
			Device.SynchronizeWithVerticalRetrace = Settings.IsVsync;
			Device.HardwareModeSwitch = false;
			Device.GraphicsProfile = GraphicsProfile.HiDef;
			Device.IsFullScreen = Settings.IsFullscreen;
			if (Device.IsFullScreen)
			{
				Device.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
				Device.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			}
			MediaPlayer.Volume = Settings.MusicVolume;
			SoundEffect.MasterVolume = Settings.EffectsVolume;
			Device.ApplyChanges();
			UpdateScale();
		}
	}
}