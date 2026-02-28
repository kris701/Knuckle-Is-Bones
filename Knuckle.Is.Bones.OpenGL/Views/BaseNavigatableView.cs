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
		public bool ShowControls { get; set; } = true;

		public static InputTypes InputType = InputTypes.Mouse;

		internal StackPanelControl _controlsPanel;
		internal List<IControl> _mouseControls;
		internal List<IControl> _keyboardControls;
		internal List<IControl> _gamepadControls;

		public BaseNavigatableView(KnuckleBoneWindow parent, Guid id, List<int> navigationLayers, Action? backAction = null, Action? acceptAction = null) : base(parent, id)
		{
			BackAction = backAction;
			AcceptAction = acceptAction;

			_mouseWatcher = new MouseWatcher(() =>
			{
				InputType = InputTypes.Mouse;
				UpdateControlsVisual();
			});

			_keyboardBackKeyWatcher = new KeyWatcher(Keys.Escape, () =>
			{
				if (BackAction != null)
				{
					BackAction.Invoke();
					parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
				}
				InputType = InputTypes.Keyboard;
				UpdateControlsVisual();
			});
			_gamepadBackKeyWatcher = new GamepadWatcher(Buttons.B, () =>
			{
				if (BackAction != null)
				{
					BackAction.Invoke();
					parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
				}
				InputType = InputTypes.Gamepad;
				UpdateControlsVisual();
			})
			{
				PlayerIndexes = new List<int>() { 0, 1, 2, 3 }
			};

			_keyboardAcceptKeyWatcher = new KeyWatcher(Keys.Enter, () =>
			{
				if (AcceptAction != null)
				{
					AcceptAction.Invoke();
					parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
				}
				InputType = InputTypes.Keyboard;
				UpdateControlsVisual();
			});
			_gamepadAcceptKeyWatcher = new GamepadWatcher(Buttons.Start, () =>
			{
				if (AcceptAction != null)
				{
					AcceptAction.Invoke();
					parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
				}
				InputType = InputTypes.Gamepad;
				UpdateControlsVisual();
			})
			{
				PlayerIndexes = new List<int>() { 0, 1, 2, 3 }
			};

			_keyboardNavigator = CreateKeyboardNavigator(this, navigationLayers);
			_keyboardNavigator.OnUpKeyDown += UpdateKeyboardNavigator;
			_keyboardNavigator.OnDownKeyDown += UpdateKeyboardNavigator;
			_keyboardNavigator.OnLeftKeyDown += UpdateKeyboardNavigator;
			_keyboardNavigator.OnRightKeyDown += UpdateKeyboardNavigator;
			_gamepadNavigator = CreateGamepadNavigator(this, navigationLayers);
			_gamepadNavigator.OnUpKeyDown += UpdateGamepadNavigator;
			_gamepadNavigator.OnDownKeyDown += UpdateGamepadNavigator;
			_gamepadNavigator.OnLeftKeyDown += UpdateGamepadNavigator;
			_gamepadNavigator.OnRightKeyDown += UpdateGamepadNavigator;
		}

		private void UpdateKeyboardNavigator()
		{
			if (_gamepadNavigator!.Selector.IsVisible)
			{
				_keyboardNavigator.Selector.X = _gamepadNavigator.Selector.X;
				_keyboardNavigator.Selector.Y = _gamepadNavigator.Selector.Y;
				_keyboardNavigator.Focused = _gamepadNavigator.Focused;
				_gamepadNavigator.Selector.IsVisible = false;
			}
			Parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
			InputType = InputTypes.Keyboard;
			UpdateControlsVisual();
		}

		private void UpdateGamepadNavigator()
		{
			if (_keyboardNavigator.Selector.IsVisible)
			{
				_gamepadNavigator.Selector.X = _keyboardNavigator.Selector.X;
				_gamepadNavigator.Selector.Y = _keyboardNavigator.Selector.Y;
				_gamepadNavigator.Focused = _keyboardNavigator.Focused;
				_keyboardNavigator.Selector.IsVisible = false;
			}
			Parent.Audio.PlaySoundEffectOnce(new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028"));
			InputType = InputTypes.Gamepad;
			UpdateControlsVisual();
		}

		public override void Initialize()
		{
			MouseAcceptButton = new AnimatedAudioButton(Parent, (a) => AcceptAction?.Invoke())
			{
				Text = "Accept",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				FillClickedColor = BasicTextures.GetClickedTexture(),
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
				Height = 50,
				Width = 200,
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
					Height = 50,
					Width = 200,
					IsVisible = BackAction != null,
				}
			};
			_keyboardControls = new List<IControl>()
			{
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx8),
					FontColor = FontHelpers.SecondaryColor,
					Text = "Navigate",
					FitTextWidth = true,
					Height = 50,
					IsVisible = ShowGeneralControls
				},
				new AnimatedTileControl()
				{
					TileSet = Parent.Textures.GetTextureSet(new Guid("0f9bc026-4b1f-4558-9f6c-2a1b5682d5c0")),
					IsVisible = ShowGeneralControls,
					Height = 50,
					Width = 50
				},
				new AnimatedTileControl()
				{
					TileSet = Parent.Textures.GetTextureSet(new Guid("f08c0341-f1b3-4c37-b32c-628ec7e2400f")),
					IsVisible = ShowGeneralControls,
					Height = 50,
					Width = 50
				},
				new TileControl()
				{
					Width = 50,
					IsVisible = ShowGeneralControls
				},
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx8),
					FontColor = FontHelpers.SecondaryColor,
					Text = "Accept",
					FitTextWidth = true,
					Height = 50,
					IsVisible = AcceptAction != null
				},
				new AnimatedTileControl()
				{
					TileSet = Parent.Textures.GetTextureSet(new Guid("08e972a7-42e7-4f50-9ffa-69ae05933d21")),
					IsVisible = AcceptAction != null,
					Height = 50,
					Width = 50
				},
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx8),
					FontColor = FontHelpers.SecondaryColor,
					Text = "Back",
					FitTextWidth = true,
					Height = 50,
					IsVisible = BackAction != null
				},
				new AnimatedTileControl()
				{
					TileSet = Parent.Textures.GetTextureSet(new Guid("61cf5d02-688a-4056-a48f-5cb1a52a9486")),
					IsVisible = BackAction != null,
					Height = 50,
					Width = 50
				}
			};
			_gamepadControls = new List<IControl>()
			{
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx8),
					FontColor = FontHelpers.SecondaryColor,
					Text = "Navigate",
					FitTextWidth = true,
					Height = 50,
					IsVisible = ShowGeneralControls
				},
				new AnimatedTileControl()
				{
					TileSet = Parent.Textures.GetTextureSet(new Guid("42f85636-661c-4f9e-86c7-76749765764b")),
					IsVisible = ShowGeneralControls,
					Height = 50,
					Width = 50
				},
				new AnimatedTileControl()
				{
					TileSet = Parent.Textures.GetTextureSet(new Guid("aea4bd4a-368c-4504-b6d8-fb177222fcdd")),
					IsVisible = ShowGeneralControls,
					Height = 50,
					Width = 50
				},
				new TileControl()
				{
					Width = 50,
					IsVisible = ShowGeneralControls
				},
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx8),
					FontColor = FontHelpers.SecondaryColor,
					Text = "Accept",
					FitTextWidth = true,
					Height = 50,
					IsVisible = AcceptAction != null
				},
				new AnimatedTileControl()
				{
					TileSet = Parent.Textures.GetTextureSet(new Guid("cf33a308-940e-499e-97ef-a7ccb794275e")),
					IsVisible = AcceptAction != null,
					Height = 50,
					Width = 50
				},
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx8),
					FontColor = FontHelpers.SecondaryColor,
					Text = "Back",
					FitTextWidth = true,
					Height = 50,
					IsVisible = BackAction != null
				},
				new AnimatedTileControl()
				{
					TileSet = Parent.Textures.GetTextureSet(new Guid("cd3c58b4-3891-4e18-915f-b99b21c2ad2c")),
					IsVisible = BackAction != null,
					Height = 50,
					Width = 50
				}
			};

			_controlsPanel = new StackPanelControl(new List<IControl>())
			{
				Height = 50,
				Width = 300,
				Gap = 30,
				X = 30,
				Y = 10,
				Orientation = StackPanelControl.Orientations.Horizontal
			};
			if (ShowControls)
			{
				UpdateControlsVisual();
				AddControl(9999, _controlsPanel);
			}

			base.Initialize();
		}

		internal void UpdateControlsVisual()
		{
			switch (InputType)
			{
				case InputTypes.Mouse:
					_keyboardNavigator.Selector.IsVisible = false;
					_gamepadNavigator.Selector.IsVisible = false;
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

		private GamepadNavigator CreateGamepadNavigator(IView view, List<int> layers)
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
				PlayerIndexes = new List<int>() { 0, 1, 2, 3 }
			};
			return navigator;
		}
	}
}
