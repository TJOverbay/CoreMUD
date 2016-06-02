using System.Collections.Generic;
using CoreMUD.Model;

namespace CoreMUD.Interfaces
{
    public interface IHaveStats
    {
        IDictionary<string, GameStat> Stats { get; set; }
    }
}
