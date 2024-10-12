using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnuckleBone.Core.Models
{
    public class SettingsDefinition
    {
        public int ScreenWidth { get; set; } = 1280;
        public int ScreenHeight { get; set; } = 720;
        public bool IsFullscreen { get; set; } = false;
        public bool IsVsync { get; set; } = true;
        public float MusicVolume { get; set; } = 0.2f;
        public float EffectsVolume { get; set; } = 0.2f;
    }
}