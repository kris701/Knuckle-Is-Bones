using Knuckle.Is.Bones.Core.Engines;
using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainGameView;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
	public partial class StartGame : BaseKnuckleBoneFadeView
	{
		private readonly PageHandler<StackPanelControl> _boardsPageHandler;
		private AnimatedTextboxControl _boardsDescription;
		private readonly PageHandler<StackPanelControl> _dicePageHandler;
		private AnimatedTextboxControl _diceDescription;
		private readonly PageHandler<StackPanelControl> _firstOpponentsPageHandler;
		private AnimatedTextboxControl _firstOpponentDescription;
		private readonly PageHandler<StackPanelControl> _secondOpponentsPageHandler;
		private AnimatedTextboxControl _secondOpponentDescription;

		private AnimatedAudioButton _startButton;
		private bool _boardSelected = false;
		private bool _diceSelected = false;
		private bool _opponentOneSelected = false;
		private bool _opponentTwoSelected = false;

		public override void Initialize()
		{
			AddControl(0, new TileControl()
			{
				Width = 1920,
				Height = 1080,
				FillColor = BasicTextures.GetBasicRectange(Color.Black)
			});

			AddControl(0, new LabelControl()
			{
				Text = $"Points: {(Parent as KnuckleBoneWindow).User.Points}",
				Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
				HorizontalAlignment = HorizontalAlignment.Middle,
				VerticalAlignment = VerticalAlignment.Top,
				Width = 500,
				Height = 100
			});

			var textureSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce"));

			_startButton = new AnimatedAudioButton(Parent, (x) =>
			{
				if (_selectedBoard == Guid.Empty)
					return;
				if (_selectedFirstOpponent == Guid.Empty)
					return;
				if (_selectedSecondOpponent == Guid.Empty)
					return;
				if (_selectedDice == Guid.Empty)
					return;

				var state = new GameState()
				{
					FirstOpponent = ResourceManager.Opponents.GetResource(_selectedFirstOpponent).Clone(),
					FirstOpponentBoard = ResourceManager.Boards.GetResource(_selectedBoard).Clone(),
					SecondOpponent = ResourceManager.Opponents.GetResource(_selectedSecondOpponent).Clone(),
					SecondOpponentBoard = ResourceManager.Boards.GetResource(_selectedBoard).Clone(),
					CurrentDice = ResourceManager.Dice.GetResource(_selectedDice).Clone(),
					User = Parent.User.Clone()
				};

				state.FirstOpponent.MoveModule.OpponentID = Guid.NewGuid();
				state.SecondOpponent.MoveModule.OpponentID = Guid.NewGuid();

				state.CurrentDice.RollValue();
				state.Turn = state.FirstOpponent.MoveModule.OpponentID;

				state.Save();

				SwitchView(new MainGame(Parent, state));
			})
			{
				Text = "Start",
				Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
				FontColor = Color.Gray,
				Y = 960,
				X = 1500,
				FillClickedColor = BasicTextures.GetClickedTexture(),
				TileSet = textureSet,
				FillDisabledColor = BasicTextures.GetBasicRectange(Color.Transparent),
				IsEnabled = false,
				Alpha = 100,
				Width = 400,
				Height = 100
			};
			AddControl(0, _startButton);
			AddControl(0, new AnimatedAudioButton(Parent, (x) => SwitchView(new MainMenu(Parent)))
			{
				Text = "Back",
				Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
				Y = 960,
				X = 20,
				FillClickedColor = BasicTextures.GetClickedTexture(),
				TileSet = textureSet,
				Width = 400,
				Height = 100
			});

			var margin = 50;
			var width = (1920 - margin * 2) / 4 - margin;

			SetupPageControl(
				_boardsPageHandler,
				margin + (width + margin) * 0,
				100,
				width,
				500,
				"Boards",
				ResourceManager.Boards.GetResources(),
				ResourceManager.Boards.GetResource,
				SelectBoard_Click,
				() =>
					{
						_boardSelected = true;
						CheckIfAllOptionsChecked();
					});
			_boardsDescription = new AnimatedTextboxControl()
			{
				Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
				Margin = 25,
				FontColor = Color.White,
				TileSet = Parent.Textures.GetTextureSet(new Guid("29744523-5a1b-43cd-abd8-ecb79006d148")),
				X = margin + (width + margin) * 0,
				Y = 600,
				Width = width,
				Height = 350,
			};
			AddControl(0, _boardsDescription);
			SetupPageControl(
				_dicePageHandler,
				margin + (width + margin) * 1,
				100,
				width,
				500,
				"Dice",
				ResourceManager.Dice.GetResources(),
				ResourceManager.Dice.GetResource,
				SelectDice_Click,
				() =>
					{
						_diceSelected = true;
						CheckIfAllOptionsChecked();
					});
			_diceDescription = new AnimatedTextboxControl()
			{
				Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
				Margin = 25,
				FontColor = Color.White,
				TileSet = Parent.Textures.GetTextureSet(new Guid("29744523-5a1b-43cd-abd8-ecb79006d148")),
				X = margin + (width + margin) * 1,
				Y = 600,
				Width = width,
				Height = 350,
			};
			AddControl(0, _diceDescription);
			SetupPageControl(
				_firstOpponentsPageHandler,
				margin + (width + margin) * 2,
				100,
				width,
				500,
				"Player 1",
				ResourceManager.Opponents.GetResources(),
				ResourceManager.Opponents.GetResource,
				SelectFirstOpponent_Click,
				() =>
				{
					_opponentOneSelected = true;
					CheckIfAllOptionsChecked();
				});
			_firstOpponentDescription = new AnimatedTextboxControl()
			{
				Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
				Margin = 25,
				FontColor = Color.White,
				TileSet = Parent.Textures.GetTextureSet(new Guid("29744523-5a1b-43cd-abd8-ecb79006d148")),
				X = margin + (width + margin) * 2,
				Y = 600,
				Width = width,
				Height = 350,
			};
			AddControl(0, _firstOpponentDescription);
			SetupPageControl(
				_secondOpponentsPageHandler,
				margin + (width + margin) * 3,
				100,
				width,
				500,
				"Player 2",
				ResourceManager.Opponents.GetResources(),
				ResourceManager.Opponents.GetResource,
				SelectSecondOpponent_Click,
				() =>
					{
						_opponentTwoSelected = true;
						CheckIfAllOptionsChecked();
					});
			_secondOpponentDescription = new AnimatedTextboxControl()
			{
				Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
				Margin = 25,
				FontColor = Color.White,
				TileSet = Parent.Textures.GetTextureSet(new Guid("29744523-5a1b-43cd-abd8-ecb79006d148")),
				X = margin + (width + margin) * 3,
				Y = 600,
				Width = width,
				Height = 350,
			};
			AddControl(0, _secondOpponentDescription);

#if DEBUG
			AddControl(0, new ButtonControl(Parent, clicked: (x) => SwitchView(new StartGame(Parent)))
			{
				X = 0,
				Y = 0,
				Width = 50,
				Height = 25,
				Text = "Reload",
				Font = Parent.Fonts.GetFont(FontSizes.Ptx10),
				FillColor = BasicTextures.GetBasicRectange(Color.White),
				FontColor = Color.Black,
				FillClickedColor = BasicTextures.GetBasicRectange(Color.Gray)
			});
#endif

			base.Initialize();
		}

		private void CheckIfAllOptionsChecked()
		{
			if (_boardSelected && _diceSelected && _opponentOneSelected && _opponentTwoSelected)
			{
				_startButton.IsEnabled = true;
				_startButton.Alpha = 256;
				_startButton.FontColor = Color.White;
			}
		}

		private void SetupPageControl(PageHandler<StackPanelControl> pagehandler, float x, float y, float width, float height, string title, List<Guid> ids, Func<Guid, IPurchasable> getMethod, Action<AnimatedAudioButton> clicked, Action onAnySelected)
		{
			AddControl(1, new LabelControl()
			{
				Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
				Text = title,
				X = x,
				Y = y + 10,
				Height = 50,
				Width = width,
				FontColor = new Color(217, 68, 144)
			});

			var items = new List<IPurchasable>();
			foreach (var id in ids)
				items.Add(getMethod(id));
			items = items.OrderByDescending(x => !x.IsPurchasable || Parent.User.PurchasedShopItems.Contains(x.ID)).ToList();

			var textureSet = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
			var controlList = new List<StackPanelControl>();
			foreach (var item in items)
			{
				var isUnlocked = !item.IsPurchasable || Parent.User.PurchasedShopItems.Contains(item.ID);
				Guid? tileSet = null;
				if (Parent.User.CompletedItems.ContainsKey(item.ID))
				{
					switch (Parent.User.CompletedItems[item.ID])
					{
						case 1: tileSet = new Guid("855231a0-daf4-4a10-b163-8da4fd7ae408"); break;
						case 2: tileSet = new Guid("2a2e927b-6691-4df8-8de1-f0a819cc37ca"); break;
						case >= 3: tileSet = new Guid("345434d1-ccfc-458f-90a4-3077094c8b8a"); break;
					}
				}
				var stackControls = new List<IControl>() {
					new AnimatedAudioButton(Parent, (x) =>
					{
						clicked(x as AnimatedAudioButton);
						onAnySelected();
					})
					{
						TileSet = textureSet,
						FillClickedColor = BasicTextures.GetClickedTexture(),
						FillDisabledColor = BasicTextures.GetBasicRectange(Color.Transparent),
						Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
						Text = $"{item.Name}",
						FontColor = isUnlocked ? Color.White : Color.Gray,
						Alpha = isUnlocked ? 256 : 100,
						Tag = item,
						IsEnabled = isUnlocked
					}
				};
				if (tileSet != null)
					stackControls.Add(new AnimatedTileControl()
					{
						Width = 50,
						Height = 50,
						TileSet = Parent.Textures.GetTextureSet((Guid)tileSet)
					});

				controlList.Add(new StackPanelControl(stackControls)
				{
					Width = width - 20,
					Height = 50,
					Orientation = StackPanelControl.Orientations.Horizontal
				});
			}
			pagehandler = new PageHandler<StackPanelControl>(this, controlList)
			{
				LeftButtonX = 10,
				LeftButtonY = -50,
				RightButtonX = width - 80,
				RightButtonY = -50,
				ItemsPrPage = 7,
				X = x + 10,
				Y = y + 70,
				Width = width,
				Height = height
			};
			AddControl(1, pagehandler);
		}
	}
}
