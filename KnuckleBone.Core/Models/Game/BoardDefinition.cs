using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnuckleBone.Core.Models.Game
{
    public class BoardDefinition
    {
        public DiceDefinition?[,] Board { get; set; }

        public int GetValue()
        {
            int value = 0;
            for (int x = 0; x < Board.Length; x++)
                for (int y = 0; y < Board.GetLength(x); y++)
                    if (Board[x, y] != null)
                        value += Board[x, y].Value;

            return value;
        }

        public bool IsFull()
        {
            for (int x = 0; x < Board.Length; x++)
                for (int y = 0; y < Board.GetLength(x); y++)
                    if (Board[x, y] != null)
                        return false;
            return true;
        }
    }
}