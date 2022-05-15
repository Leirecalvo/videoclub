using System;
using System.Configuration;
using System.Data.SqlClient;

namespace VideoClub
{
    class Program
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["videoClub"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            Usuario user = new Usuario();
            user.inicio();
        }
    }
}