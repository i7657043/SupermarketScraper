using System;

namespace SupermarketScraper.Demo.Services
{
    internal class RandomWaitTimeService : IRandomWaitTimeService
    {
        private readonly Random _random;
        private int _shortMinWait;
        private int _shortMaxWait;
        private int _longMinWait;
        private int _longMaxWait;

        public RandomWaitTimeService(int shortMinWait, int shortMaxWait, int longMinWait, int longMaxWait)
        {
            _shortMinWait = shortMinWait;
            _shortMaxWait = shortMaxWait;
            _longMinWait = longMinWait;
            _longMaxWait = longMaxWait;
            _random = new Random();
        }

        public int GetRandomLongWaitTime() => _random.Next(_longMinWait, _longMaxWait);
        public int GetRandomShortWaitTime() => _random.Next(_shortMinWait, _shortMaxWait);
    }
}
