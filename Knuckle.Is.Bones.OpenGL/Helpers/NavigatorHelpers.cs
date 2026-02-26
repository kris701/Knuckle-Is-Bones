using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using FormMatter.OpenGL.Input;
using FormMatter.OpenGL.Views;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Helpers
{
	public static class NavigatorHelpers
	{
		public static KeyboardNavigator CreateKeyboardNavigator(IView view, List<int> layers)
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
				Keys.Enter,
				0)
			{
				Layers = layers
			};
			return navigator;
		}

		public static GamepadNavigator CreateGamepadNavigator(IView view, List<int> layers, int controller)
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
