using FormMatter.OpenGL.BackgroundWorkers;
using Knuckle.Is.Bones.Core.Engines.Idle;
using Knuckle.Is.Bones.Core.Models.Saves;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.BackgroundWorkers
{
	public class IdleEngineBackgroundWorker : IBackgroundWorker
	{
		public IIdleEngine Engine { get; set; }
		public UserSaveDefinition User { get; set; }

		public IdleEngineBackgroundWorker(IIdleEngine engine, UserSaveDefinition user)
		{
			Engine = engine;
			User = user;
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
		}

		public void Initialize()
		{
		}

		public void Update(GameTime gameTime)
		{
			Engine.Update(User);
		}
	}
}
