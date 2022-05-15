using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;

namespace VideoClub
{
    public class Alquileres
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["videoClub"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString);

        public string Mail { get; set; }
        public int FilmId { get; set; }
        public int Age { get; set; }

        public Alquileres()
        {
        }

        public Alquileres(string mail, int filmid, int age)
        {
            Mail = mail;
            FilmId = filmid;
            Age = age;
        }

        public void Alquilar(string mail, int filmid, int age)
        {
            connection.Open();
            DateTime today = DateTime.Now;
            DateTime finish = today.AddDays(10);
            string query = $"INSERT INTO Rentals(FilmID, UserMail, RentalDate, RentalFinish) values ('{filmid}', '{mail}', '{today}', '{finish}'); UPDATE Films SET Situation = 'No' WHERE ID = '{filmid}'; SELECT Title FROM Films WHERE ID = '{filmid}'";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader lector = command.ExecuteReader();
            Console.Clear();
            Console.WriteLine("🐬 VHS CLUB 🐬");
            Console.Write("\n¡Película ✨📼 ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            while (lector.Read())
            {
                Console.Write($"{lector["Title"]}");
            }
            Console.ResetColor();
            Console.Write(" 📼✨ alquilada con éxito!\n");
            connection.Close();
            Peliculas films = new Peliculas(Mail, Age);
            films.menuPeliculas();
        }

        public void gestionarAlquileres(string mail, int age)
        {
            connection.Open();
            Console.Clear();
            Console.WriteLine("🐬 VHS CLUB 🐬");

            string query = $"SELECT RentalDate, RentalFinish, RentalDone, FilmID, Title FROM Rentals, Films WHERE UserMail = '{mail}' AND FilmID = ID AND RentalDone IS NOT NULL;";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader lector2 = command.ExecuteReader();
            Console.WriteLine("\nHISTORIAL DE ALQUILERES\n");

            while (lector2.Read())
            {
                DateTime date = Convert.ToDateTime($"{lector2["RentalDate"]}");
                DateTime dateDevo = Convert.ToDateTime($"{lector2["RentalDone"]}");
                DateTime dateFinish = Convert.ToDateTime($"{lector2["RentalFinish"]}");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"{lector2["Title"]}");
                Console.Write(" - Fecha devolución: " + dateDevo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                Console.Write(" - Fecha alquiler: " + date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                Console.ResetColor();
                Console.WriteLine("\n");
            }
            connection.Close();

            Console.WriteLine("\nALQUILERES ACTIVOS\n");
            verPeliculas(mail);
            devolverPeliculas(mail, age);
        }

        public void verPeliculas(string mail) {
            connection.Open();
            string query2 = $"SELECT RentalDate, RentalFinish, RentalDone, FilmID, Title FROM Rentals, Films WHERE UserMail = '{mail}' AND FilmID = ID AND RentalDone IS NULL;";
            SqlCommand command2 = new SqlCommand(query2, connection);
            SqlDataReader lector3 = command2.ExecuteReader();

            while (lector3.Read())
            {
                DateTime date = Convert.ToDateTime($"{lector3["RentalDate"]}");
                DateTime dateFinish = Convert.ToDateTime($"{lector3["RentalFinish"]}");
                DateTime today = DateTime.Now;

                if (today > dateFinish)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{lector3["FilmID"]}- {lector3["Title"]}");
                    Console.Write("\n ⚠️ Devolver YA, ¡estás fuera de plazo!");
                    Console.Write(" ⚠️ Fecha alquiler: " + date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                    Console.ResetColor();
                    Console.WriteLine("\n");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{lector3["FilmID"]}- {lector3["Title"]}");
                    Console.Write("\n 📆 Devolver antes de: " + dateFinish.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                    Console.Write(" 📆 Fecha alquiler: " + date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                    Console.ResetColor();
                    Console.WriteLine("\n");
                }
            }
            connection.Close();
        }

        public void devolverPeliculas(string mail, int age) {
            Console.WriteLine("Escribe el número de la película que quieres devolver, o pulsa 0 para volver al menú principal.");
            int devolver = Convert.ToInt32(Console.ReadLine());

            if (devolver == 0)
            {
                Console.Clear();
                Console.WriteLine("🐬 VHS CLUB 🐬");
                Peliculas volver = new Peliculas(mail, age);
                volver.menuPeliculas();
            }
            else { 
                connection.Open();
                DateTime today = DateTime.Now;
                string query = $"UPDATE Rentals SET RentalDone = '{today}' WHERE UserMail = '{mail}' AND FilmID = '{devolver}'; UPDATE Films SET Situation = 'Si' WHERE ID = '{devolver}'; SELECT Title FROM Films WHERE ID = '{devolver}'";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader lector = command.ExecuteReader();
                Console.Clear();
                Console.WriteLine("🐬 VHS CLUB 🐬");
                Console.Write("\n¡Película ✨📼 ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                while (lector.Read())
                {
                    Console.Write($"{lector["Title"]}");
                }
                Console.ResetColor();
                Console.Write(" 📼✨ devuelta con éxito!\n");
                connection.Close();
                Peliculas volver = new Peliculas(mail, age);
                volver.menuPeliculas();
            }
        }
    }
}
