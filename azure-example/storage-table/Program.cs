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
        private static string accountName = "saxsysdojotest";
        private static string keyValue = "R1a0JZi11ZZ9sWSy/PX8wazBV+SFK/7Xru0LdkIeCTV9mLxfBkij3ks72IOYM/lJ0g/kKSgwlfo5+tFrrtzILg==";

        private static IList<Auto> autos = new List<Auto>() {
            new Auto("autos", Guid.NewGuid()) { Marke = "Opel", Modell = "Astra", Farbe = "blau", PS = 120 },
            new Auto("autos", Guid.NewGuid()) { Marke = "VW", Modell = "Passat", Farbe = "rot", PS = 85 },
            new Auto("autos", Guid.NewGuid()) { Marke = "VW", Modell = "Golf", Farbe = "silber", PS = 140 },
            new Auto("autos", Guid.NewGuid()) { Marke = "Opel", Modell = "Zafira", Farbe = "rot", PS = 140 },
            new Auto("autos", Guid.NewGuid()) { Marke = "BMW", Modell = "3er", Farbe = "weiß", PS = 200 },
            new Auto("autos", Guid.NewGuid()) { Marke = "Mercedes", Modell = "E-Klasse", Farbe = "schwarz", PS = 170 }
        };

        private static IList<Person> persons = new List<Person>() {
            new Person("Gans", "Gustav") { Adress = "Gänseweg", City = "Entenhausen" },
            new Person("Beckenbauer", "Franz") { Adress = "Säbener Straße", City = "München" },
            new Person("Gottschalk", "Thomas") { Adress = "Malibu" , City = "Kalifornien" },
            new Person("Krüger", "Mike") { Adress = "Piratensender", City = "Powerplay" }
        };

        private static CloudTableClient tableClient;

        static void Main(string[] args)
        {
            var storageCredentials = new StorageCredentials(accountName, keyValue);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);

            tableClient = storageAccount.CreateCloudTableClient();

            Autos();

            //Persons();

            var tables = tableClient.ListTables();

            Console.ReadLine();
        }

        private static void Autos()
        {
            CloudTable autoTable = tableClient.GetTableReference("autos");
            autoTable.CreateIfNotExists();

            var batchOperation = new TableBatchOperation();
            foreach (var item in autos)
            {
                batchOperation.Insert(item);
            }

            autoTable.ExecuteBatch(batchOperation);

            // single line
            var insertItem = new Auto("autos", Guid.NewGuid()) { Marke = "Pontiac", Modell = "Firebird", Farbe = "schwarz", PS = 1000 };
            var insertOpertation = TableOperation.Insert(insertItem);
            autoTable.Execute(insertOpertation);
        }

        private static void Persons()
        {
            CloudTable personTable = tableClient.GetTableReference("persons");
            personTable.CreateIfNotExists();

            var batchOperation = new TableBatchOperation();
            foreach (var item in persons)
            {
                batchOperation.Insert(item);
            }

            personTable.ExecuteBatch(batchOperation);
        }
    }

    public class Auto : TableEntity
    {
        public Auto(string partition, Guid id)
        {
            this.PartitionKey = partition;
            this.RowKey = id.ToString();
        }

        public string Marke { get; set; }

        public string Modell { get; set; }

        public string Farbe { get; set; }

        public int PS { get; set; }
    }

    public class Person : TableEntity
    {
        public Person(string lastname, string firstname)
        {
            this.PartitionKey = lastname;
            this.RowKey = firstname;
        }

        public string Adress { get; set; }

        public string City { get; set; }
    }
}
