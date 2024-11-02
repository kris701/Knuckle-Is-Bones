using Knuckle.Is.Bones.Core.Engines;
using Knuckle.Is.Bones.Core.Models;
using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainGameView;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
    public partial class StartGame : BaseAnimatedView
    {
        private readonly Random _rnd = new Random();
        private PageHandler<AnimatedButtonControl> _boardsPageHandler;
        private PageHandler<AnimatedButtonControl> _dicePageHandler;
        private PageHandler<AnimatedButtonControl> _firstOpponentsPageHandler;
        private PageHandler<AnimatedButtonControl> _secondOpponentsPageHandler;

        private AnimatedButtonControl _startButton;
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
            var textureSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce"));

            _startButton = new AnimatedButtonControl(Parent, (x) =>
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
                HorizontalAlignment = HorizontalAlignment.Right,
                FillClickedColor = BasicTextures.GetClickedTexture(),
                TileSet = textureSet,
                FillDisabledColor = BasicTextures.GetBasicRectange(Color.Transparent),
                IsEnabled = false,
                Alpha = 100,
                Width = 400,
                Height = 100
            };
            AddControl(0, _startButton);
            AddControl(0, new AnimatedButtonControl(Parent, (x) => SwitchView(new MainMenu(Parent)))
            {
                Text = "Back",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Y = 980,
                HorizontalAlignment = HorizontalAlignment.Left,
                FillClickedColor = BasicTextures.GetClickedTexture(),
                TileSet = textureSet,
                Width = 400,
                Height = 100
            });

            var margin = 50;
            var width = (1920 - margin * 2) / 4;

            SetupPageControl(
                _boardsPageHandler, 
                margin + (width) * 0, 
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
            SetupPageControl(
                _dicePageHandler, 
                margin + (width) * 1, 
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
            SetupPageControl(
                _firstOpponentsPageHandler, 
                margin + (width) * 2, 
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
            SetupPageControl(
                _secondOpponentsPageHandler, 
                margin + (width) * 3, 
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
            }
        }

        private void SetupPageControl(PageHandler<AnimatedButtonControl> pagehandler, float x, float y, float width, float height, string title, List<Guid> ids, Func<Guid, IDefinition> getMethod, Action<AnimatedButtonControl> clicked, Action onAnySelected)
        {
            AddControl(1, new LabelControl()
            {
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Text = title,
                X = x,
                Y = y + 10,
                Height = 50,
                Width = width,
                FontColor = new Color(217,68,144)
            });

            var textureSet = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
            var controlList = new List<AnimatedButtonControl>();
            foreach (var id in ids)
            {
                var item = getMethod(id);
                controlList.Add(new AnimatedButtonControl(Parent, (x) =>
                {
                    clicked(x as AnimatedButtonControl);
                    onAnySelected();
                })
                {
                    TileSet = textureSet,
                    FillClickedColor = BasicTextures.GetClickedTexture(),
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    Text = $"{item.Name}",
                    FontColor = Color.White,
                    Height = 50,
                    Width = width - 20,
                    Tag = item.ID
                });
            }
            pagehandler = new PageHandler<AnimatedButtonControl>(this, controlList)
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
