using Knuckle.Is.Bones.Core.Engines;
using Knuckle.Is.Bones.Core.Models;
using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainGameView;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
    public partial class StartGame : BaseView
    {
        private readonly Random _rnd = new Random();
        private PageHandler<ButtonControl> _boardsPageHandler;
        private PageHandler<ButtonControl> _dicePageHandler;
        private PageHandler<ButtonControl> _firstOpponentsPageHandler;
        private PageHandler<ButtonControl> _secondOpponentsPageHandler;

        public override void Initialize()
        {
            AddControl(0, new ButtonControl(Parent, (x) =>
            {
                if (_selectedBoard == Guid.Empty)
                    return;
                if (_selectedFirstOpponent == Guid.Empty)
                    return;
                if (_selectedSecondOpponent == Guid.Empty)
                    return;
                if (_selectedDice == Guid.Empty)
                    return;

                var save = new GameSaveDefinition(new GameState()
                {
                    FirstOpponent = ResourceManager.Opponents.GetResource(_selectedFirstOpponent).Clone(),
                    FirstOpponentBoard = ResourceManager.Boards.GetResource(_selectedBoard).Clone(),
                    SecondOpponent = ResourceManager.Opponents.GetResource(_selectedSecondOpponent).Clone(),
                    SecondOpponentBoard = ResourceManager.Boards.GetResource(_selectedBoard).Clone(),
                    CurrentDice = ResourceManager.Dice.GetResource(_selectedDice).Clone(),
                });

                save.State.FirstOpponent.Module.OpponentID = Guid.NewGuid();
                save.State.SecondOpponent.Module.OpponentID = Guid.NewGuid();

                save.State.CurrentDice.Value = _rnd.Next(1, save.State.CurrentDice.Sides + 1);
                save.State.Turn = save.State.FirstOpponent.Module.OpponentID;

                save.Save();

                SwitchView(new MainGame(Parent, save));
            })
            {
                Text = "Start",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Y = 980,
                HorizontalAlignment = HorizontalAlignment.Middle,
                FillClickedColor = BasicTextures.GetClickedTexture(),
                FillColor = BasicTextures.GetBasicRectange(Color.Gray),
                Width = 300,
                Height = 100
            });


            SetupPageControl(_boardsPageHandler, 100, 100, 400, 500, "Boards", ResourceManager.Boards.GetResources(), ResourceManager.Boards.GetResource, SelectBoard_Click);
            SetupPageControl(_dicePageHandler, 500, 100, 400, 500, "Dice", ResourceManager.Dice.GetResources(), ResourceManager.Dice.GetResource, SelectDice_Click);
            SetupPageControl(_firstOpponentsPageHandler, 900, 100, 400, 500, "First Opponent", ResourceManager.Opponents.GetResources(), ResourceManager.Opponents.GetResource, SelectFirstOpponent_Click);
            SetupPageControl(_secondOpponentsPageHandler, 1300, 100, 400, 500, "Second Opponent", ResourceManager.Opponents.GetResources(), ResourceManager.Opponents.GetResource, SelectSecondOpponent_Click);


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

        private void SetupPageControl(PageHandler<ButtonControl> pagehandler, float x, float y, float width, float height, string title, List<Guid> ids, Func<Guid, IDefinition> getMethod, Action<ButtonControl> clicked)
        {
            AddControl(1, new LabelControl()
            {
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Text = title,
                X = x,
                Y = y + 10,
                Height = 50,
                Width = width,
                FontColor = Color.White
            });

            var controlList = new List<ButtonControl>();
            foreach (var id in ids)
            {
                var item = getMethod(id);
                controlList.Add(new ButtonControl(Parent, (x) => clicked(x))
                {
                    FillColor = BasicTextures.GetBasicRectange(Color.Gray),
                    FillClickedColor = BasicTextures.GetClickedTexture(),
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    Text = $"{item.Name}",
                    FontColor = Color.White,
                    Height = 50,
                    Width = width - 20,
                    Tag = item.ID
                });
            }
            pagehandler = new PageHandler<ButtonControl>(this, controlList)
            {
                LeftButtonX = 10,
                LeftButtonY = -50,
                RightButtonX = width - 80,
                RightButtonY = -50,
                ItemsPrPage = 12,
                X = x + 10,
                Y = y + 70,
                Width = width,
                Height = height
            };
            AddControl(1, pagehandler);
        }
    }
}
