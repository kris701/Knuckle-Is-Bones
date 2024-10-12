namespace KnuckleBones.Core.Models.Game
{
    public class BoardDefinition : IDefinition
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ColumnDefinition> Columns { get; set; }

        public int GetValue()
        {
            int value = 0;
            foreach (var column in Columns)
                value += column.GetValue();
            return value;
        }

        public bool IsFull()
        {
            foreach (var column in Columns)
                if (!column.IsFull())
                    return false;
            return true;
        }
    }
}