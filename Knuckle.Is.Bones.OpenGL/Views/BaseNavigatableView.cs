using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using FormMatter.OpenGL.Input;
using FormMatter.OpenGL.Views;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views
{
	public abstract class BaseNavigatableView : BaseTransitionView
	{
		public enum InputTypes { Mouse, Keyboard, Gamepad }

		internal readonly MouseWatcher _mouseWatcher;

		internal readonly KeyWatcher _keyboardBackKeyWatcher;
		internal readonly GamepadWatcher _gamepadBackKeyWatcher;

		internal readonly KeyWatcher _keyboardAcceptKeyWatcher;
		internal readonly GamepadWatcher _gamepadAcceptKeyWatcher;

		internal readonly KeyboardNavigator _keyboardNavigator;
		internal readonly GamepadNavigator _gamepadNavigator;

		public Action? BackAction { get; set; }
		public Action? AcceptAction { get; set; }
		public bool ShowGeneralControls { get; set; } = true;
		public AnimatedAudioButton MouseAcceptButton { get; set; }

		public static InputTypes InputType = InputTypes.Mouse;

		internal StackPanelControl _controlsPanel;
		internal List<IControl> _mouseControls;
		internal List<IControl> _keyboardControls;
		internal List<IControl> _gamepadControls;

		public BaseNavigatableView(KnuckleBoneWindow parent, Guid id, List<int> navigationLayers, Action? backAction = null, Action? acceptAction = null) : base(parent,id)
		{
			BackAction = backAction;
			AcceptAction = acceptAction;

			_mouseWatcher = new MouseWatcher(() =>
			{
				InputType = InputTypes.Mouse;
				UpdateControlsVisual();
			});

			_keyboardBackKeyWatcher = new KeyWatcher(Keys.Escape, () => { 
				if (BackAction != null)
				{
					BackAction.Invoke();
					parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
				}
				InputType = InputTypes.Keyboard;
				UpdateControlsVisual();
			});
			_gamepadBackKeyWatcher = new GamepadWatcher(Buttons.B, () => {
				if (BackAction != null)
				{
					BackAction.Invoke();
					parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
				}
				InputType = InputTypes.Gamepad;
				UpdateControlsVisual();
			});

			_keyboardAcceptKeyWatcher = new KeyWatcher(Keys.Enter, () => {
				if (AcceptAction != null)
				{
					AcceptAction.Invoke();
					parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
				}
				InputType = InputTypes.Keyboard;
				UpdateControlsVisual();
			});
			_gamepadAcceptKeyWatcher = new GamepadWatcher(Buttons.Start, () => {
				if (AcceptAction != null)
				{
					AcceptAction.Invoke();
					parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
				}
				InputType = InputTypes.Gamepad;
				UpdateControlsVisual();
			});

			_keyboardNavigator = CreateKeyboardNavigator(this, navigationLayers);
			_keyboardNavigator.OnAnyKeyDown += () => {
				if (_gamepadNavigator!.Selector.IsVisible)
				{
					_keyboardNavigator.Selector.X = _gamepadNavigator.Selector.X;
					_keyboardNavigator.Selector.Y = _gamepadNavigator.Selector.Y;
					_keyboardNavigator.Focused = _gamepadNavigator.Focused;
					_gamepadNavigator.Selector.IsVisible = false;
				}
				parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
				InputType = InputTypes.Keyboard;
				UpdateControlsVisual();
			};
			_gamepadNavigator = CreateGamepadNavigator(this, navigationLayers, parent.Settings.GamepadIndex);
			_gamepadNavigator.OnAnyKeyDown += () => {
				if (_keyboardNavigator.Selector.IsVisible)
				{
					_gamepadNavigator.Selector.X = _keyboardNavigator.Selector.X;
					_gamepadNavigator.Selector.Y = _keyboardNavigator.Selector.Y;
					_gamepadNavigator.Focused = _keyboardNavigator.Focused;
					_keyboardNavigator.Selector.IsVisible = false;
				}
				parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
				InputType = InputTypes.Gamepad;
				UpdateControlsVisual();
			};
		}

		public override void Initialize()
		{
			MouseAcceptButton = new AnimatedAudioButton(Parent, (a) => AcceptAction?.Invoke())
			{
				Text = "Accept",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				FillClickedColor = BasicTextures.GetClickedTexture(),
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
				Height = 30,
				Width = 150,
				IsVisible = AcceptAction != null
			};
			_mouseControls = new List<IControl>()
			{
				MouseAcceptButton,
				new AnimatedAudioButton(Parent, (a) => BackAction?.Invoke())
				{
					Text = "Back",
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					FillClickedColor = BasicTextures.GetClickedTexture(),
					TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
					Height = 30,
					Width = 150,
					IsVisible = BackAction != null,
				}
			};
			_keyboardControls = new List<IControl>()
			{
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					Text = "Move: Arrow",
					Width = 250,
					IsVisible = ShowGeneralControls
				},
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					Text = "Enter: Space",
					Width = 250,
					IsVisible = ShowGeneralControls
				},
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					Text = "Accept: Enter",
					Width = 250,
					IsVisible = AcceptAction != null
				},
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					Text = "Back: Esc",
					Width = 250,
					IsVisible = BackAction != null,
				}
			};
			_gamepadControls = new List<IControl>()
			{
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					Text = "Move: Dpad",
					Width = 250,
					IsVisible = ShowGeneralControls
				},
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					Text = "Enter: A",
					Width = 250,
					IsVisible = ShowGeneralControls
				},
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					Text = "Accept: Start",
					Width = 250,
					IsVisible = AcceptAction != null
				},
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					Text = "Back: B",
					Width = 250,
					IsVisible = BackAction != null,
				}
			};

			_controlsPanel = new StackPanelControl(new List<IControl>())
			{
				VerticalAlignment = VerticalAlignment.Bottom,
				Height = 50,
				Width = 300,
				Gap = 30,
				X = 30,
				Orientation = StackPanelControl.Orientations.Horizontal
			};
			UpdateControlsVisual();
			AddControl(9999, _controlsPanel);

			base.Initialize();
		}

		private void UpdateControlsVisual()
		{
			switch (InputType)
			{
				case InputTypes.Mouse:
					_controlsPanel.Children = _mouseControls;
					_controlsPanel.Initialize();
					break;
				case InputTypes.Keyboard:
					_controlsPanel.Children = _keyboardControls;
					_controlsPanel.Initialize();
					break;
				case InputTypes.Gamepad:
					_controlsPanel.Children = _gamepadControls;
					_controlsPanel.Initialize();
					break;
			}
		}

		public override void OnUpdate(GameTime gameTime)
		{
			_keyboardBackKeyWatcher.Update();
			_gamepadBackKeyWatcher.Update();

			_keyboardAcceptKeyWatcher.Update();
			_gamepadAcceptKeyWatcher.Update();

			_keyboardNavigator.Update();
			_gamepadNavigator.Update();
			_mouseWatcher.Update();
		}

		private KeyboardNavigator CreateKeyboardNavigator(IView view, List<int> layers)
		{
			var selector = new TileControl() { FillColor = BasicTextures.GetBasicCircle(FontHelpers.SecondaryColor, 10), Width = 20, Height = 20 };
			view.AddControl(9999, selector);
			var navigator = new KeyboardNavigator(
				view,
				selector,
				Keys.Left,
				Keys.Right,
				Keys.Up,
				Keys.Down,
				Keys.Space,
				0)
			{
				Layers = layers
			};
			return navigator;
		}

		private GamepadNavigator CreateGamepadNavigator(IView view, List<int> layers, int controller)
		{
			var selector = new TileControl() { FillColor = BasicTextures.GetBasicCircle(FontHelpers.SecondaryColor, 10), Width = 20, Height = 20 };
			view.AddControl(9999, selector);
			var navigator = new GamepadNavigator(
				view,
				selector,
				new List<Buttons>() { Buttons.DPadLeft, Buttons.LeftThumbstickLeft },
				new List<Buttons>() { Buttons.DPadRight, Buttons.LeftThumbstickRight },
				new List<Buttons>() { Buttons.DPadUp, Buttons.LeftThumbstickUp },
				new List<Buttons>() { Buttons.DPadDown, Buttons.LeftThumbstickDown },
				new List<Buttons>() { Buttons.X, Buttons.A },
				layers)
			{
				PlayerIndex = controller
			};
			return navigator;
		}
	}
}
