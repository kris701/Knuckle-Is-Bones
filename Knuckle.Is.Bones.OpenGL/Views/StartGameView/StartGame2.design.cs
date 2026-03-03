using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using Knuckle.Is.Bones.Core.Models;
using Knuckle.Is.Bones.Core.Models.Game;
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
	public partial class StartGame2 : BaseNavigatableView
	{
		private SelectorWheelControl _boardSelector;
		private SelectorWheelControl _diceSelector;
		private SelectorWheelControl _firstOpponentSelector;
		private SelectorWheelControl _secondOpponentSelector;

		[MemberNotNull(
			nameof(_boardSelector)
			)]
		public void Initialize()
		{
			AddControl(0, new TileControl()
			{
				Width = 1920,
				Height = 1080,
				FillColor = BasicTextures.GetBasicRectange(Color.Black)
			});

			SetupBoardSelection();
			SetupDiceSelection();
#if DEBUG
			AddControl(0, new ButtonControl(Parent, (x) => SwitchView(new StartGame2(Parent, _type)))
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
		private void SetupBoardSelection()
		{
			var allItemIds = ResourceManager.Boards.GetResources();
			var allItems = new List<IDefinition>();
			foreach (var id in allItemIds)
			{
				var board = ResourceManager.Boards.GetResource(id);
				if (board.IsPurchasable || Parent.User.PurchasedShopItems.ContainsKey(id))
					allItems.Add(board);
			}
			_boardSelector = new SelectorWheelControl(Parent, allItems, allItems[0])
			{
				X = 100,
				Y = 100
			};
			AddControl(0, _boardSelector);
		}

		[MemberNotNull(nameof(_diceSelector))]
		private void SetupDiceSelection()
		{
			var allItemIds = ResourceManager.Dice.GetResources();
			var allItems = new List<IDefinition>();
			foreach (var id in allItemIds)
			{
				var board = ResourceManager.Dice.GetResource(id);
				if (board.IsPurchasable || Parent.User.PurchasedShopItems.ContainsKey(id))
					allItems.Add(board);
			}
			_diceSelector = new SelectorWheelControl(Parent, allItems, allItems[0])
			{
				X = 550,
				Y = 100
			};
			AddControl(0, _diceSelector);
		}
	}
}
