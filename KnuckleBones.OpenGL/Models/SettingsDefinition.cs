using System;

namespace KnuckleBones.OpenGL.Models
{
    public class SettingsDefinition
    {
        public int ScreenWidth { get; set; } = 1280;
        public int ScreenHeight { get; set; } = 720;
        public bool IsFullscreen { get; set; } = false;
        public bool IsVsync { get; set; } = true;
        public float MusicVolume { get; set; } = 0.2f;
        public float EffectsVolume { get; set; } = 0.2f;
        public Guid ResourcePack { get; set; } = new Guid("4f686e3a-9bd8-41cd-854c-17cca5fce01b");
    }
}