using FormMatter.OpenGL;
using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using Knuckle.Is.Bones.Core.Models;
using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.Core.Models.Game.MoveModules;
using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
	public partial class StartGame : BaseNavigatableView
	{
		private SelectorWheelControl? _boardSelector;
		private SelectorWheelControl? _diceSelector;
		private SelectorWheelControl? _firstOpponentSelector;
		private SelectorWheelControl? _secondOpponentSelector;

		public void Initialize()
		{
			AddControl(0, new TileControl()
			{
				Width = 1920,
				Height = 1080,
				FillColor = BasicTextures.GetBasicRectange(Color.Black)
			});

			switch (_type)
			{
				case LastGameSetupModel.LastGameSetupType.PvP:
					SetupBoardSelection(500);
					SetupDiceSelection(1000);
					break;
				case LastGameSetupModel.LastGameSetupType.PvE:
					SetupBoardSelection(250);
					SetupDiceSelection(750);
					SetupSecondOpponentSelection(1250, "Opponent");
					break;
				case LastGameSetupModel.LastGameSetupType.EvE:
					SetupBoardSelection(25);
					SetupDiceSelection(500);
					SetupFirstOpponentSelection(975, "First Opponent");
					SetupSecondOpponentSelection(1450, "Second Opponent");
					break;
			}
#if DEBUG
			AddControl(0, new ButtonControl(Parent, (x) => SwitchView(new StartGame(Parent, _type)))
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

		[MemberNotNull(nameof(_boardSelector))]
		private void SetupBoardSelection(int x)
		{
			CreateTitle(x, "Board");

			var allItemIds = ResourceManager.Boards.GetResources();
			var allItems = new List<IDefinition>();
			foreach (var id in allItemIds)
			{
				var item = ResourceManager.Boards.GetResource(id);
				if (!item.IsPurchasable || Parent.User.PurchasedShopItems.ContainsKey(id))
					allItems.Add(item);
			}
			_boardSelector = new SelectorWheelControl(Parent, allItems, allItems[0])
			{
				X = x,
				Y = 150
			};
			AddControl(0, _boardSelector);
		}

		[MemberNotNull(nameof(_diceSelector))]
		private void SetupDiceSelection(int x)
		{
			CreateTitle(x, "Dice");

			var allItemIds = ResourceManager.Dice.GetResources();
			var allItems = new List<IDefinition>();
			foreach (var id in allItemIds)
			{
				var item = ResourceManager.Dice.GetResource(id);
				if (!item.IsPurchasable || Parent.User.PurchasedShopItems.ContainsKey(id))
					allItems.Add(item);
			}
			_diceSelector = new SelectorWheelControl(Parent, allItems, allItems[0])
			{
				X = x,
				Y = 150
			};
			AddControl(0, _diceSelector);
		}

		[MemberNotNull(nameof(_firstOpponentSelector))]
		private void SetupFirstOpponentSelection(int x, string title)
		{
			CreateTitle(x, title);

			var allItemIds = ResourceManager.Opponents.GetResources();
			var allItems = new List<IDefinition>();
			foreach (var id in allItemIds)
			{
				var item = ResourceManager.Opponents.GetResource(id);
				if (item.MoveModule is PlayerMoveModule)
					continue;
				if (!item.IsPurchasable || Parent.User.PurchasedShopItems.ContainsKey(id))
					allItems.Add(item);
			}
			_firstOpponentSelector = new SelectorWheelControl(Parent, allItems, allItems[0])
			{
				X = x,
				Y = 150
			};
			AddControl(0, _firstOpponentSelector);
		}

		[MemberNotNull(nameof(_secondOpponentSelector))]
		private void SetupSecondOpponentSelection(int x, string title)
		{
			CreateTitle(x, title);

			var allItemIds = ResourceManager.Opponents.GetResources();
			var allItems = new List<IDefinition>();
			foreach (var id in allItemIds)
			{
				var item = ResourceManager.Opponents.GetResource(id);
				if (item.MoveModule is PlayerMoveModule)
					continue;
				if (!item.IsPurchasable || Parent.User.PurchasedShopItems.ContainsKey(id))
					allItems.Add(item);
			}
			_secondOpponentSelector = new SelectorWheelControl(Parent, allItems, allItems[0])
			{
				X = x,
				Y = 150
			};
			AddControl(0, _secondOpponentSelector);
		}

		private void CreateTitle(int x, string title)
		{
			AddControl(0, new LabelControl()
			{
				Text = title,
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				FontColor = FontHelpers.SecondaryColor,
				X = x,
				Y = 75,
				Width = 400,
				Height = 50
			});
		}
	}
}
