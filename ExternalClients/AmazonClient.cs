using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Contracts.ExternalContracts;
using Contracts.ServiceContracts;
using ExternalClients.Interfaces;
using IceTestTask;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ExternalClients
{
    public class AmazonClient : IAmazonClient
    {
        public async Task GetFileByDate(AmazonClientRequest request)
        {
            var key = Constants.GetAmazonKey(request.Forecast);
            var bucketName = Constants.GetBucketName(request.Date);
            var destinationFolder = Constants.GetDestinationFolder(key);

            using (IAmazonS3 client = new AmazonS3Client(Constants.AccessKey, Constants.SecretKey, Amazon.RegionEndpoint.USEast1))
            {
                try
                {
                    var response = await client.GetObjectAsync(new GetObjectRequest { BucketName = bucketName, Key = key });

                    response.WriteObjectProgressEvent += Response_WriteObjectProgressEvent;
                    await response.WriteResponseStreamToFileAsync(destinationFolder, true, CancellationToken.None).ConfigureAwait(false);
                }
                catch (AmazonServiceException e)
                {
                    Console.WriteLine($"Error in GetFileByDate, Ex: {e}");
                    return key;
                }
            }
            Console.WriteLine($"File {key} Downloaded Successfully!");
            Console.ReadLine();
            Console.WriteLine("Click any key to continue...");
        }

        private static void Response_WriteObjectProgressEvent(object sender, WriteObjectProgressArgs e)
        {
            ConsoleUtility.WriteProgressBar(e.PercentDone, true);
            Console.WriteLine($"Tansfered: {e.TransferredBytes}/{e.TotalBytes} - Progress: {e.PercentDone}%");
        }

        

        public static class ConsoleUtility
        {
            const char _block = '■';
            const string _back = "\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b";
            const string _twirl = "-\\|/";
            public static void WriteProgressBar(int percent, bool update = false)
            {
                if (update)
                    Console.Write(_back);
                Console.Write("[");
                var p = (int)((percent / 10f) + .5f);
                for (var i = 0; i < 10; ++i)
                {
                    if (i >= p)
                        Console.Write(' ');
                    else
                        Console.Write(_block);
                }
                Console.Write("] ");
            }
            public static void WriteProgress(int progress, bool update = false)
            {
                if (update)
                    Console.Write("\b");
                Console.Write(_twirl[progress % _twirl.Length]);
            }
        }


    }
}
