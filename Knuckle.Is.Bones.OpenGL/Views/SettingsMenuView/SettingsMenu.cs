using Knuckle.Is.Bones.OpenGL.Models;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knuckle.Is.Bones.OpenGL.Views.SettingsMenuView
{
    public partial class SettingsMenu : BaseKnuckleBoneFadeView
    {
        public static Guid ID = new Guid("356b5d18-1aaf-4c98-aa73-2b27fe82ed1f");

        private SettingsDefinition _newSettings;

        public SettingsMenu(KnuckleBoneWindow parent) : base(parent, ID)
        {
            _newSettings = Parent.User.UIData.Clone();
            Initialize();
        }

        private void OnSaveAndApplySettings(ButtonControl sender)
        {
            Parent.User.UIData = _newSettings;
            Parent.ApplySettings();
            Parent.User.Save();
            SwitchView(new MainMenu(Parent));
        }
    }
}
