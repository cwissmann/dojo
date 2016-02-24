using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql_database
{
    class Program
    {
        private static string connectionString = "";

        static void Main(string[] args)
        {
            //InsertLines();

            ReadLines();

            Console.ReadLine();
        }

        private static void ReadLines()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                        SELECT *
                        FROM dbo.Dojo
                    ";

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("ID: {0} Name: {1} Vorname: {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                    }
                }

                conn.Close();
            }
        }

        private static void InsertLines()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                INSERT dbo.Dojo(Id, Name, Vorname)
                VALUES (@Id, @Name, @Vorname)";

                cmd.Parameters.AddWithValue("@Id", new Random().Next(1, 1000));
                cmd.Parameters.AddWithValue("@Name", "Gans");
                cmd.Parameters.AddWithValue("@Vorname", "Gustav");

                conn.Open();

                cmd.ExecuteScalar();

                conn.Close();
            }
        }
    }
}
