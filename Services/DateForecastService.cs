using IceTestTask;
using Services.Interfaces;
using Services.UserDefinedExceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DateForecastService : IDateForecastService
    {
        public string CalcForecast(string date)
        {
            var userDate = DateTime.ParseExact(date, Constants.DateFormat, CultureInfo.InvariantCulture);
            var currentDate = DateTime.Now;
            TimeSpan ts = userDate - currentDate;

            var forecast = Math.Abs((int)ts.TotalHours);

            if(forecast < 0 || forecast > 120)
            {
                throw new ForecastRangeException("Forecast should be between 0 <= forecast <= 120");
            }

            return forecast.ToString(Constants.DatePadding);
        }

    }
}
