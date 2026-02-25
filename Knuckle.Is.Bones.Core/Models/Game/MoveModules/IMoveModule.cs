using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	[JsonDerivedType(typeof(PlayerMoveModule), typeDiscriminator: nameof(PlayerMoveModule))]
	[JsonDerivedType(typeof(RandomMoveModule), typeDiscriminator: nameof(RandomMoveModule))]
	[JsonDerivedType(typeof(TurboRandomMoveModule), typeDiscriminator: nameof(TurboRandomMoveModule))]
	[JsonDerivedType(typeof(DefensiveMoveModule), typeDiscriminator: nameof(DefensiveMoveModule))]
	[JsonDerivedType(typeof(ComboMoveModule), typeDiscriminator: nameof(ComboMoveModule))]
	public interface IMoveModule : IGenericClonable<IMoveModule>
	{
		public Guid OpponentID { get; set; }

		public int GetTargetColumn();
	}
}