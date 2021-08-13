using Contracts.ExternalContracts;
using Contracts.ServiceContracts;
using ExternalClients.Interfaces;
using IceTestTask;
using Services.Extentions;
using Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Services
{
    public class AmazonService : IAmazonService
    {
        private IAmazonClient _amazonClient;
        private IDateForecastService _dateForecastService;
        private IWGribService _wGribService;

        public AmazonService(IAmazonClient amazonClient, IDateForecastService dateForecastService, IWGribService wGribService)
        {
            _amazonClient = amazonClient;
            _dateForecastService = dateForecastService;
            _wGribService = wGribService;
        }

        public async Task<string> DownloadFileFromAmazon(AmazonServiceReq request)
        {
            var forecast = _dateForecastService.CalcForecast(request.Date);

            var amazonClientRequest = new AmazonClientRequest
            {
                Date = request.Date.RemoveHours(),
                Forecast = forecast
            };

            var key = await _amazonClient.GetFileByDate(amazonClientRequest).ConfigureAwait(false);

            var res = _wGribService.CalcKelvins(key);

            Console.WriteLine($"Kalvins calculated: {res}\n");
            Console.WriteLine("Click any key to continue...\n");
            Console.ReadLine();

            if (!string.IsNullOrEmpty(res)) 
            {
                WriteToFile(((int)Convert.ToDouble(res) - 273).ToString());
            }
            else
            {
                WriteToFile($"Something wrong happened - Result: {res}");
                Console.WriteLine($"Something wrong happened - Result: {res}\n");
                Console.WriteLine("Click any key to continue...\n");
                Console.ReadLine();
            }

            return key;
        }

        private void WriteToFile(string data)
        {
            File.WriteAllText("Success.txt", data);
        }
    }
}
