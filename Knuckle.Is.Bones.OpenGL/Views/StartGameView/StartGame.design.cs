using Knuckle.Is.Bones.Core.Engines;
using Knuckle.Is.Bones.Core.Models;
using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Controls;
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
using System.Linq;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
    public partial class StartGame : BaseKnuckleBoneFadeView
    {
        private readonly Random _rnd = new Random();
        private PageHandler<AnimatedAudioButton> _boardsPageHandler;
        private AnimatedTextboxControl _boardsDescription;
        private PageHandler<AnimatedAudioButton> _dicePageHandler;
        private AnimatedTextboxControl _diceDescription;
        private PageHandler<AnimatedAudioButton> _firstOpponentsPageHandler;
        private AnimatedTextboxControl _firstOpponentDescription;
        private PageHandler<AnimatedAudioButton> _secondOpponentsPageHandler;
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
                Text = $"Points: {(Parent as KnuckleBoneWindow).User.AllTimeScore}",
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

                save.State.CurrentDice.RollValue();
                save.State.Turn = save.State.FirstOpponent.Module.OpponentID;

                save.Save();

                SwitchView(new MainGame(Parent, save));
            })
            {
                Text = "Start",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                FontColor = Color.Gray,
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
            AddControl(0, new AnimatedAudioButton(Parent, (x) => SwitchView(new MainMenu(Parent)))
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

        private void SetupPageControl(PageHandler<AnimatedAudioButton> pagehandler, float x, float y, float width, float height, string title, List<Guid> ids, Func<Guid, IUnlockable> getMethod, Action<AnimatedAudioButton> clicked, Action onAnySelected)
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

            var items = new List<IUnlockable>();
            foreach (var id in ids)
                items.Add(getMethod(id));
            items = items.OrderBy(x => x.RequiredPoints).ToList();

            var textureSet = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
            var controlList = new List<AnimatedAudioButton>();
            foreach (var item in items)
            {
                var isUnlocked = (Parent as KnuckleBoneWindow).User.AllTimeScore >= item.RequiredPoints;
                var text = $"{item.Name}";
                if (!isUnlocked)
                    text += $"({item.RequiredPoints}P)";
                controlList.Add(new AnimatedAudioButton(Parent, (x) =>
                {
                    clicked(x as AnimatedAudioButton);
                    onAnySelected();
                })
                {
                    TileSet = textureSet,
                    FillClickedColor = BasicTextures.GetClickedTexture(),
                    FillDisabledColor = BasicTextures.GetBasicRectange(Color.Transparent),
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    Text = text,
                    FontColor = isUnlocked ? Color.White : Color.Gray,
                    Alpha = isUnlocked ? 256 : 100,
                    Height = 50,
                    Width = width - 20,
                    Tag = item,
                    IsEnabled = isUnlocked
                });
            }
            pagehandler = new PageHandler<AnimatedAudioButton>(this, controlList)
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
