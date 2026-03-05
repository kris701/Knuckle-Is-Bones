using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
	public partial class GametypeSelect : BaseNavigatableView
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
				Text = "Select a gametype to play",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				FontColor = FontHelpers.SecondaryColor,
				X = 750,
				Y = 75,
				Width = 400,
				Height = 50
			});

			SetupGamemodeBox(300, LastGameSetupModel.LastGameSetupType.PvE);
			SetupGamemodeBox(750, LastGameSetupModel.LastGameSetupType.PvP);
			SetupGamemodeBox(1200, LastGameSetupModel.LastGameSetupType.EvE);

#if DEBUG
			AddControl(9999, new ButtonControl(Parent, (x) => SwitchView(new GametypeSelect(Parent)))
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

		private void SetupGamemodeBox(int x, LastGameSetupModel.LastGameSetupType type)
		{
			var controls = new List<IControl>();
			controls.Add(new AnimatedTileControl()
			{			
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.GameTypeSelect)
			});

			var titleText = "";
			var sb = new StringBuilder();
			var targetTileSet = Guid.Empty;
			switch (type)
			{
				case LastGameSetupModel.LastGameSetupType.PvE:
					titleText = "PvE";
					sb.AppendLine("Play against an AI opponent");
					sb.AppendLine(" ");
					sb.AppendLine("Winning in this gamemode gives you points that you can use in the Shop!");
					targetTileSet = TextureHelpers.GameTypeSelectPvE;
					break;
				case LastGameSetupModel.LastGameSetupType.PvP:
					titleText = "PvP";
					sb.AppendLine("Play against your friend on this device");
					sb.AppendLine(" ");
					sb.AppendLine("(Does not reward any points)");
					targetTileSet = TextureHelpers.GameTypeSelectPvP;
					break;
				case LastGameSetupModel.LastGameSetupType.EvE:
					titleText = "EvE";
					sb.AppendLine("Watch two AIs play against eachother");
					sb.AppendLine(" ");
					sb.AppendLine("(Does not reward any points)");
					targetTileSet = TextureHelpers.GameTypeSelectEvE;
					break;
			}
			controls.Add(new LabelControl()
			{
				Text = titleText,
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
				FontColor = FontHelpers.SecondaryColor,
				Y = 25,
				Width = 400,
				Height = 50
			});
			controls.Add(new TextboxControl()
			{
				Text = sb.ToString(),
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx8),
				FontColor = FontHelpers.PrimaryColor,
				Margin = 20,
				WordWrap = TextboxControl.WordWrapTypes.Word,
				Y = 100,
				Width = 400,
				Height = 200
			});
			controls.Add(new AnimatedTileControl()
			{
				TileSet = Parent.Textures.GetTextureSet(targetTileSet),
				X = 25,
				Y = 250
			});
			controls.Add(new AnimatedAudioButton(Parent, (s) => SwitchView(new StartGame(Parent, type)))
			{
				Text = "Start",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				FontColor = FontHelpers.SecondaryColor,
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
				Y = 650,
				X = 50,
				Width = 300,
				Height = 100
			});

			var panel = new CanvasPanelControl(controls) { 
				Y = 150,
				X = x
			};
			AddControl(0, panel);
		}
	}
}
