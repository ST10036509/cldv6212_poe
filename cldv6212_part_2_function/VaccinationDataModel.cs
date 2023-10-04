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
        public DateTime? VaccincationDate { get; set; }
        public string VaccinationSerialNumber { get; set; }
        public string VaccinationBarcode { get; set; }
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
            //if it is of Format (2)
            if (parts[0].Length == 10)
            {
                //assign values to Data Model variables
                data.Id = parts[0];
                data.VaccinationCenter = parts[1];

                //check if DateTime is valid (again)
                if (DateTime.TryParse(parts[2], out DateTime date))
                {
                    data.VaccincationDate = date;
                }

                data.VaccinationSerialNumber = parts[3];
            }
            //if it is Format (1)
            else
            {
                data.Id = parts[0];
                data.VaccinationBarcode = parts[0];

                //check if DateTime is valid (again)
                if (DateTime.TryParse(parts[1], out DateTime date))
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