using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDateForecastService
    {
        public string CalcForecast(string date);
    }
}
