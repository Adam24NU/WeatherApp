using WeatherApp.Models;

namespace WeatherApp.Repositories
{
    public interface ISiteRepository
    {
        Task<Site> GetSiteByTypeAsync(string type);
    }
}
