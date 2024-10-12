namespace KnuckleBones.Core.Models.Game
{
    public class DiceDefinition : IDefinition
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sides { get; set; }
        public int Value { get; set; } = 0;
    }
}