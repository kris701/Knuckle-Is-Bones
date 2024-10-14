using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnuckleBones.Core.Models.Game.Opponents
{
    public interface ICPUOpponent : IOpponent
    {
        public void SetTargetColumn(BoardDefinition board);
    }
}