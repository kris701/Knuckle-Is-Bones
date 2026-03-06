using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Knuckle.Is.Bones.OpenGL.Views.MainGameView
{
	public partial class MainGame : BaseNavigatableView
	{
		private AnimatedLabelControl _diceLabel;
		private LabelControl _firstOpponentTurnControl;
		private BoardControl _firstOpponentBoard;
		private AnimatedLabelControl _firstOpponentPoints;
		private LabelControl _secondOpponentTurnControl;
		private BoardControl _secondOpponentBoard;
		private AnimatedLabelControl _secondOpponentPoints;
		private GameOverControl _gameOverControl;

		[MemberNotNull(
			nameof(_diceLabel),
			nameof(_firstOpponentTurnControl),
			nameof(_firstOpponentBoard),
			nameof(_firstOpponentPoints),
			nameof(_secondOpponentTurnControl),
			nameof(_secondOpponentBoard),
			nameof(_secondOpponentPoints),
			nameof(_gameOverControl)
			)]
		public override void Initialize()
		{
			CreateFirstOpponent();
			CreateSeccondOpponent();
			_gameOverControl = new GameOverControl(this, Engine.State);
			AddControl(1001, _gameOverControl);

			SetupDice();

#if DEBUG
			AddControl(9999, new ButtonControl(Parent, (x) => SwitchView(new MainGame(Parent, Engine.State)))
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

			UpdateForMove();
			HideAllNavigators();
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
	}
}