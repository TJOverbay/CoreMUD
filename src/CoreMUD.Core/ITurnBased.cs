using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMUD.Core
{
    interface ITurnBased
    {
        void StartTurn();
        bool IsInTurn { get; }
        void ResetTurns();
        int TotalTurnCount { get; }
    }
}
