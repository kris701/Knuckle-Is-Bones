using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Models;
using Knuckle.Is.Bones.OpenGL.ResourcePacks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using Steamworks;
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
		public UserSaveDefinition<SettingsDefinition> User { get; private set; }

		private readonly Func<KnuckleBoneWindow, IView> _screenToLoad;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		public KnuckleBoneWindow(Func<KnuckleBoneWindow, IView> screen) : base("Knuckle Is Bones")
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		{
			Content.RootDirectory = _contentDir;
			_screenToLoad = screen;
			IsMouseVisible = true;

			if (File.Exists("user.json"))
				User = JsonSerializerHelpers.DeserializeOrDefault<UserSaveDefinition<SettingsDefinition>>(File.ReadAllText("user.json"), () => new UserSaveDefinition<SettingsDefinition>());
			else
				User = new UserSaveDefinition<SettingsDefinition>();

			ApplySettings();
		}

		protected override void Initialize()
		{
			base.Initialize();

			ResourcePackController = new ResourcePackController(this);

			BasicTextures.Initialize(Device.GraphicsDevice);
			MediaPlayer.IsRepeating = true;
			SoundEffect.Initialize();
			LoadMods();
			ResourcePackController.LoadResourcePack(User.UIData.ResourcePackID);

			SteamAPI.Init();

			AchievementHelper.UpdateAchievements(User);

			CurrentScreen = _screenToLoad(this);
			CurrentScreen.Initialize();
		}

		private void LoadMods()
		{
			if (!Directory.Exists(_modsDir))
				Directory.CreateDirectory(_modsDir);
			ResourcePackController.LoadMods(_modsDir);
		}

		public void ApplySettings()
		{
			Device.PreferredBackBufferWidth = User.UIData.ResolutionX;
			Device.PreferredBackBufferHeight = User.UIData.ResolutionY;
			Device.SynchronizeWithVerticalRetrace = User.UIData.IsVsync;
			Device.HardwareModeSwitch = false;
			Device.GraphicsProfile = GraphicsProfile.HiDef;
			Device.IsFullScreen = User.UIData.IsFullscreen;
			if (Device.IsFullScreen)
			{
				Device.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
				Device.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			}
			MediaPlayer.Volume = User.UIData.MusicVolume;
			SoundEffect.MasterVolume = User.UIData.EffectsVolume;
			Device.ApplyChanges();
			UpdateScale();
		}
	}
}