using Azure.Storage.Queues;

namespace cldv6212_part_2_console_app
{
    public class QueueManager
    {
        //GLOBAL variable declarations:
        private QueueClient queueClient;

        //create a new client object for the given queue infomration
        public QueueManager(string connectionString, string queueName)
        {
            //build the queue client object
            queueClient = new QueueClient(connectionString, queueName);
        }//end constructor

        public async Task AddMessageToQueue(string message)
        {
            //create the queue if it doesn't exist
            await queueClient.CreateAsync();
            //send the message to the queue
            await queueClient.SendMessageAsync(message);
        }//end AddMessageToQueue method
    }
}
//__________________________________...oooOOO000_End_Of_File_000OOOooo...__________________________________