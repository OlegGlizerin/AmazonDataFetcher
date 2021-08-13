using Contracts.ExternalContracts;
using Contracts.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExternalClients.Interfaces
{
    public interface IAmazonClient
    {
        public Task<string> GetFileByDate(AmazonClientRequest request);
    }
}
