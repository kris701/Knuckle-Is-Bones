namespace KnuckleBones.Core.Models.Game
{
    public class ColumnDefinition
    {
        public List<int> Cells { get; set; }

        public int GetValue()
        {
            int value = 0;
            var max = Cells.Max();
            for (int i = 1; i <= max; i++)
            {
                var count = Cells.Count(x => x == i);
                if (count > 0)
                    value += (int)Math.Pow(i, count);
            }
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