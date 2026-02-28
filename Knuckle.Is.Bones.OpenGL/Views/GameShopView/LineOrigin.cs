using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.GameShopView
{
	public class LineOrigin
	{
		public Point Location { get; set; }
		public Point Location2 { get; set; }

		public LineOrigin(Point location, Point location2)
		{
			Location = location;
			Location2 = location2;
		}
	}
}
