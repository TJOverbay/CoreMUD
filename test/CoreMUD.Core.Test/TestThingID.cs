using System.Threading;

namespace CoreMUD.Core.Test
{
    static class TestThingID
    {
        private static int currentID = 0;

        public static string Generate(string thingType)
        {
            return $"{thingType}/{Interlocked.Increment(ref currentID)}";
        }
    }
}
