using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace cldv6212_part_2_function
{
    //Data Source=LAPTOP-LMRG02CV;Initial Catalog=VaccinationData;Integrated Security=True

    public class DatabaseFunc
    {
        [FunctionName("ProcessQueueMessage")]
        public void Run(
            [QueueTrigger("cldv6212-poe-part-2", Connection = "AzureCloudDevelopmentPOEStorage")]string message, 
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {message}");

            //create Data Model of message
            VaccinationDataModel vacData = VaccinationDataModel.FromMessage(message);
        }
    }
}
//__________________________________...oooOOO000_End_Of_File_000OOOooo...__________________________________