using ARent.Models;
using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace ARent.DAO
{
    public class HouseDAO
    {
        public static void InsertRecord(HouseEntity record)
        {
            InsertHouseDBRecord(record);
        }

        public static void QueryInfoByZip(string zipcode)
        {
            GetInformationByZipCode(zipcode);
        }

        public static async Task<IList<HouseEntity>> GetInformationByZipCode(string zipcode)
        {
            DocumentClient client = Configuration.getClient();
            Database database = await RetrieveOrCreateDatabaseAsync("RentForms");
            IList<HouseEntity> results = new List<HouseEntity>();

            //Retrieve collection from the Database 
            var documentCollection = await RetrieveOrCreateCollectionAsync(database.SelfLink);
            var houses = client.CreateDocumentQuery(documentCollection.DocumentsLink,
            "SELECT * " +
            "FROM RentForms r " +
            "WHERE r.Address.Zip =" + "\"" + zipcode + "\"");
            foreach (var r in houses)
            {
                HouseEntity item = new HouseEntity();
                item = r;
                results.Add(item);
            }
            return (results);
        }    

        private  static async Task<Database> RetrieveOrCreateDatabaseAsync(string id)
        {            
            // Try to retrieve the database (Microsoft.Azure.Documents.Database) whose Id is equal to databaseId  
            DocumentClient client = Configuration.getClient();
            var database = client.CreateDatabaseQuery().Where(db => db.Id == "RentForms").AsEnumerable().FirstOrDefault();

            // If the previous call didn't return a Database, it is necessary to create it
            if (database == null)
            {
                database = await client.CreateDatabaseAsync(new Database { Id = "RentForms" });
                Console.WriteLine("Created Database: id - {0} and selfLink - {1}", database.Id, database.SelfLink);
            }

            return database;
        }

        private static async Task<DocumentCollection> RetrieveOrCreateCollectionAsync(string databaseSelfLink)
        {
            DocumentClient client = Configuration.getClient();
            // Try to retrieve the collection (Microsoft.Azure.Documents.DocumentCollection) whose Id is equal to collectionId
            var collection = client.CreateDocumentCollectionQuery(databaseSelfLink).Where(c => c.Id == "HouseForms").ToArray().FirstOrDefault();

            // If the previous call didn't return a Collection, it is necessary to create it
            if (collection == null)
            {
                collection = await client.CreateDocumentCollectionAsync(databaseSelfLink, new DocumentCollection { Id = "HouseForms" });
            }

            return collection;
        }

        private static async Task InsertHouseDBRecord(HouseEntity record)
        {
            try
            {
                DocumentClient client = Configuration.getClient();
                //Try get a Database if exists, else create the Database resource 
                Database database = await RetrieveOrCreateDatabaseAsync("RentForms");

                //Retrieve collection from the Database 
                var collection = await RetrieveOrCreateCollectionAsync(database.SelfLink);

                //persist the documents in DocumentDB
                await client.CreateDocumentAsync(collection.SelfLink, record);
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("Status code {0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Console.WriteLine("Please, press any key.");
                Console.ReadKey();
            }
        } 
    }
}