using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace cldv6212_part_2_function
{
    public class DatabaseFunc
    {
        [FunctionName("ProcessQueueMessage")]
        public void Run(
            [QueueTrigger("cldv6212-poe-part-2", Connection = "AzureCloudDevelopmentPOEStorage")]string message, 
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {message}");

            string[] messageParts = message.Split('|');
        }
    }
}
