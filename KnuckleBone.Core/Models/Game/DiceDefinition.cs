using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnuckleBone.Core.Models.Game
{
    public class DiceDefinition
    {
        private Random _rnd = new Random();

        public int Sides { get; set; }

        private int _value = -1;

        public int Value
        {
            get
            {
                if (_value != -1)
                    return _value;

                _value = _rnd.Next(1, _value + 1);

                return _value;
            }
        }
    }
}