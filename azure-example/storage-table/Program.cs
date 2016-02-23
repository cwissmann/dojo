using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_table
{
    class Program
    {
        private static string accountName = "";
        private static string keyValue = "";

        private static IList<Auto> autos = new List<Auto>() {
            new Auto("Opel", "Astra") { Farbe = "blau", PS = 120 },
            new Auto("VW", "Golf") { Farbe = "rot", PS = 85 },
            new Auto("VW", "Passat") { Farbe = "silber", PS = 140 },
            new Auto("Opel", "Zafira") { Farbe = "rot", PS = 140 },
            new Auto("BMW", "3er") { Farbe = "weiß", PS = 200 },
            new Auto("Mercedes", "E-Klasse") { Farbe = "schwarz", PS = 170 }
        };

        static void Main(string[] args)
        {
            var storageCredentials = new StorageCredentials(accountName, keyValue);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);

            var tableClient = storageAccount.CreateCloudTableClient();

            CloudTable autoTable = tableClient.GetTableReference("autos");
            autoTable.CreateIfNotExists();

            var batchOperation = new TableBatchOperation();
            foreach (var item in autos)
            {
                batchOperation.Insert(item);
            }

            autoTable.ExecuteBatch(batchOperation);

            var tables = tableClient.ListTables();
        }
    }

    public class Auto : TableEntity
    {
        public Auto(string marke, string modell)
        {
            this.PartitionKey = marke;
            this.RowKey = modell;
        }

        public string Farbe { get; set; }

        public int PS { get; set; }
    }
}
