using WeatherApp.Core.Models;

namespace WeatherApp.Core.Repositories
{
    public interface ISiteRepository
    {
        Task<Site> GetSiteByTypeAsync(string type);
    }
}
