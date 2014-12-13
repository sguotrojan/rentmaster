using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ARent.Models
{
    public class HouseEntity 
    {     
        public Address Address { get; set; }

        public Contact Contact { get; set; }

        public string HouseType { get; set; }

        public IList<String> PictureUrls { get; set; }

        public string Status { get; set; }

        public double MonthlyPrice { get; set; }

        public double? Deposit { get; set; }

        public string CurrencyCode { get; set; }
        public int? BuildYear { get; set; }
        public int? NumberOfBedRoom { get; set; }
        public int? NumberOfBathRoom { get; set; }
        public int? NumberOfLivingRoom { get; set; }
        public bool? HasGarage { get; set; }
        public bool? HasPrivateParking { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreationDate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Notes { get; set; }

        public static explicit operator HouseEntity(LandlordForm house)
        {
            var houseEntity = new HouseEntity();
            Address address = new Address();
            address.Line1 = house.Address;
            address.City = house.City;
            address.State = house.State;
            address.Zip = house.Zip;
            Contact contact = new Contact();
            contact.ContactName = house.ContactName;
            contact.Email = house.Email;
            contact.PhoneNumber = house.PhoneNumber;
            houseEntity.Address = address;
            houseEntity.Contact = contact;    
            houseEntity.MonthlyPrice = house.Rent;
            houseEntity.Deposit = house.Deposit;
            houseEntity.PictureUrls = house.PictureUrls;
            houseEntity.IsVisible = true;
            houseEntity.CreationDate = new DateTime();
            houseEntity.Latitude = house.Latitude;
            houseEntity.Longitude = house.Longitude;
            houseEntity.Status = "Available";
            houseEntity.Notes = house.Notes;
            return houseEntity;
        }
    }
}