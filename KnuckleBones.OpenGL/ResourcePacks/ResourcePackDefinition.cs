using KnuckleBone.Core.Models;
using MonoGame.OpenGL.Formatter.Audio;
using MonoGame.OpenGL.Formatter.Fonts;
using MonoGame.OpenGL.Formatter.Textures;
using System;
using System.Collections.Generic;

namespace KnuckleBones.OpenGL.ResourcePacks
{
    public class ResourcePackDefinition : IDefinition
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? BasedOn { get; set; }
        public List<TextureDefinition> Textures { get; set; }
        public List<TextureSetDefinition> TextureSets { get; set; }
        public List<SongDefinition> Songs { get; set; }
        public List<SoundEffectDefinition> SoundEffects { get; set; }
        public List<FontDefinition> Fonts { get; set; }

        public ResourcePackDefinition(Guid iD, string name, string description, Guid? basedOn, List<TextureDefinition> textures, List<TextureSetDefinition> textureSets, List<SongDefinition> songs, List<SoundEffectDefinition> soundEffects, List<FontDefinition> fonts)
        {
            ID = iD;
            Name = name;
            Description = description;
            BasedOn = basedOn;
            Textures = textures;
            TextureSets = textureSets;
            Songs = songs;
            SoundEffects = soundEffects;
            Fonts = fonts;
        }
    }
}