using FormMatter.OpenGL;
using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using FormMatter.OpenGL.Input;
using Knuckle.Is.Bones.Core.Models.Shop;
using Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.GameShopView
{
	public partial class GameShop : BaseNavigatableView
	{
		private IControl? _lastFocus;

		private FloatPoint _origin = new FloatPoint(0,0);
		private FloatPoint _lastOffset = new FloatPoint(0,0);
		private Dictionary<AnimatedAudioButton, Point> _buttonOrigins = new Dictionary<AnimatedAudioButton, Point>();
		private Dictionary<LineControl, LineOrigin> _lineOrigins = new Dictionary<LineControl, LineOrigin>();
		private bool _mouseDown = false;

		public GameShop(KnuckleBoneWindow parent) : base(parent, new Guid("169e9e54-b45f-41d4-9845-f8519d256033"), new List<int>() { 6 })
		{
			_keyboardNavigator.MaxDistance = 500;
			_gamepadNavigator.MaxDistance = 500;

			BackAction += () => SwitchView(new MainMenu(Parent));

			Initialize();

			_keyboardNavigator.Focused = _coreItem;
			_gamepadNavigator.Focused = _coreItem;

			_mouseControls.Add(new LabelControl()
			{
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx8),
				FontColor = FontHelpers.SecondaryColor,
				Text = "Move",
				FitTextWidth = true,
				Height = 50,
				IsVisible = BackAction != null
			});
			_mouseControls.Add(new AnimatedTileControl()
			{
				TileSet = Parent.Textures.GetTextureSet(new Guid("4e62791c-4a75-4455-b432-5b9186d96bea")),
				IsVisible = BackAction != null,
				Height = 50,
				Width = 50
			});
			UpdateControlsVisual();
		}

		private string BuildDescription(ShopItemDefinition item)
		{
			var sb = new StringBuilder();

			if (item.BuyTimes > 1)
			{
				if (Parent.User.PurchasedShopItems.ContainsKey(item.ID))
					sb.AppendLine($"{Parent.User.PurchasedShopItems[item.ID]}/{item.BuyTimes}");
				else
					sb.AppendLine($"0/{item.BuyTimes}");
			}

			sb.AppendLine($"{GetTextByShopType(item.ShopType)}");
			sb.AppendLine("Cost: " + item.Cost);
			sb.AppendLine(" ");
			sb.AppendLine(item.Description);

			return sb.ToString();
		}

		private void PurchaseItem(ShopItemDefinition item)
		{
			if (item.CanAffort(Parent.User) && item.Buy(Parent.User))
			{
				Parent.Audio.PlaySoundEffectOnce(SoundEffectHelpers.ShopBuySound);
				SwitchView(new GameShop(Parent));
			}
		}

		public override void OnUpdate(GameTime gameTime)
		{
			switch (InputType)
			{
				case InputTypes.Keyboard:
					if (_keyboardNavigator.Focused is AnimatedAudioButton keyBut && keyBut != _lastFocus && keyBut.Tag is ShopItemDefinition keyShop)
					{
						if (!_buttonOrigins.ContainsKey(keyBut))
							break;
						_origin = new FloatPoint(0,0);
						var diff = new FloatPoint((IWindow.BaseScreenSize.X / 2) - _buttonOrigins[keyBut].X, (IWindow.BaseScreenSize.Y / 2) - _buttonOrigins[keyBut].Y);
						UpdateOffsetLocations(diff);
						UpdateItemDescription(keyBut, keyShop);
						_keyboardNavigator.UpdateFocusedPosition();
					}
					break;
				case InputTypes.Gamepad:
					if (_gamepadNavigator.Focused is AnimatedAudioButton gamBut && gamBut != _lastFocus && gamBut.Tag is ShopItemDefinition gamShop)
					{
						if (!_buttonOrigins.ContainsKey(gamBut))
							break;
						_origin = new FloatPoint(0, 0);
						var diff = new FloatPoint((IWindow.BaseScreenSize.X / 2) - _buttonOrigins[gamBut].X, (IWindow.BaseScreenSize.Y / 2) - _buttonOrigins[gamBut].Y);
						UpdateOffsetLocations(diff);
						UpdateItemDescription(gamBut, gamShop);
						_gamepadNavigator.UpdateFocusedPosition();
					}
					break;
				case InputTypes.Mouse:
					var state = Mouse.GetState();
					if (_mouseDown)
					{
						if (state.MiddleButton == ButtonState.Released)
						{
							_mouseDown = false;
						}
						else
						{
							_descriptionControl.IsVisible = false;
							var newPos = InputHelper.GetRelativePosition(Parent.XScale, Parent.YScale);
							UpdateOffsetLocations(newPos);
						}
					}
					else
					{
						if (state.MiddleButton == ButtonState.Pressed)
						{
							_mouseDown = true;
							_origin = InputHelper.GetRelativePosition(Parent.XScale, Parent.YScale);
							_origin.X -= _lastOffset.X;
							_origin.Y -= _lastOffset.Y;
						}
					}

					break;
			}
			base.OnUpdate(gameTime);
		}

		private void UpdateOffsetLocations(FloatPoint newPos)
		{
			var checkOffset = new FloatPoint(newPos.X - _origin.X, newPos.Y - _origin.Y);
			if (_buttonOrigins.Keys.All(x =>
				((_buttonOrigins[x].X + checkOffset.X) + x.Width > IWindow.BaseScreenSize.X) ||
				((_buttonOrigins[x].X + checkOffset.X) < 0) ||
				((_buttonOrigins[x].Y + checkOffset.Y) + x.Height > IWindow.BaseScreenSize.Y) ||
				((_buttonOrigins[x].Y + checkOffset.Y) < 150)))
				return;
			_lastOffset = checkOffset;
			foreach (var control in _buttonOrigins.Keys)
			{
				control.X = _buttonOrigins[control].X + _lastOffset.X;
				control.Y = _buttonOrigins[control].Y + _lastOffset.Y;
				control.Initialize();
			}
			foreach (var control in _lineOrigins.Keys)
			{
				control.X = _lineOrigins[control].Location.X + _lastOffset.X;
				control.Y = _lineOrigins[control].Location.Y + _lastOffset.Y;
				control.X2 = _lineOrigins[control].Location2.X + _lastOffset.X;
				control.Y2 = _lineOrigins[control].Location2.Y + _lastOffset.Y;
				control.Initialize();
			}
		}

		private void UpdateItemDescription(IControl origin, ShopItemDefinition item)
		{
			_descriptionControl.Text = BuildDescription(item);
			_descriptionControl.X = origin.X + origin.Width;
			_descriptionControl.Y = origin.Y + origin.Height;
			_descriptionControl.IsVisible = true;
			_descriptionControl.Initialize();
			_lastFocus = origin;
		}
	}
}
