namespace SupermarketScraper.Demo.Services
{
    public interface IRandomWaitTimeService
    {
        int GetRandomLongWaitTime();
        int GetRandomShortWaitTime();
    }
}
