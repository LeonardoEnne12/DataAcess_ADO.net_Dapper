using System;
using Microsoft.Data.SqlClient;

namespace ADO.Net
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=Curso;User ID=sa;Password=1q2w3e4r@#$";

            
            // var connection = new SqlConnection();
            // connection.Open();

            // connection.Close();
            // connection.Dispose(); //distrói o objeto ,ou seja, é preciso criá-lo de novo 
            //using já faz o Dispose
            using (var connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Banco de dados Conectado");
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT [Id], [Title] FROM [Category]";
                    //ExecuteReader() lê infos, ExecuteNonQuery() insere infos
                    var reader = command.ExecuteReader();

                    while(reader.Read())
                    {
                        Console.WriteLine($"{reader.GetGuid(0)} - {reader.GetString(1)}");
                    }
                }
            }

        }
    }
}
