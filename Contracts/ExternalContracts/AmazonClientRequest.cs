using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.ExternalContracts
{
    public class AmazonClientRequest
    {
        public string Date { get; set; }
        public string Forecast { get; set; }
    }
}
