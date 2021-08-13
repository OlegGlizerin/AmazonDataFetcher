using Contracts.ExternalContracts;
using Contracts.ServiceContracts;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAmazonService
    {
        public Task<string> DownloadFileFromAmazon(AmazonServiceReq request);
    }
}
