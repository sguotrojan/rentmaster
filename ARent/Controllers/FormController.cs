using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using ARent.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ARent.DAO;
using System.Web.Configuration;
using System.Collections;

namespace ARent.Controllers
{
    public class FormController : Controller
    {
        private CloudBlobContainer container;
        public FormController () {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(WebConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference("housepicture");
        }

        [HttpPost]
        public ActionResult RentForm(LandlordForm form, HttpPostedFileBase[] file)
        {
           if (ModelState.IsValid)
           {
               IList<String> pictureUrls = new List<String>();
               if (file != null)
               {
                   for (int i = 0; i < file.Length; i++)
                   {
                       if (file[i] == null) 
                       { 
                           continue; 
                       }
                       string extension = System.IO.Path.GetExtension(file[i].FileName);
                       if (String.Compare(extension, ".jpg", StringComparison.OrdinalIgnoreCase) == 0
                         || String.Compare(extension, ".gif", StringComparison.OrdinalIgnoreCase) == 0
                         || String.Compare(extension, ".png", StringComparison.OrdinalIgnoreCase) == 0)
                       {
                           string blobPictureUrl = string.Format(@"{0}" + extension, Guid.NewGuid());
                           CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobPictureUrl);
                           blockBlob.UploadFromStream(file[i].InputStream);
                           pictureUrls.Add(Configuration.ImageContainURL + blobPictureUrl);
                       }
                   }
               }
               form.PictureUrls = pictureUrls;
               HouseDAO.InsertRecord((HouseEntity) form);
               return View("Success");               
           }
           return View(form);
       }

        public ActionResult RentForm()
        {
            return View();
        }

      /*  public ActionResult UploadImage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(WebConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("housepicture");
            container.CreateIfNotExists();
            //reading images in the blob
            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob pageBlob = (CloudPageBlob)item;

                    Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);

                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;

                    Console.WriteLine("Directory: {0}", directory.Uri);
                }
            }
        }*/

        private HouseType parseHouseType(String input)
        {
            switch(input) {
                case  "TH":
                    return HouseType.TownHouse;
                case "SFH":
                    return HouseType.SingleFamilyHouse;
                case "APT":
                    return HouseType.Apartment;
                default:
                    return HouseType.Undefined;
            }
        }
        [HttpGet]
        public ActionResult Success() 
        {
            return View();
        }
    }
}