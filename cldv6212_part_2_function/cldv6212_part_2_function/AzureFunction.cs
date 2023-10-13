using System;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace cldv6212_part_2_function
{
    public class AzureFunction
    {
        [FunctionName("VaccDataToSQLFunc")]
        public void Run([QueueTrigger("cldv6212-poe-part-2", Connection = "connectionString")]string myQueueItem, ILogger log)
        {
            //log a successful trigger request and process
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            //create a data model of captured message
            var data = VaccinationDataModel.FromMessage(myQueueItem);

            //add message data to sql database and log result (error/success message)
            log.LogInformation(AddToDatabase(data));
        }//end function call

        //------------------------------------------------------------------------------------------------------------------------------AddToDatabase

        //add the data captured to the specified Azure SQL Database
        public static string AddToDatabase(VaccinationDataModel data)
        {
            //connection string
            string connectionString;
            //sql connection
            SqlConnection con;

            connectionString = "Server=tcp:dbserver-vc-cldv6212-st10036509.database.windows.net,1433;Initial Catalog=db-vc-cldv6212-st10036509;Persist Security Info=False;User ID=ST10036509;Password=Randomsangh72;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (con = new SqlConnection())
            {
                //error check connection
                try
                {
                    //link connection and connection string
                    con.ConnectionString = connectionString;
                    //open connection
                    con.Open();

                    //create sql command, adapter and string
                    SqlCommand command;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    String sql = "";

                    //sql string to insert data into Vaccination_Data table
                    sql = $"INSERT INTO VACCINATION_DATA (identificationNumber, vaccineBarcode, vaccinationCenter, vaccinationDate, vaccineSerialNumber) " +
                          $"VALUES ('" + data.Id + "','" + data.VaccineBarcode + "','" + data.VaccinationCenter + "','" + data.VaccincationDate + "','" + data.VaccineSerialNumber + "')";

                    //create command
                    command = new SqlCommand(sql, con);
                    //create adapter
                    adapter.InsertCommand = command;
                    //execute command
                    adapter.InsertCommand.ExecuteNonQuery();
                    //dispose of command
                    command.Dispose();

                    //return success message for log
                    return "SUCCESSFULLY ADDED MESSAGE CONTENT TO THE DATABASE!";
                }
                catch (Exception ex)//catch exception
                {
                    //if connection is open close it
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }//end if
                    //error ocurred -- send error message for log
                    return "AN ERROR OCCURED WHEN ADDING THE MESSAGE CONTENT TO THE DATABASE!\n\n" + ex;
                }//end catch
                finally//finally
                {
                    //if connection is open close it
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }//end if
                }//end finally
            }
        }//end AddToDatabase method
    }
}
//__________________________________...oooOOO000_End_Of_File_000OOOooo...__________________________________