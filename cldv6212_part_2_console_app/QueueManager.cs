﻿using Azure.Storage.Queues;

namespace cldv6212_part_2_console_app
{
    public class QueueManager
    {
        //GLOBAL variable declarations:
        private QueueClient queueClient;

        //------------------------------------------------------------------------------------------------------------------------------constructor

        //create a new client object for the given queue infomration
        public QueueManager(string connectionString, string queueName)
        {
            //build the queue client object
            queueClient = new QueueClient(connectionString, queueName);
        }//end constructor

        //------------------------------------------------------------------------------------------------------------------------------AddMessageToQueue

        //add a given message to the specified queue
        public async Task AddMessageToQueue(string message)
        {
            //check if queue exsists and if not create one
            if (!await queueClient.ExistsAsync())
            {
                await queueClient.CreateAsync();
            }
            
            //send the message to the queue
            await queueClient.SendMessageAsync(Base64Encode(message));
        }//end AddMessageToQueue method

        //------------------------------------------------------------------------------------------------------------------------------Base64Encode

        //convert input to base64
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }//end Base64Encode method
    }
}
//__________________________________...oooOOO000_End_Of_File_000OOOooo...__________________________________