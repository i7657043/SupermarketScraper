using System.Threading.Tasks;

namespace SupermarketScraper.Demo
{
    public interface ISupermarketScraper
    {
        Task<int> RunDemoAsync();
    }
}
