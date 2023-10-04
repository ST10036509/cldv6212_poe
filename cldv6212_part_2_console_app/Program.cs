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
                Coloration(ConsoleColor.Cyan, "Vaccine Barcode");
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
                flag = true;

                do
                {
                    //request message in Format(1)
                    Coloration(ConsoleColor.White, "\n\nEnter A ");
                    Coloration(ConsoleColor.Cyan, "message ");
                    Coloration(ConsoleColor.White, "In The Following Format: ");
                    Coloration(ConsoleColor.DarkYellow, "ID:Vaccination Center:Vaccination Date:Vaccine Serial Number\n\n");
                    Coloration(ConsoleColor.DarkGray, "Keep In Mind That The Following Must Be Adheared by:\n\n");
                    Coloration(ConsoleColor.DarkYellow, "(1) "); ;
                    Coloration(ConsoleColor.DarkGray, "The ID Must Be A Valid SOUTH AFRICAN ID Or SOUTH AFRICAN PASSPORT Number\n");
                    Coloration(ConsoleColor.DarkYellow, "(2) ");
                    Coloration(ConsoleColor.DarkGray, "Your Vaccination Date Must Be A Valid Date Value In Any Normal Date Format (ie. DD/MM/YYY)\n");
                    Coloration(ConsoleColor.DarkYellow, "(3) ");
                    Coloration(ConsoleColor.DarkGray, "All Pieces Of Data Must Be Entered\n");
                    Coloration(ConsoleColor.DarkYellow, "(4) ");
                    Coloration(ConsoleColor.DarkGray, "The Data Must Be Entered In The Order Given In The Format Above\n");
                    Coloration(ConsoleColor.DarkYellow, "(5) ");
                    Coloration(ConsoleColor.DarkGray, "Colons (:) Must Be Used To Separate The Data Items With No SPACES Or Other Characters Included In The Message\n\n");
                    Coloration(ConsoleColor.DarkYellow, ">>");

                    //fetch input message input form userS
                    input = ReadLine();

                    int valid = ValidateMessage(input);

                    if (valid == 0)
                    {
                        //add message to queue
                        //await queueManager.AddMessageToQueue(message);
                        //success message
                        Coloration(ConsoleColor.DarkGreen, "Message added to the queue successfully!");

                        //exit loop
                        flag = false;
                    }

                } while (flag);
                ////validate id number loop:
                //input = ValidateIDNumberLoop();
                ////add id to message
                //message += input;

                ////validate vaccination center loop:
                //input = ValidateVaccinationCenterLoop();
                ////add vaccination center to message
                //message += ":" + input;

                ////validate vaccination date loop:
                //input = ValidateVaccinationDateLoop();
                ////add vaccination date to message
                //message += ":" + input;

                ////validate vaccination number loop:
                //input = ValidateVaccinationNumberLoop();
                ////add vaccination number to message
                //message += ":" + input;

                
            }
            //vaccination barcode picked
            else if (input == "2")
            {
                ////validate vaccination barcode loop:
                //input = ValidateVaccinationBarcodeLoop();
                ////add vaccination barcode to message
                //message += input;

                ////validate vaccination date loop:
                //input = ValidateVaccinationDateLoop();
                ////add vaccination date to message
                //message += ":" + input;

                ////validate vaccination center loop:
                //input = ValidateVaccinationCenterLoop();
                ////add vaccination center to message
                //message += ":" + input;

                ////validate id number loop:
                //input = ValidateIDNumberLoop();
                ////add id to message
                //message += ":" + input;

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

        //check if message is valid:
        // 0 : valid message
        // 1 : invalid message
        public static int ValidateMessage(string message)
        { 
            //break string into data parts
            string[] parts = message.Split(':');

            //check if a message has been entered and if it is in the correct format
            if ((message.Equals(null)) || (parts.Length != 4))
            {
                //generate error message
                ErrorMessage("PLEASE ENTER A VALID MESSAGE!");
                //invalid message >> error out
                return 1;
            }

            //Validation of message parts:
            //validate ID
            if (ValidateIDNumber(parts[0]) !=  "0")
            {
                //generate error message
                ErrorMessage(ValidateIDNumber(parts[0]));
                //invalid id >> error out
                return 1;
            }
            //validate vaccination center
            else if (ValidateVaccinationCenter(parts[1]) != "0")
            {
                //generate error message
                ErrorMessage(ValidateVaccinationCenter(parts[1]));
                //invalid center >> error out
                return 1;
            }
            //validate vaccination date
            else if (ValidateVaccinationDate(parts[2]) != "0")
            {
                //generate error message
                ErrorMessage(ValidateVaccinationDate(parts[2]));
                //invalid date >> error out
                return 1;
            }
            //validate vaccination number
            else if (ValidateVaccineSerialNumber(parts[3]) != "0")
            {
                //generate error message
                ErrorMessage(ValidateVaccineSerialNumber(parts[3]));
                //invalid serial number >> error out
                return 1;
            }

            //valid message
            return 0;
        }//end ValidateMessage method

        //------------------------------------------------------------------------------------------------------------------------------ValidateIDNumber

        private static string ValidateIDNumber(string id)
        {

            //check if id is null and validate input as either a ID or Passport number
            if (id == "")
            {
                //error out
                return "PLEASE ENTER A VALID INPUT FOR ID!";
            }
            else if (IdentifyIDNumber(id) == 3)
            {
                //error out 
                return "THE ENTERED IDENTIFCATION NUMBER IS NOT A VALID SOUTH AFRICAN ID OR PASSPORT NUMBER!";
            }    
            //end loop

            //valid id
            return "0";
            
        }//end ValidateIDNumber method

        //------------------------------------------------------------------------------------------------------------------------------ValidateVaccinationCenter

        private static string ValidateVaccinationCenter(string center)
        {
            //check if input is null
            if (center == "")
            {
                //error message if input is null
                return "PLEASE ENTER A VALID INPUT FOR VACCINATION CENTER!";
            }

            //valid center
            return "0";
        }//end ValidateVaccinationCenter method

        //------------------------------------------------------------------------------------------------------------------------------ValidateVaccinationDate

        private static string ValidateVaccinationDate(string date)
        {
            DateOnly tempDate;

            //check if input is null
            if (date == "")
            {
                //error message if input is null
                return "PLEASE ENTER A VALID INPUT!";
            }
            else if (!DateOnly.TryParse(date, out tempDate))
            {
                //error message if input is null
                return "PLEASE ENTER A VALID DATE!";
            }

            //valid date
            return "0";
        }//end ValidateVaccinationDate method

        //------------------------------------------------------------------------------------------------------------------------------ValidateVaccineSerialNumber

        private static string ValidateVaccineSerialNumber(string serialNumber)
        {
            //method to check that the serial number only contains digits and no characters
            Boolean IsAllDigits(string s) => s.All(Char.IsDigit);

            //check if input is null
            if (serialNumber == "")
            {
                //error message if input is null
                return "PLEASE ENTER A INPUT!";
            }
            //check if serial number is 10 digits and contains only digits
            else if ((serialNumber.Length != 10) || (IsAllDigits(serialNumber) != true))
            {
                //error message if input is null
                return "PLEASE ENTER A VALID VACCINE NUMBER!";
            }

            //valid serial number
            return "0";
        }//end ValidateVaccineSerialNumber method

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