using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARent.Models
{
    public class House
    {
        public String City { get; set; }

        [Required(ErrorMessage ="AddressRequired")]
        public Address Address { get; set; }
        public Contact Contact { get; set; }

        public Coordinate Coordinate { get; set; }

        public Guid Guid { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public HouseType HouseType { get; set; }

        public IList<string> PictureUrls { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; }

        public int MonthlyPrice { get; set; }

        public string CurrencyCode { get; set; }
        public int? BuildYear { get; set; }
        public int? NumberOfBedRoom { get; set; }
        public int? NumberOfBathRoom { get; set; }
        public int? NumberOfLivingRoom { get; set; }
        public bool? HasGarage { get; set; }
        public bool? HasPrivateParking { get; set; }

        public House() { }     
    }
}