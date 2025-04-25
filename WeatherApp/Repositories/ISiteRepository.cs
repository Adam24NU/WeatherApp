using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Repositories
{
    public interface ISiteRepository
    {
        Task<Site> GetSiteByTypeAsync(string type);
    }
}
