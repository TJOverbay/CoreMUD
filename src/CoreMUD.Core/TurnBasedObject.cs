using System;
using App.Common;

namespace CoreMUD.Core
{
    public abstract class TurnBasedObject : ITurnBased, INamedObject
    {
        private int _totalTurnCount;
        private bool _isInTurn;

        bool ITurnBased.IsInTurn
        {
            get
            {
                return _isInTurn;
            }
        }

        int ITurnBased.TotalTurnCount
        {
            get
            {
                return _totalTurnCount;
            }
        }

        protected string Name { get; set; }

        void ITurnBased.EndTurn()
        {
            if (!_isInTurn)
            {
                throw new InvalidOperationException(
                    $"This {Name.ToStringOrDefault("object")} is not in a turn");
            }

            _isInTurn = false;
            _totalTurnCount++;
        }

        void ITurnBased.StartTurn()
        {
            if (_isInTurn)
            {
                throw new InvalidOperationException(
                    $"This {Name.ToStringOrDefault("object")} is already in a turn");
            }

            _isInTurn = true;
        }

        void ITurnBased.ResetTurns()
        {
            _totalTurnCount = 0;
            _isInTurn = false;
        }
    }
}
