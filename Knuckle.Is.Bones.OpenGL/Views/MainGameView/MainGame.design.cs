using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using Knuckle.Is.Bones.Core.Engines;
using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Knuckle.Is.Bones.OpenGL.Views.MainGameView
{
	public partial class MainGame : BaseTransitionView
	{
		private AnimatedLabelControl _diceLabel;
		private LabelControl _firstOpponentTurnControl;
		private BoardControl _firstOpponentBoard;
		private AnimatedLabelControl _firstOpponentPoints;
		private LabelControl _secondOpponentTurnControl;
		private BoardControl _secondOpponentBoard;
		private AnimatedLabelControl _secondOpponentPoints;
		private CanvasPanelControl _gameOverPanel;
		private LabelControl _winnerLabel;
		private LabelControl _pointsGainedLabel;

		[MemberNotNull(
			nameof(_diceLabel),
			nameof(_firstOpponentTurnControl),
			nameof(_firstOpponentBoard),
			nameof(_firstOpponentPoints),
			nameof(_secondOpponentTurnControl),
			nameof(_secondOpponentBoard),
			nameof(_secondOpponentPoints),
			nameof(_gameOverPanel),
			nameof(_winnerLabel),
			nameof(_pointsGainedLabel)
			)]
		public override void Initialize()
		{
			AddControl(0, new TileControl()
			{
				Width = 1920,
				Height = 1080,
				FillColor = BasicTextures.GetBasicRectange(Color.Black)
			});

			SetupControlsView();

			CreateFirstOpponent();
			CreateSeccondOpponent();

			SetupDice();
			SetupGameOverView();

#if DEBUG
			AddControl(0, new ButtonControl(Parent, (x) => SwitchView(new MainGame(Parent, Engine.State)))
			{
				X = 0,
				Y = 0,
				Width = 50,
				Height = 25,
				Text = "Reload",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx10),
				FillColor = BasicTextures.GetBasicRectange(Color.White),
				FontColor = Color.Black,
				FillClickedColor = BasicTextures.GetBasicRectange(Color.Gray)
			});
#endif

			base.Initialize();
		}

		private void SetupControlsView()
		{
			AddControl(0, new AnimatedAudioButton(Parent, (b) => Escape())
			{
				X = 50,
				Y = 50,
				Height = 100,
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				FontColor = Color.White,
				Text = $"Back",
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
				FillClickedColor = BasicTextures.GetClickedTexture(),
			});
		}

		[MemberNotNull(
			nameof(_firstOpponentTurnControl),
			nameof(_firstOpponentBoard),
			nameof(_firstOpponentPoints)
			)]
		private void CreateFirstOpponent()
		{
			_firstOpponentTurnControl = new LabelControl()
			{
				Text = $"{Engine.State.FirstOpponent.Name}",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				Width = 300,
				Height = 100,
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Top,
			};
			AddControl(10, _firstOpponentTurnControl);
			_firstOpponentPoints = new AnimatedLabelControl()
			{
				X = 1390,
				Y = 175,
				Width = 150,
				Height = 150,
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
				FontColor = Color.Gold,
				Text = $"{Engine.State.GetFirstOpponentBoardValue()}",
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.GamePoints)
			};
			AddControl(10, _firstOpponentPoints);

			if (Parent.Textures.ContainsTextureSet(Engine.State.FirstOpponent.ID))
			{
				AddControl(10, new AnimatedTileControl()
				{
					Width = 300,
					Height = 300,
					X = 1600,
					Y = 75,
					TileSet = Parent.Textures.GetTextureSet(Engine.State.FirstOpponent.ID)
				});
			}

			_firstOpponentBoard = new BoardControl(Parent, Engine.State.FirstOpponentBoard, 710, 10, 500, 500, MoveSet, true);
			AddControl(10, _firstOpponentBoard);
		}

		[MemberNotNull(
			nameof(_secondOpponentTurnControl),
			nameof(_secondOpponentBoard),
			nameof(_secondOpponentPoints)
			)]
		private void CreateSeccondOpponent()
		{
			_secondOpponentTurnControl = new LabelControl()
			{
				Text = $"{Engine.State.SecondOpponent.Name}",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				Width = 300,
				Height = 100,
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Left,
			};
			AddControl(20, _secondOpponentTurnControl);
			_secondOpponentPoints = new AnimatedLabelControl()
			{
				X = 375,
				Y = 735,
				Width = 150,
				Height = 150,
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
				FontColor = Color.Gold,
				Text = $"{Engine.State.GetSecondOpponentBoardValue()}",
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.GamePoints)
			};
			AddControl(20, _secondOpponentPoints);

			if (Parent.Textures.ContainsTextureSet(Engine.State.SecondOpponent.ID))
			{
				AddControl(20, new AnimatedTileControl()
				{
					Width = 300,
					Height = 300,
					Y = 725,
					TileSet = Parent.Textures.GetTextureSet(Engine.State.SecondOpponent.ID)
				});
			}

			_secondOpponentBoard = new BoardControl(Parent, Engine.State.SecondOpponentBoard, 710, 570, 500, 500, MoveSet, false);
			AddControl(20, _secondOpponentBoard);
		}

		[MemberNotNull(
			nameof(_diceLabel)
			)]
		private void SetupDice()
		{
			var targetTileSet = Parent.Textures.GetTextureSet(TextureHelpers.GameSquare);
			if (Parent.Textures.ContainsTextureSet(Engine.State.CurrentDice.ID))
				targetTileSet = Parent.Textures.GetTextureSet(Engine.State.CurrentDice.ID);

			_diceLabel = new AnimatedLabelControl()
			{
				X = 375,
				Y = 475,
				Width = 150,
				Height = 150,
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
				Text = "",
				FontColor = Color.White,
				TileSet = targetTileSet
			};
			AddControl(1000, _diceLabel);
		}

		[MemberNotNull(
			nameof(_gameOverPanel),
			nameof(_winnerLabel),
			nameof(_pointsGainedLabel)
			)]
		private void SetupGameOverView()
		{
			_winnerLabel = new LabelControl()
			{
				Width = 500,
				Height = 75,
				HorizontalAlignment = HorizontalAlignment.Middle,
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				Text = "",
				FontColor = Color.White,
			};
			_pointsGainedLabel = new LabelControl()
			{
				Width = 500,
				Height = 75,
				HorizontalAlignment = HorizontalAlignment.Middle,
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				Text = "",
				FontColor = Color.White,
			};
			_gameOverPanel = new CanvasPanelControl(new List<IControl>()
			{
				new AnimatedTileControl()
				{
					Width = 600,
					Height = 500,
					TileSet = Parent.Textures.GetTextureSet(TextureHelpers.GameGameOver)
				},
				new StackPanelControl(new List<IControl>()
				{
					new LabelControl()
					{
						Width = 500,
						Height = 100,
						HorizontalAlignment = HorizontalAlignment.Middle,
						Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
						Text = $"Game Over!",
						FontColor = Color.White,
					},
					_winnerLabel,
					_pointsGainedLabel,
					new AnimatedAudioButton(Parent, (x) =>
					{
						var state = new GameState(
							ResourceManager.Opponents.GetResource(Engine.State.FirstOpponent.ID).Clone(),
							ResourceManager.Boards.GetResource(Engine.State.FirstOpponentBoard.ID).Clone(),
							ResourceManager.Opponents.GetResource(Engine.State.SecondOpponent.ID).Clone(),
							ResourceManager.Boards.GetResource(Engine.State.FirstOpponentBoard.ID).Clone(),
							ResourceManager.Dice.GetResource(Engine.State.CurrentDice.ID).Clone(),
							Engine.State.User.Clone()
							);

						GameSaveHelpers.Save(state);

						SwitchView(new MainGame(Parent, state));
					})
					{
						Width = 500,
						Height = 110,
						HorizontalAlignment = HorizontalAlignment.Middle,
						Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
						Text = "Try Again",
						FontColor = Color.White,
						TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
						FillClickedColor = BasicTextures.GetClickedTexture(),
					},
					new AnimatedAudioButton(Parent, (x) => SwitchView(new MainMenu(Parent)))
					{
						Width = 500,
						Height = 110,
						HorizontalAlignment = HorizontalAlignment.Middle,
						Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
						Text = "Main Menu",
						FontColor = Color.White,
						TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
						FillClickedColor = BasicTextures.GetClickedTexture(),
					}
				})
			})
			{
				HorizontalAlignment = HorizontalAlignment.Middle,
				VerticalAlignment = VerticalAlignment.Middle,
				Width = 600,
				Height = 500,
				IsVisible = false
			};
			;
			AddControl(1001, _gameOverPanel);
		}
	}
}