﻿using Azure.Storage.Queues;

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
            // byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            //string base64message = Convert.ToBase64String(data);

            //check if queue exsists and if not create one
            if (!await queueClient.ExistsAsync())
            {
                await queueClient.CreateAsync();
            }
            
            //send the message to the queue
            await queueClient.SendMessageAsync(message);
        }//end AddMessageToQueue method
    }
}
//__________________________________...oooOOO000_End_Of_File_000OOOooo...__________________________________