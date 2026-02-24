using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.HowToPlayView
{
	public partial class HowToPlay : BaseKnuckleBoneFadeView
	{
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
				Y = 100,
				Text = "How to Play",
				HorizontalAlignment = HorizontalAlignment.Middle,
				Font = Parent.Fonts.GetFont(FontSizes.Ptx48),
				FontColor = new Color(217, 68, 144),
			});

			// https://cult-of-the-lamb.fandom.com/wiki/Knucklebones
			var sb = new StringBuilder();
			sb.AppendLine($"The players take turns. On a player's turn, they roll a single 6-sided die, and must place it in a column on their board. A filled column does not accept any more dice.");
			sb.AppendLine(" ");
			sb.AppendLine($"Each player has a score, which is the sum of all the dice values on their board.");
			sb.AppendLine(" ");
			sb.AppendLine($"If a player places multiple dice of the same value in the same column, the score awarded for each of those dice is multiplied by the number of dice of the same value in that column. e.g. if a column contains 4-1-4, then the score for that column is 4x2 + 1x1 + 4x2 = 17.");
			sb.AppendLine(" ");
			sb.AppendLine($"When a player places a die, all dice of the same value in the corresponding column of the opponent's board gets destroyed. Players can use this mechanic to destroy their opponent's high-scoring combos.");
			sb.AppendLine(" ");
			sb.AppendLine($"The game ends when either player completely fills up their board. The player with the higher score wins.");
			sb.AppendLine(" ");
			sb.AppendLine($"You get points, if you win, based on the score of the player board. AI vs AI battles gives no points. Use points to unlock more dice and boards. Different AIs have different score multipliers, based on how difficult they are.");

			AddControl(0, new TextboxControl()
			{
				Width = 1000,
				Height = 700,
				HorizontalAlignment = HorizontalAlignment.Middle,
				VerticalAlignment = VerticalAlignment.Middle,
				FontColor = Color.White,
				WordWrap = TextboxControl.WordWrapTypes.Word,
				Text = sb.ToString(),
				Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
			});
			AddControl(0, new AnimatedAudioButton(Parent, (x) => SwitchView(new MainMenu(Parent)))
			{
				Text = "Back",
				Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
				FillClickedColor = BasicTextures.GetClickedTexture(),
				TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
				Width = 400,
				Height = 100,
				Y = 960,
				X = 20,
			});

#if DEBUG
			AddControl(0, new ButtonControl(Parent, clicked: (x) => SwitchView(new HowToPlay(Parent)))
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
