using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.ComponentModel;
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
            string input;
            bool flag;
            int formatUsed = -1;//impossible value

            //main loop
            while (true)
            {
                //get message loop:
                do
                { 
                    flag = true;//reassign flag

                    //request message
                    Coloration(ConsoleColor.Yellow, "Enter Your Message Below:\n\n");
                    Coloration(ConsoleColor.DarkMagenta, "------------------------------------------------------------------------------------------------------------\n\n");
                    Coloration(ConsoleColor.DarkGray, "The Message Must Be In One Of The Following Formats:\n\n");
                    Coloration(ConsoleColor.White, "(1) ");
                    Coloration(ConsoleColor.DarkCyan, "ID:VaccinationCenter:VaccinationDate:VaccineSerialNumber\n\n");
                    Coloration(ConsoleColor.White, "(2) ");
                    Coloration(ConsoleColor.DarkCyan, "VaccineBarcode:VaccinationDate:VaccinationCenter:ID\n\n");

                    Coloration(ConsoleColor.DarkMagenta, "------------------------------------------------------------------------------------------------------------\n\n");
                    Coloration(ConsoleColor.DarkGray, "Keep The Following In Mind:\n\n");
                    Coloration(ConsoleColor.White, "(1) ");
                    Coloration(ConsoleColor.DarkGray, "The ");
                    Coloration(ConsoleColor.DarkYellow, "VaccineBarcode ");
                    Coloration(ConsoleColor.DarkGray, "And ");
                    Coloration(ConsoleColor.DarkYellow, "VaccineSerialNumber ");
                    Coloration(ConsoleColor.DarkGray, "Must Be 12 And 10 Digits Respectively \n    And Comprise Of ONLY Numeric Characters\n");
                    Coloration(ConsoleColor.White, "(2) ");
                    Coloration(ConsoleColor.DarkGray, "The ");
                    Coloration(ConsoleColor.DarkYellow, "IdentificationNumber (ID) ");
                    Coloration(ConsoleColor.DarkGray, "Must Be A Valid SOUTH AFRICAN ID or Passport Number\n");
                    Coloration(ConsoleColor.White, "(3) ");
                    Coloration(ConsoleColor.DarkGray, "The ");
                    Coloration(ConsoleColor.DarkYellow, "VaccinationCenter ");
                    Coloration(ConsoleColor.DarkGray, "Cannot Be Left EMPTY\n");
                    Coloration(ConsoleColor.White, "(4) ");
                    Coloration(ConsoleColor.DarkGray, "The ");
                    Coloration(ConsoleColor.DarkYellow, "VaccinationDate ");
                    Coloration(ConsoleColor.DarkGray, "Must Be In The Format: DD/MM/YYYY\n");
                    Coloration(ConsoleColor.White, "(5) ");
                    Coloration(ConsoleColor.DarkGray, "A Colon (:) Must Be Used To Seperate Each Value In The Message As Shown In The Formats Listed Above\n");

                    Coloration(ConsoleColor.DarkGray, "\n(Type ");
                    Coloration(ConsoleColor.Red, "'Exit' ");
                    Coloration(ConsoleColor.DarkGray, "To Close The Application)");
                    Coloration(ConsoleColor.DarkYellow, "\n\n>> ");

                    //fetch user input
                    input = ReadLine();

                    //check if exit call is made
                    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        //close out console application
                        ExitApplication();
                    }
                    //get the format used (if any) and check if values are valid -- if not error
                    else if ((formatUsed = ValidateMessage(input)) == 2)
                    {
                        //clear the console
                        Clear();
                        //error message if input is out of range
                        ErrorMessage("PLEASE ENSURE THAT YOUR MESSAGE IS IN ONE OF THE BELOW FORMATS\n" +
                                     "AND THAT ALL DATA ENTERED IS CORRECT!");
                    }
                    //end loop
                    else
                    {
                        flag = false;
                    }
                } while (flag);

                //clear the console
                Clear();
                //add message to queue
                await queueManager.AddMessageToQueue(input);
                //call sender animation
                MessageSenderAnimation();
                //output success message
                SuccessMessage();
            }
        }//end main method

        //------------------------------------------------------------------------------------------------------------------------------ValidateMessageInCorrectFormats

        //check if the message is in valid format and return a valaue corresponding to the chosen format
        // 0 : format (1)
        // 1 : format (2)
        // 2 : invalid format
        public static int ValidateMessage(string message)
        {
            //try to process message and determine format
            try
            {
                //break message into parts
                string[] parts = message.Split(':');

                //method to check if string contains only digits:
                Boolean IsAllDigits(string s) => s.All(Char.IsDigit);

                //check if more/or less than 4 parts to the message have been input
                if ((message.Equals(null)) || parts.Length != 4)
                {
                    //no valid format used
                    return 2;
                }
                //in format (1)
                if ((new[] { 0, 1 }.Contains(IdentifyIDNumber(parts[0])))  && //if it starts with either a valid id or passport number
                     (ValidateVaccineSerialNumber(parts[3]) == 0)) //AND ends in a valid VaccineSerialNumber
                {
                    //check if a valid VaccinationCenter has been entered
                    //AND a valid VaccinationDate has been entered
                    if ((ValidateVaccinationCenter(parts[1]) != 0) || 
                        (ValidateVaccinationDate(parts[2]) != 0))
                    {
                        //invalid center or date >> error out
                        return 2;
                    }
                    //valid format of (1)
                    return 0;
                }
                //in foramt (2)
                else if ((ValidateVaccineBarcode(parts[0]) == 0)  &&//if it starts with a valid VaccineBarcode
                     (new[] { 0, 1 }.Contains(IdentifyIDNumber(parts[3]))))//AND ends in either a valid id or passport number
                {
                    //check if a valid VaccinationCenter has been entered
                    //AND a valid VaccinationDate has been entered
                    if ((ValidateVaccinationCenter(parts[2]) != 0) ||
                        (ValidateVaccinationDate(parts[1]) != 0))
                    {
                        //invalid center or date >> error out
                        return 2;
                    }
                    //valid format of (2)
                    return 1;
                }
                //not in a valid format
                else
                {
                    //no valid format used
                    return 2;
                }
            }
            catch (Exception e)
            {
                //no valid format used
                return 2;
            }
        }//end ValidateMessageInCorrectFormats method

        //------------------------------------------------------------------------------------------------------------------------------IdentifyIDNumber

        //method to identify if the query value is a passport number or an id number:
        // 0 : passport number
        // 1 : id number,
        // 2 : unknown value
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
                return !(queryValue.Length == 13) ? 2 : 1;
            }//end if
            else
            {
                //NOT an id number
                //check if passport number: 9 digits
                if (!(queryValue.Length == 9))
                {
                    //unknown value passed
                    return 2;
                }//end if
                else
                {
                    //check if passport number: 9 digits AND 8 numbers
                    if (!(count == 8))
                    {
                        //unknown value passed
                        return 2;
                    }//end if
                    else
                    {
                        //check if passport number: begins with a letter
                        return !Char.IsLetter(queryValue[0]) ? 2 : 0;
                    }//end else
                }//end else
            }//end else
        }//end IdentifyIDNumber method

        //------------------------------------------------------------------------------------------------------------------------------ValidateVaccinationCenter

        private static int ValidateVaccinationCenter(string center)
        {
            //check if input is null
            if (center == "")
            {
                //error out
                return 1;
            }

            //valid center
            return 0;
        }//end ValidateVaccinationCenter method

        //------------------------------------------------------------------------------------------------------------------------------ValidateVaccinationDate

        private static int ValidateVaccinationDate(string date)
        {
            //temptorary date holder for validation
            DateOnly tempDate;

            //check if input is null
            if (date == "")
            {
                //error out
                return 1;
            }
            else if (!DateOnly.TryParse(date, out tempDate))
            {
                //error out
                return 1;
            }

            //valid date
            return 0;
        }//end ValidateVaccinationDate method

        //------------------------------------------------------------------------------------------------------------------------------ValidateVaccineSerialNumber

        private static int ValidateVaccineSerialNumber(string serialNumber)
        {
            //method to check that the serial number only contains digits and no characters
            Boolean IsAllDigits(string s) => s.All(Char.IsDigit);

            //error out
            if (serialNumber == "")
            {
                //error out
                return 1;
            }
            //check if serial number is 10 digits and contains only digits
            else if ((serialNumber.Length != 10) || (IsAllDigits(serialNumber) != true))
            {
                //error out
                return 1;
            }

            //valid serial number
            return 0;
        }//end ValidateVaccineSerialNumber method

        //------------------------------------------------------------------------------------------------------------------------------ValidateVaccineBarcode

        private static int ValidateVaccineBarcode(string barcode)
        {
            //method to check that the barcode number only contains digits and no characters
            Boolean IsAllDigits(string s) => s.All(Char.IsDigit);

            //check if input is null
            if (barcode == "")
            {
                //error out
                return 1;
            }
            //check if barcode number is 12 digits and contains only digits
            else if ((barcode.Length != 12) || IsAllDigits(barcode) != true)
            {
                //error out
                return 1;
            }

            //valid barcode number
            return 0;
        }//end ValidateVaccineBarcode method 

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

        //------------------------------------------------------------------------------------------------------------------------------MessageSenderAnimation

        protected static void MessageSenderAnimation()
        {
            //output colorised message
            Coloration(ConsoleColor.DarkYellow, "Sending Message To Queue");

            //output colorized animation
            for (int i = 0; i < 3; i++)
            {
                //output colorized char
                Coloration(ConsoleColor.DarkYellow, ".");
                //wait
                Thread.Sleep(500);

            }//end for-loop

            //wait
            Thread.Sleep(500);
        }//end MessageSenderAnimation method

        //------------------------------------------------------------------------------------------------------------------------------SuccessMessage

        protected static void SuccessMessage()
        {
            //clear console
            Clear();

            //success message
            Coloration(ConsoleColor.DarkGreen, "Message added to the queue successfully!");
            //output hint
            Coloration(ConsoleColor.DarkGray, "\n\nPress any key to proceed...");
            //wait dor user interaction
            ReadKey();

            //clear console
            Clear();
        }//end SuccessMessage method

        //------------------------------------------------------------------------------------------------------------------------------ErrorMessage

        private static void ErrorMessage(string message)
        {
            //output error with given message
            Coloration(ConsoleColor.Red, "\nERROR: ");
            Coloration(ConsoleColor.White, message + "\n\n");
            Coloration(ConsoleColor.DarkGray, "============================================================\n\n\n");
        }//end ErrorMessage method

        //------------------------------------------------------------------------------------------------------------------------------ExitApplication

        //safely close application
        protected static void ExitApplication()
        {
            //clear console
            Clear();
            //output colorized message
            Coloration(ConsoleColor.Red, "Exiting Program");

            //output colorized animation
            for (int i = 0; i < 3; i++)
            {

                //output colorized char
                Coloration(ConsoleColor.Red, ".");
                //wait
                Thread.Sleep(500);

            }//end for-loop

            //wait
            Thread.Sleep(500);
            //end program
            Environment.Exit(0);

        }//end ExitApplication method
    }
}
//__________________________________...oooOOO000_End_Of_File_000OOOooo...__________________________________