using Knuckle.Is.Bones.Core.Engines;
using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainGameView;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
    public partial class StartGame : BaseView
    {
        private readonly Random _rnd = new Random();

        public override void Initialize()
        {
            AddControl(0, new ButtonControl(Parent, (x) =>
            {
                var save = new GameSaveDefinition(new GameState()
                {
                    FirstOpponent = ResourceManager.Opponents.GetResource(new Guid("d6032478-b6ec-483e-8750-5976830d66b2")).Clone(),
                    FirstOpponentBoard = ResourceManager.Boards.GetResource(new Guid("907bddf8-cbe1-49f4-a1f8-92ad5266f116")).Clone(),
                    SecondOpponent = ResourceManager.Opponents.GetResource(new Guid("42244cf9-6ad3-4729-8376-a0d323440a18")).Clone(),
                    SecondOpponentBoard = ResourceManager.Boards.GetResource(new Guid("907bddf8-cbe1-49f4-a1f8-92ad5266f116")).Clone(),
                    CurrentDice = ResourceManager.Dice.GetResource(new Guid("fb539a3a-9989-4623-88d1-bf216320f717")).Clone(),
                });

                save.State.FirstOpponent.Module.OpponentID = Guid.NewGuid();
                save.State.SecondOpponent.Module.OpponentID = Guid.NewGuid();

                save.State.CurrentDice.Value = _rnd.Next(1, save.State.CurrentDice.Sides + 1);
                save.State.Turn = save.State.FirstOpponent.Module.OpponentID;

                SwitchView(new MainGame(Parent, save));
            })
            {
                Text = "Start",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Y = 100,
                HorizontalAlignment = HorizontalAlignment.Middle,
                FillClickedColor = BasicTextures.GetClickedTexture(),
                FillColor = BasicTextures.GetBasicRectange(Color.Gray),
                Width = 200,
                Height = 100
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
    }
}
