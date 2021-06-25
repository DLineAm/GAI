using System;

namespace GAI_API.Models
{
    public class Drivers
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public Nullable<int> passport_serial { get; set; }
        public Nullable<int> passport_number { get; set; }
        public Nullable<int> postcode { get; set; }
        public string address { get; set; }
        public string address_life { get; set; }
        public string company { get; set; }
        public string jobname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string photo { get; set; }
        public byte[] image { get; set; }
        public string description { get; set; }
    }
}