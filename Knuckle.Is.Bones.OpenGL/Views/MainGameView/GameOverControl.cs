using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using Knuckle.Is.Bones.Core.Engines.Game;
using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views.MainGameView
{
	public class GameOverControl : CollectionControl
	{
		private readonly LabelControl _winnerLabel;
		private readonly LabelControl _pointsGainedLabel;
		private readonly CanvasPanelControl _pointsGainedCanvas;
		private readonly StackPanelControl _pointsGainedPanel;

		private readonly GameState _initialState;
		private readonly MainGame _parent;

		public GameOverControl(MainGame parent, GameState initialState)
		{
			Children = new List<IControl>();

			_parent = parent;
			_initialState = initialState;

			Children.Add(new AnimatedTileControl()
			{
				Width = 600,
				Height = 500,
				TileSet = parent.Parent.Textures.GetTextureSet(TextureHelpers.GameGameOver)
			});

			_winnerLabel = new LabelControl()
			{
				Width = 500,
				Height = 50,
				HorizontalAlignment = HorizontalAlignment.Middle,
				Font = parent.Parent.Fonts.GetFont(FontHelpers.Ptx16),
				Text = "",
				FontColor = FontHelpers.SecondaryColor,
			};
			_pointsGainedLabel = new LabelControl()
			{
				Font = parent.Parent.Fonts.GetFont(FontHelpers.Ptx12),
				FontColor = FontHelpers.PrimaryColor,
				HorizontalAlignment = HorizontalAlignment.Middle,
				Text = "",
				Height = 75,
				Width = 500
			};
			_pointsGainedPanel = new StackPanelControl(new List<IControl>())
			{
				Width = 300,
				X = 50,
				Y = 50,
				Height = 400
			};
			_pointsGainedCanvas = new CanvasPanelControl(new List<IControl>()
			{
				new AnimatedTileControl()
				{
					Width = 400,
					Height = 500,
					TileSet = parent.Parent.Textures.GetTextureSet(TextureHelpers.GameGameOver)
				},
				_pointsGainedPanel
			})
			{
				VerticalAlignment = VerticalAlignment.Middle,
				Width = 300,
				Height = 500,
				X = 625,
				IsVisible = false
			};
			Children.Add(new StackPanelControl(new List<IControl>()
			{
				new LabelControl()
				{
					Width = 500,
					Height = 100,
					HorizontalAlignment = HorizontalAlignment.Middle,
					Font = parent.Parent.Fonts.GetFont(FontHelpers.Ptx24),
					Text = $"Game Over!",
					FontColor = FontHelpers.PrimaryColor,
				},
				_winnerLabel,
				_pointsGainedLabel,
				CreatePlayAgainButton(parent, _initialState),
				CreateMainMenuButton(parent)
			}));

			Children.Add(_pointsGainedCanvas);

			VerticalAlignment = VerticalAlignment.Middle;
			HorizontalAlignment = HorizontalAlignment.Middle;
			Width = 600;
			Height = 500;

			IsVisible = false;
		}

		private AnimatedAudioButton CreatePlayAgainButton(MainGame parent, GameState state)
		{
			return new AnimatedAudioButton(parent.Parent, (x) =>
			{
				var state = new GameState(
					ResourceManager.Opponents.GetResource(_initialState.FirstOpponent.ID).Clone(),
					ResourceManager.Boards.GetResource(_initialState.FirstOpponentBoard.ID).Clone(),
					ResourceManager.Opponents.GetResource(_initialState.SecondOpponent.ID).Clone(),
					ResourceManager.Boards.GetResource(_initialState.FirstOpponentBoard.ID).Clone(),
					ResourceManager.Dice.GetResource(_initialState.CurrentDice.ID).Clone(),
					_initialState.User.Clone()
					);

				state.FirstOpponent.MoveModule.OpponentID = Guid.NewGuid();
				state.SecondOpponent.MoveModule.OpponentID = Guid.NewGuid();
				state.SetRandomStartingPlayer();

				GameSaveHelpers.Save(state);

				parent.SwitchView(new MainGame(parent.Parent, state));
			})
			{
				Width = 500,
				Height = 110,
				HorizontalAlignment = HorizontalAlignment.Middle,
				Font = parent.Parent.Fonts.GetFont(FontHelpers.Ptx16),
				Text = "Try Again",
				FontColor = FontHelpers.PrimaryColor,
				TileSet = parent.Parent.Textures.GetTextureSet(TextureHelpers.Button),
				FillClickedColor = BasicTextures.GetClickedTexture(),
			};
		}

		private AnimatedAudioButton CreateMainMenuButton(MainGame parent)
		{
			return new AnimatedAudioButton(parent.Parent, (x) => parent.SwitchView(new MainMenu(parent.Parent)))
			{
				Width = 500,
				Height = 110,
				HorizontalAlignment = HorizontalAlignment.Middle,
				Font = parent.Parent.Fonts.GetFont(FontHelpers.Ptx16),
				Text = "Main Menu",
				FontColor = FontHelpers.PrimaryColor,
				TileSet = parent.Parent.Textures.GetTextureSet(TextureHelpers.Button),
				FillClickedColor = BasicTextures.GetClickedTexture(),
			};
		}

		public void Show(GameResult result)
		{
			IsVisible = true;
			UpdateVisibility();
			Initialize();
			_pointsGainedCanvas.IsVisible = false;

			if (result.PointsGained > 0)
			{
				var breakdownControls = new List<IControl>()
					{
						new LabelControl()
						{
							Font = _parent.Parent.Fonts.GetFont(FontHelpers.Ptx12),
							FontColor = FontHelpers.PrimaryColor,
							Text = "You gained:",
							Height = 20,
							FitTextWidth = true
						}
					};

				foreach (var item in result.PointBreakdown)
				{
					var newLabelSet = new List<IControl>();
					if (item.Value == 0)
					{
						newLabelSet.Add(new LabelControl()
						{
							Text = "x ",
							Font = _parent.Parent.Fonts.GetFont(FontHelpers.Ptx8),
							FontColor = FontHelpers.PrimaryColor,
							Height = 20,
							FitTextWidth = true
						});
						newLabelSet.Add(new LabelControl()
						{
							Text = item.Count == 1 ? $"{item.Multiplier}" : $"{item.Multiplier}^{item.Count}",
							Font = _parent.Parent.Fonts.GetFont(FontHelpers.Ptx8),
							FontColor = FontHelpers.SecondaryColor,
							Height = 20,
							FitTextWidth = true
						});
					}
					else
					{
						newLabelSet.Add(new LabelControl()
						{
							Text = $"{item.Value}",
							Font = _parent.Parent.Fonts.GetFont(FontHelpers.Ptx8),
							FontColor = FontHelpers.SecondaryColor,
							Height = 20,
							FitTextWidth = true
						});
					}
					newLabelSet.Add(new LabelControl()
					{
						Text = $" ({item.Type})",
						Font = _parent.Parent.Fonts.GetFont(FontHelpers.Ptx8),
						FontColor = FontHelpers.PrimaryColor,
						Height = 20,
						FitTextWidth = true
					});

					breakdownControls.Add(new StackPanelControl(newLabelSet)
					{
						Orientation = StackPanelControl.Orientations.Horizontal,
						Height = 20,
						Width = _pointsGainedPanel.Width
					});
				}

				breakdownControls.Add(new StackPanelControl(new List<IControl>()
						{
							new LabelControl()
							{
								Text = " = ",
								Font = _parent.Parent.Fonts.GetFont(FontHelpers.Ptx8),
								FontColor = FontHelpers.PrimaryColor,
								Height = 20,
								FitTextWidth = true
							},
							new LabelControl()
							{
								Text = $"{result.PointsGained}",
								Font = _parent.Parent.Fonts.GetFont(FontHelpers.Ptx8),
								FontColor = FontHelpers.SecondaryColor,
								Height = 20,
								FitTextWidth = true
							},
							new LabelControl()
							{
								Text = $" points",
								Font = _parent.Parent.Fonts.GetFont(FontHelpers.Ptx8),
								FontColor = FontHelpers.PrimaryColor,
								Height = 20,
								FitTextWidth = true
							}
						})
				{
					Orientation = StackPanelControl.Orientations.Horizontal,
					Height = 20,
					Width = _pointsGainedPanel.Width
				});
				_pointsGainedPanel.Children = breakdownControls;
				_pointsGainedPanel.Initialize();

				_pointsGainedCanvas.IsVisible = true;

				_pointsGainedLabel.Text = $"Gained {result.PointsGained} points";
			}
			else
			{
				_pointsGainedLabel.Text = "No points gained";
			}

			if (result.HadPlayer)
			{
				if (result.PlayerWon)
					_winnerLabel.Text = $"You Won!";
				else
					_winnerLabel.Text = $"You lost to {result.WinnerName}!";
			}
			else
				_winnerLabel.Text = $"{result.WinnerName} Won!";
		}
	}
}
