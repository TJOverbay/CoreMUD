using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMUD.Model
{
    public class GameTime
    {
        public TimeSpan TotalGametime { get; set; }
        public TimeSpan ElapsedGametime { get; set; }

        public bool IsRunningSlowly { get; set; }

        public GameTime()
        {
            TotalGametime = TimeSpan.Zero;
            ElapsedGametime = TimeSpan.Zero;
            IsRunningSlowly = false;
        }

        public GameTime(TimeSpan totalGametime, TimeSpan elapsedGameTime)
        {
            TotalGametime = totalGametime;
            ElapsedGametime = elapsedGameTime;
            IsRunningSlowly = false;
        }

        public GameTime(TimeSpan totalRealTime, TimeSpan elapsedRealTime, bool isRunningSlowly)
        {
            TotalGametime = totalRealTime;
            ElapsedGametime = elapsedRealTime;
            IsRunningSlowly = isRunningSlowly;
        }
    }
}
