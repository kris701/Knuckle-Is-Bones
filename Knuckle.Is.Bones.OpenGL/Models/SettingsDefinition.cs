using Knuckle.Is.Bones.Core.Models;

namespace Knuckle.Is.Bones.OpenGL.Models
{
	public class SettingsDefinition : IGenericClonable<SettingsDefinition>
	{
		public int ResolutionX { get; set; } = 1920;
		public int ResolutionY { get; set; } = 1080;
		public bool IsFullscreen { get; set; } = false;
		public bool IsVsync { get; set; } = true;
		public float MusicVolume { get; set; } = 0.2f;
		public float EffectsVolume { get; set; } = 0.2f;

		public SettingsDefinition Clone() => new SettingsDefinition()
		{
			ResolutionX = ResolutionX,
			ResolutionY = ResolutionY,
			IsFullscreen = IsFullscreen,
			IsVsync = IsVsync,
			MusicVolume = MusicVolume,
			EffectsVolume = EffectsVolume
		};
	}
}
