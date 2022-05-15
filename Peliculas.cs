using System;
using System.Configuration;
using System.Data.SqlClient;

namespace VideoClub
{
    public class Peliculas
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["videoClub"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString);

        public string Mail { get; set; }
        public int FilmId { get; set; }
        public int Repetir { get; set; }
        public int Age { get; set; }

        public Peliculas()
        {
        }

        public Peliculas(string mail, int age)
        {
            Mail = mail;
            Age = age;
        }

        public void menuPeliculas() {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n¿Qué quieres hacer ahora?");
            Console.ResetColor();
            Console.WriteLine("1. Alquilar peliculas");
            Console.WriteLine("2. Gestionar alquileres");
            Console.WriteLine("3. Salir");
            int opcion2 = Convert.ToInt32(Console.ReadLine());
            switch (opcion2)
            {
                case 1:
                    verPeliculas(Age);
                    break;
                case 2:
                    Alquileres alq = new Alquileres();
                    alq.gestionarAlquileres(Mail, Age);
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("🐬 VHS CLUB 🐬");
                    Console.WriteLine("Gracias por la visita, vuelve cuando quieras.");
                    Console.WriteLine("Para iniciar sesión pulsa un número.");
                    int menu = Convert.ToInt32(Console.ReadLine());
                    if (menu == 0 || menu == 1 || menu == 2 || menu == 3 || menu == 4 || menu == 5 || menu == 6 || menu == 7 || menu == 8 || menu == 9)
                    {
                        Console.Clear();
                        Usuario user2 = new Usuario();
                        user2.inicio();
                        break;
                    }
                    else {
                        break;
                    }
                
            }
        }

        public void verPeliculas(int age)
        {
            Repetir = 0;
            Console.Clear();
            if (Age >= 18) {
                do { 
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT ID, Title, Synopsis, Age, Situation FROM Films", connection);
                    SqlDataReader lector = command.ExecuteReader();
                    Console.WriteLine("🐬 VHS CLUB 🐬"); 
                    while (lector.Read())
                    {
                        string disp = $"{lector["Situation"]}";
                        VisualizarPelis(disp, lector);
                    }
                    connection.Close();
                    Console.WriteLine("\nEscribe el número de la película para alquilarla, o pulsa 0 para volver.");
                    FilmId = Convert.ToInt32(Console.ReadLine());
                    if (FilmId == 0) {
                        Console.Clear();
                        Console.WriteLine("🐬 VHS CLUB 🐬");
                        Peliculas volver = new Peliculas(Mail, Age);
                        volver.menuPeliculas();
                    }

                    connection.Open();
                    SqlCommand command4 = new SqlCommand($"SELECT Situation FROM Films WHERE ID= {FilmId}", connection);
                    SqlDataReader lector4 = command4.ExecuteReader();
                    while (lector4.Read())
                    {
                        string disp = $"{lector4["Situation"]}";
                        if (disp == "No")
                        {
                            Repetir = 1;
                            Console.WriteLine("\nUps, no puedes alquilar esa peli. Prueba con otra.");
                        }
                        else
                        {
                            Repetir = 0;
                            connection.Close();
                            Alquileres rent = new Alquileres(Mail, FilmId, Age);
                            rent.Alquilar(Mail, FilmId, Age);
                        }
                    }
                    connection.Close();
                    
                } while (Repetir == 1);
            }
            if (Age < 18)
            {
                do
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT ID, Title, Synopsis, Age, Situation FROM Films WHERE Age = 0", connection);
                    SqlDataReader lector = command.ExecuteReader();
                    Console.WriteLine("🐬 VHS CLUB 🐬");
                    
                    // MUESTRA PELIS
                    while (lector.Read())
                    {
                        string disp = $"{lector["Situation"]}";
                        VisualizarPelis(disp, lector);
                    }
                    connection.Close();

                    if (Repetir == 1)
                    {
                        Console.WriteLine("\nUps, no puedes alquilar esa peli, es para mayores de 18 años. Prueba con otra.");

                    }
       
                    Repetir = 0;
                    VerificarEdad();
                } while (Repetir == 1);

            }
        }

        public void VisualizarPelis(string disp, SqlDataReader lector) {
            if (disp == "Si")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{lector["ID"]} - {lector["Title"]} 🟢 ¡Está disponible!");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"{lector["Synopsis"]}");
                Console.WriteLine($"Edad recomentada: +{lector["Age"]}");
                Console.ResetColor();
            }
            if (disp == "No")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n{lector["ID"]} - {lector["Title"]} 🟣 No está disponible :(");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"{lector["Synopsis"]}");
                Console.WriteLine($"Edad recomentada: +{lector["Age"]}");
                Console.ResetColor();
            }
        }

        // VERIFICAR EDAD
        public void VerificarEdad()
        {
            connection.Open();
            Console.WriteLine("\nEscribe el número de la película para alquilarla:");
            FilmId = Convert.ToInt32(Console.ReadLine());
            SqlCommand command = new SqlCommand($"SELECT Age, Situation FROM Films WHERE Id = '{FilmId}'", connection);
            SqlDataReader lector3 = command.ExecuteReader();
            int age = 0;
            int edad = 0;

            while (lector3.Read())
            {
                string ageU = $"{lector3["Age"]}";
                edad = Convert.ToInt32(ageU);
            }
            connection.Close();
            if (edad == 18 || edad > 18)
            {
                if (age >= 18)
                {
                    Alquileres rent = new Alquileres(Mail, FilmId, Age);
                    rent.Alquilar(Mail, FilmId, Age);
                    connection.Close();

                }
                if (age < 18)
                {
                    Repetir = 1;
                    connection.Close();
                }
            }
            else
            {
                if (age < 18)
                {
                    Repetir = 1;
                    connection.Close();
                }
                Alquileres rent = new Alquileres(Mail, FilmId, Age);
                rent.Alquilar(Mail, FilmId, Age);
                connection.Close();
            }
        }

    }
}