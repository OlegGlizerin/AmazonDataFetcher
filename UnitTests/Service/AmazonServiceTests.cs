using Contracts.ExternalContracts;
using Contracts.ServiceContracts;
using ExternalClients.Interfaces;
using FakeItEasy;
using IceTestTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using Services.Extentions;
using Services.Helpers;
using Services.Interfaces;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class AmazonServiceTests
    {
        private IAmazonService _amazonService;
        private IAmazonClient _amazonClient;
        private IDateForecastService _dateForecastService;
        private IWGribService _wGribService;
        private IConsole _console;

        private AmazonServiceReq _amazonServiceRequest;
        private AmazonClientRequest _amazonClientRequest;
        private readonly string _correctDate = "2021/12/12 12:12";
        private readonly string _forecastRes = "015";
        private readonly string _gribResKelvins = "223";

        [TestInitialize]
        public void TestInit()
        {
            _amazonClient = A.Fake<IAmazonClient>();
            _dateForecastService = A.Fake<IDateForecastService>();
            _wGribService = A.Fake<IWGribService>();
            _console = A.Fake<IConsole>();

            _amazonService = new AmazonService(_amazonClient, _dateForecastService, _wGribService, _console);

            _amazonServiceRequest = new AmazonServiceReq
            {
                Date = _correctDate
            };

            _amazonClientRequest = new AmazonClientRequest
            {
                Date = _correctDate,
                Forecast = _forecastRes
            };
        }

        [TestMethod]
        public async Task AmazonService_DownloadFileFromAmazonHappyFlow_SuccssfullyDownloaded()
        {
            //Arrange
            var fileNameForGrib = Constants.GetAmazonKey(_forecastRes);
            A.CallTo(() => _dateForecastService.CalcForecast(_correctDate))
               .Returns(_forecastRes);
            A.CallTo(() => _amazonClient.GetFileByDate(A<AmazonClientRequest>.Ignored)).DoesNothing();
            A.CallTo(() => _wGribService.CalcKelvins(fileNameForGrib))
               .Returns(_gribResKelvins);
            A.CallTo(() => _console.ReadLine()).Returns("");

            //Act
            var res = await _amazonService.DownloadFileFromAmazon(_amazonServiceRequest).ConfigureAwait(false);

            //Assert
            A.CallTo(() => _dateForecastService.CalcForecast(A<string>.That
                .Matches(x => x == _correctDate)))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _amazonClient.GetFileByDate(A<AmazonClientRequest>.That
                .Matches(x => x.Date == _amazonClientRequest.Date.RemoveHoursAndSlashes() && x.Forecast == _amazonClientRequest.Forecast)))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _wGribService.CalcKelvins(A<string>.That
                .Matches(x => x == fileNameForGrib)))
                .MustHaveHappenedOnceExactly();
            Assert.AreEqual(Constants.GetAmazonKey(_forecastRes), res);
        }
    }
}
