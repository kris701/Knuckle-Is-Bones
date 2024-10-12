namespace KnuckleBones.Core.Models.Game
{
    public class ColumnDefinition
    {
        public List<int> Cells { get; set; }

        public int GetValue()
        {
            int value = 0;
            foreach (var cell in Cells)
                value += cell;
            return value;
        }

        public bool IsFull()
        {
            foreach (var cell in Cells)
                if (cell == 0)
                    return false;
            return true;
        }
    }
}