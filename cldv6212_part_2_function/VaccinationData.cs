using Azure.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cldv6212_part_2_function
{
    public class VaccinationData
    {

        //getter amd setters
        public string Id { get; set; }
        public string VaccinationCenter { get; set; }
        public DateTime? VaccincationDate { get; set; }
        public string VaccinationSerialNumber { get; set; }
        public string VaccinationBarcode { get; set; }
        //

        public static VaccinationData FromMessage(string message)
        {
            string[] parts = message.Split('|');

            var data = new VaccinationData();

            if (parts[0].Length == 10)
            {
                data.Id = parts[0];
                data.VaccinationCenter = parts[1];

                if (DateTime.TryParse(parts[2], out DateTime date))
                {
                    data.VaccincationDate = date;
                }

                data.VaccinationSerialNumber = parts[3];
            }
            else
            {
                data.VaccinationBarcode = parts[0];

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