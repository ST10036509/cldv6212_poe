using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace cldv6212_part_2_function_app
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run(
            [QueueTrigger("cldv6212-poe-part-2", Connection = "AzureCldvStorage")]string message, 
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {message}");
        }
    }
}
