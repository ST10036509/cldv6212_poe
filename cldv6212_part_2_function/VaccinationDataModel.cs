using Azure.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cldv6212_part_2_function
{
    public class VaccinationDataModel
    {

        //getter amd setters
        public string Id { get; set; }
        public string VaccinationCenter { get; set; }
        public DateOnly? VaccincationDate { get; set; }
        public string VaccineSerialNumber { get; set; }
        public string VaccineBarcode { get; set; }
        //

        //capture message and break it down into Data Model context based on format
        //Formats:
        //(1) Id:VaccinationCenter:VaccinationDate:VaccineSerialNumber
        //(2) VaccineBarcode:VaccinationDate:VaccinationCenter:Id
        public static VaccinationDataModel FromMessage(string message)
        {
            //break string into data parts
            string[] parts = message.Split(':');

            //create a new Data Model instance
            var data = new VaccinationDataModel();

            //check format:
            //if it is of Format (1)
            if (parts[0].Length != 12)
            {
                //assign values to Data Model variables
                data.Id = parts[0];
                data.VaccinationCenter = parts[1];

                //check if DateTime is valid (again)
                if (DateOnly.TryParse(parts[2], out DateOnly date))
                {
                    data.VaccincationDate = date;
                }

                data.VaccineSerialNumber = parts[3];
            }
            //if it is Format (2)
            else
            {
                data.VaccineBarcode = parts[0];

                //check if DateTime is valid (again)
                if (DateOnly.TryParse(parts[1], out DateOnly date))
                {
                    data.VaccincationDate = date;
                }

                data.VaccinationCenter = parts[2];
                data.Id = parts[3];
            }

            return data;
        }
    }
}
//__________________________________...oooOOO000_End_Of_File_000OOOooo...__________________________________