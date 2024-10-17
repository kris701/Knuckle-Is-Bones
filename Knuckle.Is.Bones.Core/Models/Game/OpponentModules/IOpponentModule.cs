using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.OpponentModules
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "OpponentModule")]
    [JsonDerivedType(typeof(PlayerOpponentModule), typeDiscriminator: "Player")]
    [JsonDerivedType(typeof(RandomPositionOpponentModule), typeDiscriminator: "Random")]
    public interface IOpponentModule : IGenericClonable<IOpponentModule>
    {
        public Guid OpponentID { get; set; }

        public void SetTargetColumn(BoardDefinition board);

        public int GetTargetColumn();
    }
}