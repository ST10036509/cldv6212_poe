using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
//using Microsoft.Extensions.Logging;
using cldv6212_part_2_function;

namespace cldv6212_part_2_function
{
    public class AzureFuntion
    {
        [FunctionName("VaccinationRecordsFunction")]
        public void Run([QueueTrigger("prog6212-ice-task-5", Connection = "AzureWebJobsStorage")]string message/*, ILogger log*/)
        {
            
            var data = VaccinationDataModel.FromMessage(message);

            var dbContext = new VaccinationDataContext();
            dbContext.VaccinationData.Add(data);
        }
    }
}
