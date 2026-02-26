using FormMatter.OpenGL.Input;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views
{
	public abstract class BaseNavigatableView : BaseTransitionView
	{
		internal readonly KeyWatcher _keyboardEscapeKeyWatcher;
		internal readonly GamepadWatcher _gamepadBackKeyWatcher;
		internal readonly KeyboardNavigator _keyboardNavigator;
		internal readonly GamepadNavigator _gamepadNavigator;

		public Action? BackAction { get; set; }

		public BaseNavigatableView(KnuckleBoneWindow parent, Guid id, List<int> navigationLayers) : base(parent,id)
		{
			_keyboardEscapeKeyWatcher = new KeyWatcher(Keys.Escape, () => { 
				BackAction?.Invoke(); 
				parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028")); 
			});
			_gamepadBackKeyWatcher = new GamepadWatcher(Buttons.B, () => { 
				BackAction?.Invoke(); 
				parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028")); 
			});
			_keyboardNavigator = NavigatorHelpers.CreateKeyboardNavigator(this, navigationLayers);
			_keyboardNavigator.OnAnyKeyDown += () => {
				if (_gamepadNavigator!.Selector.IsVisible)
				{
					_keyboardNavigator.Selector.X = _gamepadNavigator.Selector.X;
					_keyboardNavigator.Selector.Y = _gamepadNavigator.Selector.Y;
					_keyboardNavigator.Focused = _gamepadNavigator.Focused;
					_gamepadNavigator.Selector.IsVisible = false;
				}
				parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
			};
			_gamepadNavigator = NavigatorHelpers.CreateGamepadNavigator(this, navigationLayers, parent.Settings.GamepadIndex);
			_gamepadNavigator.OnAnyKeyDown += () => {
				if (_keyboardNavigator.Selector.IsVisible)
				{
					_gamepadNavigator.Selector.X = _keyboardNavigator.Selector.X;
					_gamepadNavigator.Selector.Y = _keyboardNavigator.Selector.Y;
					_gamepadNavigator.Focused = _keyboardNavigator.Focused;
					_keyboardNavigator.Selector.IsVisible = false;
				}
				parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
			};
		}

		public override void OnUpdate(GameTime gameTime)
		{
			_keyboardEscapeKeyWatcher.Update();
			_gamepadBackKeyWatcher.Update();
			_keyboardNavigator.Update();
			_gamepadNavigator.Update();
		}
	}
}
