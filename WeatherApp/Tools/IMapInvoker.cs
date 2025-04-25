using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Tools
{
    public interface IMapInvoker
    {
        Task UpdateMapLocationAsync(double latitude, double longitude);
    }
}
