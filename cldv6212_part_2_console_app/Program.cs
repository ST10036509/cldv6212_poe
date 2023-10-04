using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Diagnostics.Metrics;
using System.Numerics;
using static System.Console;

namespace cldv6212_part_2_console_app
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            //declare queue variables:
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=azjhbrsgvcwccnst10099da;AccountKey=EhL5fvzUJhvvNoKZirNP3ZWOgsuEtopqvtoo51XWKStt6IZfxog9dFg38cVEu4BEWEQRxXYTiSRm+AStyt0u4Q==;EndpointSuffix=core.windows.net";
            string queueName = "cldv6212-poe-part-2";

            //create queue manager
            var queueManager = new QueueManager(connectionString, queueName);


            //declare variables:
            string input = "";
            string message = "";
            bool flag;

            //main program loop:
            do
            {
                flag = true;//reassign flag

                //request message
                Coloration(ConsoleColor.White, "What Would You Like To Use To Proceed:\n\n");
                Coloration(ConsoleColor.DarkYellow, "(1) ");
                Coloration(ConsoleColor.Cyan, "Identification Number");
                Coloration(ConsoleColor.DarkGray, " (either a valid SOUTH AFRICAN ID or SOUTH AFRICAN Passport Number)\n");
                Coloration(ConsoleColor.DarkYellow, "(2) ");
                Coloration(ConsoleColor.Cyan, "Vaccination Barcode");
                Coloration(ConsoleColor.DarkGray, "\n\n\n(Enter The Number Corresponding With Your Choice OR Type 'Exit' To Close The Application)");
                Coloration(ConsoleColor.DarkYellow, "\n>> ");

                //fetch user input
                input = ReadLine();

                //check if input is null
                if (input == "")
                {
                    //error message if input is null
                    ErrorMessage("PLEASE ENTER A VALID INPUT!");
                }
                //check if exit call is made
                else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    //close out console application
                    Environment.Exit(0);
                }
                //input out of option range
                else if ((input != "1") && (input != "2"))
                {
                    //error message if input is out of range
                    ErrorMessage("PLEASE PICK A VALID OPTION FROM THE ONES ABOVE!");
                }
                //end loop
                else
                {
                    flag = false;
                }
            } while (flag);

            //id number picked
            if (input == "1")
            {
                //validate id number loop:
                input = ValidateIDNumberLoop();
                //add id to message
                message += input;

                //validate vaccination center loop:
                input = ValidateVaccinationCenterLoop();
                //add vaccination center to message
                message += "|" + input;

                //validate vaccination date loop:
                input = ValidateVaccinationDateLoop();
                //add vaccination date to message
                message += "|" + input;

                //validate vaccination number loop:
                input = ValidateVaccinationNumberLoop();
                //add vaccination number to message
                message += "|" + input;

                //add message to queue
                await queueManager.AddMessageToQueue(message);
                //success message
                Coloration(ConsoleColor.DarkGreen, "Message added to the queue successfully!");
            }
            //vaccination barcode picked
            else if (input == "2")
            {
                //validate vaccination barcode loop:
                input = ValidateVaccinationBarcodeLoop();
                //add vaccination barcode to message
                message += input;

                //validate vaccination date loop:
                input = ValidateVaccinationDateLoop();
                //add vaccination date to message
                message += "|" + input;

                //validate vaccination center loop:
                input = ValidateVaccinationCenterLoop();
                //add vaccination center to message
                message += "|" + input;

                //validate id number loop:
                input = ValidateIDNumberLoop();
                //add id to message
                message += "|" + input;

                //add message to queue
                await queueManager.AddMessageToQueue(message);
                //success message
                Coloration(ConsoleColor.DarkGreen, "\n\nMessage added to the queue successfully!\n\n");
            }
        }//end main method

        //------------------------------------------------------------------------------------------------------------------------------IdentifyIDNumber

        //method to identify if the query value is a passport number or an id number:
        // 1 : passport number
        // 2 : id number,
        // 3 : unknown value
        private static int IdentifyIDNumber(string queryValue)
        {
            //method to check if string contains only digits:
            Boolean IsAllDigits(string s) => s.All(Char.IsDigit);

            //method to count the number of numeric digits in the string:
            var count = queryValue.Count(x => Char.IsDigit(x));

            //check if passport number: contains letters AND numbers
            if (IsAllDigits(queryValue))
            {
                //NOT a passport number
                //check if id number: 13 digits
                return !(queryValue.Length == 13) ? 3 : 2;
            }//end if
            else
            {
                //NOT an id number
                //check if passport number: 9 digits
                if (!(queryValue.Length == 9))
                {
                    //unknown value passed
                    return 3;
                }//end if
                else
                {
                    //check if passport number: 9 digits AND 8 numbers
                    if (!(count == 8))
                    {
                        //unknown value passed
                        return 3;
                    }//end if
                    else
                    {
                        //check if passport number: begins with a letter
                        return !Char.IsLetter(queryValue[0]) ? 3 : 1;
                    }//end else
                }//end else
            }//end else
        }//end IdentifyIDNumber method

        //------------------------------------------------------------------------------------------------------------------------------ValidateIDNumberLoop

        private static string ValidateIDNumberLoop()
        {
            //declare variables:
            bool flag;
            string input = "";

            do
            {
                flag = true;//reassign flag

                //request message
                Coloration(ConsoleColor.White, "\n\nEnter A Valid ");
                Coloration(ConsoleColor.Cyan, "SOUTH AFRICAN IDENTIFICATION ");
                Coloration(ConsoleColor.White, "or ");
                Coloration(ConsoleColor.Cyan, "SOUTH AFRICAN PASSPORT ");
                Coloration(ConsoleColor.White, "Number:");
                Coloration(ConsoleColor.DarkYellow, "\n>> ");

                //fetch user input
                input = ReadLine();

                if (input == "")
                {
                    //error message if input is null
                    ErrorMessage("PLEASE ENTER A VALID INPUT!");
                }
                //check if exit call is made
                else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    //close out console application
                    Environment.Exit(0);
                }
                //validate input as either a ID or Passport number
                else if (IdentifyIDNumber(input) == 3)
                {
                    //error message if input is null
                    ErrorMessage("THE ENTERED VALUE IS NOT A VALID SOUTH AFRICAN ID OR PASSPORT NUMBER!");
                }
                //end loop
                else
                {
                    flag = false;
                }
            } while (flag);

            return input;
        }//end ValidateIDNumberLoop method

        //------------------------------------------------------------------------------------------------------------------------------ValidateVaccinationCenterLoop

        private static string ValidateVaccinationCenterLoop()
        {
            //declare variables:
            bool flag;
            string input = "";

            do
            {
                flag = true;//reassign flag

                //request message
                Coloration(ConsoleColor.White, "\n\nEnter A ");
                Coloration(ConsoleColor.Cyan, "VACCINATION CENTER");
                Coloration(ConsoleColor.White, ":");
                Coloration(ConsoleColor.DarkYellow, "\n>> ");

                //fetch user input
                input = ReadLine();

                //check if input is null
                if (input == "")
                {
                    //error message if input is null
                    ErrorMessage("PLEASE ENTER A VALID INPUT!");
                }
                //check if exit call is made
                else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    //close out console application
                    Environment.Exit(0);
                }
                //end loop
                else
                {
                    flag = false;
                }
            } while (flag);

            return input;
        }//end ValidateVaccinationCenterLoop method

        //------------------------------------------------------------------------------------------------------------------------------ValidateVaccinationDateLoop

        private static string ValidateVaccinationDateLoop()
        {
            //declare variables:
            bool flag;
            string input = "";

            do
            {
                flag = true;//reassign flag

                //temp variable to store date
                DateTime dateValue;

                //request message
                Coloration(ConsoleColor.White, "\n\nEnter A ");
                Coloration(ConsoleColor.Cyan, "VACCINATION DATE ");
                Coloration(ConsoleColor.DarkGray, "(MM/DD/YYYY)");
                Coloration(ConsoleColor.White, ":");
                Coloration(ConsoleColor.DarkYellow, "\n>> ");

                //fetch user input
                input = ReadLine();

                //check if input is null
                if (input == "")
                {
                    //error message if input is null
                    ErrorMessage("PLEASE ENTER A VALID INPUT!");
                }
                //check if exit call is made
                else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    //close out console application
                    Environment.Exit(0);
                }
                else if (!DateTime.TryParse(input, out dateValue))
                {
                    //error message if input is null
                    ErrorMessage("PLEASE ENTER A VALID DATE!");
                }
                //end loop
                else
                {
                    flag = false;
                }
            } while (flag);

            return input;
        }//end ValidateVaccinationCenterLoop method

        //------------------------------------------------------------------------------------------------------------------------------ValidateVaccinationNumberLoop

        private static string ValidateVaccinationNumberLoop()
        {
            //declare variables:
            bool flag;
            string input = "";

            do
            {
                flag = true;//reassign flag

                //request message
                Coloration(ConsoleColor.White, "\n\nEnter A ");
                Coloration(ConsoleColor.Cyan, "VACCINATION NUMBER ");
                Coloration(ConsoleColor.DarkGray, ":");
                Coloration(ConsoleColor.DarkYellow, "\n>> ");

                //fetch user input
                input = ReadLine();

                //check if input is null
                if (input == "")
                {
                    //error message if input is null
                    ErrorMessage("PLEASE ENTER A VALID INPUT!");
                }
                //check if exit call is made
                else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    //close out console application
                    Environment.Exit(0);
                }
                else if (input.Length != 10)
                {
                    //error message if input is null
                    ErrorMessage("PLEASE ENTER A VALID VACCINATION NUMBER!");
                }
                //end loop
                else
                {
                    flag = false;
                }
            } while (flag);

            return input;
        }//end ValidateVaccinationCenterLoop method

        //------------------------------------------------------------------------------------------------------------------------------ValidateVaccinationBarcodeLoop

        private static string ValidateVaccinationBarcodeLoop()
        {
            //declare variables:
            bool flag;
            string input = "";

            do
            {
                flag = true;//reassign flag

                //request message
                Coloration(ConsoleColor.White, "\n\nEnter A ");
                Coloration(ConsoleColor.Cyan, "VACCINATION BARCODE ");
                Coloration(ConsoleColor.DarkGray, ":");
                Coloration(ConsoleColor.DarkYellow, "\n>> ");

                //fetch user input
                input = ReadLine();

                //check if input is null
                if (input == "")
                {
                    //error message if input is null
                    ErrorMessage("PLEASE ENTER A VALID INPUT!");
                }
                //check if exit call is made
                else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    //close out console application
                    Environment.Exit(0);
                }
                else if (input.Length != 13)
                {
                    //error message if input is null
                    ErrorMessage("PLEASE ENTER A VALID VACCINATION NUMBER!");
                }
                //end loop
                else
                {
                    flag = false;
                }
            } while (flag);

            return input;
        }//end ValidateVaccinationCenterLoop method 

        //------------------------------------------------------------------------------------------------------------------------------Coloration

        //method to output colorized text
        private static void Coloration(ConsoleColor color, string message)
        {
            //variable declarations:
            var originalColor = Console.ForegroundColor;

            //keep track of original color
            Console.ForegroundColor = color;
            //print message with new color
            Console.Write(message);
            //return console to oringal color
            Console.ForegroundColor = originalColor;

        }//end Coloration method

        //------------------------------------------------------------------------------------------------------------------------------ErrorMessage

        private static void ErrorMessage(string message)
        {
            //output error with given message
            Coloration(ConsoleColor.Red, "\nERROR: ");
            Coloration(ConsoleColor.White, message + "\n\n");
            Coloration(ConsoleColor.DarkGray, "============================================================\n\n\n");
        }//end ErrorMessage method
    }
}
//__________________________________...oooOOO000_End_Of_File_000OOOooo...__________________________________