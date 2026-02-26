using Knuckle.Is.Bones.Core.Models;
using System;

namespace Knuckle.Is.Bones.OpenGL.Models
{
	public enum GameSpeedTypes { Turtle, Slow, Normal, Fast, Speedy}

	public class SettingsDefinition : IGenericClonable<SettingsDefinition>
	{
		public int ResolutionX { get; set; } = 1920;
		public int ResolutionY { get; set; } = 1080;
		public bool IsFullscreen { get; set; } = true;
		public bool IsVsync { get; set; } = true;
		public float MusicVolume { get; set; } = 0.2f;
		public float EffectsVolume { get; set; } = 0.2f;
		public GameSpeedTypes GameSpeed { get; set; } = GameSpeedTypes.Normal;
		public Guid ResourcePackID { get; set; } = new Guid("4f686e3a-9bd8-41cd-854c-17cca5fce01b");

		public SettingsDefinition Clone() => new SettingsDefinition()
		{
			ResolutionX = ResolutionX,
			ResolutionY = ResolutionY,
			IsFullscreen = IsFullscreen,
			IsVsync = IsVsync,
			MusicVolume = MusicVolume,
			EffectsVolume = EffectsVolume,
			ResourcePackID = ResourcePackID,
			GameSpeed = GameSpeed
		};

		public override bool Equals(object? obj)
		{
			return obj is SettingsDefinition definition &&
				   ResolutionX == definition.ResolutionX &&
				   ResolutionY == definition.ResolutionY &&
				   IsFullscreen == definition.IsFullscreen &&
				   IsVsync == definition.IsVsync &&
				   MusicVolume == definition.MusicVolume &&
				   EffectsVolume == definition.EffectsVolume &&
				   GameSpeed == definition.GameSpeed &&
				   ResourcePackID.Equals(definition.ResourcePackID);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(ResolutionX, ResolutionY, IsFullscreen, IsVsync, MusicVolume, EffectsVolume, GameSpeed, ResourcePackID);
		}
	}
}
