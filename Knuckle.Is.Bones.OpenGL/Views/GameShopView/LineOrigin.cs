using Microsoft.Xna.Framework;

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
