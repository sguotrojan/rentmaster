using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARent.DAO
{
    public class Configuration
    {
        private  static string EndpointUrl = "https://rentmasterdb.documents.azure.com:443/";
        private  static string AuthorizationKey = "wPsCTLDbs/ex67rHzMcTX5R5d7qqHvkELm533A2+pvwVOk5RKnYFKNvfh01/I+gCC4SZ79tTOUXcH+p1MtFUEA==";
        public   static string ImageContainURL = "https://housepicture.blob.core.windows.net/housepicture/";
        public   static DocumentClient client;
        private Configuration() {
            client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
        }

        public static DocumentClient  getClient()
        {
            if(client == null)
                return new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
            return client;
        }
    }
}