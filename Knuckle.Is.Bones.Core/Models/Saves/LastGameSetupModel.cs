using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Models.Saves
{
	public class LastGameSetupModel
	{
		public Guid BoardID { get; set; }
		public Guid DiceID { get; set; }
		public Guid FirstOpponentID { get; set; }
		public Guid SecondOpponentID { get; set; }
	}
}
