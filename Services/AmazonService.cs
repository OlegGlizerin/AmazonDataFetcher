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

            var key = Constants.GetAmazonKey(forecast);

            if(!KeyAlreadyDownloaded(key))
            {
                var amazonClientRequest = new AmazonClientRequest
                {
                    Date = request.Date.RemoveHours(),
                    Forecast = forecast
                };
                await _amazonClient.GetFileByDate(amazonClientRequest).ConfigureAwait(false);
            }

            var kalvins = _wGribService.CalcKelvins(key);

            Console.WriteLine($"Kalvins calculated: {kalvins}\n");
            Console.WriteLine("Finished, Click any key and enter to close this window...\n");
            Console.ReadLine();

            if (!string.IsNullOrEmpty(kalvins)) 
            {
                WriteToFile(KalvinsToCelcius(kalvins).ToString());
            }
            else
            {
                HandleError(kalvins);
            }
            return key;
        }

        private void HandleError(string kalvins)
        {
            WriteToFile($"Something wrong happened - Result: {kalvins}");
            Console.WriteLine($"Something wrong happened - Result: {kalvins}\n");
            Console.WriteLine("Click any key and enter to continue...\n");
            Console.ReadLine();
        }

        private int KalvinsToCelcius(string kalvins)
        {
            return (int)Convert.ToDouble(kalvins) - 273;
        }

        private void WriteToFile(string data)
        {
            File.WriteAllText("Success.txt", data);
        }

        private bool KeyAlreadyDownloaded(string key)
        {
            if(File.Exists(Constants.GetDestinationFolder(key)))
            {
                Console.WriteLine($"The Key: {key} Already Downloaded in folder: {Constants.GetDestinationFolder(key)}");
                return true;
            }
            Console.WriteLine($"The Key: {key} is not Downloaded, will proceed to download process. To folder: {Constants.GetDestinationFolder(key)}");
            return false;
        }
    }
}
