using Contracts.ServiceContracts;
using ExternalClients;
using ExternalClients.Interfaces;
using Services;
using Services.Helpers;
using Services.Interfaces;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace IceTestTask
{
    public class Program
    {
        private static IAmazonService _amazonService = new AmazonService(new AmazonClient(), new DateForecastService(), new WGribService(), new ConsoleWrapper());

        public async static Task Main(string[] args)
        {
            try
            {
                await _amazonService.DownloadFileFromAmazon(new AmazonServiceReq
                {
                    Date = args[0] + " " + args[1]
                }).ConfigureAwait(false);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error in the program - ex: {e}");
                Console.ReadLine();
            }
        }
    }
}
